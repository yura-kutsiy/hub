from kubernetes import client, config

config.load_incluster_config()

def get_pods():
    config.load_incluster_config()

    v1 = client.CoreV1Api()
    pods = v1.list_namespaced_pod(namespace='app', watch=False)
    return pods

get_pods()