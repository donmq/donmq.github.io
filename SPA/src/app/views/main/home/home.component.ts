import { Component, OnInit } from '@angular/core';
import Typed from 'typed.js';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent implements OnInit{


  ngOnInit() {
    const options = {
      strings: [
        'Data',
        'Developer',
        'Data Science'],
      typeSpeed: 100,
      backSpeed: 50,
      showCursor: true,
      cursorChar: '',
      loop: true
    };
    const typed = new Typed('.typed-element', options);
  }

  ngOnDestroy() {
  }
}
