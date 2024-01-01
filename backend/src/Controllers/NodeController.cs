using Microsoft.AspNetCore.Mvc;
using Nodes;

namespace kuberApi.NodeControllers
{
    [ApiController]
    [Route("kuber/")]
    public class KubernetesController : ControllerBase
    {
        private readonly KubernetesNodes _kubernetesNodes;

        public KubernetesController(KubernetesNodes kubernetesNodes)
        {
            _kubernetesNodes = kubernetesNodes;
        }

        // GET kuber/nodes
        [HttpGet("nodes/nodesIP")]
        public async Task<ActionResult<IEnumerable<string>>> GetKubernetesNodes()
        {
            return await _kubernetesNodes.GetKubernetesNodes();
        }
    }
}
