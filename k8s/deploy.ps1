# Set-StrictMode -Version Latest
Set-StrictMode -Version Latest

# Function to delete a resource if it exists
function Delete-IfExists {
    param (
        [string]$resourceType,
        [string]$filePath
    )
    $resourceName = (Get-Content $filePath | Select-String -Pattern 'name:' | Select-Object -First 1).ToString().Split(':')[1].Trim()
    if (kubectl get $resourceType $resourceName -n default -o name -ErrorAction SilentlyContinue) {
        kubectl delete -f $filePath
    }
}

# Delete existing configurations
Delete-IfExists -resourceType "deployment" -filePath "k8s/mssql/deployment.yml"
Delete-IfExists -resourceType "service" -filePath "k8s/mssql/service.yml"
Delete-IfExists -resourceType "persistentvolume" -filePath "k8s/mssql/persistenceVolume.yml"
Delete-IfExists -resourceType "persistentvolumeclaim" -filePath "k8s/mssql/persistenceVolumeClaim.yml"
Delete-IfExists -resourceType "deployment" -filePath "k8s/redis/deployment.yml"
Delete-IfExists -resourceType "service" -filePath "k8s/redis/service.yml"
Delete-IfExists -resourceType "deployment" -filePath "k8s/rabbitmq/deployment.yml"
Delete-IfExists -resourceType "service" -filePath "k8s/rabbitmq/service.yml"
Delete-IfExists -resourceType "deployment" -filePath "k8s/api/consulting-api/deployment.yml"
Delete-IfExists -resourceType "service" -filePath "k8s/api/consulting-api/service.yml"
Delete-IfExists -resourceType "deployment" -filePath "k8s/api/create-api/deployment.yml"
Delete-IfExists -resourceType "service" -filePath "k8s/api/create-api/service.yml"
Delete-IfExists -resourceType "deployment" -filePath "k8s/api/delete-api/deployment.yml"
Delete-IfExists -resourceType "service" -filePath "k8s/api/delete-api/service.yml"
Delete-IfExists -resourceType "deployment" -filePath "k8s/api/update-api/deployment.yml"
Delete-IfExists -resourceType "service" -filePath "k8s/api/update-api/service.yml"
Delete-IfExists -resourceType "deployment" -filePath "k8s/worker/create-worker/deployment.yml"
Delete-IfExists -resourceType "service" -filePath "k8s/worker/create-worker/service.yml"
Delete-IfExists -resourceType "deployment" -filePath "k8s/worker/delete-worker/deployment.yml"
Delete-IfExists -resourceType "service" -filePath "k8s/worker/delete-worker/service.yml"
Delete-IfExists -resourceType "deployment" -filePath "k8s/worker/update-worker/deployment.yml"
Delete-IfExists -resourceType "service" -filePath "k8s/worker/update-worker/service.yml"
Delete-IfExists -resourceType "deployment" -filePath "k8s/monitoring/prometheus/deployment.yml"
Delete-IfExists -resourceType "service" -filePath "k8s/monitoring/prometheus/service.yml"
Delete-IfExists -resourceType "persistentvolume" -filePath "k8s/monitoring/prometheus/persistenceVolume.yml"
Delete-IfExists -resourceType "persistentvolumeclaim" -filePath "k8s/monitoring/prometheus/persistenceVolumeClaim.yml"
Delete-IfExists -resourceType "configmap" -filePath "k8s/monitoring/prometheus/prometheus.yml"
Delete-IfExists -resourceType "deployment" -filePath "k8s/monitoring/grafana/deployment.yml"
Delete-IfExists -resourceType "service" -filePath "k8s/monitoring/grafana/service.yml"
Delete-IfExists -resourceType "deployment" -filePath "k8s/monitoring/node/deployment.yml"
Delete-IfExists -resourceType "service" -filePath "k8s/monitoring/node/service.yml"

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