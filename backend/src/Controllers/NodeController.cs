using Microsoft.AspNetCore.Mvc;
using Nodes;

namespace kuberApi.NodeControllers
{
    [ApiController]
    [Route("kuber/")]
    public class KubernetesController : ControllerBase
    {
        private readonly KubernetesNodes _kubernetesNodes;

        private readonly ILogger<KubernetesController> _logger;

        public KubernetesController(ILogger<KubernetesController> logger)
        {
            _logger = logger;
        }
        public KubernetesController(KubernetesNodes kubernetesNodes)
        {
            _kubernetesNodes = kubernetesNodes;
        }

        // GET kuber/nodes
        [HttpGet("nodes/nodesIP")]
        public async Task<ActionResult<IEnumerable<string>>> GetKubernetesNodes()
        {
            _logger.LogInformation("Retrived nodes IP");
            return await _kubernetesNodes.GetKubernetesNodes();
        }
    }
}
