import { Component, OnInit } from '@angular/core';
import * as namespaces from '../assets/namespaces.json';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'UI';
  ngOnInit(): void {
    console.log(namespaces);

  }
}
