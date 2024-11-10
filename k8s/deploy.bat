@echo off
setlocal

REM Delete existing configurations
kubectl delete -f k8s/mssql/deployment.yml
kubectl delete -f k8s/mssql/service.yml
kubectl delete -f k8s/mssql/persistenceVolume.yml
kubectl delete -f k8s/mssql/persistenceVolumeClaim.yml
kubectl delete -f k8s/redis/deployment.yml
kubectl delete -f k8s/redis/service.yml
kubectl delete -f k8s/rabbitmq/deployment.yml
kubectl delete -f k8s/rabbitmq/service.yml
kubectl delete -f k8s/api/consulting-api/deployment.yml
kubectl delete -f k8s/api/consulting-api/service.yml
kubectl delete -f k8s/api/create-api/deployment.yml
kubectl delete -f k8s/api/create-api/service.yml
kubectl delete -f k8s/api/delete-api/deployment.yml
kubectl delete -f k8s/api/delete-api/service.yml
kubectl delete -f k8s/api/update-api/deployment.yml
kubectl delete -f k8s/api/update-api/service.yml
kubectl delete -f k8s/worker/create-worker/deployment.yml
kubectl delete -f k8s/worker/create-worker/service.yml
kubectl delete -f k8s/worker/delete-worker/deployment.yml
kubectl delete -f k8s/worker/delete-worker/service.yml
kubectl delete -f k8s/worker/update-worker/deployment.yml
kubectl delete -f k8s/worker/update-worker/service.yml
kubectl delete -f k8s/monitoring/prometheus/deployment.yml
kubectl delete -f k8s/monitoring/prometheus/service.yml
kubectl delete -f k8s/monitoring/prometheus/persistenceVolume.yml
kubectl delete -f k8s/monitoring/prometheus/persistenceVolumeClaim.yml
kubectl delete -f k8s/monitoring/prometheus/prometheus.yml
kubectl delete -f k8s/monitoring/grafana/deployment.yml
kubectl delete -f k8s/monitoring/grafana/service.yml
kubectl delete -f k8s/monitoring/node/deployment.yml
kubectl delete -f k8s/monitoring/node/service.yml

REM Apply shared configurations
kubectl apply -f k8s/shared/configmap.yml

REM Apply SQL Server configurations
kubectl apply -f k8s/mssql/persistenceVolume.yml
kubectl apply -f k8s/mssql/persistenceVolumeClaim.yml
kubectl apply -f k8s/mssql/deployment.yml
kubectl apply -f k8s/mssql/service.yml

REM Wait for SQL Server to be ready
echo Waiting for SQL Server to be ready...
kubectl wait --for=condition=available --timeout=600s deployment/mssql

REM Apply Redis configurations
kubectl apply -f k8s/redis/deployment.yml
kubectl apply -f k8s/redis/service.yml

REM Apply RabbitMQ configurations
kubectl apply -f k8s/rabbitmq/deployment.yml
kubectl apply -f k8s/rabbitmq/service.yml

REM Wait for RabbitMQ to be ready
echo Waiting for RabbitMQ to be ready...
kubectl wait --for=condition=available --timeout=600s deployment/rabbitmq

REM Apply Prometheus configurations
kubectl apply -f k8s/monitoring/prometheus/persistenceVolume.yml
kubectl apply -f k8s/monitoring/prometheus/persistenceVolumeClaim.yml
kubectl apply -f k8s/monitoring/prometheus/deployment.yml
kubectl apply -f k8s/monitoring/prometheus/service.yml
kubectl apply -f k8s/monitoring/prometheus/prometheus.yml

REM Apply Grafana configurations
kubectl apply -f k8s/monitoring/grafana/deployment.yml
kubectl apply -f k8s/monitoring/grafana/service.yml

REM Apply Node Exporter configurations
kubectl apply -f k8s/monitoring/node/deployment.yml
kubectl apply -f k8s/monitoring/node/service.yml

REM Apply API configurations
kubectl apply -f k8s/api/consulting-api/deployment.yml
kubectl apply -f k8s/api/consulting-api/service.yml
kubectl apply -f k8s/api/create-api/deployment.yml
kubectl apply -f k8s/api/create-api/service.yml
kubectl apply -f k8s/api/delete-api/deployment.yml
kubectl apply -f k8s/api/delete-api/service.yml
kubectl apply -f k8s/api/update-api/deployment.yml
kubectl apply -f k8s/api/update-api/service.yml

REM Wait for APIs to be ready
echo Waiting for APIs to be ready...
kubectl wait --for=condition=available --timeout=600s deployment/consulting-api
kubectl wait --for=condition=available --timeout=600s deployment/create-api
kubectl wait --for=condition=available --timeout=600s deployment/delete-api
kubectl wait --for=condition=available --timeout=600s deployment/update-api

REM Apply Worker configurations
kubectl apply -f k8s/worker/create-worker/deployment.yml
kubectl apply -f k8s/worker/create-worker/service.yml
kubectl apply -f k8s/worker/delete-worker/deployment.yml
kubectl apply -f k8s/worker/delete-worker/service.yml
kubectl apply -f k8s/worker/update-worker/deployment.yml
kubectl apply -f k8s/worker/update-worker/service.yml

REM Wait for Workers to be ready
echo Waiting for Workers to be ready...
kubectl wait --for=condition=available --timeout=600s deployment/worker-create
kubectl wait --for=condition=available --timeout=600s deployment/worker-delete
kubectl wait --for=condition=available --timeout=600s deployment/worker-update

echo All deployments have been applied successfully.
endlocal