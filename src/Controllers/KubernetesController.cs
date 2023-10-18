using Microsoft.AspNetCore.Mvc;
using Services;
using k8s;
using Config;

namespace kuberApi.Controllers
{
    [ApiController]
    [Route("kuber/")]
    public class KubernetesController : ControllerBase
    {
        private readonly KubernetesServices _kubernetesService;

        public KubernetesController(KubernetesServices kubernetesService)
        {
            _kubernetesService = kubernetesService;
        }

        // GET api/kuber/services
        [HttpGet("services")]
        public ActionResult<IEnumerable<string>> GetKubernetesServices()
        {
            // Logic to retrieve information about Kubernetes services
            var services = _kubernetesService.GetKubernetesServicesAsync().Result; // Synchronously waiting for the result (avoid this in production)
            return Ok(services);
        }

        // GET api/kuber/services/all
        [HttpGet("services/nodePorts")]
        public ActionResult<IEnumerable<string>> GetKubernetesServicesNodePorts()
        {
            // Logic to retrieve information about Kubernetes services
            var services = _kubernetesService.GetKubernetesServicesNodePortsAsync().Result; // Synchronously waiting for the result (avoid this in production)
            return Ok(services);
        }

        // GET api/kuber/nodes
        [HttpGet("nodes/nodesIP")]
        public async Task<ActionResult<IEnumerable<string>>> GetKubernetesNodes()
        {
            KubernetesClientConfiguration config = KubernetesConfig.GetConfiguration();
            var client = new Kubernetes(config);

            var nodeList = await client.ListNodeAsync();
            var nodeIps = new List<string>();

            foreach (var node in nodeList.Items)
            {
                foreach (var address in node.Status.Addresses)
                {
                    if (address.Type == "InternalIP")
                    {
                        nodeIps.Add(address.Address);
                    }
                }
            }

            return Ok(nodeIps);
        }

        // GET api/kuber/{namespace}/pods
        [HttpGet("{namespace}/pods")]
        public async Task<ActionResult<IEnumerable<string>>> GetKubernetesPods(string @namespace)
        {
            KubernetesClientConfiguration config = KubernetesConfig.GetConfiguration();
            var client = new Kubernetes(config);

            var podList = await client.ListNamespacedPodAsync(@namespace);
            var podNames = new List<string>();

            foreach (var pod in podList.Items)
            {
                podNames.Add(pod.Metadata.Name);
            }

            return Ok(podNames);
        }

    }
}