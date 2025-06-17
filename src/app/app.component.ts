import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DynamicFormComponent } from './dynamic-form/dynamic-form.component';
import { InspectionResultsComponent } from './inspection-results/inspection-results.component';
import { EnhancedInspectionService } from './services/enhanced-inspection.service';
import { FormConfig, FormData, InspectionResult } from './models/form.models';
import formConfigData from './form-new-config.json';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, DynamicFormComponent, InspectionResultsComponent],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'BHEL Dynamic Forms';
  formConfig!: FormConfig;
  inspectionResult: InspectionResult | null = null;
  showResults = false;
  isSubmitting = false;
  submitError: string | null = null;

  constructor(private enhancedInspectionService: EnhancedInspectionService) {}

  ngOnInit() {
    this.formConfig = formConfigData as FormConfig;
  }

  onFormSubmit(formData: FormData) {
    console.log('Form submitted:', formData);
    
    // Validate form data
    const validation = this.enhancedInspectionService.validateFormData(formData);
    if (!validation.isValid) {
      alert('Please fix the following errors:\n' + validation.errors.join('\n'));
      return;
    }

    this.isSubmitting = true;
    this.submitError = null;
    
    // Submit to backend with fallback to local analysis
    this.enhancedInspectionService.submitInspection(formData, this.formConfig).subscribe({
      next: (result) => {
        console.log('Analysis result:', result);
        this.inspectionResult = result;
        this.showResults = true;
        this.isSubmitting = false;
        
        // Scroll to results
        setTimeout(() => {
          const resultsElement = document.getElementById('inspection-results');
          if (resultsElement) {
            resultsElement.scrollIntoView({ behavior: 'smooth' });
          }
        }, 100);
      },
      error: (error) => {
        console.error('Submission failed:', error);
        this.submitError = error.message || 'Failed to submit inspection. Please try again.';
        this.isSubmitting = false;
        
        // Show error to user
        alert(this.submitError);
      }
    });
  }

  resetForm() {
    this.showResults = false;
    this.inspectionResult = null;
    this.submitError = null;
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }
}