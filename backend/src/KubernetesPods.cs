using k8s;
using Config;

namespace Pods
{
    public class PodInfo
    {
        public string? Name { get; set; }
        public string? Status { get; set; }
        public int Restarts { get; set; }
        public string? Age { get; set; }
    }

    public static class KubernetesPods
    {
        public static async Task<IEnumerable<PodInfo>> GetKubernetesPods(string @namespace)
        {
            KubernetesClientConfiguration config = KubernetesConfig.GetConfiguration();
            var client = new Kubernetes(config);

            var podList = await client.ListNamespacedPodAsync(@namespace);
            var podInfos = new List<PodInfo>();

            foreach (var pod in podList.Items)
            {
                var podInfo = new PodInfo
                {
                    Name = pod.Metadata.Name,
                    Status = pod.Status.Phase,
                    Restarts = pod.Status.ContainerStatuses.Sum(container => container.RestartCount),
                    Age = pod.Metadata.CreationTimestamp.ToString()
                };

                podInfos.Add(podInfo);
            }

            return podInfos;
        }
    }
}
