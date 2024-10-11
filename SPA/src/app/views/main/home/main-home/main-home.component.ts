import { Component, OnInit } from '@angular/core';
import { KeyValuePair } from '@utilities/key-value-pair';
import { MainHomeDto, MainHomeParam } from '@models/home';


@Component({
  selector: 'app-main-home',
  templateUrl: './main-home.component.html',
  styleUrl: './main-home.component.scss'
})
export class MainHomeComponent implements OnInit {
  data: MainHomeDto = <MainHomeDto>{};
  players: KeyValuePair[] = [
    {
      key: 'a',
      value: 'test'
    }, {
      key: 'a',
      value: 'test'
    }, {
      key: 'a',
      value: 'test'
    }, {
      key: 'a',
      value: 'test'
    }
  ]
  param: MainHomeParam = <MainHomeParam>{};
  player: string

  ngOnInit() {

  }

  ngOnDestroy() {
  }
}
