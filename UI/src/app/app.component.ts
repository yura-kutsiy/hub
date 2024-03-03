import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import * as sharedConfig from '../assets/namespaces.json';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})

export class AppComponent implements OnInit {
  sharedConfig = sharedConfig;

  podsInfo: {
    ageInSeconds: number;
    age: string;
    name: string;
    restarts: number;
    status: string;
  }[] = [];

  activeRowNumber: number | null = null;
  namespace?: string;
  activeNamespace: string | null = null;

  constructor(public httpClient: HttpClient) { }

  ngOnInit(): void {
    console.log(sharedConfig);
    this.getPodsInfo('default');
    // this.getPodsInfo(sharedConfig.namespaces[0]); parse to object
  }

  getPodsInfo(namespace: string) {
    console.log(namespace);
    this.httpClient.get('http://192.168.0.28:31135/kuber/' + namespace + '/pods').subscribe((podsInfo: any) => {
      console.log(podsInfo);
      this.podsInfo = this.convertAge(podsInfo);
      this.activeNamespace = namespace;
      this.activeRowNumber = null;
    });
  }

  getPodLog(podName: string) {
    this.httpClient.get('http://192.168.0.28:31135/kuber/' + this.namespace + '/pods/' + podName + '/logs', { responseType: 'text' }).subscribe((logs: any) => {
      console.log(logs);
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

  openPodDetailes(index: number) {
    this.activeRowNumber = this.activeRowNumber === index ? null : index;
  }

}