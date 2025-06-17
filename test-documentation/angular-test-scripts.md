# Angular Application Test Scripts

## 1. Manual Testing Scripts for Angular Application

### Script 1: Basic Form Functionality Test
```javascript
// Open browser console and run these commands to test form functionality

// Test 1: Check if form is properly initialized
console.log('Form component loaded:', !!document.querySelector('app-dynamic-form'));
console.log('Form sections count:', document.querySelectorAll('.form-section').length);

// Test 2: Check table generation
console.log('Measurement table rows:', document.querySelectorAll('.measurements-table tbody tr').length);
console.log('Measurement columns:', document.querySelectorAll('.measurements-table thead th').length);

// Test 3: Fill form programmatically for testing
function fillTestData() {
  // Fill basic information
  document.querySelector('#jobNumber').value = 'TEST-JOB-001';
  document.querySelector('#customer').value = 'Test Customer';
  document.querySelector('#frame').value = 'FR9-TEST';
  document.querySelector('#component').value = 'Stage 1 Shroud';
  document.querySelector('#partNumber').value = 'PART-001';
  document.querySelector('#quantity').value = '90';
  document.querySelector('#operator').value = 'Test Operator';
  document.querySelector('#date').value = '2024-01-15';
  document.querySelector('#instrumentId').value = 'INST-001';
  document.querySelector('#calibrationDueDate').value = '2024-12-31';
  document.querySelector('#inspectedBy').value = 'Test Inspector';
  document.querySelector('#reviewedBy').value = 'Test Reviewer';
  
  // Trigger change events
  document.querySelectorAll('input, select').forEach(input => {
    input.dispatchEvent(new Event('input', { bubbles: true }));
    input.dispatchEvent(new Event('change', { bubbles: true }));
  });
  
  console.log('Test data filled');
}

// Test 4: Fill measurement data
function fillMeasurementData() {
  // Fill first 3 rows with test data
  const rows = document.querySelectorAll('.measurements-table tbody tr');
  
  // Row 1 - Good measurements
  const row1 = rows[0];
  row1.querySelector('[formcontrolname="partNo"]').value = 'BLADE-001';
  row1.querySelector('[formcontrolname="serialNo"]').value = 'SN-001';
  row1.querySelector('[formcontrolname="passFail"]').value = 'Pass';
  row1.querySelector('[formcontrolname="d1"]').value = '0.045';
  row1.querySelector('[formcontrolname="d2"]').value = '0.048';
  row1.querySelector('[formcontrolname="d3"]').value = '0.052';
  
  // Row 2 - Measurements outside tolerance
  const row2 = rows[1];
  row2.querySelector('[formcontrolname="partNo"]').value = 'BLADE-002';
  row2.querySelector('[formcontrolname="serialNo"]').value = 'SN-002';
  row2.querySelector('[formcontrolname="passFail"]').value = 'Fail';
  row2.querySelector('[formcontrolname="d1"]').value = '0.035';
  row2.querySelector('[formcontrolname="d2"]').value = '0.038';
  row2.querySelector('[formcontrolname="d3"]').value = '0.032';
  
  // Row 3 - Good measurements
  const row3 = rows[2];
  row3.querySelector('[formcontrolname="partNo"]').value = 'BLADE-003';
  row3.querySelector('[formcontrolname="serialNo"]').value = 'SN-003';
  row3.querySelector('[formcontrolname="passFail"]').value = 'Pass';
  row3.querySelector('[formcontrolname="d1"]').value = '0.049';
  row3.querySelector('[formcontrolname="d2"]').value = '0.050';
  row3.querySelector('[formcontrolname="d3"]').value = '0.048';
  
  // Trigger change events
  document.querySelectorAll('.measurements-table input, .measurements-table select').forEach(input => {
    input.dispatchEvent(new Event('input', { bubbles: true }));
    input.dispatchEvent(new Event('change', { bubbles: true }));
  });
  
  console.log('Measurement data filled');
}

// Test 5: Submit form programmatically
function submitTestForm() {
  const submitButton = document.querySelector('.submit-btn');
  if (submitButton && !submitButton.disabled) {
    submitButton.click();
    console.log('Form submitted');
  } else {
    console.log('Submit button is disabled or not found');
  }
}

// Run complete test
function runCompleteTest() {
  console.log('Starting complete form test...');
  fillTestData();
  setTimeout(() => {
    fillMeasurementData();
    setTimeout(() => {
      submitTestForm();
    }, 1000);
  }, 1000);
}
```

### Script 2: Validation Testing
```javascript
// Test form validation
function testValidation() {
  console.log('Testing form validation...');
  
  // Test required field validation
  const submitButton = document.querySelector('.submit-btn');
  submitButton.click();
  
  // Check for validation errors
  const errors = document.querySelectorAll('.error-message');
  console.log('Validation errors found:', errors.length);
  
  errors.forEach((error, index) => {
    console.log(`Error ${index + 1}:`, error.textContent);
  });
  
  // Test invalid measurement values
  const d1Input = document.querySelector('[formcontrolname="d1"]');
  if (d1Input) {
    d1Input.value = '-0.001'; // Invalid negative value
    d1Input.dispatchEvent(new Event('input', { bubbles: true }));
    d1Input.dispatchEvent(new Event('blur', { bubbles: true }));
    
    setTimeout(() => {
      const validationError = d1Input.parentElement.querySelector('.error-message');
      console.log('Negative value validation:', validationError ? validationError.textContent : 'No error shown');
    }, 100);
  }
  
  // Test too high value
  const d2Input = document.querySelector('[formcontrolname="d2"]');
  if (d2Input) {
    d2Input.value = '0.150'; // Invalid high value
    d2Input.dispatchEvent(new Event('input', { bubbles: true }));
    d2Input.dispatchEvent(new Event('blur', { bubbles: true }));
    
    setTimeout(() => {
      const validationError = d2Input.parentElement.querySelector('.error-message');
      console.log('High value validation:', validationError ? validationError.textContent : 'No error shown');
    }, 100);
  }
}
```

### Script 3: Backend Integration Testing
```javascript
// Test backend connectivity
async function testBackendConnection() {
  console.log('Testing backend connection...');
  
  try {
    const response = await fetch('https://localhost:7000/api/inspectionforms', {
      method: 'GET',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      }
    });
    
    if (response.ok) {
      console.log('Backend connection successful');
      const data = await response.json();
      console.log('Received data:', data);
    } else {
      console.log('Backend connection failed:', response.status, response.statusText);
    }
  } catch (error) {
    console.log('Backend connection error:', error.message);
  }
}

// Test form submission to backend
async function testFormSubmission() {
  console.log('Testing form submission to backend...');
  
  const testData = {
    formId: "WIF-BKT-08",
    revision: "Rev1.0",
    jobNumber: "TEST-JOB-001",
    customer: "Test Customer",
    frame: "FR9-TEST",
    component: "Stage 1 Shroud",
    partNumber: "PART-001",
    quantity: 90,
    operator: "Test Operator",
    inspectionDate: "2024-01-15T00:00:00Z",
    instrumentId: "INST-001",
    calibrationDueDate: "2024-12-31T00:00:00Z",
    inspectedBy: "Test Inspector",
    reviewedBy: "Test Reviewer",
    bladeMeasurements: [
      {
        bladeNumber: 1,
        partNumber: "BLADE-001",
        serialNumber: "SN-001",
        passFail: 1,
        d1: 0.045,
        d2: 0.048,
        d3: 0.052
      }
    ]
  };
  
  try {
    const response = await fetch('https://localhost:7000/api/inspectionforms', {
      method: 'POST',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(testData)
    });
    
    if (response.ok) {
      const result = await response.json();
      console.log('Form submission successful:', result);
      return result.id;
    } else {
      console.log('Form submission failed:', response.status, response.statusText);
      const errorText = await response.text();
      console.log('Error details:', errorText);
    }
  } catch (error) {
    console.log('Form submission error:', error.message);
  }
}
```

### Script 4: Performance Testing
```javascript
// Test form performance with large dataset
function testPerformance() {
  console.log('Starting performance test...');
  
  const startTime = performance.now();
  
  // Fill all 90 rows with data
  const rows = document.querySelectorAll('.measurements-table tbody tr');
  
  rows.forEach((row, index) => {
    if (index < 90) {
      const partNoInput = row.querySelector('[formcontrolname="partNo"]');
      const serialNoInput = row.querySelector('[formcontrolname="serialNo"]');
      const passFailSelect = row.querySelector('[formcontrolname="passFail"]');
      
      if (partNoInput) partNoInput.value = `BLADE-${String(index + 1).padStart(3, '0')}`;
      if (serialNoInput) serialNoInput.value = `SN-${String(index + 1).padStart(3, '0')}`;
      if (passFailSelect) passFailSelect.value = 'Pass';
      
      // Fill measurement fields
      const measurementFields = ['d1', 'd2', 'd3', 'd4', 'd5', 'd6', 'd7', 'd8', 
                                't1', 't2', 't3', 't4', 't5', 'e1', 'e2', 'e3'];
      
      measurementFields.forEach(field => {
        const input = row.querySelector(`[formcontrolname="${field}"]`);
        if (input) {
          input.value = (0.045 + Math.random() * 0.010).toFixed(3); // Random value between 0.045-0.055
        }
      });
    }
  });
  
  const endTime = performance.now();
  console.log(`Performance test completed in ${endTime - startTime} milliseconds`);
  
  // Test form submission performance
  const submitStartTime = performance.now();
  fillTestData(); // Fill basic info
  
  setTimeout(() => {
    submitTestForm();
    const submitEndTime = performance.now();
    console.log(`Form submission initiated in ${submitEndTime - submitStartTime} milliseconds`);
  }, 100);
}
```

### Script 5: Memory Leak Testing
```javascript
// Test for memory leaks
function testMemoryLeaks() {
  console.log('Starting memory leak test...');
  
  let initialMemory = performance.memory ? performance.memory.usedJSHeapSize : 0;
  console.log('Initial memory usage:', initialMemory);
  
  // Simulate multiple form submissions
  let submissionCount = 0;
  const maxSubmissions = 10;
  
  function simulateSubmission() {
    if (submissionCount < maxSubmissions) {
      console.log(`Simulation ${submissionCount + 1}/${maxSubmissions}`);
      
      // Fill and submit form
      fillTestData();
      fillMeasurementData();
      
      setTimeout(() => {
        submitTestForm();
        submissionCount++;
        
        // Check memory usage
        if (performance.memory) {
          const currentMemory = performance.memory.usedJSHeapSize;
          const memoryIncrease = currentMemory - initialMemory;
          console.log(`Memory usage after submission ${submissionCount}:`, currentMemory);
          console.log(`Memory increase:`, memoryIncrease);
        }
        
        // Continue simulation
        setTimeout(simulateSubmission, 2000);
      }, 1000);
    } else {
      console.log('Memory leak test completed');
      if (performance.memory) {
        const finalMemory = performance.memory.usedJSHeapSize;
        const totalIncrease = finalMemory - initialMemory;
        console.log('Final memory usage:', finalMemory);
        console.log('Total memory increase:', totalIncrease);
        
        if (totalIncrease > 50 * 1024 * 1024) { // 50MB threshold
          console.warn('Potential memory leak detected!');
        } else {
          console.log('Memory usage within acceptable limits');
        }
      }
    }
  }
  
  simulateSubmission();
}
```

## 2. Automated Test Execution

### Run All Tests
```javascript
// Execute all test scripts in sequence
async function runAllTests() {
  console.log('=== Starting Comprehensive Test Suite ===');
  
  // Test 1: Basic functionality
  console.log('\n1. Testing basic functionality...');
  runCompleteTest();
  
  // Wait for form submission to complete
  await new Promise(resolve => setTimeout(resolve, 5000));
  
  // Test 2: Validation
  console.log('\n2. Testing validation...');
  testValidation();
  
  // Wait for validation tests
  await new Promise(resolve => setTimeout(resolve, 2000));
  
  // Test 3: Backend connection
  console.log('\n3. Testing backend connection...');
  await testBackendConnection();
  
  // Test 4: Form submission
  console.log('\n4. Testing form submission...');
  await testFormSubmission();
  
  // Test 5: Performance
  console.log('\n5. Testing performance...');
  testPerformance();
  
  // Wait for performance test
  await new Promise(resolve => setTimeout(resolve, 3000));
  
  // Test 6: Memory leaks
  console.log('\n6. Testing memory leaks...');
  testMemoryLeaks();
  
  console.log('\n=== Test Suite Completed ===');
}

// Run the complete test suite
runAllTests();
```

## 3. Test Results Validation

### Check Test Results
```javascript
// Function to validate test results
function validateTestResults() {
  const results = {
    formRendered: !!document.querySelector('app-dynamic-form'),
    sectionsCount: document.querySelectorAll('.form-section').length,
    tableRows: document.querySelectorAll('.measurements-table tbody tr').length,
    submitButtonExists: !!document.querySelector('.submit-btn'),
    validationWorking: document.querySelectorAll('.error-message').length > 0,
    resultsDisplayed: !!document.querySelector('app-inspection-results')
  };
  
  console.log('Test Results Summary:', results);
  
  // Validate expected results
  const expectedResults = {
    formRendered: true,
    sectionsCount: 3,
    tableRows: 90,
    submitButtonExists: true
  };
  
  let allTestsPassed = true;
  
  Object.keys(expectedResults).forEach(key => {
    if (results[key] !== expectedResults[key]) {
      console.error(`Test failed for ${key}: expected ${expectedResults[key]}, got ${results[key]}`);
      allTestsPassed = false;
    }
  });
  
  if (allTestsPassed) {
    console.log('✅ All basic tests passed!');
  } else {
    console.log('❌ Some tests failed!');
  }
  
  return results;
}
```

These test scripts provide comprehensive coverage for both manual and automated testing of your Angular application. You can run them in the browser console to verify functionality, performance, and integration with your backend API.