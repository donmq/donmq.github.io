import { Component } from '@angular/core';
import { INavData } from '@coreui/angular';

@Component({
  selector: 'app-default-layout',
  standalone: true,
  imports: [],
  templateUrl: './default-layout.component.html',
  styleUrl: './default-layout.component.scss'
})
export class DefaultLayoutComponent {
  public navItems: INavData[] = [];
  user: {
    name: '';
  }

}
