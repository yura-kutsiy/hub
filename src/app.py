from flask import Flask, jsonify
from kubernetes import client, config
from flask_cors import CORS

config.load_incluster_config()

app = Flask(__name__)
CORS(app)

@app.route('/kuber/pods')
def get_pods():
    config.load_incluster_config()

    v1 = client.CoreV1Api()
    pods_list = v1.list_namespaced_pod(namespace='default', watch=False)
    pods_data = []
    for pod in pods_list.items:
        pod_dict = pod.to_dict()
        pod_data = {
            "name": pod_dict['metadata']['name'],
            "status": pod_dict['status']['phase'],
            "age": pod_dict['metadata']['creation_timestamp']
        }
        pods_data.append(pod_data)
    return jsonify(pods_data)

@app.route('/kuber/<namespace>/pods')
def get_namespaced_pods(namespace):
    config.load_incluster_config()

    v1 = client.CoreV1Api()
    pods_list = v1.list_namespaced_pod(namespace=namespace, watch=False)
    namespaced_pods = [item.metadata.name for item in pods_list.items]
    return namespaced_pods

@app.route('/kuber/<namespace>/<pod_name>/logs')
def get_logs(pod_name, namespace):
    config.load_incluster_config()
    
    v1 = client.CoreV1Api()
    pod_logs = v1.read_namespaced_pod_log(name=pod_name, namespace=namespace)
    return pod_logs

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=8000)