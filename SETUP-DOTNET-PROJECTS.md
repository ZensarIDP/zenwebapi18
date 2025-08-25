# .NET Project Setup Guide

This template provides deployment infrastructure for **your existing .NET projects** without any boilerplate code.

## 🚀 Quick Start

1. **Delete this file** after reading the instructions
2. **Copy your entire .NET solution** to the repository root (replace all placeholder files)
3. **Commit and push** - the deployment pipeline will automatically detect and build your projects

## ✅ What This Template Provides

- **Dockerfile**: Automatically detects and builds any .NET solution structure
- **GitHub Actions**: Complete CI/CD pipeline for building, testing, and deploying
- **Kubernetes Manifests**: Ready-to-use deployment configurations
- **GCP Scripts**: Automated cloud infrastructure setup
- **Zero boilerplate**: No sample code to remove

## 📁 Supported Project Structures

### Single Project
```
/
├── YourApp.csproj
├── Program.cs
├── Controllers/ (for Web APIs)
└── ... (your source files)
```

### Multi-Project Solution
```
/
├── YourSolution.sln
├── src/
│   ├── YourApp.Api/
│   │   ├── YourApp.Api.csproj
│   │   └── Program.cs
│   ├── YourApp.Core/
│   │   └── YourApp.Core.csproj
│   └── YourApp.Infrastructure/
│       └── YourApp.Infrastructure.csproj
└── tests/
    └── YourApp.Tests/
        └── YourApp.Tests.csproj
```

### Complex Nested Structure
```
/
├── Solution.sln
├── apps/
│   ├── WebApi/
│   ├── BackgroundService/
│   └── Console/
├── libs/
│   ├── Domain/
│   ├── Infrastructure/
│   └── Shared/
└── tools/
    └── DbMigrator/
```

## 🔍 Automatic Project Detection

The build process automatically finds your startup project:

1. **Web API Projects** (Priority 1): Projects with `Microsoft.AspNetCore` references
2. **Executable Projects** (Priority 2): Projects with `<OutputType>Exe</OutputType>`
3. **Fallback** (Priority 3): First `.csproj` found

## 🐳 Docker Build Process

1. Copies your **entire project structure** (no file reorganization needed)
2. Runs `dotnet restore` for all projects in your solution
3. Automatically detects the startup project
4. Builds and publishes to a production-ready container
5. Runs as non-root user with health checks

## 🚀 Deployment Targets

- **Google Cloud Run**: Serverless container deployment
- **Google Kubernetes Engine**: Full Kubernetes deployment
- **Automatic scaling**: Based on traffic and resource usage

## ⚙️ Environment Variables

Pre-configured for production:
```
ASPNETCORE_URLS=http://+:8080
ASPNETCORE_ENVIRONMENT=Production
```

## 🔧 What You Need to Do

### 1. Replace Template Files
- Delete this file and any other placeholder files
- Copy your existing .NET solution to the repository root

### 2. Verify Your Project
Ensure your project works locally:
```bash
dotnet restore
dotnet build
dotnet run --project YourStartupProject
```

### 3. Configure Secrets (if needed)
Add GitHub repository secrets for:
- `GCP_CREDENTIALS`: Your Google Cloud service account key
- Any database connection strings or API keys

### 4. Deploy
Push your code - the GitHub Actions workflow will:
- Build your application
- Create Docker image
- Deploy to your selected cloud target

## 🩺 Health Checks

The container includes intelligent health checks:
- Web APIs: Checks `/health` endpoint, then root endpoint
- Console apps: Basic container health
- Custom health checks: Add your own endpoints as needed

## 🆘 Troubleshooting

### Build Fails
- Ensure your project builds locally with `dotnet build`
- Check all NuGet package references are valid
- Verify you're targeting .NET 8.0 or compatible

### Deployment Fails
- Check GitHub Actions logs for specific errors
- Verify GCP credentials are correctly configured
- Ensure your application listens on port 8080

### Container Crashes
- Check container logs in Cloud Run or GKE console
- Verify all required configuration is present
- Ensure database connections (if any) are accessible

## 📝 Notes

- **No code changes required**: Your existing .NET project should work as-is
- **Any .NET version**: The Dockerfile supports .NET 6, 7, 8, and future versions
- **Any project type**: Web APIs, console apps, background services, etc.
- **Production ready**: Includes security best practices and monitoring

---

**Ready to deploy?** Delete this file, add your project, and push to trigger deployment! 🎉
