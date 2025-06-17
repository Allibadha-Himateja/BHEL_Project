# Angular Frontend Test Cases

## 1. Form Rendering Tests

### Test Case 1: Dynamic Form Generation
**Objective**: Verify that the dynamic form renders correctly based on JSON configuration

**Steps**:
1. Open the Angular application
2. Verify that the form header displays "BLADE ALSEAL COATING THICKNESS INSPECTION"
3. Check that all sections are rendered:
   - Basic Information
   - Coating Thickness Measurements
   - Verification
4. Verify that all fields in Basic Information section are present:
   - Job #, Customer, Frame, Component, Part No., Quantity, Operator, Date, Instrument ID#, Calibration Due Date

**Expected Result**: Form renders with all sections and fields as defined in JSON config

### Test Case 2: Table Generation
**Objective**: Verify that the measurements table is generated correctly

**Steps**:
1. Navigate to "Coating Thickness Measurements" section
2. Verify table has 90 rows (for 90 blades)
3. Check that all measurement columns are present (D1-D8, T1-T5, E1-E3)
4. Verify that Blade # column is auto-populated with numbers 1-90
5. Check that Pass/Fail dropdown has correct options

**Expected Result**: Table displays with correct structure and pre-populated data

## 2. Form Validation Tests

### Test Case 3: Required Field Validation
**Objective**: Test that required fields are properly validated

**Steps**:
1. Try to submit form without filling required fields
2. Verify validation messages appear
3. Fill one required field at a time and check validation updates

**Expected Result**: Form shows validation errors and prevents submission until all required fields are filled

### Test Case 4: Measurement Value Validation
**Objective**: Test measurement field validation

**Steps**:
1. Enter invalid values in measurement fields:
   - Negative numbers
   - Values > 0.100
   - Non-numeric values
2. Verify validation prevents invalid entries
3. Enter valid values (0.000 - 0.100) and verify acceptance

**Expected Result**: Only valid measurement values are accepted

### Test Case 5: Date Validation
**Objective**: Test date field validation

**Steps**:
1. Enter future dates beyond 30 days
2. Enter dates before 2020
3. Enter valid dates within range

**Expected Result**: Only valid dates within specified range are accepted

## 3. Backend Integration Tests

### Test Case 6: Successful Form Submission
**Objective**: Test successful submission to backend

**Prerequisites**: Backend API is running and accessible

**Steps**:
1. Fill out complete form with valid data:
   ```
   Job #: TEST-JOB-001
   Customer: Test Customer
   Frame: FR9-TEST
   Component: Stage 1 Shroud
   Part No.: PART-001
   Quantity: 90
   Operator: Test Operator
   Date: [Current Date]
   Instrument ID#: INST-001
   Calibration Due Date: [Future Date]
   Inspected By: Test Inspector
   Reviewed By: Test Reviewer
   ```
2. Add measurement data for at least 3 blades:
   ```
   Blade 1: All measurements between 0.045-0.055, Pass/Fail: Pass
   Blade 2: Some measurements outside tolerance (0.035-0.039), Pass/Fail: Fail
   Blade 3: All measurements within tolerance (0.048-0.052), Pass/Fail: Pass
   ```
3. Submit the form
4. Verify success message and results display

**Expected Result**: Form submits successfully, analysis results are displayed

### Test Case 7: Backend Failure Fallback
**Objective**: Test fallback to local analysis when backend is unavailable

**Prerequisites**: Backend API is stopped or unreachable

**Steps**:
1. Fill out complete form with valid data
2. Submit the form
3. Verify that local analysis is performed
4. Check that results are still displayed

**Expected Result**: Form submission continues with local analysis, user sees results

### Test Case 8: Network Error Handling
**Objective**: Test handling of network errors

**Prerequisites**: Simulate network issues (disconnect internet or block API calls)

**Steps**:
1. Fill out and submit form
2. Verify error handling
3. Check that appropriate error message is shown

**Expected Result**: User-friendly error message is displayed, form doesn't crash

## 4. Analysis Results Tests

### Test Case 9: Repair Decision Analysis
**Objective**: Test analysis logic for repair decision

**Test Data**:
```
Blade 1: D1=0.045, D2=0.048, D3=0.052, T1=0.047, E1=0.049 (all within tolerance)
Blade 2: D1=0.038, D2=0.042, D3=0.055 (2 measurements outside tolerance but within repair limits)
Blade 3: All measurements within tolerance
```

**Steps**:
1. Enter the test data
2. Submit form
3. Verify analysis results

**Expected Result**: Decision = "REPAIR", Confidence ≥ 80%, appropriate reasons listed

### Test Case 10: Replace Decision Analysis
**Objective**: Test analysis logic for replace decision

**Test Data**:
```
Multiple blades with 4-10 measurements outside tolerance
Some measurements near repair limits
No critical deviations
```

**Steps**:
1. Enter test data with significant wear
2. Submit form
3. Verify analysis results

**Expected Result**: Decision = "REPLACE", Confidence ≥ 75%, appropriate reasons listed

### Test Case 11: Reject Decision Analysis
**Objective**: Test analysis logic for reject decision

**Test Data**:
```
Blade 1: D1=0.025, D2=0.028 (below repair limits)
Blade 2: Multiple measurements outside tolerance
More than 10 measurements total outside tolerance
```

**Steps**:
1. Enter test data with critical deviations
2. Submit form
3. Verify analysis results

**Expected Result**: Decision = "REJECT", Confidence ≥ 90%, critical issues highlighted

## 5. User Interface Tests

### Test Case 12: Responsive Design
**Objective**: Test form responsiveness on different screen sizes

**Steps**:
1. Test on desktop (1920x1080)
2. Test on tablet (768x1024)
3. Test on mobile (375x667)
4. Verify table scrolling and form layout

**Expected Result**: Form is usable and properly formatted on all screen sizes

### Test Case 13: Form Reset Functionality
**Objective**: Test form reset after viewing results

**Steps**:
1. Submit a form and view results
2. Click "Back to Form" button
3. Verify form is reset to initial state

**Expected Result**: Form returns to empty state, ready for new inspection

### Test Case 14: Print Functionality
**Objective**: Test print report functionality

**Steps**:
1. Complete form submission and view results
2. Click "Print Report" button
3. Verify print preview shows properly formatted report

**Expected Result**: Print preview displays complete inspection report

## 6. Data Persistence Tests

### Test Case 15: Form Data Retention
**Objective**: Test that form data is retained during session

**Steps**:
1. Fill out partial form data
2. Navigate away from form (if applicable)
3. Return to form
4. Verify data is still present

**Expected Result**: Form data is retained during browser session

### Test Case 16: Local Storage Handling
**Objective**: Test local storage usage (if implemented)

**Steps**:
1. Fill out form
2. Refresh browser
3. Check if any data is retained

**Expected Result**: Behavior matches application design (data retained or cleared as intended)

## 7. Error Handling Tests

### Test Case 17: Invalid JSON Configuration
**Objective**: Test handling of corrupted form configuration

**Steps**:
1. Modify form-new-config.json with invalid JSON
2. Reload application
3. Verify error handling

**Expected Result**: Application shows appropriate error message, doesn't crash

### Test Case 18: Missing Form Fields
**Objective**: Test handling of missing field definitions

**Steps**:
1. Remove required field from JSON configuration
2. Test form rendering and submission

**Expected Result**: Application handles missing fields gracefully

## 8. Performance Tests

### Test Case 19: Large Dataset Handling
**Objective**: Test form performance with maximum data

**Steps**:
1. Fill out all 90 blade measurements
2. Enter data in all measurement fields
3. Submit form and measure response time

**Expected Result**: Form remains responsive, submission completes within acceptable time

### Test Case 20: Memory Usage
**Objective**: Test for memory leaks

**Steps**:
1. Submit multiple forms in sequence
2. Monitor browser memory usage
3. Check for memory leaks

**Expected Result**: Memory usage remains stable, no significant leaks

## 9. Integration Test Scenarios

### Test Case 21: End-to-End Workflow
**Objective**: Test complete inspection workflow

**Steps**:
1. Create new inspection with valid data
2. Submit and verify backend storage
3. Retrieve inspection from backend
4. Verify data integrity
5. Perform re-analysis
6. Update inspection status

**Expected Result**: Complete workflow functions correctly

### Test Case 22: Concurrent User Testing
**Objective**: Test multiple users using system simultaneously

**Steps**:
1. Open application in multiple browser tabs/windows
2. Submit different inspections simultaneously
3. Verify no data corruption or conflicts

**Expected Result**: All submissions process correctly without interference

## 10. Accessibility Tests

### Test Case 23: Keyboard Navigation
**Objective**: Test form accessibility via keyboard

**Steps**:
1. Navigate entire form using only keyboard (Tab, Enter, Arrow keys)
2. Verify all fields are accessible
3. Test form submission via keyboard

**Expected Result**: All form elements are keyboard accessible

### Test Case 24: Screen Reader Compatibility
**Objective**: Test form with screen reader software

**Steps**:
1. Use screen reader to navigate form
2. Verify labels are properly read
3. Test error message accessibility

**Expected Result**: Form is fully accessible to screen readers