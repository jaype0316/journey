import { Component } from '@angular/core';
@Component({
  selector: 'app-root',
  templateUrl: 'app.component.html',
  styleUrls: ['app.component.scss'],
})
export class AppComponent {
  public appPages = [
    { title: 'Chapers', url: '/tabs/chapters', icon: 'folder' },
    { title: 'Quote', url: '/tabs/quote', icon: 'reader' },
    { title: 'Profile', url: '/tabs/profile', icon: 'person' }
  ];
  public labels = [{title: 'About', url: '/tabs/about', icon: 'finger-print'}];
  constructor() {}
}
