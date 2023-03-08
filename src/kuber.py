from kubernetes import client, config

config.load_incluster_config()

def get_pods():
    config.load_incluster_config()

    v1 = client.CoreV1Api()
    pods_list = v1.list_namespaced_pod(namespace='app', watch=False)
    pods = [item.metadata.name for item in pods_list.items]
    return pods
