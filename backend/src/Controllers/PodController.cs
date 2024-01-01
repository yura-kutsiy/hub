using Microsoft.AspNetCore.Mvc;
using Pods;

namespace kuberApi.PodControllers
{
    [ApiController]
    [Route("kuber/")]
    public class KubernetesController : ControllerBase
    {
        // GET api/kuber/{namespace}/pods
        [HttpGet("{namespace}/pods")]
        public async Task<ActionResult<IEnumerable<PodInfo>>> GetKubernetesPods(string @namespace)
        {
            var podInfos = await KubernetesPods.GetKubernetesPods(@namespace);
            return Ok(podInfos);
        }
    }
}
