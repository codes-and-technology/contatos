function Delete-Resource {
    param (
        [string]$ResourcePath
    )
    # Verifica se o recurso existe
    if (kubectl get -f $ResourcePath -o name > $null 2>&1) {
        Write-Host "Deleting resource: $ResourcePath"
        kubectl delete -f $ResourcePath
    } else {
        Write-Host "Resource not found: $ResourcePath"
    }
}

# Delete SQL Server configurations
Write-Host "Deleting SQL Server configurations..."
Delete-Resource -ResourcePath "k8s/mssql/service.yml"
Delete-Resource -ResourcePath "k8s/mssql/deployment.yml"
Delete-Resource -ResourcePath "k8s/mssql/persistenceVolumeClaim.yml"
Delete-Resource -ResourcePath "k8s/mssql/persistenceVolume.yml"

# Delete Redis configurations
Write-Host "Deleting Redis configurations..."
Delete-Resource -ResourcePath "k8s/redis/service.yml"
Delete-Resource -ResourcePath "k8s/redis/deployment.yml"

# Delete RabbitMQ configurations
Write-Host "Deleting RabbitMQ configurations..."
Delete-Resource -ResourcePath "k8s/rabbitmq/service.yml"
Delete-Resource -ResourcePath "k8s/rabbitmq/deployment.yml"

# Delete Prometheus configurations
Write-Host "Deleting Prometheus configurations..."
Delete-Resource -ResourcePath "k8s/monitoring/prometheus/service.yml"
Delete-Resource -ResourcePath "k8s/monitoring/prometheus/deployment.yml"
Delete-Resource -ResourcePath "k8s/monitoring/prometheus/persistenceVolumeClaim.yml"
Delete-Resource -ResourcePath "k8s/monitoring/prometheus/persistenceVolume.yml"
Delete-Resource -ResourcePath "k8s/monitoring/prometheus/prometheus.yml"

# Delete Grafana configurations
Write-Host "Deleting Grafana configurations..."
Delete-Resource -ResourcePath "k8s/monitoring/grafana/service.yml"
Delete-Resource -ResourcePath "k8s/monitoring/grafana/deployment.yml"
Delete-Resource -ResourcePath "k8s/monitoring/grafana/grafana-dashboards-configmap.yml"
Delete-Resource -ResourcePath "k8s/monitoring/grafana/grafana-datasources-configmap.yml"
Delete-Resource -ResourcePath "k8s/monitoring/grafana/grafana-provisioning.yml"

# Delete Node Exporter configurations
Write-Host "Deleting Node Exporter configurations..."
Delete-Resource -ResourcePath "k8s/monitoring/node/service.yml"
Delete-Resource -ResourcePath "k8s/monitoring/node/deployment.yml"

# Delete API configurations
Write-Host "Deleting API configurations..."
Delete-Resource -ResourcePath "k8s/api/consulting-api/service.yml"
Delete-Resource -ResourcePath "k8s/api/consulting-api/deployment.yml"
Delete-Resource -ResourcePath "k8s/api/create-api/service.yml"
Delete-Resource -ResourcePath "k8s/api/create-api/deployment.yml"
Delete-Resource -ResourcePath "k8s/api/delete-api/service.yml"
Delete-Resource -ResourcePath "k8s/api/delete-api/deployment.yml"
Delete-Resource -ResourcePath "k8s/api/update-api/service.yml"
Delete-Resource -ResourcePath "k8s/api/update-api/deployment.yml"

# Delete Worker configurations
Write-Host "Deleting Worker configurations..."
Delete-Resource -ResourcePath "k8s/worker/create-worker/service.yml"
Delete-Resource -ResourcePath "k8s/worker/create-worker/deployment.yml"
Delete-Resource -ResourcePath "k8s/worker/delete-worker/service.yml"
Delete-Resource -ResourcePath "k8s/worker/delete-worker/deployment.yml"
Delete-Resource -ResourcePath "k8s/worker/update-worker/service.yml"
Delete-Resource -ResourcePath "k8s/worker/update-worker/deployment.yml"

Write-Host "All resources have been processed."
