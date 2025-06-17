# BHEL Inspection Application

A comprehensive Angular application for BHEL Dynamic Forms Inspection System with backend integration capabilities.

## Features

- **Dynamic Form Generation**: Forms are generated from JSON configuration
- **Backend Integration**: Full CRUD operations with .NET Core API
- **Local Analysis Fallback**: Works offline with local analysis engine
- **Responsive Design**: Works on desktop, tablet, and mobile devices
- **Real-time Validation**: Client-side and server-side validation
- **Comprehensive Analysis**: Automated inspection analysis with confidence scoring

## Quick Start

### Prerequisites
- Node.js 18+ and npm
- Angular CLI 17+

### Installation

1. **Install dependencies**:
   ```bash
   npm install
   ```

2. **Configure API URL**:
   Edit `src/environments/environment.ts` to point to your backend API:
   ```typescript
   export const environment = {
     production: false,
     apiUrl: 'https://localhost:7000/api'  // Your API URL
   };
   ```

3. **Start the development server**:
   ```bash
   npm start
   ```

4. **Open your browser**:
   Navigate to `http://localhost:4200`

## Project Structure

```
src/
├── app/
│   ├── components/
│   │   ├── dynamic-form/          # Dynamic form component
│   │   └── inspection-results/    # Results display component
│   ├── models/
│   │   ├── form.models.ts         # Frontend form models
│   │   └── backend-dto.models.ts  # Backend API models
│   ├── services/
│   │   ├── api.service.ts         # Generic HTTP service
│   │   ├── backend-inspection.service.ts  # Backend integration
│   │   ├── inspection.service.ts  # Local analysis engine
│   │   └── enhanced-inspection.service.ts # Combined service
│   ├── form-new-config.json       # Form configuration
│   └── environments/              # Environment configurations
└── test-documentation/            # Comprehensive test cases
```

## Configuration

### Form Configuration
The form is configured via `src/app/form-new-config.json`. This JSON file defines:
- Form structure and sections
- Field types and validation rules
- Business rules and tolerances
- Analysis criteria

### Backend Integration
Configure the backend API URL in environment files:
- `src/environments/environment.ts` (development)
- `src/environments/environment.prod.ts` (production)

## Testing

### Frontend Testing
Run the test scripts in browser console (see `test-documentation/angular-test-scripts.md`):

```javascript
// Basic functionality test
runCompleteTest();

// Backend connectivity test
testBackendConnection();

// Performance test
testPerformance();

// Complete test suite
runAllTests();
```

### API Testing
Use the provided Postman collection (`test-documentation/postman-collection.json`) to test backend API endpoints.

## Key Features

### Dynamic Form Generation
- Forms are generated from JSON configuration
- Supports multiple field types: text, number, date, dropdown, table, etc.
- Complex table structures with grouped headers
- Real-time validation

### Backend Integration
- Full CRUD operations for inspection forms
- Automatic analysis upon form submission
- Fallback to local analysis if backend is unavailable
- Error handling and retry mechanisms

### Analysis Engine
- Automated tolerance checking
- Statistical analysis of measurements
- Decision logic: Okay/Repair/Replace/Reject
- Confidence scoring and detailed reasoning

### Responsive Design
- Works on all device sizes
- Optimized table scrolling for mobile
- Touch-friendly interface
- Print-ready reports

## Development

### Adding New Field Types
1. Update `FormField` interface in `models/form.models.ts`
2. Add template logic in `dynamic-form.component.html`
3. Update form building logic in `dynamic-form.component.ts`

### Customizing Analysis Logic
1. Modify `inspection.service.ts` for local analysis
2. Update business rules in `form-new-config.json`
3. Adjust tolerance specifications as needed

### Backend Integration
1. Update DTOs in `models/backend-dto.models.ts`
2. Modify mapping logic in `backend-inspection.service.ts`
3. Update API endpoints in `api.service.ts`

## Deployment

### Development
```bash
npm start
```

### Production Build
```bash
npm run build
```

### Docker Deployment
```bash
# Build the application
npm run build

# Create Docker image (Dockerfile not included)
# Deploy to your preferred hosting platform
```

## Browser Support

- Chrome 90+
- Firefox 88+
- Safari 14+
- Edge 90+

## Performance

- Optimized for large datasets (90+ table rows)
- Lazy loading for improved initial load time
- Memory leak prevention
- Efficient change detection

## Security

- Input validation and sanitization
- XSS prevention
- CSRF protection (when integrated with backend)
- Secure API communication

## Contributing

1. Follow Angular coding standards
2. Add tests for new features
3. Update documentation
4. Ensure responsive design compatibility

## License

Proprietary software for BHEL Dynamic Forms System.