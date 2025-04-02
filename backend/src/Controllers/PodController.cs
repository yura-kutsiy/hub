using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Config;
using Pods;
using Microsoft.Extensions.Logging;

namespace kuberApi.PodControllers
{
    [ApiController]
    [Route("kuber")]
    public class PodController : ControllerBase
    {
        private readonly ILogger<PodController> _logger;
        private readonly AppSettings _settings;

        public PodController(ILogger<PodController> logger, IOptions<AppSettings> settings)
        {
            _logger = logger;
            _settings = settings.Value;
        }

        // GET kuber/{namespace}/pods
        [HttpGet("{namespace}/pods")]
        public async Task<ActionResult<IEnumerable<PodInfo>>> GetPods(string @namespace)
        {
            try
            {
                var pods = await KubernetesPods.GetKubernetesPods(@namespace);
                _logger.LogInformation("Retrieved {count} pods from namespace {namespace}", pods.Count(), @namespace);
                return Ok(pods);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving pods from namespace {namespace}", @namespace);
                return StatusCode(500, "Failed to retrieve pods");
            }
        }

        // GET kuber/{namespace}/pods/{podName}/logs
        [HttpGet("{namespace}/pods/{podName}/logs")]
        public async Task<ActionResult<string>> GetPodLogs(string @namespace, string podName)
        {
            if (_settings?.FeatureFlags?.EnablePodLogs != true)
            {
                _logger.LogWarning("Pod logs feature is disabled");
                return StatusCode(403, "Pod logs feature is disabled");
            }

            try
            {
                string logContent = await KubernetesPods.GetPodLogs(@namespace, podName);
                _logger.LogInformation("Pod {podName} logs retrieved successfully", podName);
                return Ok(logContent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving logs for {podName} in namespace {namespace}", podName, @namespace);
                return StatusCode(500, "Failed to retrieve pod logs");
            }
        }

        // GET kuber/{namespace}/pods/{podName}/logs/{containerName}
        [HttpGet("{namespace}/pods/{podName}/logs/{containerName}")]
        public async Task<ActionResult<string>> GetPodLogs(string @namespace, string podName, string containerName)
        {
            if (_settings?.FeatureFlags?.EnablePodLogs != true)
            {
                _logger.LogWarning("Pod logs feature is disabled");
                return StatusCode(403, "Pod logs feature is disabled");
            }

            try
            {
                string logContent = await KubernetesPods.GetPodLogs(@namespace, podName, containerName);
                _logger.LogInformation("Container {containerName} logs retrieved successfully", containerName);
                return Ok(logContent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving logs for {containerName} from {podName} in namespace {namespace}", containerName, podName, @namespace);
                return StatusCode(500, "Failed to retrieve container logs");
            }
        }

        // GET kuber/{namespace}/pods/{podName}/events
        [HttpGet("{namespace}/pods/{podName}/events")]
        public async Task<ActionResult<IEnumerable<PodEventInfo>>> GetPodEvents(string @namespace, string podName)
        {
            if (_settings?.FeatureFlags?.EnablePodEvents != true)
            {
                _logger.LogWarning("Pod events feature is disabled");
                return StatusCode(403, "Pod events feature is disabled");
            }

            try
            {
                var events = await KubernetesPods.GetPodEvents(@namespace, podName);
                _logger.LogInformation("Retrieved {count} events for pod {podName}", events.Count(), podName);
                return Ok(events);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving events for {podName} in namespace {namespace}", podName, @namespace);
                return StatusCode(500, "Failed to retrieve pod events");
            }
        }

        // GET kuber/{namespace}/pods/{podName}/description
        [HttpGet("{namespace}/pods/{podName}/description")]
        public async Task<ActionResult<string>> GetPodDescription(string @namespace, string podName)
        {
            if (_settings?.FeatureFlags?.EnablePodDescription != true)
            {
                _logger.LogWarning("Pod description feature is disabled");
                return StatusCode(403, "Pod description feature is disabled");
            }

            try
            {
                var description = await KubernetesPods.GetPodDescription(@namespace, podName);
                _logger.LogInformation("Pod {podName} description retrieved successfully", podName);
                return Ok(description);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving description for {podName} in namespace {namespace}", podName, @namespace);
                return StatusCode(500, "Failed to retrieve pod description");
            }
        }
    }
}
