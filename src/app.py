from flask import Flask, jsonify, send_from_directory
from kubernetes import client, config
from flask_cors import CORS
import datetime
from flask_caching import Cache
import os

config.load_incluster_config()

app = Flask(__name__)
CORS(app)
cache = Cache(app, config={'CACHE_TYPE': 'null'})

@app.route('/kuber/pods')
@cache.cached(timeout=0)
def get_pods():
    config.load_incluster_config()
    v1 = client.CoreV1Api()

    pods_list = v1.list_namespaced_pod(namespace='app', watch=False) # + "include_uninitialized=True" for terminating state, with k3s doesn't work (error)
    pods = []
    for item in pods_list.items:
        pod = {
            'name': item.metadata.name,
            'status': item.status.phase,
            'restarts': item.status.container_statuses[0].restart_count,
            'age': (datetime.datetime.utcnow().replace(tzinfo=datetime.timezone.utc) - item.status.start_time).total_seconds()
        }
        pods.append(pod)
    return jsonify(pods), 200, {'Cache-Control': 'public, max-age=0'}

@app.route('/kuber/<namespace>/pods')
@cache.cached(timeout=0)
def get_namespaced_pods(namespace):
    config.load_incluster_config()
    v1 = client.CoreV1Api()

    pods_list = v1.list_namespaced_pod(namespace=namespace, watch=False) #  + "include_uninitialized=True" for terminating state, with k3s doesn't work (error)
    pods = []
    for item in pods_list.items:
        pod = {
            'name': item.metadata.name,
            'status': item.status.phase,
            'restarts': item.status.container_statuses[0].restart_count,
            'age': (datetime.datetime.utcnow().replace(tzinfo=datetime.timezone.utc) - item.status.start_time).total_seconds()
        }
        pods.append(pod)
    return jsonify(pods), 200, {'Cache-Control': 'public, max-age=0'}

@app.route('/kuber/<namespace>/<pod_name>/logs')
@cache.cached(timeout=0)
def get_logs(pod_name, namespace):
    config.load_incluster_config()
    
    v1 = client.CoreV1Api()
    pod_logs = v1.read_namespaced_pod_log(name=pod_name, namespace=namespace)
    return pod_logs, 200, {'Cache-Control': 'public, max-age=0'}

@app.route('/kuber/namespaces')
def get_namespaces():
    with open(os.path.join(os.getcwd(), 'namespaces.json'), 'r') as f:
        namespace = f.read()
    return namespace

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=8000)
