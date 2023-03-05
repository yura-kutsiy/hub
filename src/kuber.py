from kubernetes import client, config

config.load_incluster_config()

def get_namespace():
    config.load_incluster_config()

    v1 = client.CoreV1Api()
    print("Listing pods with their IPs:")
    ret = v1.list_namespaced_pod(namespace='app', watch=False)
    for i in ret.items:
        print("%s\t%s\t%s" %
              (i.status.pod_ip, i.metadata.namespace, i.metadata.name))
        print("Info: Pod name is -> " + i.metadata.name)
        print("Info: Pod's logs:")
        print(v1.read_namespaced_pod_log(i.metadata.name, 'app'))

if __name__ == '__main__':
    get_namespace()