using Microsoft.AspNetCore.Mvc;
using Services;

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

    }
}
