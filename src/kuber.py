from kubernetes import client, config

config.load_incluster_config()

def get_pods():
    config.load_incluster_config()

    v1 = client.CoreV1Api()
    pods_list = v1.list_namespaced_pod(namespace='app', watch=False)
    pods = [item.metadata.name for item in pods_list.items]
    print(pods)
    # return pods
get_pods()
def get_namespaced_pods(namespace):
    config.load_incluster_config()

    v1 = client.CoreV1Api()
    pods_list = v1.list_namespaced_pod(f"namespace='{namespace}', watch=False")
    namespaced_pods = [item.metadata.name for item in pods_list.items]
    print(namespaced_pods)
    # return namespaced_pods

get_namespaced_pods(namespace='kube-system')