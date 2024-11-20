# Função Inline para verificar e deletar recursos
Write-Host "Starting resource cleanup..."

# Função para verificar e deletar um recurso baseado no arquivo YAML
function Delete-IfExists {
    param ([string]$YamlFilePath)
    if (Test-Path $YamlFilePath) {
        $resourceExists = kubectl get -f $YamlFilePath -o name > $null 2>&1
        if ($LASTEXITCODE -eq 0) {
            Write-Host "Deleting resource from: $YamlFilePath"
            kubectl delete -f $YamlFilePath
        } else {
            Write-Host "Resource not found: $YamlFilePath"
        }
    } else {
        Write-Host "YAML file not found: $YamlFilePath"
    }
}

# Deletar Configurações do SQL Server
Write-Host "Deleting SQL Server configurations..."
Delete-IfExists "k8s/mssql/service.yml"
Delete-IfExists "k8s/mssql/deployment.yml"
Delete-IfExists "k8s/mssql/persistenceVolumeClaim.yml"
Delete-IfExists "k8s/mssql/persistenceVolume.yml"

# Deletar Configurações do Redis
Write-Host "Deleting Redis configurations..."
Delete-IfExists "k8s/redis/service.yml"
Delete-IfExists "k8s/redis/deployment.yml"

# Deletar Configurações do RabbitMQ
Write-Host "Deleting RabbitMQ configurations..."
Delete-IfExists "k8s/rabbitmq/service.yml"
Delete-IfExists "k8s/rabbitmq/deployment.yml"

# Deletar Configurações do Prometheus
Write-Host "Deleting Prometheus configurations..."
Delete-IfExists "k8s/monitoring/prometheus/service.yml"
Delete-IfExists "k8s/monitoring/prometheus/deployment.yml"
Delete-IfExists "k8s/monitoring/prometheus/persistenceVolumeClaim.yml"
Delete-IfExists "k8s/monitoring/prometheus/persistenceVolume.yml"
Delete-IfExists "k8s/monitoring/prometheus/prometheus.yml"

# Deletar Configurações do Grafana
Write-Host "Deleting Grafana configurations..."
Delete-IfExists "k8s/monitoring/grafana/service.yml"
Delete-IfExists "k8s/monitoring/grafana/deployment.yml"
Delete-IfExists "k8s/monitoring/grafana/grafana-dashboards-configmap.yml"
Delete-IfExists "k8s/monitoring/grafana/grafana-datasources-configmap.yml"
Delete-IfExists "k8s/monitoring/grafana/grafana-provisioning.yml"

# Deletar Configurações do Node Exporter
Write-Host "Deleting Node Exporter configurations..."
Delete-IfExists "k8s/monitoring/node/service.yml"
Delete-IfExists "k8s/monitoring/node/deployment.yml"

# Deletar Configurações da API
Write-Host "Deleting API configurations..."
Delete-IfExists "k8s/api/consulting-api/service.yml"
Delete-IfExists "k8s/api/consulting-api/deployment.yml"
Delete-IfExists "k8s/api/create-api/service.yml"
Delete-IfExists "k8s/api/create-api/deployment.yml"
Delete-IfExists "k8s/api/delete-api/service.yml"
Delete-IfExists "k8s/api/delete-api/deployment.yml"
Delete-IfExists "k8s/api/update-api/service.yml"
Delete-IfExists "k8s/api/update-api/deployment.yml"

# Deletar Configurações dos Workers
Write-Host "Deleting Worker configurations..."
Delete-IfExists "k8s/worker/create-worker/service.yml"
Delete-IfExists "k8s/worker/create-worker/deployment.yml"
Delete-IfExists "k8s/worker/delete-worker/service.yml"
Delete-IfExists "k8s/worker/delete-worker/deployment.yml"
Delete-IfExists "k8s/worker/update-worker/service.yml"
Delete-IfExists "k8s/worker/update-worker/deployment.yml"

Write-Host "Resource cleanup completed."
