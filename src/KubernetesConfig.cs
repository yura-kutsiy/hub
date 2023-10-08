using k8s;

namespace Config
{
    public class KubernetesConfig
    {
        public static KubernetesClientConfiguration GetConfiguration()
        {
            KubernetesClientConfiguration config;

            try
            {
                // Try using in-cluster config first
                config = KubernetesClientConfiguration.InClusterConfig();
            }
            catch (Exception)
            {
                // If in-cluster config fails, use config from file
                config = KubernetesClientConfiguration.BuildConfigFromConfigFile();
            }

            return config;
        }
    }
}
