# zenwebapi18

My test app

## � Empty .NET Template

This repository provides **deployment infrastructure only** - no boilerplate .NET code included.

### ⚡ Quick Start
1. **Copy your existing .NET project** to this repository root
2. **Read and delete** `SETUP-DOTNET-PROJECTS.md` for detailed instructions
3. **Push your code** - deployment pipeline is ready to go!

### 🏗️ What's Included
- **Dockerfile**: Automatically builds any .NET solution structure
- **GitHub Actions**: Complete CI/CD pipeline
- **Kubernetes Manifests**: Production-ready deployment configs
- **Cloud Infrastructure**: Automated GCP setup scripts

## 📋 Configuration Summary

### Features

- **Cloud Provider**: GCP
- **Deployment Type**: Gke
- **Region**: asia-south1
- **GKE Cluster**: zenhotel-cluster
- **Namespace**: default

## Getting Started

### 1. GCP Setup (First Time Only)

Before deploying, you need to set up your Google Cloud Platform project:

**Option A: Automated Setup (Recommended)**
```bash
# Run the automated setup script
chmod +x scripts/setup-gcp.sh
./scripts/setup-gcp.sh YOUR_PROJECT_ID YOUR_REGION YOUR_SERVICE_NAME

# Example:
./scripts/setup-gcp.sh my-project-123 us-central1 zenwebapi18
```

**Option B: Manual Setup**
See [GCP-SETUP.md](./GCP-SETUP.md) for detailed manual setup instructions.

### 2. GitHub Secrets Configuration

Add the following secrets to your GitHub repository (Settings → Secrets and variables → Actions):

- `GCP_SA_KEY`: Contents of the service account key file (created by setup script)
- `GCP_PROJECT_ID`: Your GCP project ID

### 3. Deploy Your Application

1. Clone this repository
2. Add your .NET application code to this repository
3. Push to the `main` branch to trigger the deployment pipeline

## Deployment

### Application Deployment
Your application will be deployed to **Google Kubernetes Engine (GKE)** cluster:
- **Cluster**: zenhotel-cluster
- **Namespace**: default
- **Region**: asia-south1

## CI/CD Pipeline

The repository includes GitHub Actions workflows for:
2. **Application Deployment** (`deploy.yml`) - Runs on every push to main branch

## Prerequisites

Make sure the following GitHub secrets are configured in your repository:

- `GCP_PROJECT_ID`: Your Google Cloud Project ID
- `GCP_REGION`: Your Google Cloud Region
- `GCP_SA_KEY`: Base64 encoded service account key with necessary permissions

## Development

Add your .NET application code and push to trigger the deployment pipeline. The template supports both:

- **ASP.NET Web API** (Backend)
- **ASP.NET Web App** (Frontend)

The Dockerfile and deployment scripts are generic and will work with both application types.
