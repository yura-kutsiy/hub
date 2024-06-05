using Microsoft.AspNetCore.Mvc;
using Services;
using Microsoft.Extensions.Logging;

namespace kuberApi.Controllers
{
    [ApiController]
    [Route("kuber/")]
    public class KubernetesController : ControllerBase
    {
        private readonly KubernetesServices _kubernetesService;
        private readonly ILogger<KubernetesController> _logger;

        public KubernetesController(KubernetesServices kubernetesService, ILogger<KubernetesController> logger)
        {
            _kubernetesService = kubernetesService;
            _logger = logger;
        }

        // GET kuber/services
        [HttpGet("services")]
        public ActionResult<IEnumerable<string>> GetKubernetesServices()
        {
            try
            {
                // Logic to retrieve information about Kubernetes services
                var services = _kubernetesService.GetKubernetesServicesAsync().Result; // Synchronously waiting for the result (avoid this in production)
                _logger.LogInformation("Services retrieved successfully");
                return Ok(services);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Kubernetes services.");
                return StatusCode(500, $"Error retrieving services: {ex.Message}");
            }
        }

        // GET kuber/services/all
        [HttpGet("services/nodePorts")]
        public ActionResult<IEnumerable<string>> GetKubernetesServicesNodePorts()
        {
            try
            {
                // Logic to retrieve information about Kubernetes services
                var services = _kubernetesService.GetKubernetesServicesNodePortsAsync().Result; // Synchronously waiting for the result (avoid this in production)
                _logger.LogInformation("Services with node ports retrieved successfully");
                return Ok(services);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Kubernetes services with node ports.");
                return StatusCode(500, $"Error retrieving services with node ports: {ex.Message}");
            }
        }
    }
}
