using k8s;

namespace services
{
    public class KubernetesServiceDto
    {
        public string? Name { get; set; }
        public int? NodePort { get; set; }
    }
    public class KubernetesServices
    {
        public async Task<List<string>> GetKubernetesServicesAsync()
        {
            var config = KubernetesClientConfiguration.BuildConfigFromConfigFile();
            using var client = new Kubernetes(config);

            var services = new List<string>();

            var serviceList = await client.ListServiceForAllNamespacesAsync();
            foreach (var service in serviceList.Items)
            {
                services.Add(service.Metadata.Name);
            }

            return services;
        }
        public async Task<List<KubernetesServiceDto>> GetKubernetesServicesNodePortsAsync()
        {
            var config = KubernetesClientConfiguration.BuildConfigFromConfigFile();
            using var client = new Kubernetes(config);

            var services = new List<KubernetesServiceDto>();

            var serviceList = await client.ListServiceForAllNamespacesAsync();
            foreach (var service in serviceList.Items)
            {
                var nodePort = service.Spec.Ports?.FirstOrDefault()?.NodePort;
                if (nodePort.HasValue) // Only include services with non-null nodePort
                {
                    var serviceDto = new KubernetesServiceDto
                    {
                        Name = service.Metadata.Name,
                        NodePort = nodePort.Value
                    };

                    services.Add(serviceDto);
                }
            }

            return services;
        }


    }
}
