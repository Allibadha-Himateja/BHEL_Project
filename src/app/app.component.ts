import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DynamicFormComponent } from './dynamic-form/dynamic-form.component';
import { InspectionResultsComponent } from './inspection-results/inspection-results.component';
import { InspectionService } from './services/inspection.service';
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

  constructor(private inspectionService: InspectionService) {}

  ngOnInit() {
    this.formConfig = formConfigData as FormConfig;
  }

  onFormSubmit(formData: FormData) {
    console.log('Form submitted:', formData);
    
    // Analyze the inspection data
    this.inspectionResult = this.inspectionService.analyzeInspection(formData, this.formConfig);
    this.showResults = true;
    
    // Scroll to results
    setTimeout(() => {
      const resultsElement = document.getElementById('inspection-results');
      if (resultsElement) {
        resultsElement.scrollIntoView({ behavior: 'smooth' });
      }
    }, 100);
  }

  resetForm() {
    this.showResults = false;
    this.inspectionResult = null;
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }
}

