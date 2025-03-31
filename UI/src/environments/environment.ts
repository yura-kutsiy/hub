export const environment = {
    production: false,
    apiUrl: 'http://localhost:8000',
    featureFlags: {
        enablePodDescription: true,
        enablePodLogs: true,
        enablePodEvents: true,
        enableAdvancedMetrics: false
    },
    logging: {
        level: 'debug',
        enableConsoleLogging: true
    }
}; 