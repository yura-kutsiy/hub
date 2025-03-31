export const environment = {
    production: true,
    apiUrl: '/api', // This will be configured in your production environment
    featureFlags: {
        enablePodDescription: true,
        enablePodLogs: true,
        enablePodEvents: true,
        enableAdvancedMetrics: true
    },
    logging: {
        level: 'error',
        enableConsoleLogging: false
    }
}; 