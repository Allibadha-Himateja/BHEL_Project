# BHEL Inspection API - Entity Framework Code First Backend

This is a comprehensive Entity Framework Code First backend for the BHEL Dynamic Forms Inspection System. The API provides full CRUD operations for inspection forms, blade measurements, and automated analysis capabilities.

## Features

### Core Functionality
- **Inspection Form Management**: Complete CRUD operations for inspection forms
- **Blade Measurements**: Detailed measurement tracking with 16 measurement points (D1-D8, T1-T5, E1-E3)
- **Automated Analysis**: Intelligent analysis engine with tolerance checking and decision making
- **Audit Trail**: Complete audit logging for all inspection activities
- **Tolerance Management**: Configurable tolerance specifications per form type

### Database Design
- **Optimized Entity Relationships**: Proper foreign keys and navigation properties
- **Performance Indexes**: Strategic indexing for common query patterns
- **Data Integrity**: Comprehensive validation and constraints
- **Scalable Architecture**: Designed to handle large volumes of inspection data

### API Features
- **RESTful Design**: Standard HTTP methods and status codes
- **Comprehensive Validation**: FluentValidation for robust input validation
- **AutoMapper Integration**: Efficient object mapping between entities and DTOs
- **Swagger Documentation**: Complete API documentation with examples
- **CORS Support**: Configured for Angular frontend integration

## Database Schema

### Core Entities

#### InspectionForm
- Primary inspection record with job details
- Links to blade measurements and analysis results
- Audit trail and status tracking

#### BladeMeasurement
- Individual blade measurement data
- 16 measurement points with decimal precision
- Pass/Fail status per blade

#### InspectionAnalysis
- Automated analysis results
- Decision logic (Okay/Repair/Replace/Reject)
- Statistical analysis and confidence scoring

#### MeasurementDeviation
- Detailed deviation analysis per measurement
- Tolerance checking and critical deviation flagging
- Statistical calculations

#### ToleranceSpecification
- Configurable tolerance limits per measurement type
- Form-specific and revision-specific tolerances
- Repair limits and nominal values

#### InspectionAuditLog
- Complete audit trail of all changes
- User tracking and timestamp logging
- Change history preservation

## Getting Started

### Prerequisites
- .NET 8.0 SDK
- SQL Server or SQL Server LocalDB
- Visual Studio 2022 or VS Code

### Installation

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd BhelInspectionApi
   ```

2. **Update Connection String**
   Edit `appsettings.json` and `appsettings.Development.json` to configure your database connection:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=your-server;Database=BhelInspectionDb;Trusted_Connection=true;MultipleActiveResultSets=true"
     }
   }
   ```

3. **Install Dependencies**
   ```bash
   dotnet restore
   ```

4. **Create Database**
   The database will be automatically created when you first run the application, or you can create it manually:
   ```bash
   dotnet ef database update
   ```

5. **Run the Application**
   ```bash
   dotnet run
   ```

6. **Access Swagger UI**
   Navigate to `https://localhost:7xxx/swagger` to explore the API documentation.

## API Endpoints

### Inspection Forms
- `GET /api/inspectionforms` - Get all inspection forms (with filtering and pagination)
- `GET /api/inspectionforms/{id}` - Get specific inspection form
- `POST /api/inspectionforms` - Create new inspection form
- `PUT /api/inspectionforms/{id}` - Update inspection form
- `DELETE /api/inspectionforms/{id}` - Delete inspection form
- `POST /api/inspectionforms/{id}/analyze` - Perform analysis
- `POST /api/inspectionforms/{id}/submit` - Submit inspection for review

### Query Parameters
- `jobNumber` - Filter by job number
- `customer` - Filter by customer name
- `status` - Filter by inspection status
- `page` - Page number for pagination
- `pageSize` - Number of items per page

## Business Logic

### Analysis Engine
The system includes a sophisticated analysis engine that:

1. **Validates Measurements**: Checks all measurements against tolerance specifications
2. **Calculates Deviations**: Computes deviation percentages and absolute values
3. **Identifies Critical Issues**: Flags measurements outside repair limits
4. **Makes Decisions**: Automatically determines Okay/Repair/Replace/Reject status
5. **Provides Confidence Scoring**: Statistical confidence in the decision
6. **Generates Recommendations**: Detailed recommendations based on analysis

### Decision Logic
- **Okay**: All measurements within tolerance
- **Repair**: Minor deviations (1-3 measurements outside tolerance)
- **Replace**: Significant wear (4-10 measurements outside tolerance)
- **Reject**: Critical deviations (>10 measurements or safety-critical issues)

### Tolerance Management
- Form-specific tolerance specifications
- Revision control for tolerance changes
- Configurable repair limits
- Statistical analysis capabilities

## Data Models

### Key DTOs
- `InspectionFormDto` - Complete inspection form data
- `CreateInspectionFormDto` - New inspection form creation
- `BladeMeasurementDto` - Individual blade measurement data
- `InspectionAnalysisDto` - Analysis results and recommendations
- `MeasurementDeviationDto` - Detailed deviation analysis

### Validation Rules
- Comprehensive input validation using FluentValidation
- Business rule enforcement
- Data type and range validation
- Pattern matching for part numbers and job numbers

## Performance Considerations

### Database Optimization
- Strategic indexing on frequently queried columns
- Composite indexes for complex queries
- Proper foreign key relationships
- Efficient pagination support

### Query Optimization
- Include statements for related data
- Projection to DTOs to reduce data transfer
- Async/await patterns throughout
- Connection pooling and resource management

## Security Features

### Data Protection
- Input validation and sanitization
- SQL injection prevention through Entity Framework
- Proper error handling without information disclosure
- Audit trail for all changes

### CORS Configuration
- Configured for Angular frontend integration
- Restrictive origin policies
- Credential support for authenticated requests

## Monitoring and Logging

### Logging
- Structured logging with Serilog integration ready
- Error tracking and performance monitoring
- Audit trail logging
- Database command logging in development

### Health Checks
- Database connectivity checks
- Service dependency monitoring
- Performance metrics collection

## Deployment

### Production Considerations
- Connection string security
- Environment-specific configurations
- Database migration strategies
- Performance monitoring setup

### Docker Support
The application is ready for containerization with proper configuration management.

## Integration with Angular Frontend

### API Compatibility
- CORS configured for Angular development server
- RESTful endpoints matching Angular service expectations
- Comprehensive error responses
- Pagination headers for frontend grid components

### Data Flow
1. Angular form submission → API validation → Database storage
2. Automatic analysis trigger → Results calculation → Frontend display
3. Real-time status updates and audit trail

## Contributing

### Code Standards
- Follow .NET coding conventions
- Use async/await patterns
- Implement proper error handling
- Add comprehensive unit tests
- Document public APIs

### Database Changes
- Use Entity Framework migrations
- Test migration scripts thoroughly
- Maintain backward compatibility
- Document schema changes

## Support

For technical support or questions about the BHEL Inspection API, please refer to the API documentation or contact the development team.

## License

This project is proprietary software developed for BHEL Dynamic Forms System.