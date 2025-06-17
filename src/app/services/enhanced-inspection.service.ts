import { Injectable } from '@angular/core';
import { Observable, of, forkJoin } from 'rxjs';
import { map, catchError, switchMap } from 'rxjs/operators';
import { BackendInspectionService } from './backend-inspection.service';
import { InspectionService } from './inspection.service';
import { FormConfig, FormData, InspectionResult } from '../models/form.models';
import { InspectionAnalysisDto, InspectionDecision } from '../models/backend-dto.models';

@Injectable({
  providedIn: 'root'
})
export class EnhancedInspectionService {

  constructor(
    private backendService: BackendInspectionService,
    private localInspectionService: InspectionService
  ) {}

  // Submit inspection with backend integration
  submitInspection(formData: FormData, config: FormConfig): Observable<InspectionResult> {
    return this.backendService.createInspection(formData).pipe(
      switchMap(createdInspection => {
        // Trigger backend analysis
        return this.backendService.analyzeInspection(createdInspection.id).pipe(
          map(analysis => this.mapBackendAnalysisToResult(analysis)),
          catchError(error => {
            console.warn('Backend analysis failed, using local analysis:', error);
            // Fallback to local analysis
            return of(this.localInspectionService.analyzeInspection(formData, config));
          })
        );
      }),
      catchError(error => {
        console.warn('Backend submission failed, using local analysis:', error);
        // Fallback to local analysis only
        return of(this.localInspectionService.analyzeInspection(formData, config));
      })
    );
  }

  // Get inspection history
  getInspectionHistory(filters?: {
    jobNumber?: string;
    customer?: string;
    status?: string;
    page?: number;
    pageSize?: number;
  }): Observable<any[]> {
    return this.backendService.getInspections(filters).pipe(
      map(inspections => inspections.map(inspection => ({
        id: inspection.id,
        jobNumber: inspection.jobNumber,
        customer: inspection.customer,
        partNumber: inspection.partNumber,
        status: this.getStatusText(inspection.status),
        decision: inspection.inspectionAnalysis ? 
          this.getDecisionText(inspection.inspectionAnalysis.decision) : 'Pending',
        inspectionDate: inspection.inspectionDate,
        createdAt: inspection.createdAt
      }))),
      catchError(error => {
        console.error('Failed to load inspection history:', error);
        return of([]);
      })
    );
  }

  // Get detailed inspection
  getInspectionDetails(id: number): Observable<any> {
    return this.backendService.getInspection(id).pipe(
      map(inspection => ({
        ...inspection,
        statusText: this.getStatusText(inspection.status),
        decisionText: inspection.inspectionAnalysis ? 
          this.getDecisionText(inspection.inspectionAnalysis.decision) : 'Pending'
      })),
      catchError(error => {
        console.error('Failed to load inspection details:', error);
        throw error;
      })
    );
  }

  // Re-analyze existing inspection
  reAnalyzeInspection(id: number): Observable<InspectionResult> {
    return this.backendService.analyzeInspection(id).pipe(
      map(analysis => this.mapBackendAnalysisToResult(analysis)),
      catchError(error => {
        console.error('Re-analysis failed:', error);
        throw error;
      })
    );
  }

  // Map backend analysis to frontend result format
  private mapBackendAnalysisToResult(analysis: InspectionAnalysisDto): InspectionResult {
    return {
      decision: this.mapDecisionToString(analysis.decision),
      confidence: analysis.confidence,
      reasons: analysis.reasons || [],
      summary: analysis.summary,
      measurements: {
        total: analysis.totalMeasurements,
        withinTolerance: analysis.withinTolerance,
        outsideTolerance: analysis.outsideTolerance,
        criticalDeviations: analysis.criticalDeviations
      }
    };
  }

  // Map decision enum to string
  private mapDecisionToString(decision: InspectionDecision): 'repair' | 'replace' | 'reject' {
    switch (decision) {
      case InspectionDecision.Okay:
      case InspectionDecision.Repair:
        return 'repair';
      case InspectionDecision.Replace:
        return 'replace';
      case InspectionDecision.Reject:
        return 'reject';
      default:
        return 'repair';
    }
  }

  // Get status text
  private getStatusText(status: number): string {
    const statusMap: { [key: number]: string } = {
      0: 'Draft',
      1: 'Submitted',
      2: 'In Review',
      3: 'Approved',
      4: 'Rejected',
      5: 'Completed'
    };
    return statusMap[status] || 'Unknown';
  }

  // Get decision text
  private getDecisionText(decision: InspectionDecision): string {
    const decisionMap: { [key: number]: string } = {
      0: 'Not Set',
      1: 'Okay',
      2: 'Repair',
      3: 'Replace',
      4: 'Reject'
    };
    return decisionMap[decision] || 'Unknown';
  }

  // Validate form data before submission
  validateFormData(formData: FormData): { isValid: boolean; errors: string[] } {
    const errors: string[] = [];

    // Required field validation
    if (!formData.jobNumber) errors.push('Job Number is required');
    if (!formData.customer) errors.push('Customer is required');
    if (!formData.partNumber) errors.push('Part Number is required');
    if (!formData.operator) errors.push('Operator is required');
    if (!formData.inspectedBy) errors.push('Inspected By is required');
    if (!formData.reviewedBy) errors.push('Reviewed By is required');

    // Measurement validation
    const bladeMeasurements = formData.bladeMeasurements || [];
    const validMeasurements = bladeMeasurements.filter(blade => 
      this.hasValidMeasurements(blade)
    );

    if (validMeasurements.length === 0) {
      errors.push('At least one blade with measurements is required');
    }

    return {
      isValid: errors.length === 0,
      errors
    };
  }

  // Check if blade has valid measurements
  private hasValidMeasurements(blade: any): boolean {
    const measurementFields = ['d1', 'd2', 'd3', 'd4', 'd5', 'd6', 'd7', 'd8', 
                              't1', 't2', 't3', 't4', 't5', 'e1', 'e2', 'e3'];
    
    return measurementFields.some(field => 
      blade[field] !== null && 
      blade[field] !== undefined && 
      blade[field] !== '' && 
      !isNaN(parseFloat(blade[field]))
    );
  }
}