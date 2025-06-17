# Backend API Test Cases

## 1. Inspection Forms Controller Tests

### 1.1 GET /api/inspectionforms - Get All Inspections

**Test Case 1: Get all inspections without filters**
```
Method: GET
URL: https://localhost:7000/api/inspectionforms
Headers: 
  Content-Type: application/json
  Accept: application/json

Expected Response: 200 OK
Response Body: Array of InspectionFormDto objects
```

**Test Case 2: Get inspections with pagination**
```
Method: GET
URL: https://localhost:7000/api/inspectionforms?page=1&pageSize=5
Headers: 
  Content-Type: application/json

Expected Response: 200 OK
Headers should include:
  X-Total-Count: [number]
  X-Page: 1
  X-Page-Size: 5
```

**Test Case 3: Get inspections with filters**
```
Method: GET
URL: https://localhost:7000/api/inspectionforms?jobNumber=JOB-001&customer=BHEL&status=1
Headers: 
  Content-Type: application/json

Expected Response: 200 OK
Response Body: Filtered array of inspections
```

### 1.2 GET /api/inspectionforms/{id} - Get Specific Inspection

**Test Case 4: Get existing inspection**
```
Method: GET
URL: https://localhost:7000/api/inspectionforms/1
Headers: 
  Content-Type: application/json

Expected Response: 200 OK
Response Body: Complete InspectionFormDto with measurements and analysis
```

**Test Case 5: Get non-existing inspection**
```
Method: GET
URL: https://localhost:7000/api/inspectionforms/999999
Headers: 
  Content-Type: application/json

Expected Response: 404 Not Found
```

### 1.3 POST /api/inspectionforms - Create New Inspection

**Test Case 6: Create valid inspection**
```json
Method: POST
URL: https://localhost:7000/api/inspectionforms
Headers: 
  Content-Type: application/json
  Accept: application/json

Body:
{
  "formId": "WIF-BKT-08",
  "revision": "Rev1.0",
  "jobNumber": "JOB-TEST-001",
  "customer": "BHEL Test Customer",
  "frame": "FR9-TEST",
  "component": "Stage 1 Shroud",
  "partNumber": "PART-TEST-001",
  "quantity": 90,
  "operator": "John Doe",
  "inspectionDate": "2024-01-15T00:00:00Z",
  "instrumentId": "INST-001",
  "calibrationDueDate": "2024-12-31T00:00:00Z",
  "inspectedBy": "Inspector Smith",
  "reviewedBy": "Reviewer Jones",
  "bladeMeasurements": [
    {
      "bladeNumber": 1,
      "partNumber": "BLADE-001",
      "serialNumber": "SN-001",
      "passFail": 1,
      "d1": 0.045,
      "d2": 0.048,
      "d3": 0.052,
      "d4": 0.047,
      "d5": 0.049,
      "d6": 0.051,
      "d7": 0.046,
      "d8": 0.050,
      "t1": 0.048,
      "t2": 0.049,
      "t3": 0.047,
      "t4": 0.051,
      "t5": 0.048,
      "e1": 0.049,
      "e2": 0.050,
      "e3": 0.048
    },
    {
      "bladeNumber": 2,
      "partNumber": "BLADE-002",
      "serialNumber": "SN-002",
      "passFail": 2,
      "d1": 0.035,
      "d2": 0.038,
      "d3": 0.032,
      "d4": 0.037,
      "d5": 0.039,
      "d6": 0.031,
      "d7": 0.036,
      "d8": 0.030,
      "t1": 0.038,
      "t2": 0.039,
      "t3": 0.037,
      "t4": 0.031,
      "t5": 0.038,
      "e1": 0.039,
      "e2": 0.030,
      "e3": 0.038
    }
  ]
}

Expected Response: 201 Created
Response Body: Created InspectionFormDto with ID and analysis results
```

**Test Case 7: Create inspection with missing required fields**
```json
Method: POST
URL: https://localhost:7000/api/inspectionforms
Headers: 
  Content-Type: application/json

Body:
{
  "formId": "WIF-BKT-08",
  "revision": "Rev1.0"
  // Missing required fields
}

Expected Response: 400 Bad Request
Response Body: Validation errors
```

**Test Case 8: Create inspection with invalid measurements**
```json
Method: POST
URL: https://localhost:7000/api/inspectionforms
Headers: 
  Content-Type: application/json

Body:
{
  "formId": "WIF-BKT-08",
  "revision": "Rev1.0",
  "jobNumber": "JOB-TEST-002",
  "customer": "BHEL Test Customer",
  "frame": "FR9-TEST",
  "component": "Stage 1 Shroud",
  "partNumber": "PART-TEST-002",
  "quantity": 90,
  "operator": "John Doe",
  "inspectionDate": "2024-01-15T00:00:00Z",
  "instrumentId": "INST-001",
  "calibrationDueDate": "2024-12-31T00:00:00Z",
  "inspectedBy": "Inspector Smith",
  "reviewedBy": "Reviewer Jones",
  "bladeMeasurements": [
    {
      "bladeNumber": 1,
      "partNumber": "BLADE-001",
      "serialNumber": "SN-001",
      "passFail": 1,
      "d1": -0.001,  // Invalid negative value
      "d2": 0.150    // Invalid too high value
    }
  ]
}

Expected Response: 400 Bad Request
Response Body: Validation errors for measurements
```

### 1.4 PUT /api/inspectionforms/{id} - Update Inspection

**Test Case 9: Update existing inspection**
```json
Method: PUT
URL: https://localhost:7000/api/inspectionforms/1
Headers: 
  Content-Type: application/json

Body:
{
  "jobNumber": "JOB-UPDATED-001",
  "customer": "Updated Customer Name",
  "status": 1
}

Expected Response: 204 No Content
```

**Test Case 10: Update non-existing inspection**
```json
Method: PUT
URL: https://localhost:7000/api/inspectionforms/999999
Headers: 
  Content-Type: application/json

Body:
{
  "jobNumber": "JOB-UPDATED-001"
}

Expected Response: 404 Not Found
```

### 1.5 DELETE /api/inspectionforms/{id} - Delete Inspection

**Test Case 11: Delete existing inspection**
```
Method: DELETE
URL: https://localhost:7000/api/inspectionforms/1
Headers: 
  Content-Type: application/json

Expected Response: 204 No Content
```

**Test Case 12: Delete non-existing inspection**
```
Method: DELETE
URL: https://localhost:7000/api/inspectionforms/999999
Headers: 
  Content-Type: application/json

Expected Response: 404 Not Found
```

### 1.6 POST /api/inspectionforms/{id}/analyze - Analyze Inspection

**Test Case 13: Analyze existing inspection**
```json
Method: POST
URL: https://localhost:7000/api/inspectionforms/1/analyze
Headers: 
  Content-Type: application/json

Body: {}

Expected Response: 200 OK
Response Body: InspectionAnalysisDto with decision, confidence, and details
```

**Test Case 14: Analyze non-existing inspection**
```json
Method: POST
URL: https://localhost:7000/api/inspectionforms/999999/analyze
Headers: 
  Content-Type: application/json

Body: {}

Expected Response: 404 Not Found
```

### 1.7 POST /api/inspectionforms/{id}/submit - Submit Inspection

**Test Case 15: Submit draft inspection**
```json
Method: POST
URL: https://localhost:7000/api/inspectionforms/1/submit
Headers: 
  Content-Type: application/json

Body: {}

Expected Response: 204 No Content
```

**Test Case 16: Submit already submitted inspection**
```json
Method: POST
URL: https://localhost:7000/api/inspectionforms/1/submit
Headers: 
  Content-Type: application/json

Body: {}

Expected Response: 400 Bad Request
Response Body: Error message about inspection status
```

## 2. Edge Cases and Error Scenarios

**Test Case 17: Large payload test**
```json
Method: POST
URL: https://localhost:7000/api/inspectionforms
Headers: 
  Content-Type: application/json

Body: Create inspection with 90 blade measurements (maximum)

Expected Response: 201 Created
```

**Test Case 18: Concurrent access test**
```
1. Create an inspection
2. Simultaneously update the same inspection from two different requests
3. Verify proper concurrency handling

Expected: One request succeeds, other gets conflict error
```

**Test Case 19: Database connection failure simulation**
```
1. Stop database service
2. Try to create inspection
3. Verify proper error handling

Expected Response: 500 Internal Server Error with appropriate message
```

## 3. Performance Test Cases

**Test Case 20: Load test - Multiple inspections**
```
Create 100 inspections simultaneously
Measure response times and success rates

Expected: All requests complete within acceptable time limits
```

**Test Case 21: Large dataset pagination**
```
Create 1000+ inspections
Test pagination with various page sizes
Verify performance remains acceptable

Expected: Consistent response times regardless of dataset size
```