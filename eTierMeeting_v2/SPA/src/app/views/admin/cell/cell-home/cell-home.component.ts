import { Component, OnInit, ViewChild } from "@angular/core";
import { TranslateService } from "@ngx-translate/core";
import { Select2OptionData } from "ng-select2";
import { ModalDirective } from "ngx-bootstrap/modal";
import { NgxSpinnerService } from "ngx-spinner";
import { Cell } from "../../../../_core/_models/cell";
import { OperationResult } from "../../../../_core/_models/operation-result";
import {
  Pagination,
  PaginationResult,
} from "../../../../_core/_models/pagination";
import { AlertifyService } from "../../../../_core/_services/alertify.service";
import { BuildingService } from "../../../../_core/_services/building.service";
import { CellService } from "../../../../_core/_services/cell.service";
import { PdcService } from "../../../../_core/_services/pdc.service";
import { SweetAlertService } from "../../../../_core/_services/sweet-alert.service";
import { ChangeDetectorRef } from "@angular/core";
@Component({
  selector: "app-cell-home",
  templateUrl: "./cell-home.component.html",
  styleUrls: ["./cell-home.component.scss"],
})
export class CellHomeComponent implements OnInit {
  @ViewChild("childModal", { static: false }) childModal: ModalDirective;
  listCells: Cell[];
  cell: any = {
    cellID: 0,
    cellCode: null,
    cellName: null,
    pdcid: 0,
    pdcName: null,
    buildingID: 0,
    buildingName: null,
    updateBy: null,
    visible: true,
    updateTime: null,
  };

  checkCell = true;

  pdcs: Array<Select2OptionData>;
  buildings: Array<Select2OptionData>;

  keyword: string = "";
  lang: string;
  cellUpdate: any = [];

  pagination: Pagination = {
    currentPage: 1,
    pageSize: 5,
    totalCount: 0,
    totalPage: 0,
  };

  constructor(
    private cellService: CellService,
    private buildingService: BuildingService,
    private pdcService: PdcService,
    private spinnerService: NgxSpinnerService,
    private sweetAlertifyService: SweetAlertService,
    private translate: TranslateService,
    private alertifyService: AlertifyService,
    private cdref: ChangeDetectorRef
  ) { }
  ngAfterContentChecked() {
    this.cdref.detectChanges();
  }
  ngOnInit() {
    this.getAllPdc();
    this.getAllBuilding();
    this.loadData();
  }

  loadData() {
    this.spinnerService.show();
    this.cellService.searchCell(this.keyword, this.pagination).subscribe(
      (res: PaginationResult<Cell>) => {
        this.listCells = res.result;
        this.pagination = res.pagination;
        this.spinnerService.hide();
      },
      (error) => {
        this.alertifyService.error(
          this.translate.instant("error.system_error")
        );
        this.spinnerService.hide();
      }
    );
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page === undefined ? 1 : event.page;
    this.loadData();
  }

  searchCell() {
    this.pagination.currentPage = 1;
    this.loadData();
  }

  clearSearch() {
    this.pagination.currentPage = 1;
    this.keyword = "";
    this.loadData();
  }

  keywordAdd(value: string) {
    this.keyword = value;
    this.searchCell();
  }

  cancel() {
    this.searchCell();
  }

  removeCell(cell: Cell) {
    this.lang = localStorage.getItem("lang");
    this.sweetAlertifyService.confirm(
      "Delete",
      this.translate.instant("alert.admin.are_you_sure_to_delete_this_cell"),
      () => {
        this.cellService
          .removeCell(cell, this.lang)
          .subscribe((res: OperationResult) => {
            if (res.success) {
              this.sweetAlertifyService.success("Success", res.message);
              this.loadData();
            } else {
              this.sweetAlertifyService.error("Failed", res.message);
            }
          });
      }
    );
  }

  updateBuilding() {
    this.lang = localStorage.getItem("lang");
    if (
      this.cellUpdate.cellName == null ||
      this.cellUpdate.cellName.trim() === ""
    ) {
      return this.sweetAlertifyService.warning(
        "Error",
        this.translate.instant("alert.admin.please_enter_cell_code")
      );
    }
    if (this.cellUpdate.pdcid == 0) {
      return this.sweetAlertifyService.warning(
        "Error",
        this.translate.instant("alert.admin.please_choose_pdc")
      );
    }
    if (this.cellUpdate.buildingID == 0) {
      return this.sweetAlertifyService.warning(
        "Error",
        this.translate.instant("alert.admin.please_choose_building")
      );
    }

    this.cellUpdate.cellName = this.cellUpdate.cellName.trim();
    this.cellUpdate.pdcid = +this.cellUpdate.pdcid;
    this.cellUpdate.buildingID = +this.cellUpdate.buildingID;

    this.cellService
      .updateCell(this.cellUpdate, this.lang)
      .subscribe((res: OperationResult) => {
        if (res.success) {
          this.sweetAlertifyService.success("Success", res.message);
          this.childModal.hide();
          this.keyword = this.cellUpdate.cellName;
          this.cancel();
        } else {
          this.sweetAlertifyService.error("Error", res.message);
        }
      });
  }

  showChildModal(cell: Cell): void {
    const cellObject = {
      cellID: cell.cellID,
      cellName: cell.cellName,
      cellCode: cell.cellCode,
      pdcid: cell.pdcid,
      buildingID: cell.buildingID,
    };
    this.cellUpdate = cellObject;
    this.childModal.show();
  }

  hideChildModal(): void {
    this.childModal.hide();
  }

  exportExcelData() {
    this.cellService.exportExcelData();
  }

  getAllBuilding() {
    this.buildingService.getAllBuilding().subscribe((res) => {
      this.buildings = res.map((item) => {
        return { id: item.buildingID.toString(), text: item.buildingName };
      });
      this.buildings.unshift({
        id: "0",
        text: this.translate.instant("admin.admincell.all_building"),
      });
    });
  }

  getAllPdc() {
    this.pdcService.getAllPdc().subscribe((res) => {
      this.pdcs = res.map((item) => {
        return { id: item.pdcid.toString(), text: item.pdcName };
      });
      this.pdcs.unshift({
        id: "0",
        text: this.translate.instant("admin.admincell.all_set"),
      });
    });
  }

  getDataCell() {
    if (this.cell.cellCode != null) {
      this.cellService.getDataCell(this.cell.cellCode).subscribe((res) => {
        if (res) {
          this.cell.cellCode = res.cellCode;
          this.cell.cellName = res.cellName;
          this.cell.pdcid = res.pdcid;
          this.checkCell = false;
        }
      });
    }
  }

  checkEmpty() {
    if (this.cell.cellCode == null || this.cell.cellCode === "") {
      this.cell.cellName = "";
      this.cell.pdcid = 0;
      this.cell.buildingID = 0;
    }
    this.checkCell = true;
  }
}
