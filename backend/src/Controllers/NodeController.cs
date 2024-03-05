using Microsoft.AspNetCore.Mvc;
using Nodes;
using Microsoft.Extensions.Logging;

namespace kuberApi.NodeControllers
{
    [ApiController]
    [Route("kuber/")]
    public class KubernetesController : ControllerBase
    {
        private readonly KubernetesNodes _kubernetesNodes;
        private readonly ILogger<KubernetesController> _logger;

        public KubernetesController(KubernetesNodes kubernetesNodes, ILogger<KubernetesController> logger)
        {
            _kubernetesNodes = kubernetesNodes;
            _logger = logger;
        }

        // GET kuber/nodes
        [HttpGet("nodes/nodesIP")]
        public async Task<ActionResult<IEnumerable<string>>> GetKubernetesNodes()
        {
            _logger.LogInformation("Retrieved nodes IP");
            return await _kubernetesNodes.GetKubernetesNodes();
        }
    }
}
