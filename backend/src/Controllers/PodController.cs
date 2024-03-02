using Microsoft.AspNetCore.Mvc;
using Pods;
using Microsoft.Extensions.Logging;

namespace kuberApi.PodControllers
{
    [ApiController]
    [Route("kuber/")]
    public class KubernetesController : ControllerBase
    {
        private readonly ILogger<KubernetesController> _logger;

        public KubernetesController(ILogger<KubernetesController> logger)
        {
            _logger = logger;
        }

        // GET kuber/{namespace}/pods
        [HttpGet("{namespace}/pods")]
        public async Task<ActionResult<IEnumerable<PodInfo>>> GetKubernetesPods(string @namespace)
        {
            try
            {
                var podInfos = await KubernetesPods.GetKubernetesPods(@namespace);

                if (podInfos == null || !podInfos.Any())
                {
                    _logger.LogWarning("No pods found for the namespace: {Namespace}", @namespace);
                    return NotFound();
                }

                _logger.LogInformation("Pods retrieved successfully namespace: {Namespace}", @namespace);
                return Ok(podInfos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve Kubernetes Pods for namespace: {Namespace}", @namespace);
                return StatusCode(500, $"Error retrieving pods: {ex.Message}");
            }
        }

        // GET kuber/{namespace}/pods/{podName}/logs/{containerName}
        [HttpGet("{namespace}/pods/{podName}/logs")]
        // [HttpGet("{namespace}/pods/{podName}/logs/{containerName}")]
        public async Task<ActionResult<string>> GetPodLogs(string @namespace, string podName)
        // public async Task<ActionResult<string>> GetPodLogs(string @namespace, string podName, string containerName)
        {
            try
            {
                string logContent = await KubernetesPods.GetPodLogs(@namespace, podName);
                _logger.LogInformation("Pod {podName} logs retrieved successfully", @podName);
                return Ok(logContent);
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately and return a meaningful error response
                _logger.LogError("Error retrieving logs for {podName} in namespace {namespace}", @podName, @namespace);
                return StatusCode(500, $"Error retrieving logs: {ex.Message}");
            }
        }
    }
}
