import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { EnhancedInspectionService } from '../../services/enhanced-inspection.service';

@Component({
  selector: 'app-inspection-history',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="history-container">
      <div class="history-header">
        <h2>Inspection History</h2>
        <div class="filters">
          <input 
            type="text" 
            [(ngModel)]="filters.jobNumber" 
            placeholder="Job Number"
            (input)="onFilterChange()">
          <input 
            type="text" 
            [(ngModel)]="filters.customer" 
            placeholder="Customer"
            (input)="onFilterChange()">
          <select [(ngModel)]="filters.status" (change)="onFilterChange()">
            <option value="">All Status</option>
            <option value="Draft">Draft</option>
            <option value="Submitted">Submitted</option>
            <option value="Approved">Approved</option>
            <option value="Completed">Completed</option>
          </select>
        </div>
      </div>

      <div class="history-table" *ngIf="!loading">
        <table>
          <thead>
            <tr>
              <th>Job Number</th>
              <th>Customer</th>
              <th>Part Number</th>
              <th>Status</th>
              <th>Decision</th>
              <th>Inspection Date</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            <tr *ngFor="let inspection of inspections">
              <td>{{ inspection.jobNumber }}</td>
              <td>{{ inspection.customer }}</td>
              <td>{{ inspection.partNumber }}</td>
              <td>
                <span class="status-badge" [ngClass]="getStatusClass(inspection.status)">
                  {{ inspection.status }}
                </span>
              </td>
              <td>
                <span class="decision-badge" [ngClass]="getDecisionClass(inspection.decision)">
                  {{ inspection.decision }}
                </span>
              </td>
              <td>{{ inspection.inspectionDate | date:'short' }}</td>
              <td>
                <button class="btn btn-sm" (click)="viewDetails(inspection.id)">View</button>
                <button class="btn btn-sm" (click)="reAnalyze(inspection.id)">Re-analyze</button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <div *ngIf="loading" class="loading">
        Loading inspections...
      </div>

      <div *ngIf="!loading && inspections.length === 0" class="no-data">
        No inspections found.
      </div>
    </div>
  `,
  styles: [`
    .history-container {
      max-width: 1200px;
      margin: 0 auto;
      padding: 20px;
    }

    .history-header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      margin-bottom: 20px;
    }

    .filters {
      display: flex;
      gap: 10px;
    }

    .filters input, .filters select {
      padding: 8px 12px;
      border: 1px solid #ddd;
      border-radius: 4px;
    }

    .history-table {
      background: white;
      border-radius: 8px;
      overflow: hidden;
      box-shadow: 0 2px 10px rgba(0,0,0,0.1);
    }

    table {
      width: 100%;
      border-collapse: collapse;
    }

    th, td {
      padding: 12px;
      text-align: left;
      border-bottom: 1px solid #eee;
    }

    th {
      background: #f8f9fa;
      font-weight: 600;
    }

    .status-badge, .decision-badge {
      padding: 4px 8px;
      border-radius: 12px;
      font-size: 0.8em;
      font-weight: 600;
    }

    .status-draft { background: #ffeaa7; color: #2d3436; }
    .status-submitted { background: #74b9ff; color: white; }
    .status-approved { background: #00b894; color: white; }
    .status-completed { background: #6c5ce7; color: white; }

    .decision-repair { background: #00b894; color: white; }
    .decision-replace { background: #fdcb6e; color: #2d3436; }
    .decision-reject { background: #e17055; color: white; }

    .btn {
      padding: 6px 12px;
      border: none;
      border-radius: 4px;
      cursor: pointer;
      margin-right: 5px;
      background: #3498db;
      color: white;
    }

    .btn:hover {
      background: #2980b9;
    }

    .loading, .no-data {
      text-align: center;
      padding: 40px;
      color: #666;
    }
  `]
})
export class InspectionHistoryComponent implements OnInit {
  inspections: any[] = [];
  loading = false;
  filters = {
    jobNumber: '',
    customer: '',
    status: '',
    page: 1,
    pageSize: 20
  };

  constructor(private enhancedInspectionService: EnhancedInspectionService) {}

  ngOnInit() {
    this.loadInspections();
  }

  loadInspections() {
    this.loading = true;
    this.enhancedInspectionService.getInspectionHistory(this.filters).subscribe({
      next: (inspections) => {
        this.inspections = inspections;
        this.loading = false;
      },
      error: (error) => {
        console.error('Failed to load inspections:', error);
        this.loading = false;
      }
    });
  }

  onFilterChange() {
    this.filters.page = 1;
    this.loadInspections();
  }

  getStatusClass(status: string): string {
    return `status-${status.toLowerCase().replace(' ', '-')}`;
  }

  getDecisionClass(decision: string): string {
    return `decision-${decision.toLowerCase()}`;
  }

  viewDetails(id: number) {
    // Navigate to details view or open modal
    console.log('View details for inspection:', id);
  }

  reAnalyze(id: number) {
    this.enhancedInspectionService.reAnalyzeInspection(id).subscribe({
      next: (result) => {
        console.log('Re-analysis result:', result);
        alert('Re-analysis completed successfully!');
        this.loadInspections();
      },
      error: (error) => {
        console.error('Re-analysis failed:', error);
        alert('Re-analysis failed: ' + error.message);
      }
    });
  }
}