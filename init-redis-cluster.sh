#!/bin/bash

# Ждём, пока все узлы Redis станут доступны
echo "Waiting for Redis nodes to be ready..."
for port in 6379 6380 6381 6382; do
  until redis-cli -h redis-$(( ($port - 6379) + 1 )) -p $port ping | grep -q PONG; do
    echo "Waiting for pilot-redis-$(( ($port - 6379) + 1 )):$port..."
    sleep 1
  done
  echo "pilot-redis-$(( ($port - 6379) + 1 )):$port is up"
done

# Проверяем, существует ли кластер
if redis-cli -h pilot-redis-1 -p 6379 cluster info | grep -q "cluster_state:ok"; then
  echo "Redis Cluster already initialized"
  exit 0
fi

# Создаём кластер
echo "Creating Redis Cluster..."
redis-cli --cluster create \
  pilot-redis-1:6379 pilot-redis-2:6380 pilot-redis-3:6381 pilot-redis-4:6382 \
  --cluster-replicas 1 --cluster-yes

echo "Redis Cluster initialized successfully"