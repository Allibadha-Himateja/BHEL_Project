// Backend Models Interface Definitions
export interface InspectionForm {
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
    createdAt: Date;
    updatedAt: Date;
    status: InspectionStatus;
    measurements: BladeMeasurement[];
    analysisResult: InspectionAnalysis;
}

export interface BladeMeasurement {
    id: number;
    inspectionFormId: number;
    bladeNumber: number;
    partNumber: string;
    serialNumber: string;
    passFail: string;
    d1: number;
    d2: number;
    d3: number;
    d4: number;
    d5: number;
    d6: number;
    d7: number;
    d8: number;
    t1: number;
    t2: number;
    t3: number;
    t4: number;
    t5: number;
    e1: number;
    e2: number;
    e3: number;
    createdAt: Date;
    updatedAt: Date;
}

export interface InspectionAnalysis {
    id: number;
    inspectionFormId: number;
    decision: InspectionDecision;
    confidence: number;
    summary: string;
    totalMeasurements: number;
    withinTolerance: number;
    outsideTolerance: number;
    criticalDeviations: number;
    reasons: string;
    createdAt: Date;
    updatedAt: Date;
}

export enum InspectionStatus {
    Draft = 'Draft',
    Submitted = 'Submitted',
    Reviewed = 'Reviewed',
    Approved = 'Approved',
    Rejected = 'Rejected'
}

export enum InspectionDecision {
    Okay = 'Okay',
    Repair = 'Repair',
    Replace = 'Replace',
    Reject = 'Reject'
}

// API Response Types
export interface ApiResponse<T> {
    success: boolean;
    message: string;
    data?: T;
    errors?: string[];
}

export interface PaginatedResponse<T> {
    items: T[];
    totalCount: number;
    pageNumber: number;
    pageSize: number;
    totalPages: number;
}

// Request Types
export interface CreateInspectionRequest {
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
    measurements: Omit<BladeMeasurement, 'id' | 'inspectionFormId' | 'createdAt' | 'updatedAt'>[];
}

export interface UpdateInspectionRequest extends Partial<CreateInspectionRequest> {
    id: number;
    status?: InspectionStatus;
}

export interface InspectionFilter {
    jobNumber?: string;
    customer?: string;
    partNumber?: string;
    status?: InspectionStatus;
    decision?: InspectionDecision;
    startDate?: Date;
    endDate?: Date;
    pageNumber?: number;
    pageSize?: number;
} 