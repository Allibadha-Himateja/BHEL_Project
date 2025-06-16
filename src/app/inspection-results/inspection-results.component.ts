import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { InspectionResult } from '../models/form.models';

@Component({
  selector: 'app-inspection-results',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './inspection-results.component.html',
  styleUrls: ['./inspection-results.component.scss']
})
export class InspectionResultsComponent {
  @Input() result!: InspectionResult;

  getDecisionClass(): string {
    switch (this.result.decision) {
      case 'repair': return 'decision-repair';
      case 'replace': return 'decision-replace';
      case 'reject': return 'decision-reject';
      default: return '';
    }
  }

  getDecisionIcon(): string {
    switch (this.result.decision) {
      case 'repair': return '🔧';
      case 'replace': return '🔄';
      case 'reject': return '❌';
      default: return '';
    }
  }

  getConfidenceColor(): string {
    if (this.result.confidence >= 90) return '#27ae60';
    if (this.result.confidence >= 80) return '#f39c12';
    return '#e74c3c';
  }
}

