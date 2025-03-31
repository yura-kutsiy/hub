import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, map, retry } from 'rxjs/operators';
import { environment } from '../../environments/environment';
import { LoggingService } from './logging.service';

export interface Pod {
    name: string;
    restarts: number;
    status: string;
    containers: string[];
    age: string;
}

export interface PodDescription {
    name: string;
    namespace: string;
    status: string;
    containers: {
        name: string;
        status: string;
        image: string;
    }[];
    labels: { [key: string]: string };
    annotations: { [key: string]: string };
}

export interface PodEvent {
    type: string;
    reason: string;
    message: string;
    timestamp: string;
}

@Injectable({
    providedIn: 'root'
})
export class KubernetesService {
    private readonly apiUrl = environment.apiUrl;

    constructor(
        private http: HttpClient,
        private loggingService: LoggingService
    ) { }

    getPods(namespace: string): Observable<Pod[]> {
        return this.http.get<any[]>(`${this.apiUrl}/kuber/${namespace}/pods`).pipe(
            map(pods => this.convertPods(pods)),
            retry(1),
            catchError(this.handleError.bind(this))
        );
    }

    getPodDescription(namespace: string, podName: string): Observable<PodDescription> {
        return this.http.get<PodDescription>(`${this.apiUrl}/kuber/${namespace}/pods/${podName}/description`).pipe(
            retry(1),
            catchError(this.handleError.bind(this))
        );
    }

    getPodLogs(namespace: string, podName: string, containerName?: string): Observable<string> {
        const url = containerName
            ? `${this.apiUrl}/kuber/${namespace}/pods/${podName}/logs/${containerName}`
            : `${this.apiUrl}/kuber/${namespace}/pods/${podName}/logs`;
        return this.http.get<string>(url).pipe(
            retry(1),
            catchError(this.handleError.bind(this))
        );
    }

    getPodEvents(namespace: string, podName: string): Observable<PodEvent[]> {
        return this.http.get<PodEvent[]>(`${this.apiUrl}/kuber/${namespace}/pods/${podName}/events`).pipe(
            retry(1),
            catchError(this.handleError.bind(this))
        );
    }

    private convertPods(pods: any[]): Pod[] {
        return pods.map(pod => ({
            name: pod.name,
            restarts: pod.restarts,
            status: pod.status,
            containers: pod.containers || [],
            age: pod.age
        }));
    }

    private handleError(error: HttpErrorResponse): Observable<never> {
        let errorMessage = 'An error occurred';

        if (error.error instanceof ErrorEvent) {
            // Client-side error
            errorMessage = `Error: ${error.error.message}`;
        } else {
            // Server-side error
            errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
        }

        this.loggingService.error(errorMessage, error);
        return throwError(() => new Error(errorMessage));
    }
} 