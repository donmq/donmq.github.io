import { Component, OnInit } from '@angular/core';
import { KeyValuePair } from '@utilities/key-value-pair';
import { MainHomeDto, MainHomeParam } from '@models/home';
import { HomeMainService } from './../../../../_core/services/home-main.service';


@Component({
  selector: 'app-main-home',
  templateUrl: './main-home.component.html',
  styleUrl: './main-home.component.scss'
})
export class MainHomeComponent implements OnInit {
  data: MainHomeDto = <MainHomeDto>{};
  players: KeyValuePair[] = []
  exercise: KeyValuePair[] = []
  param: MainHomeParam = <MainHomeParam>{};

  constructor(private service: HomeMainService) { }

  ngOnInit() {
    this.getListPlayer();
    this.getListExersice();
  }

  getListPlayer() {
    this.service.getListPlayers().subscribe({
      next: res => {
        this.players = res
      }
    })
  }

  getListExersice() {
    this.service.getListExercise().subscribe({
      next: res => {
        this.exercise = res
      }
    })
  }
  onAdd(){
    
  }
}
