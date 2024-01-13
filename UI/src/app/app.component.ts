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
    name: string;
    restarts: number;
    status: string;
  }[] = [];

  constructor(public httpClient: HttpClient) { }

  ngOnInit(): void {
    console.log(sharedConfig);
  }

  getPodsInfo(namespace: string) {
    console.log(namespace);
    this.httpClient.get('http://192.168.0.28:31135/kuber/' + namespace + '/pods').subscribe((podsInfo: any) => {
      console.log(podsInfo);
      this.podsInfo = podsInfo;
    });
  }

}