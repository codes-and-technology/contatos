# Delete SQL Server configurations
Write-Host "Deleting SQL Server configurations..."
kubectl delete -f k8s/mssql/service.yml
kubectl delete -f k8s/mssql/deployment.yml
kubectl delete -f k8s/mssql/persistenceVolumeClaim.yml
kubectl delete -f k8s/mssql/persistenceVolume.yml

# Delete Redis configurations
Write-Host "Deleting Redis configurations..."
kubectl delete -f k8s/redis/service.yml
kubectl delete -f k8s/redis/deployment.yml

# Delete RabbitMQ configurations
Write-Host "Deleting RabbitMQ configurations..."
kubectl delete -f k8s/rabbitmq/service.yml
kubectl delete -f k8s/rabbitmq/deployment.yml

# Delete Prometheus configurations
Write-Host "Deleting Prometheus configurations..."
kubectl delete -f k8s/monitoring/prometheus/service.yml
kubectl delete -f k8s/monitoring/prometheus/deployment.yml
kubectl delete -f k8s/monitoring/prometheus/persistenceVolumeClaim.yml
kubectl delete -f k8s/monitoring/prometheus/persistenceVolume.yml
kubectl delete -f k8s/monitoring/prometheus/prometheus.yml

# Delete Grafana configurations
Write-Host "Deleting Grafana configurations..."
kubectl delete -f k8s/monitoring/grafana/service.yml
kubectl delete -f k8s/monitoring/grafana/deployment.yml
kubectl delete -f k8s/monitoring/grafana/grafana-dashboards-configmap.yml
kubectl delete -f k8s/monitoring/grafana/grafana-datasources-configmap.yml
kubectl delete -f k8s/monitoring/grafana/grafana-provisioning.yml

# Delete Node Exporter configurations
Write-Host "Deleting Node Exporter configurations..."
kubectl delete -f k8s/monitoring/node/service.yml
kubectl delete -f k8s/monitoring/node/deployment.yml

# Delete API configurations
Write-Host "Deleting API configurations..."
kubectl delete -f k8s/api/consulting-api/service.yml
kubectl delete -f k8s/api/consulting-api/deployment.yml
kubectl delete -f k8s/api/create-api/service.yml
kubectl delete -f k8s/api/create-api/deployment.yml
kubectl delete -f k8s/api/delete-api/service.yml
kubectl delete -f k8s/api/delete-api/deployment.yml
kubectl delete -f k8s/api/update-api/service.yml
kubectl delete -f k8s/api/update-api/deployment.yml

# Delete Worker configurations
Write-Host "Deleting Worker configurations..."
kubectl delete -f k8s/worker/create-worker/service.yml
kubectl delete -f k8s/worker/create-worker/deployment.yml
kubectl delete -f k8s/worker/delete-worker/service.yml
kubectl delete -f k8s/worker/delete-worker/deployment.yml
kubectl delete -f k8s/worker/update-worker/service.yml
kubectl delete -f k8s/worker/update-worker/deployment.yml

Write-Host "All resources have been deleted successfully."
