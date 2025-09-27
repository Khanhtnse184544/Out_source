# Backend Conventions & Best Practices

## 🗄️ Database Configuration

### Supabase Connection Setup
- **Provider**: PostgreSQL với Supabase
- **Connection Type**: Transaction Pooler (Port 6543)
- **ORM**: Entity Framework Core với Npgsql

### Connection String Format
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "User Id=postgres.oliumyitihpmujlxdvrn;Password=[YOUR-PASSWORD];Server=aws-1-ap-southeast-1.pooler.supabase.com;Port=6543;Database=postgres"
  }
}
```

### ✅ Best Practices
- **Single Configuration Point**: Chỉ cấu hình connection string trong `Program.cs`
- **No Hardcoded Passwords**: Sử dụng environment variables hoặc Azure Key Vault
- **Dependency Injection**: Đăng ký DbContext trong Program.cs

### ❌ Avoid
- Hardcode password trong appsettings.json
- Duplicate connection string configuration
- OnConfiguring method trong DbContext (đã loại bỏ)

## 🏗️ Project Structure

### Layer Architecture
```
├── GameUserServicesBackend/     # API Layer
│   ├── Controllers/            # API Controllers
│   ├── Program.cs             # Startup & DI Configuration
│   └── appsettings.json       # Configuration
├── BLL/                        # Business Logic Layer
│   └── Services/             # Business Services
├── DAL/                       # Data Access Layer
│   ├── Context/              # Entity Framework Context
│   ├── DAO/                  # Data Access Objects
│   └── Repositories/         # Repository Pattern
```

### Naming Conventions
- **Controllers**: `[Entity]Controller.cs` (e.g., `UserController.cs`)
- **Services**: `[Entity]Services.cs` (e.g., `UserServices.cs`)
- **Repositories**: `[Entity]Repository.cs` (e.g., `UserRepository.cs`)
- **DAO**: `[Entity]DAO.cs` (e.g., `UserDAO.cs`)

## 🔧 Dependency Injection Setup

### Service Registration Pattern
```csharp
// Repository Layer
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<CategoryDetailsRepository>();

// Business Logic Layer
builder.Services.AddScoped<UserServices>();
builder.Services.AddScoped<CategoryDetailServices>();

// Data Access Layer
builder.Services.AddScoped<UserDAO>();
builder.Services.AddScoped<CateDAO>();

// Database Context
builder.Services.AddDbContext<db_userservicesContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);
```

## 🌐 CORS Configuration

### Unity Game Client Support
```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowUnity", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
```

## 🚀 Azure Deployment

### Environment Configuration
- **Development**: Sử dụng appsettings.json
- **Production**: Sử dụng Azure App Service Configuration

### Azure App Service Settings
```
ConnectionStrings__DefaultConnection = [Connection String]
ASPNETCORE_ENVIRONMENT = Production
```

### Security Best Practices
1. **Environment Variables**: Lưu sensitive data trong Azure Configuration
2. **Azure Key Vault**: Cho production environments với high security requirements
3. **No Hardcoded Secrets**: Không commit password vào source code

## 📊 Database Schema

### Entity Naming
- **Tables**: snake_case (e.g., `categorydetails`, `transactionhistory`)
- **Schema**: `userservices`
- **Primary Keys**: Composite keys where appropriate

### Entity Framework Configuration
- **Provider**: Npgsql.EntityFrameworkCore.PostgreSQL
- **Version**: 9.0.4
- **Extensions**: PostgreSQL specific extensions enabled

## 🔒 Security Guidelines

### Password Management
- ✅ Use environment variables
- ✅ Azure Key Vault for production
- ❌ Never hardcode in source code
- ❌ Never commit to version control

### API Security
- CORS configured for Unity client
- HTTPS redirection enabled
- Authorization middleware configured

## 📝 Code Standards

### File Organization
- **Controllers**: API endpoints và request handling
- **Services**: Business logic và validation
- **Repositories**: Data access và CRUD operations
- **DAO**: Data transfer objects

### Error Handling
- Repository layer: Return meaningful error messages
- Service layer: Handle business logic exceptions
- Controller layer: Return appropriate HTTP status codes

## 🧪 Development Guidelines

### Local Development
1. Sử dụng appsettings.Development.json cho local config
2. Connection string có thể chứa local database
3. Enable Swagger cho API documentation

### Production Deployment
1. Environment variables cho configuration
2. Disable Swagger trong production
3. Enable HTTPS redirection
4. Configure proper logging levels

---

## 📚 Quick Reference

### Essential Commands
```bash
# Add Entity Framework package
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL

# Add Configuration package
dotnet add package Microsoft.Extensions.Configuration.Json

# Database migrations (if needed)
dotnet ef migrations add [MigrationName]
dotnet ef database update
```

### Key Files to Monitor
- `Program.cs` - Service registration và configuration
- `appsettings.json` - Development configuration
- `db_userservicesContext.cs` - Database context
- Controllers - API endpoints

---

*Last Updated: $(date)*
*Project: GameUserServicesBackend*
*Framework: .NET 8.0*
