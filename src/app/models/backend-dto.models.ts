// DTO Models matching the backend API

export interface InspectionFormDto {
  id: number;
  formId: string;
  revision: string;
  jobNumber: string;
  customer: string;
  frame: string;
  component: string;
  partNumber: string;
  quantity: number;
  operator: string;
  inspectionDate: Date;
  instrumentId: string;
  calibrationDueDate: Date;
  inspectedBy: string;
  reviewedBy: string;
  status: InspectionStatus;
  createdAt: Date;
  updatedAt: Date;
  bladeMeasurements: BladeMeasurementDto[];
  inspectionAnalysis?: InspectionAnalysisDto;
}

export interface CreateInspectionFormDto {
  formId: string;
  revision: string;
  jobNumber: string;
  customer: string;
  frame: string;
  component: string;
  partNumber: string;
  quantity: number;
  operator: string;
  inspectionDate: Date;
  instrumentId: string;
  calibrationDueDate: Date;
  inspectedBy: string;
  reviewedBy: string;
  bladeMeasurements: CreateBladeMeasurementDto[];
}

export interface UpdateInspectionFormDto {
  jobNumber?: string;
  customer?: string;
  frame?: string;
  component?: string;
  partNumber?: string;
  quantity?: number;
  operator?: string;
  inspectionDate?: Date;
  instrumentId?: string;
  calibrationDueDate?: Date;
  inspectedBy?: string;
  reviewedBy?: string;
  status?: InspectionStatus;
}

export interface BladeMeasurementDto {
  id: number;
  inspectionFormId: number;
  bladeNumber: number;
  partNumber?: string;
  serialNumber?: string;
  passFail: PassFailStatus;
  d1?: number;
  d2?: number;
  d3?: number;
  d4?: number;
  d5?: number;
  d6?: number;
  d7?: number;
  d8?: number;
  t1?: number;
  t2?: number;
  t3?: number;
  t4?: number;
  t5?: number;
  e1?: number;
  e2?: number;
  e3?: number;
  createdAt: Date;
  updatedAt: Date;
  measurementDeviations: MeasurementDeviationDto[];
}

export interface CreateBladeMeasurementDto {
  bladeNumber: number;
  partNumber?: string;
  serialNumber?: string;
  passFail: PassFailStatus;
  d1?: number;
  d2?: number;
  d3?: number;
  d4?: number;
  d5?: number;
  d6?: number;
  d7?: number;
  d8?: number;
  t1?: number;
  t2?: number;
  t3?: number;
  t4?: number;
  t5?: number;
  e1?: number;
  e2?: number;
  e3?: number;
}

export interface InspectionAnalysisDto {
  id: number;
  inspectionFormId: number;
  decision: InspectionDecision;
  confidence: number;
  summary: string;
  totalMeasurements: number;
  withinTolerance: number;
  outsideTolerance: number;
  criticalDeviations: number;
  reasons: string[];
  averageDeviation?: number;
  maxDeviation?: number;
  standardDeviation?: number;
  createdAt: Date;
  updatedAt: Date;
}

export interface MeasurementDeviationDto {
  id: number;
  bladeMeasurementId: number;
  measurementType: string;
  actualValue: number;
  nominalValue: number;
  minTolerance: number;
  maxTolerance: number;
  deviationValue: number;
  deviationPercentage: number;
  isWithinTolerance: boolean;
  isCritical: boolean;
  createdAt: Date;
}

// Enums
export enum InspectionStatus {
  Draft = 0,
  Submitted = 1,
  InReview = 2,
  Approved = 3,
  Rejected = 4,
  Completed = 5
}

export enum PassFailStatus {
  NotSet = 0,
  Pass = 1,
  Fail = 2
}

export enum InspectionDecision {
  NotSet = 0,
  Okay = 1,
  Repair = 2,
  Replace = 3,
  Reject = 4
}

// API Response wrapper
export interface ApiResponse<T> {
  success: boolean;
  data: T;
  message?: string;
  errors?: string[];
}

// Pagination
export interface PaginatedResponse<T> {
  items: T[];
  totalCount: number;
  page: number;
  pageSize: number;
  totalPages: number;
}