#!/bin/bash

# Automated GCP Setup Script for .NET Backstage Template
# This script automates the setup of GCP resources required for deployment

set -e

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Function to print colored output
print_status() {
    echo -e "${GREEN}[INFO]${NC} $1"
}

print_warning() {
    echo -e "${YELLOW}[WARNING]${NC} $1"
}

print_error() {
    echo -e "${RED}[ERROR]${NC} $1"
}

print_step() {
    echo -e "${BLUE}[STEP]${NC} $1"
}

# Check if required parameters are provided
if [ $# -lt 3 ]; then
    print_error "Usage: $0 <PROJECT_ID> <REGION> <SERVICE_NAME> [SERVICE_ACCOUNT_NAME]"
    print_error "Example: $0 my-project-123 us-central1 myapp"
    exit 1
fi

PROJECT_ID=$1
REGION=$2
SERVICE_NAME=$3
SERVICE_ACCOUNT_NAME=${4:-backstage-deployer}
REPO_NAME="${SERVICE_NAME}-repo"

print_step "Starting automated setup for:"
print_status "Project: $PROJECT_ID"
print_status "Region: $REGION"
print_status "Service: $SERVICE_NAME"
print_status "Repository: $REPO_NAME"

# Set the active project
print_step "Setting active project to $PROJECT_ID"
gcloud config set project $PROJECT_ID

# Enable required APIs
print_step "Enabling required APIs..."
APIS=(
    "artifactregistry.googleapis.com"
    "cloudbuild.googleapis.com"
    "run.googleapis.com"
    "container.googleapis.com"
    "cloudresourcemanager.googleapis.com"
    "iam.googleapis.com"
)

for api in "${APIS[@]}"; do
    print_status "Enabling $api..."
    gcloud services enable $api --quiet || print_warning "Failed to enable $api - it may already be enabled"
done

# Wait for APIs to be ready
print_status "Waiting for APIs to be ready..."
sleep 10

# Create Artifact Registry repository
print_step "Creating Artifact Registry repository..."
if gcloud artifacts repositories describe $REPO_NAME --location=$REGION --quiet >/dev/null 2>&1; then
    print_status "Repository '$REPO_NAME' already exists"
else
    print_status "Creating repository '$REPO_NAME'..."
    gcloud artifacts repositories create $REPO_NAME \
        --repository-format=docker \
        --location=$REGION \
        --description="Docker repository for $SERVICE_NAME" \
        --quiet
    print_status "Repository created successfully"
fi

# Create service account if it doesn't exist
print_step "Creating service account..."
if gcloud iam service-accounts describe ${SERVICE_ACCOUNT_NAME}@${PROJECT_ID}.iam.gserviceaccount.com --quiet >/dev/null 2>&1; then
    print_status "Service account '${SERVICE_ACCOUNT_NAME}' already exists"
else
    print_status "Creating service account '${SERVICE_ACCOUNT_NAME}'..."
    gcloud iam service-accounts create $SERVICE_ACCOUNT_NAME \
        --description="Service account for Backstage deployments" \
        --display-name="Backstage Deployer" \
        --quiet
    print_status "Service account created successfully"
fi

# Grant required IAM roles
print_step "Granting IAM roles to service account..."
ROLES=(
    "roles/artifactregistry.admin"
    "roles/artifactregistry.writer"
    "roles/run.admin"
    "roles/container.admin"
    "roles/iam.serviceAccountUser"
    "roles/cloudbuild.builds.builder"
)

for role in "${ROLES[@]}"; do
    print_status "Granting role: $role"
    gcloud projects add-iam-policy-binding $PROJECT_ID \
        --member="serviceAccount:${SERVICE_ACCOUNT_NAME}@${PROJECT_ID}.iam.gserviceaccount.com" \
        --role="$role" \
        --quiet || print_warning "Failed to grant $role - may already be granted"
done

# Create service account key
print_step "Creating service account key..."
KEY_FILE="${SERVICE_ACCOUNT_NAME}-key.json"
if [ -f "$KEY_FILE" ]; then
    print_warning "Key file $KEY_FILE already exists. Skipping key creation."
    print_warning "If you need a new key, delete the existing file and run this script again."
else
    gcloud iam service-accounts keys create $KEY_FILE \
        --iam-account=${SERVICE_ACCOUNT_NAME}@${PROJECT_ID}.iam.gserviceaccount.com \
        --quiet
    print_status "Service account key created: $KEY_FILE"
fi

# Verify setup
print_step "Verifying setup..."

# Check repository
if gcloud artifacts repositories describe $REPO_NAME --location=$REGION --quiet >/dev/null 2>&1; then
    print_status "✅ Artifact Registry repository verified"
else
    print_error "❌ Artifact Registry repository not found"
fi

# Check service account
if gcloud iam service-accounts describe ${SERVICE_ACCOUNT_NAME}@${PROJECT_ID}.iam.gserviceaccount.com --quiet >/dev/null 2>&1; then
    print_status "✅ Service account verified"
else
    print_error "❌ Service account not found"
fi

# Test authentication
print_status "Testing service account authentication..."
gcloud auth activate-service-account --key-file=$KEY_FILE --quiet
if gcloud auth list --filter="account:${SERVICE_ACCOUNT_NAME}@${PROJECT_ID}.iam.gserviceaccount.com" --quiet | grep -q "${SERVICE_ACCOUNT_NAME}"; then
    print_status "✅ Service account authentication successful"
else
    print_error "❌ Service account authentication failed"
fi

print_step "Setup completed successfully!"
echo ""
print_status "🎉 Your GCP project is now configured for .NET deployments!"
echo ""
print_status "Next steps:"
echo "1. Add the following secrets to your GitHub repository:"
echo "   - GCP_SA_KEY: Contents of $KEY_FILE"
echo "   - GCP_PROJECT_ID: $PROJECT_ID"
echo "   - GCP_REGION: $REGION"
echo ""
print_status "2. Keep the key file secure and never commit it to version control"
echo ""
print_status "3. Your repository URL format will be:"
echo "   ${REGION}-docker.pkg.dev/${PROJECT_ID}/${REPO_NAME}/[IMAGE_NAME]"
echo ""
print_warning "Security reminder: Regularly rotate your service account keys!"
