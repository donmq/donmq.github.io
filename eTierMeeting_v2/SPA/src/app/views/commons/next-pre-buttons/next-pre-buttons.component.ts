import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CommonService } from '@services/common.service';

@Component({
  selector: 'next-pre-buttons',
  templateUrl: './next-pre-buttons.component.html',
  styleUrls: ['./next-pre-buttons.component.scss']
})
export class NextPreButtonsComponent implements OnInit {
  @Input() previousLink: string;
  @Input() nextLink: string;

  constructor(private router: Router, private commonService: CommonService) { }

  ngOnInit(): void {
  }

  previousPage() {
    this.commonService.dataPreOrNext.set("previousPage");
    this.router.navigate([this.previousLink], { queryParams: { nav: 'prev' } });
  }

  nextPage() {
    this.commonService.dataPreOrNext.set("nextPage");
    this.router.navigate([this.nextLink], { queryParams: { nav: 'next' } });
  }
}
