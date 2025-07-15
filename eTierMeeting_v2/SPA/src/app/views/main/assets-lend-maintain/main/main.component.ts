import { Component, OnInit } from '@angular/core';
import { Pagination } from './../../../../_core/_models/pagination';
import { AssetsLendMaintainDto, AssetsLendMaintainParam } from '../../../../_core/_models/assets-lend-maintain';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.scss']
})
export class MainComponent implements OnInit {
  param: AssetsLendMaintainParam = {
    machineID: null,
    lendDate: '',
    lendTo: null,
    return: 'all'
  }
  pagination: Pagination = {
    currentPage: 1,
    pageSize: 10,
    totalCount: 0,
    totalPage: 0,
  };
  lendDate: string
  listLendTo
  data: AssetsLendMaintainDto[] = []
  constructor() { }

  ngOnInit() {
  }

  dateCheck() { }
  searchHistoryInventory() { }
  search() { }
  download() { }

}
