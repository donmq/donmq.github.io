import { Component, OnInit, ViewChild } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { Building } from '../../../../_core/_models/building';
import { OperationResult } from '../../../../_core/_models/operation-result';
import { Pagination, PaginationResult } from '../../../../_core/_models/pagination';
import { AlertifyService } from '../../../../_core/_services/alertify.service';
import { BuildingService } from '../../../../_core/_services/building.service';
import { SweetAlertService } from '../../../../_core/_services/sweet-alert.service';

@Component({
  selector: 'app-building-home',
  templateUrl: './building-home.component.html',
  styleUrls: ['./building-home.component.scss']
})
export class BuildingHomeComponent implements OnInit {
  @ViewChild('childModal', { static: false }) childModal: ModalDirective;
  listBuildings: Building[];
  building: Building = {
    buildingID: 0,
    buildingCode: null,
    buildingName: null,
    visible: true
  };
  lang: string;
  keyword: string = '';
  buildingUpdate: any = [];

  pagination: Pagination = {
    currentPage: 1,
    pageSize: 5,
    totalCount: 0,
    totalPage: 0,
  };


  constructor(
    private buildingService: BuildingService,
    private sweetAlertifyService: SweetAlertService,
    private spinnerService: NgxSpinnerService,
    private translate: TranslateService,
    private alertifyService: AlertifyService,
  ) { }

  ngOnInit() {
    this.loadData();
  }

  loadData() {
    this.spinnerService.show();
    this.buildingService.searchBuilding(this.keyword, this.pagination).subscribe((res: PaginationResult<Building>) => {
      this.listBuildings = res.result;
      this.pagination = res.pagination;
      this.spinnerService.hide();
    }, error => {
      this.alertifyService.error(this.translate.instant('error.system_error'));
      this.spinnerService.hide();
    });
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page === undefined ? 1 : event.page;
    this.loadData();
  }

  searchBuilding() {
    this.pagination.currentPage = 1;
    this.loadData()
  }

  clearSearch() {
    this.pagination.currentPage = 1;
    this.keyword = '';
    this.loadData();
  }

  keywordAdd(value: string) {
    this.keyword = value;
    this.searchBuilding();
  }

  removeBuilding(building: Building) {
    this.lang = localStorage.getItem('lang');
    this.sweetAlertifyService.confirm('Delete', this.translate.instant('alert.admin.are_you_sure_to_delete_this_building'), () => {
      this.buildingService.removeBuilding(building, this.lang).subscribe((res: OperationResult) => {
        if (res.success) {
          this.sweetAlertifyService.success('Success', res.message);
          this.loadData();
          this.keyword = '';
        } else {
          this.sweetAlertifyService.error('Failed', res.message);
        }
      });
    });
  }

  updateBuilding() {
    this.lang = localStorage.getItem('lang');
    if (this.buildingUpdate.buildingName == null || this.buildingUpdate.buildingName.trim() === '') {
      return this.sweetAlertifyService.warning('Error', this.translate.instant('alert.admin.please_enter_building_name'));
    }
    if (this.buildingUpdate.buildingCode == null || this.buildingUpdate.buildingCode.trim() === '') {
      return this.sweetAlertifyService.warning('Error', this.translate.instant('alert.admin.please_enter_building_code'));
    }
    this.buildingUpdate.buildingName = this.buildingUpdate.buildingName.trim();
    this.buildingUpdate.buildingCode = this.buildingUpdate.buildingCode.trim();
    this.buildingService.updateBuilding(this.buildingUpdate, this.lang).subscribe((res: OperationResult) => {
      if (res.success) {
        this.sweetAlertifyService.success('Success', res.message);
        this.keyword = this.buildingUpdate.buildingName;
        this.searchBuilding()
        this.childModal.hide();
      } else {
        this.sweetAlertifyService.error('Error', res.message);
      }
    });
  }

  showChildModal(building: Building): void {
    const buildingObject = {
      buildingCode: building.buildingCode,
      buildingName: building.buildingName,
      buildingID: building.buildingID
    };
    this.buildingUpdate = buildingObject;
    this.childModal.show();
  }

  hideChildModal(): void {
    this.childModal.hide();
  }
}
