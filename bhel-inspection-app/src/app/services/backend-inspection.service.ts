import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { ApiService } from './api.service';
import { 
  InspectionFormDto, 
  CreateInspectionFormDto, 
  InspectionAnalysisDto,
  BladeMeasurementDto 
} from '../models/backend-dto.models';
import { FormData } from '../models/form.models';

@Injectable({
  providedIn: 'root'
})
export class BackendInspectionService {

  constructor(private apiService: ApiService) {}

  // Create inspection form
  createInspection(formData: FormData): Observable<InspectionFormDto> {
    const createDto = this.mapFormDataToCreateDto(formData);
    return this.apiService.post<InspectionFormDto>('inspectionforms', createDto);
  }

  // Get inspection by ID
  getInspection(id: number): Observable<InspectionFormDto> {
    return this.apiService.get<InspectionFormDto>(`inspectionforms/${id}`);
  }

  // Get all inspections with filtering
  getInspections(filters?: {
    jobNumber?: string;
    customer?: string;
    status?: string;
    page?: number;
    pageSize?: number;
  }): Observable<InspectionFormDto[]> {
    let params = new URLSearchParams();
    
    if (filters) {
      Object.entries(filters).forEach(([key, value]) => {
        if (value !== undefined && value !== null && value !== '') {
          params.append(key, value.toString());
        }
      });
    }

    return this.apiService.get<InspectionFormDto[]>('inspectionforms', params as any);
  }

  // Analyze inspection
  analyzeInspection(id: number): Observable<InspectionAnalysisDto> {
    return this.apiService.post<InspectionAnalysisDto>(`inspectionforms/${id}/analyze`, {});
  }

  // Submit inspection
  submitInspection(id: number): Observable<void> {
    return this.apiService.post<void>(`inspectionforms/${id}/submit`, {});
  }

  // Update inspection
  updateInspection(id: number, formData: Partial<FormData>): Observable<void> {
    const updateDto = this.mapFormDataToUpdateDto(formData);
    return this.apiService.put<void>(`inspectionforms/${id}`, updateDto);
  }

  // Delete inspection
  deleteInspection(id: number): Observable<void> {
    return this.apiService.delete<void>(`inspectionforms/${id}`);
  }

  // Map form data to create DTO
  private mapFormDataToCreateDto(formData: FormData): CreateInspectionFormDto {
    const bladeMeasurements = this.mapBladeMeasurements(formData.bladeMeasurements || []);

    return {
      formId: 'WIF-BKT-08',
      revision: 'Rev1.0',
      jobNumber: formData.jobNumber || '',
      customer: formData.customer || '',
      frame: formData.frame || '',
      component: formData.component || '',
      partNumber: formData.partNumber || '',
      quantity: parseInt(formData.quantity) || 1,
      operator: formData.operator || '',
      inspectionDate: new Date(formData.date || new Date()),
      instrumentId: formData.instrumentId || '',
      calibrationDueDate: new Date(formData.calibrationDueDate || new Date()),
      inspectedBy: formData.inspectedBy || '',
      reviewedBy: formData.reviewedBy || '',
      bladeMeasurements: bladeMeasurements
    };
  }

  // Map form data to update DTO
  private mapFormDataToUpdateDto(formData: Partial<FormData>): any {
    const updateDto: any = {};

    if (formData.jobNumber !== undefined) updateDto.jobNumber = formData.jobNumber;
    if (formData.customer !== undefined) updateDto.customer = formData.customer;
    if (formData.frame !== undefined) updateDto.frame = formData.frame;
    if (formData.component !== undefined) updateDto.component = formData.component;
    if (formData.partNumber !== undefined) updateDto.partNumber = formData.partNumber;
    if (formData.quantity !== undefined) updateDto.quantity = parseInt(formData.quantity);
    if (formData.operator !== undefined) updateDto.operator = formData.operator;
    if (formData.date !== undefined) updateDto.inspectionDate = new Date(formData.date);
    if (formData.instrumentId !== undefined) updateDto.instrumentId = formData.instrumentId;
    if (formData.calibrationDueDate !== undefined) updateDto.calibrationDueDate = new Date(formData.calibrationDueDate);
    if (formData.inspectedBy !== undefined) updateDto.inspectedBy = formData.inspectedBy;
    if (formData.reviewedBy !== undefined) updateDto.reviewedBy = formData.reviewedBy;

    return updateDto;
  }

  // Map blade measurements from form data
  private mapBladeMeasurements(bladeMeasurements: any[]): any[] {
    return bladeMeasurements
      .filter(blade => this.hasValidMeasurements(blade))
      .map((blade, index) => ({
        bladeNumber: blade.bladeNo || (index + 1),
        partNumber: blade.partNo || '',
        serialNumber: blade.serialNo || '',
        passFail: this.mapPassFailStatus(blade.passFail),
        d1: this.parseDecimal(blade.d1),
        d2: this.parseDecimal(blade.d2),
        d3: this.parseDecimal(blade.d3),
        d4: this.parseDecimal(blade.d4),
        d5: this.parseDecimal(blade.d5),
        d6: this.parseDecimal(blade.d6),
        d7: this.parseDecimal(blade.d7),
        d8: this.parseDecimal(blade.d8),
        t1: this.parseDecimal(blade.t1),
        t2: this.parseDecimal(blade.t2),
        t3: this.parseDecimal(blade.t3),
        t4: this.parseDecimal(blade.t4),
        t5: this.parseDecimal(blade.t5),
        e1: this.parseDecimal(blade.e1),
        e2: this.parseDecimal(blade.e2),
        e3: this.parseDecimal(blade.e3)
      }));
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

  // Parse decimal values
  private parseDecimal(value: any): number | null {
    if (value === null || value === undefined || value === '') {
      return null;
    }
    const parsed = parseFloat(value);
    return isNaN(parsed) ? null : parsed;
  }

  // Map pass/fail status
  private mapPassFailStatus(status: string): number {
    switch (status?.toLowerCase()) {
      case 'pass': return 1;
      case 'fail': return 2;
      default: return 0; // NotSet
    }
  }
}