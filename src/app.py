from flask import Flask
from kubernetes import client, config

config.load_incluster_config()

app = Flask(__name__)

@app.route('/kuber/pods')
def get_pods():
    config.load_incluster_config()

    v1 = client.CoreV1Api()
    pods_list = v1.list_namespaced_pod(namespace='app', watch=False)
    pods = [item.metadata.name for item in pods_list.items]
    return pods

@app.route('/kuber/<namespace>/pods')
def get_namespaced_pods(namespace):
    config.load_incluster_config()

    v1 = client.CoreV1Api()
    pods_list = v1.list_namespaced_pod(namespace=namespace, watch=False)
    namespaced_pods = [item.metadata.name for item in pods_list.items]
    return namespaced_pods

@app.route('/kuber/logs/<pod_name>')
def get_logs(pod_name):
    config.load_incluster_config()
    
    v1 = client.CoreV1Api()
    pod_logs = v1.read_namespaced_pod_log(name=pod_name, namespace='app')
    return pod_logs

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=8000)