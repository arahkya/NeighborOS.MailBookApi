# apply deployment file to k8s
kubectl apply -f ../../deployment.yaml

# this use for get all deployment in k8s
# kubectl get deployments

# delete deployment
# kubectl delete deployment mailbook-api-depl

# get service that run in k8s such as Node Port (np) -- Example in file nodepod.yaml
# kubectl get services