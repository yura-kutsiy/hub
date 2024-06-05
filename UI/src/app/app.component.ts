import { Component, OnInit, isDevMode } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import * as sharedConfig from '../assets/namespaces.json';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})


export class AppComponent implements OnInit {

  podsInfo: {
    ageInSeconds: number;
    age: string;
    name: string;
    restarts: number;
    status: string;
  }[] = [];

  sharedConfig = sharedConfig;
  activeRowNumber: number | null = null;
  namespace?: string;
  activeNamespace: string | null = null;
  url: string | null = isDevMode() ? 'http://localhost:8000' : '';

  constructor(public httpClient: HttpClient) { }

  ngOnInit(): void {
    console.log(sharedConfig);
    this.getPodsInfo(this.sharedConfig.namespaces[3]);
  }

  getPodsInfo(namespace: string) {
    this.activeNamespace = namespace;
    this.namespace = namespace;
    this.activeRowNumber = null;
    this.httpClient.get(this.url + '/kuber/' + namespace + '/pods').subscribe((podsInfo: any) => {
      this.podsInfo = this.convertAge(podsInfo);
      console.log(podsInfo);
    });
  }

  getPodLog(podName: string) {
    this.httpClient.get(this.url + '/kuber/' + this.namespace + '/pods/' + podName + '/logs', { responseType: 'text' }).subscribe((logs: any) => {
      console.log(logs);
    });
  }

  openPodDetailes(index: number) {
    this.activeRowNumber = this.activeRowNumber === index ? null : index;
  }

  getPodEvents(podName: string) {
    this.httpClient.get(this.url + '/kuber/' + this.namespace + '/pods/' + podName + '/events').subscribe((events: any) => {
      console.log(events);
    });
  }

  convertAge(podsInfo: any[]): any[] {
    return podsInfo.map(pod => {
      let age: string;
      const ageInSeconds = pod.ageInSeconds;

      if (ageInSeconds < 60) {
        age = `${Math.floor(ageInSeconds)}s`;
      } else if (ageInSeconds < 6100) {
        const minutes = Math.floor(ageInSeconds / 60);
        age = `${minutes}m`;
      } else if (ageInSeconds < 86400) {
        const hours = Math.floor(ageInSeconds / 3600);
        const minutes = Math.floor((ageInSeconds % 3600) / 60);
        age = `${hours}h ${minutes}m`;
      } else if (ageInSeconds < 259200) {
        const days = Math.floor(ageInSeconds / 86400);
        const hours = Math.floor((ageInSeconds % 86400) / 3600);
        age = `${days}d ${hours}h`;
      } else {
        const days = Math.floor(ageInSeconds / 86400);
        age = `${days}d`;
      }

      return {
        ...pod,
        age: age
      };
    });
  }

}