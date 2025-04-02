namespace Config
{
    public class AppSettings
    {
        public CorsSettings? Cors { get; set; }
        public KubernetesSettings? Kubernetes { get; set; }
        public FeatureFlags? FeatureFlags { get; set; }
    }

    public class CorsSettings
    {
        public string[]? AllowedOrigins { get; set; }
    }

    public class KubernetesSettings
    {
        public string? ConfigPath { get; set; }
        public string? DefaultNamespace { get; set; }
    }

    public class FeatureFlags
    {
        public bool EnablePodDescription { get; set; }
        public bool EnablePodLogs { get; set; }
        public bool EnablePodEvents { get; set; }
        public bool EnableAdvancedMetrics { get; set; }
    }
}