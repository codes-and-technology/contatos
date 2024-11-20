# Apply shared configurations
kubectl apply -f k8s/shared/configmap.yml

# Apply SQL Server configurations
kubectl apply -f k8s/mssql/persistenceVolume.yml
kubectl apply -f k8s/mssql/persistenceVolumeClaim.yml
kubectl apply -f k8s/mssql/deployment.yml
kubectl apply -f k8s/mssql/service.yml

# Wait for SQL Server to be ready
Write-Host "Waiting for SQL Server to be ready..."
kubectl wait --for=condition=available --timeout=600s deployment/mssql

# Display current date and time
Write-Host "Waiting 10 seconds for database to be UP: $(Get-Date)"

# Wait for 10 seconds to ensure SQL Server is fully up
Start-Sleep -Seconds 10

# Execute SQL script to create database structure
Write-Host "Creating database structure $(Get-Date)..."
Invoke-Sqlcmd -ConnectionString "Server=localhost,1433;Database=master;User ID=sa;Password=sql@123456;TrustServerCertificate=True" -InputFile ".\k8s\create-database.sql"

# Apply Redis configurations
kubectl apply -f k8s/redis/deployment.yml
kubectl apply -f k8s/redis/service.yml

# Apply RabbitMQ configurations
kubectl apply -f k8s/rabbitmq/deployment.yml
kubectl apply -f k8s/rabbitmq/service.yml

# Wait for RabbitMQ to be ready
Write-Host "Waiting for RabbitMQ to be ready..."
kubectl wait --for=condition=available --timeout=600s deployment/rabbitmq

# Apply Prometheus configurations
kubectl apply -f k8s/monitoring/prometheus/persistenceVolume.yml
kubectl apply -f k8s/monitoring/prometheus/persistenceVolumeClaim.yml
kubectl apply -f k8s/monitoring/prometheus/deployment.yml
kubectl apply -f k8s/monitoring/prometheus/service.yml
kubectl apply -f k8s/monitoring/prometheus/prometheus.yml

# Apply Grafana configurations
kubectl apply -f k8s/monitoring/grafana/grafana-dashboards-configmap.yml  
kubectl apply -f k8s/monitoring/grafana/grafana-datasources-configmap.yml  
kubectl apply -f k8s/monitoring/grafana/grafana-provisioning.yml 
kubectl apply -f k8s/monitoring/grafana/deployment.yml
kubectl apply -f k8s/monitoring/grafana/service.yml

# Apply Node Exporter configurations
kubectl apply -f k8s/monitoring/node/deployment.yml
kubectl apply -f k8s/monitoring/node/service.yml

# Apply API configurations
kubectl apply -f k8s/api/consulting-api/deployment.yml
kubectl apply -f k8s/api/consulting-api/service.yml
kubectl apply -f k8s/api/create-api/deployment.yml
kubectl apply -f k8s/api/create-api/service.yml
kubectl apply -f k8s/api/delete-api/deployment.yml
kubectl apply -f k8s/api/delete-api/service.yml
kubectl apply -f k8s/api/update-api/deployment.yml
kubectl apply -f k8s/api/update-api/service.yml

# Apply Worker configurations
kubectl apply -f k8s/worker/create-worker/deployment.yml
kubectl apply -f k8s/worker/create-worker/service.yml
kubectl apply -f k8s/worker/delete-worker/deployment.yml
kubectl apply -f k8s/worker/delete-worker/service.yml
kubectl apply -f k8s/worker/update-worker/deployment.yml
kubectl apply -f k8s/worker/update-worker/service.yml

Write-Host "All deployments have been applied successfully."

# Check if API and Worker pods are running
Write-Host "Checking if API and Worker pods are running..."

$maxAttempts = 10
$attempt = 0
$allPodsReady = $false

while ($attempt -lt $maxAttempts -and -not $allPodsReady) {
    $attempt++
    Write-Host "Attempt $attempt of $maxAttempts..."

    # List all pods and their status
    $pods = kubectl get pods -o json | ConvertFrom-Json

    # Filter API and Worker pods
    $apiPods = $pods.items | Where-Object { $_.metadata.labels.app -match "api" }
    $workerPods = $pods.items | Where-Object { $_.metadata.labels.app -match "worker" }

    # Check status of API and Worker pods
    $pendingPods = $false
    foreach ($pod in $apiPods + $workerPods) {
        Write-Host "Pod: $($pod.metadata.name) - Status: $($pod.status.phase)"
        if ($pod.status.phase -eq "Pending") {
            $pendingPods = $true
        }
    }

    if (-not $pendingPods) {
        $allPodsReady = $true
    } else {
        Write-Host "Some pods are still pending. Waiting for 5 seconds before retrying..."
        Start-Sleep -Seconds 5
    }
}

if ($allPodsReady) {
    Write-Host "All API and Worker pods are running."
} else {
    Write-Host "Some API or Worker pods are still pending after $maxAttempts attempts."
}

Write-Host "Pod status check completed."
