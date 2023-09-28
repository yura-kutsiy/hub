from flask import Flask, jsonify, send_from_directory
from kubernetes import client, config
from flask_cors import CORS
import datetime
from flask_caching import Cache
import os

try:
    config.load_incluster_config()
except config.config_exception.ConfigException:
    config.load_kube_config()

app = Flask(__name__)
CORS(app)
cache = Cache(app, config={'CACHE_TYPE': 'null'})

@app.route('/kuber/pods')
@cache.cached(timeout=0)
def get_pods():
    # config.load_incluster_config()
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
    # config.load_incluster_config()
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
    # config.load_incluster_config()
    
    v1 = client.CoreV1Api()
    pod_logs = v1.read_namespaced_pod_log(name=pod_name, namespace=namespace)
    return pod_logs, 200, {'Cache-Control': 'public, max-age=0'}

@app.route('/kuber/namespaces')
def get_namespaces():
    with open(os.path.join(os.getcwd(), 'namespaces.json'), 'r') as f:
        namespace = f.read()
    return namespace

@app.route('/kuber/<namespace>/services')
@cache.cached(timeout=0)
def get_services(namespace):
    # config.load_incluster_config()
    v1 = client.CoreV1Api()

    services_list = v1.list_namespaced_service(namespace=namespace, watch=False)
    nodeport_services = []

    for item in services_list.items:
        if item.spec.type == "NodePort":
            service = {
                'name': item.metadata.name,
                'type': item.spec.type,
                'ports': [
                    {
                        'name': port.name,
                        'port': port.port,
                        'targetPort': port.target_port,
                        'nodePort': port.node_port,
                    }
                    for port in item.spec.ports
                ],
            }
            nodeport_services.append(service)

    return jsonify(nodeport_services), 200, {'Cache-Control': 'public, max-age=0'}

@app.route('/kuber/nodes/ips')
@cache.cached(timeout=0)
def get_nodes():
    # config.load_incluster_config()
    v1 = client.CoreV1Api()

    nodes_list = v1.list_node()
    node_ips = []

    for node in nodes_list.items:
        for address in node.status.addresses:
            if address.type == "InternalIP":
                node_ips.append(address.address)

    return jsonify(node_ips), 200, {'Cache-Control': 'public, max-age=0'}

@app.route('/kuber/nodeport-services')
@cache.cached(timeout=0)
def get_nodeport_services_and_nodes():
    v1 = client.CoreV1Api()
    apps_v1 = client.AppsV1Api()

    nodeport_services = []

    # List all services in the cluster
    services_list = v1.list_service_for_all_namespaces().items

    # Fetch the list of nodes and their IPs
    nodes_list = v1.list_node().items
    node_ips = {node.metadata.name: node.status.addresses[0].address for node in nodes_list if node.status.addresses}

    # Iterate through each service
    for service in services_list:
        # Check if the service is of type NodePort
        if service.spec.type == "NodePort":
            nodeport_service = {
                'namespace': service.metadata.namespace,
                'name': service.metadata.name,
                'node_ports': [serialize_service_port(port) for port in service.spec.ports],
                'nodes': [],
            }

            # List all pods in the same namespace as the service
            pods_list = v1.list_namespaced_pod(namespace=service.metadata.namespace).items

            # Iterate through each pod to find the node it's running on
            for pod in pods_list:
                nodeport_service_name = pod.metadata.name.split("-")[0]
                if nodeport_service_name == service.metadata.name:
                    # Add the node to the list of nodes associated with the service
                    nodeport_service['nodes'].append(pod.spec.node_name)

            # Determine the Node IP for each NodePort
            for node_name in nodeport_service['nodes']:
                for port in nodeport_service['node_ports']:
                    port['node_ip'] = node_ips.get(node_name, 'Unknown')

            nodeport_services.append(nodeport_service)

    return jsonify(nodeport_services), 200, {'Cache-Control': 'public, max-age=0'}



def serialize_service_port(service_port):
    return {
        'name': service_port.name,
        'port': service_port.port,
        'targetPort': service_port.target_port,
        'nodePort': service_port.node_port,
    }


if __name__ == '__main__':
    app.run(host='0.0.0.0', port=8000)
