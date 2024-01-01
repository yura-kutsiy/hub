using Microsoft.AspNetCore.Mvc;
using k8s;
using Config;

namespace Nodes
{
    public class KubernetesNodes
    {
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

            return new OkObjectResult(nodeIps);
        }
    }
}
