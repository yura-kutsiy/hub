import { Component, OnInit, Inject } from '@angular/core';
import { environment } from '../environments/environment';
import { KubernetesService, Pod, PodDescription, PodEvent } from './services/kubernetes.service';
import { LoggingService } from './services/logging.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  pods: Pod[] = [];
  activeRowNumber: number | null = null;
  namespace: string = 'default';
  loading = false;
  error: string | null = null;
  podDescription: PodDescription | null = null;
  podEvents: PodEvent[] = [];
  podLogs: string = '';
  environment = environment;

  constructor(
    private kubernetesService: KubernetesService,
    private loggingService: LoggingService
  ) { }

  ngOnInit() {
    this.loadPods();
  }

  loadPods() {
    this.loading = true;
    this.error = null;
    this.kubernetesService.getPods(this.namespace).subscribe({
      next: (pods) => {
        this.pods = pods;
        this.loading = false;
        this.loggingService.debug('Pods loaded successfully', pods);
      },
      error: (error) => {
        this.error = 'Failed to load pods. Please try again.';
        this.loading = false;
        this.loggingService.error('Failed to load pods', error);
      }
    });
  }

  openPodDetails(index: number) {
    this.activeRowNumber = this.activeRowNumber === index ? null : index;
  }

  getPodDescription(podName: string) {
    if (!environment.featureFlags.enablePodDescription) {
      this.loggingService.warn('Pod description feature is disabled');
      return;
    }

    this.loading = true;
    this.error = null;
    this.kubernetesService.getPodDescription(this.namespace, podName).subscribe({
      next: (description) => {
        this.podDescription = description;
        this.loading = false;
        this.loggingService.debug('Pod description loaded successfully', description);
      },
      error: (error) => {
        this.error = 'Failed to load pod description. Please try again.';
        this.loading = false;
        this.loggingService.error('Failed to load pod description', error);
      }
    });
  }

  getPodLog(podName: string) {
    if (!environment.featureFlags.enablePodLogs) {
      this.loggingService.warn('Pod logs feature is disabled');
      return;
    }

    this.loading = true;
    this.error = null;
    this.kubernetesService.getPodLogs(this.namespace, podName).subscribe({
      next: (logs) => {
        this.podLogs = logs;
        this.loading = false;
        this.loggingService.debug('Pod logs loaded successfully');
      },
      error: (error) => {
        this.error = 'Failed to load pod logs. Please try again.';
        this.loading = false;
        this.loggingService.error('Failed to load pod logs', error);
      }
    });
  }

  getPodEvents(podName: string) {
    if (!environment.featureFlags.enablePodEvents) {
      this.loggingService.warn('Pod events feature is disabled');
      return;
    }

    this.loading = true;
    this.error = null;
    this.kubernetesService.getPodEvents(this.namespace, podName).subscribe({
      next: (events) => {
        this.podEvents = events;
        this.loading = false;
        this.loggingService.debug('Pod events loaded successfully', events);
      },
      error: (error) => {
        this.error = 'Failed to load pod events. Please try again.';
        this.loading = false;
        this.loggingService.error('Failed to load pod events', error);
      }
    });
  }

  clearError() {
    this.error = null;
  }
}