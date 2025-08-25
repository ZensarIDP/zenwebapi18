# Database Configuration Guide
Infrastructure provisioning is disabled for this template. If you need a database:

1. Enable infrastructure provisioning in the template
2. Or manually create your database instance
3. Configure connection strings manually in your application

## Manual Database Setup

If you're using an existing database, configure your connection string in:

### appsettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Your_Connection_String_Here"
  }
}
```

### Program.cs
```csharp
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// Configure your DbContext based on your database type
```
