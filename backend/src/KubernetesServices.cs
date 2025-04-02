using k8s;
using k8s.Models;
using Config;
using Microsoft.Extensions.Logging;
using Exceptions;

namespace Services
{
    public class KubernetesServiceDto
    {
        public string? Name { get; set; }
        public int? NodePort { get; set; }
    }

    public class KubernetesServices
    {
        private readonly ILogger<KubernetesServices> _logger;
        private readonly IKubernetes _kubernetesClient;

        public KubernetesServices(ILogger<KubernetesServices> logger)
        {
            _logger = logger;
            KubernetesClientConfiguration config = KubernetesConfig.GetConfiguration();
            _kubernetesClient = new Kubernetes(config);
        }

        public async Task<List<string>> GetKubernetesServicesAsync()
        {
            try
            {
                _logger.LogInformation("Fetching list of Kubernetes services");
                var serviceList = await GetServiceList();

                var services = new List<string>();
                foreach (var service in serviceList.Items)
                {
                    services.Add(service.Metadata.Name);
                }

                _logger.LogInformation("Successfully retrieved {Count} services", services.Count);
                return services;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching Kubernetes services");
                throw new KubernetesApiException("Failed to fetch Kubernetes services", 500, ex);
            }
        }

        public async Task<List<KubernetesServiceDto>> GetKubernetesServicesNodePortsAsync()
        {
            try
            {
                _logger.LogInformation("Fetching Kubernetes services with node ports");
                var serviceList = await GetServiceList();

                var services = new List<KubernetesServiceDto>();
                foreach (var service in serviceList.Items)
                {
                    var nodePort = service.Spec.Ports?.FirstOrDefault()?.NodePort;
                    if (nodePort.HasValue)
                    {
                        var serviceDto = new KubernetesServiceDto
                        {
                            Name = service.Metadata.Name,
                            NodePort = nodePort.Value
                        };

                        services.Add(serviceDto);
                    }
                }

                _logger.LogInformation("Successfully retrieved {Count} services with node ports", services.Count);
                return services;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching Kubernetes services with node ports");
                throw new KubernetesApiException("Failed to fetch Kubernetes services with node ports", 500, ex);
            }
        }

        private async Task<V1ServiceList> GetServiceList()
        {
            try
            {
                _logger.LogDebug("Making API call to list services for all namespaces");
                var serviceList = await _kubernetesClient.CoreV1.ListServiceForAllNamespacesAsync();
                return serviceList ?? throw new KubernetesApiException("Received null response from Kubernetes API", 503);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to list services from Kubernetes API");
                throw new KubernetesApiException("Failed to communicate with Kubernetes API", 503, ex);
            }
        }
    }
}
