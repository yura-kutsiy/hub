using k8s;
using k8s.Models;
using Config;

namespace Services
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
            var serviceList = await GetServiceList();

            var services = new List<string>();
            foreach (var service in serviceList.Items)
            {
                services.Add(service.Metadata.Name);
            }

            return services;
        }
        public async Task<List<KubernetesServiceDto>> GetKubernetesServicesNodePortsAsync()
        {
            var serviceList = await GetServiceList();

            var services = new List<KubernetesServiceDto>();
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

        private async Task<V1ServiceList> GetServiceList()
        {
            KubernetesClientConfiguration config = KubernetesConfig.GetConfiguration();
            var client = new Kubernetes(config);
            var serviceList = await client.ListServiceForAllNamespacesAsync();
            
            return serviceList;
        }
    }
}
