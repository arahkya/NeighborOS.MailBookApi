# NeighborOS

## Git
### Clean Cache
After modified .gitignore file. You need to cleanup cache in git to make .gitignore file effective by run this command 
> git rm --cached -r .


## K8S
### Apply deployment file to k8s
> kubectl apply -f ../../deployment.yaml

### This use for get all deployment in k8s
> kubectl get deployments

### Delete deployment
> kubectl delete deployment mailbook-api-depl

### Get service that run in k8s such as Node Port (np) -- Example in file nodepod.yaml
> kubectl get services

### Restart deployment and also pods and container within
> kubectl rollout restart deployment mailbook-api-depl

### create secret
> kubectl create secret generic $(secret_name) --from-literal=SA_PASSWORD="$(secret_value)"
**Example**
> kubectl create secret generic mailbook-db-secret --from-literal=SA_PASSWORD="vkiydKN8986"

### get pod event
> kubectl describe pod $(pod_name)
**Example**
> kubectl describe pod mailbook-db-depl-7d68858566-5hrvf

### Execute command in pod
> kubectl exec mailbook-db-depl-65d6f57db7-p6lnf -- /opt/mssql-tools/bin/sqlcmd -S . -U sa -P vkiydKN8986 -Q "select Name from sys.Databases"

## References
[.NET Microservices – Full Course by Les Jackson](https://www.youtube.com/watch?v=DgVjEo3OGBI)