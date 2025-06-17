import { Injectable } from '@angular/core';
import { FormConfig, FormData, InspectionResult } from '../models/form.models';

@Injectable({
  providedIn: 'root'
})
export class InspectionService {

  constructor() { }

  analyzeInspection(formData: FormData, config: FormConfig): InspectionResult {
    const measurements = formData['measurements'] || [];
    const tolerances = config.businessRules.tolerances;
    
    let totalMeasurements = 0;
    let withinTolerance = 0;
    let outsideTolerance = 0;
    let criticalDeviations = 0;
    const reasons: string[] = [];

    // Analyze measurements
    measurements.forEach((row: any, index: number) => {
      if (row.d1 !== null && row.d1 !== undefined && row.d1 !== '') {
        totalMeasurements++;
        const d1Value = parseFloat(row.d1);
        const d1Tolerance = tolerances['d1'];
        
        if (d1Value >= d1Tolerance.min && d1Value <= d1Tolerance.max) {
          withinTolerance++;
        } else {
          outsideTolerance++;
          const deviation = Math.abs(d1Value - d1Tolerance.nominal) / d1Tolerance.nominal * 100;
          if (deviation > 20) {
            criticalDeviations++;
            reasons.push(`D1 measurement ${index + 1}: Critical deviation (${deviation.toFixed(1)}%)`);
          } else if (deviation > 10) {
            reasons.push(`D1 measurement ${index + 1}: Significant deviation (${deviation.toFixed(1)}%)`);
          }
        }
      }

      if (row.d2 !== null && row.d2 !== undefined && row.d2 !== '') {
        totalMeasurements++;
        const d2Value = parseFloat(row.d2);
        const d2Tolerance = tolerances['d2'];
        
        if (d2Value >= d2Tolerance.min && d2Value <= d2Tolerance.max) {
          withinTolerance++;
        } else {
          outsideTolerance++;
          const deviation = Math.abs(d2Value - d2Tolerance.nominal) / d2Tolerance.nominal * 100;
          if (deviation > 20) {
            criticalDeviations++;
            reasons.push(`D2 measurement ${index + 1}: Critical deviation (${deviation.toFixed(1)}%)`);
          } else if (deviation > 10) {
            reasons.push(`D2 measurement ${index + 1}: Significant deviation (${deviation.toFixed(1)}%)`);
          }
        }
      }

      if (row.d3 !== null && row.d3 !== undefined && row.d3 !== '') {
        totalMeasurements++;
        const d3Value = parseFloat(row.d3);
        const d3Tolerance = tolerances['d3'];
        
        if (d3Value >= d3Tolerance.min && d3Value <= d3Tolerance.max) {
          withinTolerance++;
        } else {
          outsideTolerance++;
          const deviation = Math.abs(d3Value - d3Tolerance.nominal) / d3Tolerance.nominal * 100;
          if (deviation > 20) {
            criticalDeviations++;
            reasons.push(`D3 measurement ${index + 1}: Critical deviation (${deviation.toFixed(1)}%)`);
          } else if (deviation > 10) {
            reasons.push(`D3 measurement ${index + 1}: Significant deviation (${deviation.toFixed(1)}%)`);
          }
        }
      }
    });

    // Decision logic
    let decision: 'repair' | 'replace' | 'reject';
    let confidence: number;
    let summary: string;

    if (criticalDeviations > 0 || outsideTolerance > 10) {
      decision = 'reject';
      confidence = 95;
      summary = `Component should be REJECTED due to ${criticalDeviations} critical deviations and ${outsideTolerance} measurements outside tolerance.`;
      reasons.unshift('Critical safety dimensions compromised');
    } else if (outsideTolerance > 3 && outsideTolerance <= 10) {
      decision = 'replace';
      confidence = 85;
      summary = `Component should be REPLACED due to ${outsideTolerance} measurements outside tolerance limits.`;
      reasons.unshift('Significant wear detected, replacement recommended');
    } else if (outsideTolerance > 0 && outsideTolerance <= 3) {
      decision = 'repair';
      confidence = 80;
      summary = `Component can be REPAIRED with ${outsideTolerance} minor deviations within repair limits.`;
      reasons.unshift('Minor deviations within repairable limits');
    } else {
      decision = 'repair';
      confidence = 90;
      summary = 'All measurements within tolerance. Component is in good condition.';
      reasons.unshift('All dimensions within acceptable limits');
    }

    return {
      decision,
      confidence,
      reasons,
      summary,
      measurements: {
        total: totalMeasurements,
        withinTolerance,
        outsideTolerance,
        criticalDeviations
      }
    };
  }
}