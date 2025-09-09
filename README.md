# TeambaseInsurance API

A comprehensive insurance management API built with .NET 8, featuring employee management and premium calculation capabilities with configurable pricing models and proration methods.

## 🏗️ Architecture

This project follows **Clean Architecture** principles with clear separation of concerns:

```
TeambaseInsuranceApi1/
├── TeambaseInsuranceApi/           # 🌐 Web API Layer (Entry Point)
├── TeambaseInsurance.Presentation/ # 🎯 Presentation Layer (Controllers)
├── TeambaseInsurance.Service/      # 🔧 Business Logic Layer
├── TeambaseInsurance.ServiceContracts/ # 📋 Service Interfaces
├── TeambaseInsurance.Repository/   # 💾 Data Access Layer
├── TeambaseInsurance.RepositoryContracts/ # 📋 Repository Interfaces
├── Entities/                      # 📊 Domain Models
└── Shared/                        # 🔄 Common DTOs & Configurations
```

## ✨ Features

### 👥 Employee Management
- Create, read, update employees
- Track employee demographics (name, date of birth, gender)
- Manage policy dates and join dates
- Automatic timestamping for audit trails

### 💰 Premium Calculation
- **Multiple Pricing Models:**
  - `FlatRate`: Base rate for all employees
  - `AgeRated`: Age-based premium calculation
  - `GenderAgeRated`: Combined age and gender-based pricing
  
- **Proration Methods:**
  - `ByDays`: Daily proration calculation
  - `ByMonths`: Monthly proration calculation

- **Configurable Rate Tables:** Age-based rate configuration via `appsettings.json`

## 🚀 Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server LocalDB](https://docs.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb) (or SQL Server)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)

### Installation

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd TeambaseInsuranceApi1
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Update database connection** (if needed)
   
   Edit `TeambaseInsuranceApi/appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=TeambaseInsuranceDb;Integrated Security=True;"
     }
   }
   ```

4. **Run database migrations**
   ```bash
   cd TeambaseInsuranceApi
   dotnet ef database update
   ```

5. **Run the application**
   ```bash
   dotnet run
   ```

6. **Access Swagger UI**
   
   Navigate to: `https://localhost:7000/swagger` (or the port shown in console)

## 📡 API Endpoints

### Employee Management

| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/api/v1/employee` | Get all employees |
| `GET` | `/api/v1/employee/{id}` | Get employee by ID |
| `POST` | `/api/v1/employee` | Create new employee |

### Premium Calculator

| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/api/v1/premium-calculator/get-premium` | Calculate employee premium |

#### Premium Calculation Parameters

```json
{
  "employeeId": 1,
  "pricingModel": "GenderAgeRated", // FlatRate | AgeRated | GenderAgeRated
  "prorationMethod": "ByDays"       // ByDays | ByMonths
}
```

## 📋 Request/Response Examples

### Create Employee
```bash
POST /api/v1/employee
Content-Type: application/json

{
  "firstName": "John",
  "lastName": "Doe", 
  "dateOfBirth": "1990-05-15",
  "gender": "MALE",
  "joinDate": "2024-01-01T00:00:00",
  "policyEndDate": "2024-12-31T23:59:59"
}
```

### Calculate Premium
```bash
GET /api/v1/premium-calculator/get-premium?employeeId=1&pricingModel=GenderAgeRated&prorationMethod=ByDays
```

**Response:**
```json
{
  "isSuccess": true,
  "message": "Premium calculated successfully",
  "data": {
    "employeeId": 1,
    "basePremium": 1000.00,
    "ageFactor": 1.2,
    "genderFactor": 1.0,
    "prorationFactor": 0.85,
    "finalPremium": 1020.00
  }
}
```

## ⚙️ Configuration

### Age Rate Configuration
Configure age-based rates in `appsettings.json`:

```json
{
  "AgeRateConfig": [
    { "MinAge": 20, "MaxAge": 29, "Rate": 300 },
    { "MinAge": 30, "MaxAge": 39, "Rate": 400 },
    { "MinAge": 40, "MaxAge": 49, "Rate": 500 }
  ]
}
```

### Premium Configuration
```json
{
  "PremiumConfig": {
    "BaseRate": 1000,
    "GenderMultiplier": {
      "Male": 1.0,
      "Female": 0.9
    },
    "DefaultAgeRate": 1000
  }
}
```

## 🗄️ Database Schema

### Employee Table
```sql
CREATE TABLE Employees (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    DateOfBirth DATE NOT NULL,
    Gender NVARCHAR(10) NOT NULL,
    JoinDate DATETIME NOT NULL,
    PolicyEndDate DATETIME NOT NULL,
    CreatedAt DATETIME NOT NULL,
    LastModified DATETIME NULL
)
```

## 🏛️ Project Structure Details

### Key Components

- **Controllers**: Handle HTTP requests and responses
- **Services**: Implement business logic and rules
- **Repositories**: Handle data access and persistence
- **DTOs**: Data transfer objects for API communication
- **Entities**: Domain models representing business entities
- **AutoMapper**: Object-to-object mapping configuration

### Key Technologies
- **ASP.NET Core 8.0**: Web framework
- **Entity Framework Core**: ORM for database operations
- **AutoMapper**: Object mapping
- **Swagger/OpenAPI**: API documentation
- **Ardalis.Result**: Result pattern implementation
- **SQL Server**: Database engine

## 🧪 Testing

Run tests using:
```bash
dotnet test
```

## 📝 Business Rules

1. **Employee Management**
   - All employees must have valid names, date of birth, and gender
   - Gender must be either "MALE" or "FEMALE"
   - Join date cannot be in the future
   - Policy end date must be after join date

2. **Premium Calculation**
   - Age is calculated based on current date and date of birth
   - Gender factors are applied only in GenderAgeRated model
   - Proration is based on the time between join date and policy end date
   - Default rates apply when specific age ranges are not configured

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request
 

## 🆘 Support

For support and questions:
- Create an issue in the GitHub repository
- Contact the development team

## 🗺️ Roadmap

- [ ] Add authentication and authorization
- [ ] Implement employee update and delete operations
- [ ] Add unit and integration tests
- [ ] Implement caching for premium calculations
- [ ] Add logging and monitoring
- [ ] Create admin dashboard for rate configuration
- [ ] Add bulk operations for employee management
