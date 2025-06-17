// src/app/dynamic-form/dynamic-form.component.ts

import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormArray, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { FormConfig, FormData, FormField, TableDataColumn, TableColumn } from '../models/form.models';

@Component({
  selector: 'app-dynamic-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './dynamic-form.component.html',
  styleUrls: ['./dynamic-form.component.scss']
})
export class DynamicFormComponent implements OnInit {
  @Input() config!: FormConfig;
  @Output() formSubmit = new EventEmitter<FormData>();
  
  dynamicForm!: FormGroup;

  constructor(private fb: FormBuilder) {}

  ngOnInit() {
    if (this.config) {
      this.buildForm();
    }
  }

  buildForm() {
    const formControls: { [key: string]: any } = {};

    this.config.sections.forEach(section => {
      section.fields.forEach(field => {
        
        if (field.type === 'table' && field.columns) {
          formControls[field.id] = this.fb.array(
            Array.from({ length: field.rows || 1 }, (_, i) => {
              const rowControls: { [key: string]: any } = {};
              field.columns!.forEach(column => {
                rowControls[column.id] = this.fb.control(column.id === 'serialNo' ? i + 1 : '');
              });
              return this.fb.group(rowControls);
            })
          );
        } else if (field.type === 'grouped-table' && field.dataColumns) {
            formControls[field.id] = this.fb.array(
            // CORRECTED: Added a fallback empty array to prevent type errors
            Array.from({ length: field.rows || 1 }, (_, rowIndex) => this.createTableRow(field.dataColumns || [], rowIndex))
          );
        } else {
          formControls[field.id] = this.createFormControl(field);
        }
      });
    });

    this.dynamicForm = this.fb.group(formControls);
  }

  createTableRow(columns: TableDataColumn[], rowIndex: number): FormGroup {
    const rowControls: { [key: string]: any } = {};
    columns.forEach(column => {
      let initialValue: any = '';
      if (column.readonly && column.id === 'bladeNo') { initialValue = rowIndex + 1; } 
      else if (column.type === 'checkbox') { initialValue = false; }
      rowControls[column.id] = this.fb.control(initialValue);
    });
    return this.fb.group(rowControls);
  }

  createFormControl(field: FormField) {
    let initialValue: any = '';
    // CORRECTED: Removed check for 'select' to resolve the type error. `dropdown` is what you use.
    if (field.type === 'checkbox') { initialValue = false; }
    else if (field.type === 'dropdown' && field.options) {
      initialValue = field.options[0]?.value || '';
    }

    const validators = field.required ? [Validators.required] : [];
    const control = this.fb.control(initialValue, validators);
    if(field.readonly) { control.disable(); }
    return control;
  }
  
  getFormArray(fieldId: string): FormArray {
    return this.dynamicForm.get(fieldId) as FormArray;
  }

  onSubmit() {
    if (this.dynamicForm.valid) {
      this.formSubmit.emit(this.dynamicForm.getRawValue());
    } else {
      this.markFormGroupTouched(this.dynamicForm);
    }
  }

  private markFormGroupTouched(formGroup: FormGroup | FormArray) {
    Object.values(formGroup.controls).forEach(control => {
      control.markAsTouched();
      if (control instanceof FormGroup || control instanceof FormArray) { this.markFormGroupTouched(control); }
    });
  }

  isFieldRequired(field: FormField): boolean { return !!field.required; }
  
  getFieldError(fieldId: string): string | null {
    const control = this.dynamicForm.get(fieldId);
    if (control?.errors && control.touched) {
      if (control.errors['required']) { return 'This field is required'; }
    }
    return null;
  }
  
  onImageError(event: any): void { console.error('Image failed to load:', event.target.src); }
  onImageLoad(event: any): void { console.log('Image loaded successfully:', event.target.src); }
}