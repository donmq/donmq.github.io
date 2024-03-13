import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-about',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './about.component.html',
  styleUrl: './about.component.scss'
})
export class AboutComponent implements OnInit {

  data = [
    { title: 'Tên:', content: 'Mai Quý Đôn' },
    { title: 'Ngày sinh:', content: '08/09/1998' },
    { title: 'Địa chỉ:', content: '534/3B - An Thạnh - Thuận An - Bình Dương' },
    { title: 'Email:', content: 'donmqpk00888@gmail.com' },
    { title: 'SĐT:', content: '0978 064 768' },
  ]

  ngOnInit(): void {
    // console.log(this.data)
  }

}
