# GCP Setup for .NET Application Deployment

This document outlines the required Google Cloud Platform setup steps for deploying your .NET application.

## Prerequisites

Before the GitHub Actions workflow can successfully deploy your application, you need to set up the following in your GCP project:

## 1. Enable Required APIs

```bash
# Enable Artifact Registry API
gcloud services enable artifactregistry.googleapis.com

# Enable Cloud Run API (if using Cloud Run)
gcloud services enable run.googleapis.com

# Enable Kubernetes Engine API (if using GKE)
gcloud services enable container.googleapis.com

# Enable Cloud Resource Manager API
gcloud services enable cloudresourcemanager.googleapis.com
```

## 2. Create Artifact Registry Repository

```bash
# Replace YOUR_REGION with your preferred region (e.g., us-central1, asia-south1)
gcloud artifacts repositories create docker-repo \
  --repository-format=docker \
  --location=YOUR_REGION \
  --description="Docker repository for .NET applications"
```

## 3. Create Service Account

```bash
# Create service account
gcloud iam service-accounts create backstage-deployer \
  --description="Service account for Backstage deployments" \
  --display-name="Backstage Deployer"
```

## 4. Grant Required IAM Roles

```bash
# Get your project ID
PROJECT_ID=$(gcloud config get-value project)

# Grant required roles to the service account
gcloud projects add-iam-policy-binding $PROJECT_ID \
  --member="serviceAccount:backstage-deployer@$PROJECT_ID.iam.gserviceaccount.com" \
  --role="roles/artifactregistry.writer"

gcloud projects add-iam-policy-binding $PROJECT_ID \
  --member="serviceAccount:backstage-deployer@$PROJECT_ID.iam.gserviceaccount.com" \
  --role="roles/run.admin"

gcloud projects add-iam-policy-binding $PROJECT_ID \
  --member="serviceAccount:backstage-deployer@$PROJECT_ID.iam.gserviceaccount.com" \
  --role="roles/container.admin"

gcloud projects add-iam-policy-binding $PROJECT_ID \
  --member="serviceAccount:backstage-deployer@$PROJECT_ID.iam.gserviceaccount.com" \
  --role="roles/iam.serviceAccountUser"

# Optional: Grant Service Usage Admin to allow enabling APIs
gcloud projects add-iam-policy-binding $PROJECT_ID \
  --member="serviceAccount:backstage-deployer@$PROJECT_ID.iam.gserviceaccount.com" \
  --role="roles/serviceusage.serviceUsageAdmin"
```

## 5. Create and Download Service Account Key

```bash
# Create service account key
gcloud iam service-accounts keys create backstage-key.json \
  --iam-account=backstage-deployer@$PROJECT_ID.iam.gserviceaccount.com
```

## 6. Set up GitHub Secrets

In your GitHub repository, go to Settings → Secrets and variables → Actions, and add:

- `GCP_SA_KEY`: The entire contents of the `backstage-key.json` file
- `GCP_PROJECT_ID`: Your GCP project ID
- `GCP_REGION`: Your preferred region (e.g., `us-central1`, `asia-south1`)

## 7. GKE Setup (if using GKE deployment)

If you plan to deploy to Google Kubernetes Engine:

```bash
# Create GKE cluster
gcloud container clusters create YOUR_CLUSTER_NAME \
  --location=YOUR_REGION \
  --num-nodes=3 \
  --enable-autoscaling \
  --min-nodes=1 \
  --max-nodes=5 \
  --machine-type=e2-medium
```

## Troubleshooting

### Common Issues

1. **API Not Enabled Error**: Make sure all required APIs are enabled in your project
2. **Permission Denied**: Verify the service account has all required IAM roles
3. **Repository Not Found**: Ensure the Artifact Registry repository exists
4. **Invalid Project**: Check that the project ID is correct in GitHub secrets

### Manual Repository Creation

If the automatic repository creation fails, create it manually:

```bash
gcloud artifacts repositories create docker-repo \
  --repository-format=docker \
  --location=YOUR_REGION \
  --project=YOUR_PROJECT_ID
```

### Verify Setup

To verify everything is set up correctly:

```bash
# Test authentication
gcloud auth list

# Check enabled APIs
gcloud services list --enabled

# List Artifact Registry repositories
gcloud artifacts repositories list

# Test Docker authentication
gcloud auth configure-docker YOUR_REGION-docker.pkg.dev
```

## Security Best Practices

1. **Principle of Least Privilege**: Only grant the minimum required roles
2. **Regular Key Rotation**: Periodically rotate service account keys
3. **Monitor Usage**: Use Cloud Logging to monitor service account activity
4. **Secure Storage**: Never commit service account keys to version control

## Support

If you encounter issues during setup, check:
- [Google Cloud Console](https://console.cloud.google.com)
- [Artifact Registry Documentation](https://cloud.google.com/artifact-registry/docs)
- [Cloud Run Documentation](https://cloud.google.com/run/docs)
- [GKE Documentation](https://cloud.google.com/kubernetes-engine/docs)
