using k8s;
using Config;

namespace Pods
{
    public class PodInfo
    {
        public string? Name { get; set; }
        public string? Status { get; set; }
        public int Restarts { get; set; }
        public int AgeInSeconds { get; set; }
    }

    public static class KubernetesPods
    {
        public static async Task<IEnumerable<PodInfo>> GetKubernetesPods(string @namespace)
        {
            KubernetesClientConfiguration config = KubernetesConfig.GetConfiguration();
            var client = new Kubernetes(config);

            var podList = await client.ListNamespacedPodAsync(@namespace);
            var podInfos = new List<PodInfo>();

            // Get the current time in the UTC time zone
            var currentTimeUtc = DateTime.UtcNow;

            foreach (var pod in podList.Items)
            {
                // Convert pod creation timestamp to UTC
                var creationTimestampUtc = pod.Metadata.CreationTimestamp?.ToUniversalTime();

                if (creationTimestampUtc != null)
                {
                    var ageTimeSpan = currentTimeUtc - creationTimestampUtc.GetValueOrDefault();
                    var ageInSeconds = (int)ageTimeSpan.TotalSeconds;

                    var podInfo = new PodInfo
                    {
                        Name = pod.Metadata.Name,
                        Status = pod.Status.Phase,
                        Restarts = pod.Status.ContainerStatuses.Sum(container => container.RestartCount),
                        AgeInSeconds = ageInSeconds
                    };

                    podInfos.Add(podInfo);
                }
            }

            return podInfos;
        }

        public static async Task<string> GetPodLogs(string @namespace, string podName)
        // public static async Task<string> GetPodLogs(string @namespace, string podName, string containerName)
        {
            KubernetesClientConfiguration config = KubernetesConfig.GetConfiguration();
            var client = new Kubernetes(config);

            var response = await client.CoreV1.ReadNamespacedPodLogWithHttpMessagesAsync(
                podName, @namespace, follow: false).ConfigureAwait(false);
                // podName, @namespace, container: containerName, follow: false).ConfigureAwait(false);

            using (var streamReader = new StreamReader(response.Body))
            {
                return await streamReader.ReadToEndAsync();
            }
        }
    }
}