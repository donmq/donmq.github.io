import { Component, OnInit, TemplateRef } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { interval } from 'rxjs';
import { take } from 'rxjs/operators';
import { environment } from '@environments/environment';
import { ActionHseConstants } from '@constants/action-hse-constants';
import { ActionConstants, MessageConstants } from '@constants/system.constants';
import { HSEDataSearchDto, HSESearchParam } from '@models/hseResultUpload/hse-data';
import { ImageDataUpload, ImageFileInfo } from '@models/hseResultUpload/image-file-info';
import { PaginatedResult, Pagination } from '@models/pagination';
import { HseResultUploadService } from '@services/production/T1/C2B/hseResultUpload.service';
import { KeyValuePair } from '@utilities/key-value-pair';
import { InjectBase } from '@utilities/inject-base-app';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.scss']
})
export class MainComponent extends InjectBase implements OnInit {
  baseUrl: string = environment.baseUrl;
  _time: string = '';
  buildings: KeyValuePair[];
  depts: KeyValuePair[];
  building: string = null;
  deptId: string = null;

  modalRef?: BsModalRef;
  paginationImage: Pagination; // Pagination của images trong modal
  paginationTable: Pagination; // Pagination của table
  dataHSEScore: HSEDataSearchDto[] = []; // Data show table
  fileRemarks: ImageFileInfo[] = []; // data show list images trong modal

  checkImageAlert: boolean = false; // Check có show khung thông báo Image Alert hay ko.
  checkClickSearchImageAlert: boolean = false; // Check đã click vào link ' Click here to find out them.' chưa
  actionHseConstants = ActionHseConstants;
  hseScoreModelEdit: HSEDataSearchDto; // model hse edit score(modal edit score)

  // upload image
  hseModelUploadImgCurrent: HSEDataSearchDto; // model cần upload or edit images trong modal
  checkEditImages: boolean = false; // Check có bấm button edit images ko
  constructor(private hseResultUploadService: HseResultUploadService,
              private modalService: BsModalService) {
    super();
  }

  ngOnInit() {
    this.paginationImage = {currentPage: 1,itemsPerPage: 1,totalItems: 0,totalPages: 0}
    this.paginationTable = {currentPage: 1,itemsPerPage: 10,totalItems: 0,totalPages: 0}
    this.getBuildings();
  }

  // ====================================== Start Show Data Building and Dept on Select===========================================//
  getBuildings() {
    this.spinnerService.show();
    this.hseResultUploadService.getBuildings().subscribe({
      next: (res) => {
        this.buildings = res.map(item => {
          return {key: item, value: item};
        });
        this.spinnerService.hide();
      },
      error: () => {
        this.snotifyService.error(MessageConstants.SYSTEM_ERROR_MSG);
        this.spinnerService.hide();
      }
    })
  }
  changedBuilding(e: any) {
    this.building = e;
    if(this.building !== '') {
      this.getDeptInBuilding(this.building);
    }
  }
  getDeptInBuilding(param: string) {
    this.spinnerService.show();
    this.hseResultUploadService.getDeptsInBuilding(param).subscribe({
      next: (res) => {
        this.depts = res.map(item => {
          return {key: item, value: item};
        });
        this.spinnerService.hide();
      },
      error: () => {
        this.snotifyService.error(MessageConstants.SYSTEM_ERROR_MSG);
        this.spinnerService.hide();
      }
    })
  }
  // ====================================== End Show Data Building and Dept on Select===========================================//

  //==================================================== Start Search Data======================================================//
  getDataTable(clickImageAlert: boolean) {
    if(this.functionUtility.checkEmpty(this._time)) {
      this.snotifyService.error('Please option Year-Month');
    } else {
      let time = new Date(this._time);
      let paramSearch: HSESearchParam = {
        year: time.getFullYear(),
        month: time.getMonth() + 1,
        building: this.building,
        deptID: this.deptId,
        clickImageAlert: clickImageAlert
      }
      this.spinnerService.show();
      this.hseResultUploadService.search(this.paginationTable.currentPage,this.paginationTable.itemsPerPage, paramSearch).subscribe(
      (res: PaginatedResult<HSEDataSearchDto[]>) => {
        this.dataHSEScore = res.result;
        this.paginationTable = res.pagination;
        // console.log(res);
        if(this.dataHSEScore.length > 0) {
          this.checkImageAlert = this.dataHSEScore[0].checkImageAlert;
        } else {
          this.checkImageAlert = false;
        }
        this.spinnerService.hide();
    }, error => {
      this.snotifyService.error(MessageConstants.SYSTEM_ERROR_MSG);
      this.spinnerService.hide();
    })
    }
  }
  search() {
    this.checkClickSearchImageAlert = false;
    this.paginationTable.currentPage !== 1 ? (this.paginationTable.currentPage = 1) : this.getDataTable(false);
  }
  searchImageAlert() {
    this.checkClickSearchImageAlert = true;
    this.paginationTable.currentPage !== 1 ? (this.paginationTable.currentPage = 1) : this.getDataTable(true);
  }
  pageChangedTable(event: any): void {
    this.paginationTable.currentPage = event.page;
    this.getDataTable(this.checkClickSearchImageAlert);
  }
  //==================================================== End Search Data======================================================//
 

  setValuePagination() {
    this.paginationImage.totalItems = this.fileRemarks.length;
  }
  changeDept(e: any) {
    this.deptId = e;
  }
  onOpenCalendar(container) {
    container.monthSelectHandler = (event: any): void => {
      container._store.dispatch(container._actions.select(event.date));
    };     
    container.setViewMode('month');
  }

  //=========================================================== Start Xử Lý Modal Edit Score=========================================================//
  openModalEditScore(template: TemplateRef<any>, hseScoreModel: HSEDataSearchDto) {
    this.modalRef = this.modalService.show(template);
    this.hseScoreModelEdit = {...hseScoreModel};
  }
  resetScoreEdit() {
    let hseRecordTable = this.dataHSEScore.find(x => x.hsE_Score_ID == this.hseScoreModelEdit.hsE_Score_ID);
    this.hseScoreModelEdit.score = hseRecordTable?.score;
  }
  saveEditScore() {
    if(this.hseScoreModelEdit.score === 0 || this.hseScoreModelEdit.score === null) {
      this.snotifyService.error("Please input score not empty!!!");
    } else {
      let hseRecordTable = this.dataHSEScore.find(x => x.hsE_Score_ID == this.hseScoreModelEdit.hsE_Score_ID);
      if(hseRecordTable.score == this.hseScoreModelEdit.score) {
        this.snotifyService.error("Please change value score");
      } else {
        this.spinnerService.show();
        this.hseResultUploadService.updateScoreData(this.hseScoreModelEdit).subscribe({
          next: (res) => {
            if(res) {
              this.snotifyService.success(MessageConstants.UPDATED_OK_MSG, ActionConstants.EDIT);
              this.checkClickSearchImageAlert === false ? this.search() : this.searchImageAlert();
              this.modalRef?.hide();
              this.spinnerService.hide();
            } else {
              this.handleUploadError();
            }
          },
          error: () => {
            this.handleUploadError();
          }
        })
      }
    }
  }
  //===========================================================End Xử Lý Modal Edit Score===========================================================//
  
  // ==========================================================Start Xử lý Image Trong Modal==========================================================//
	onSelect(event) {
    let models = event.addedFiles.map(item => {
      let model: ImageFileInfo = {image: item,remark: ''}
      return model;
    })
    this.fileRemarks.push(...models);
    this.setValuePagination();
	}

	onRemove(i: number) {
		this.fileRemarks.splice(i, 1);
    this.setValuePagination();
    if(this.paginationImage.currentPage === this.fileRemarks.length + 1) {
      this.paginationImage.currentPage = this.paginationImage.currentPage === 1 ? 1 : this.paginationImage.currentPage - 1;
    } else {
      let time = interval(100);
      time.pipe(take(1)).subscribe(res => {
        this.paginationImage.currentPage = this.paginationImage.currentPage === 1 ? 1 : this.paginationImage.currentPage - 1;
      })
    }
	}

  openModalImages(template: TemplateRef<any>, model: HSEDataSearchDto, isEdit: boolean) {
    //isEdit = true thì là record HSE đó đã up hình và giờ user muốn chỉnh sửa
    //isEdit = false thì là record HSE đó cần up hình và chưa up lần nào.
    this.fileRemarks.length = 0;
    this.paginationImage.currentPage = 1;
    this.modalRef = this.modalService.show(template);
    this.hseModelUploadImgCurrent = {...model};
    this.checkEditImages = isEdit;
    if(isEdit) {
      this.spinnerService.show();
      this.hseResultUploadService.getListImageToHseID(model.hsE_Score_ID).subscribe({
        next: (res) => {
          let imgs = res.map(item => {return item.name});
          let remarks = res.map(item => {return item.remark});
          let urlFolder = this.baseUrl + 'Production/T2/HSEUpload/';
          let files: File[] = [];
          this.functionUtility.convertToFile(imgs, urlFolder,files);
          let time1 = interval(500);
          time1.pipe(take(1)).subscribe(res => {
            this.fileRemarks = files.map((item,i) => {
                return {image: item, remark: remarks[i]}
            })
            this.setValuePagination();
        
          })
          this.spinnerService.hide();
        }
      })
    }
  }
  closeModalImage() {
    this.modalRef.hide();
    this.fileRemarks.length = 0;
    this.paginationImage = {currentPage: 1,itemsPerPage: 1,totalItems: 0,totalPages: 0}
  }
  saveModalImages() {
    if(!this.checkEditImages&&this.fileRemarks.length === 0) {
      this.snotifyService.error('Please choose file images!!!')
    } else {
      let param: ImageDataUpload = {
        images: this.fileRemarks.map(item => {return item.image}),
        remarks: this.fileRemarks.map(item => {return item.remark}),
        hseID: this.hseModelUploadImgCurrent.hsE_Score_ID
      }
      this.spinnerService.show();
      if(!this.checkEditImages) {
        // Upload Images cho hSE chưa up load lần nào
        this.hseResultUploadService.uploadImages(param).subscribe({
          next: (res) => {
            if(res) {
              this.snotifyService.success(MessageConstants.UPLOAD_OK_MSG);
              this.checkClickSearchImageAlert === false ? this.search() : this.searchImageAlert();
              this.modalRef.hide();
              this.spinnerService.hide();
            } else {
              this.handleUploadError();
            }
          },
          error: () => {
            this.handleUploadError();
          }
        });
      } else {
        // Edit images cho HSE đã upload hình,giờ muốn chỉnh sửa.
        this.hseResultUploadService.editImages(param).subscribe({
          next: (res) => {
            if(res) {
              this.snotifyService.success(MessageConstants.UPLOAD_OK_MSG);
              this.checkClickSearchImageAlert === false ? this.search() : this.searchImageAlert();
              this.modalRef.hide();
              this.spinnerService.hide();
            } else {
              this.handleUploadError();
            }
          },
          error: () => {
            this.handleUploadError();
          }
        })
      }
    }
  }
  pageChangedImage(event: any): void {
    this.paginationImage.currentPage = event.page;
    
  }

  removeHseScore(id: number) {
    this.snotifyService.confirm(MessageConstants.CONFIRM_DELETE,ActionConstants.DELETE, () => {
      this.spinnerService.show();
      this.hseResultUploadService.removeHseScore(id).subscribe({
        next: (res) => {
          if(res) {
            this.snotifyService.success(MessageConstants.DELETED_OK_MSG, ActionConstants.DELETE);
            this.checkClickSearchImageAlert === false ? this.search() : this.searchImageAlert();
            this.spinnerService.hide();
          }
        },
        error: () => {
          this.spinnerService.hide();
          this.snotifyService.error(MessageConstants.DELETED_ERROR_MSG);
        }
      })
    });
    
  }

  // ==========================================================End Xử lý Image Trong Modal==========================================================//


  getTemplate() {
    this.spinnerService.show();
    this.hseResultUploadService.getTemplate().subscribe({
      next: (result: Blob) => {
        this.functionUtility.exportExcel(result, 'HSE_Result_Upload');
        this.spinnerService.hide();
      },
      error: () => {
        this.spinnerService.hide();
      }
    })
  }
  uploadExcel(event: any) {
    if (event.target.files[0] && event.target.files[0].name.includes('xlsx')) {
      const form = new FormData();
      form.append('file', event.target.files[0]);
      this.spinnerService.show();
      this.hseResultUploadService.uploadExcel(form).subscribe({
        next: (res) => {
          if(res) {
            this.snotifyService.success(MessageConstants.UPLOAD_OK_MSG);
          }else{
            this.snotifyService.error(MessageConstants.UPLOAD_ERROR_MSG);
          }
          this.spinnerService.hide();
        },
        error: () => {
          this.handleUploadError();
        }
      })

    }
  }

  handleUploadError() {
    this.snotifyService.success(MessageConstants.UPLOAD_OK_MSG);
    this.spinnerService.hide();
  }

  clear() {
    this._time = null;
    this.building = null;
    this.deptId = null;
    this.fileRemarks = [];
    this.dataHSEScore = [];
    this.checkImageAlert = false;
    this.checkClickSearchImageAlert = false;
  }
}
