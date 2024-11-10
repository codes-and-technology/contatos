@echo off
setlocal

REM Delete SQL Server configurations
kubectl delete -f sql\mssql-deployment.yml
kubectl delete -f sql\mssql-service.yml
kubectl delete -f sql\persistenceVolume.yml
kubectl delete -f sql\persistenceVolumeClaim.yml

REM Delete Redis configurations
kubectl delete -f redis\redis-deployment.yml

REM Delete RabbitMQ configurations
kubectl delete -f rabbitmq\rabbitmq-deployment.yml

REM Delete API configurations
kubectl delete -f api\consulting-api-deployment.yml
kubectl delete -f api\create-api-deployment.yml
kubectl delete -f api\delete-api-deployment.yml
kubectl delete -f api\update-api-deployment.yml

REM Delete Worker configurations
kubectl delete -f worker\worker-create-deployment.yml
kubectl delete -f worker\worker-delete-deployment.yml
kubectl delete -f worker\worker-update-deployment.yml

REM Delete Prometheus configurations
kubectl delete -f monitoring\prometheus-deployment.yml

REM Delete Grafana configurations
kubectl delete -f monitoring\grafana-deployment.yml

REM Delete Node Exporter configurations
kubectl delete -f monitoring\node-exporter-deployment.yml

REM Apply SQL Server configurations
kubectl apply -f sql\persistenceVolume.yml
kubectl apply -f sql\persistenceVolumeClaim.yml
kubectl apply -f sql\mssql-deployment.yml
kubectl apply -f sql\mssql-service.yml

REM Wait for SQL Server to be ready
echo Waiting for SQL Server to be ready...
kubectl wait --for=condition=available --timeout=600s deployment/mssql

REM Apply Redis configurations
kubectl apply -f redis\redis-deployment.yml

REM Wait for Redis to be ready
echo Waiting for Redis to be ready...
kubectl wait --for=condition=available --timeout=600s deployment/redis

REM Apply RabbitMQ configurations
kubectl apply -f rabbitmq\rabbitmq-deployment.yml

REM Wait for RabbitMQ to be ready
echo Waiting for RabbitMQ to be ready...
kubectl wait --for=condition=available --timeout=600s deployment/rabbitmq

REM Apply API configurations
kubectl apply -f api\consulting-api-deployment.yml
kubectl apply -f api\create-api-deployment.yml
kubectl apply -f api\delete-api-deployment.yml
kubectl apply -f api\update-api-deployment.yml

REM Wait for APIs to be ready
echo Waiting for APIs to be ready...
kubectl wait --for=condition=available --timeout=600s deployment/consulting-api
kubectl wait --for=condition=available --timeout=600s deployment/create-api
kubectl wait --for=condition=available --timeout=600s deployment/delete-api
kubectl wait --for=condition=available --timeout=600s deployment/update-api

REM Apply Worker configurations
kubectl apply -f worker\worker-create-deployment.yml
kubectl apply -f worker\worker-delete-deployment.yml
kubectl apply -f worker\worker-update-deployment.yml

REM Wait for Workers to be ready
echo Waiting for Workers to be ready...
kubectl wait --for=condition=available --timeout=600s deployment/worker-create
kubectl wait --for=condition=available --timeout=600s deployment/worker-delete
kubectl wait --for=condition=available --timeout=600s deployment/worker-update

REM Apply Prometheus configurations
kubectl apply -f monitoring\prometheus-deployment.yml

REM Apply Grafana configurations
kubectl apply -f monitoring\grafana-deployment.yml

REM Apply Node Exporter configurations
kubectl apply -f monitoring\node-exporter-deployment.yml

echo All deployments have been applied successfully.
endlocal
