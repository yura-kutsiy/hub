using Microsoft.AspNetCore.Mvc;
using Pods;

namespace kuberApi.PodControllers
{
    [ApiController]
    [Route("kuber/")]
    public class KubernetesController : ControllerBase
    {
        // GET kuber/{namespace}/pods
        [HttpGet("{namespace}/pods")]
        public async Task<ActionResult<IEnumerable<PodInfo>>> GetKubernetesPods(string @namespace)
        {
            var podInfos = await KubernetesPods.GetKubernetesPods(@namespace);

            return Ok(podInfos);
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
                return Ok(logContent);
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately and return a meaningful error response
                return StatusCode(500, $"Error retrieving logs: {ex.Message}");
            }
        }
    }
}
