import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ModalDirective, ModalOptions } from 'ngx-bootstrap/modal';
import { environment } from '@environments/environment';
import { eTM_Video_Play_LogDTO } from '@models/eTM_Video_Play_LogDTO';
import { eTM_Video } from '@models/production/T1/UPF/eTM_Video';
import { ProductionT1UpfSafetyService } from '@services/production/T1/UPF/production-t1-upf-safety.service';
import { CommonComponent } from '@commons/common/common';
import { ActivatedRoute } from '@angular/router';
import { CommonService } from "@services/common.service";
import { LocalStorageConstants } from "@constants/storage.constants";

@Component({
  selector: 'app-safety-main',
  templateUrl: './safety-main.component.html',
  styleUrls: ['./safety-main.component.scss']
})
export class SafetyMainComponent extends CommonComponent implements OnInit, OnDestroy {
  @ViewChild('videoModal', { static: false }) videoModal: ModalDirective;
  @ViewChild('videoPlayerEl', { static: false }) videoPlayerEl: ElementRef;
  todayData: eTM_Video[] = [];
  baseUrl: string = environment.baseUrl;
  videoSrc: string = '';
  bsModalConfig: ModalOptions = {
    backdrop: true,
    animated: true
  }
  previousLink: string = '';
  nextLink: string = '';

  constructor(
    private productionT1UPFSafetyService: ProductionT1UpfSafetyService,
    private commonService: CommonService,
    private route: ActivatedRoute
  ) {
    super(commonService, route);
  }

  ngOnInit() {
    this.deptId = this.route.snapshot.params.deptId;
    this.center_Level = localStorage.getItem(LocalStorageConstants.CENTER_LEVEL);
    this.tier_Level = localStorage.getItem(LocalStorageConstants.TIER_LEVEL);
    this.previousLink = this.center_Level + "/" + this.tier_Level + '/modelpreparation/modelpreparationmain/UPF/' + this.deptId;
    this.nextLink = this.center_Level + "/" + this.tier_Level + '/quality/qualitymain/UPF/' + this.deptId;
    this.getTodayData();
    setTimeout(() => {
      this.addMeetingLogPage();
    }, 1000);
  }

  async ngOnDestroy() {
    await this.updateMeetingLogPage();
  }

  getTodayData = () => {
    this.productionT1UPFSafetyService.getTodayData(this.deptId).subscribe({
      next: res => {
        this.todayData = res;
      }, error: error => {
        console.log(error);
      }
    })
  }


  pauseVideo = () => {
    this.videoPlayerEl.nativeElement.pause();
  }

  showVideoModal = (item: eTM_Video) => {
    this.videoSrc = `${this.baseUrl}${item.video_Path}`;
    this.videoModal.show();
  }

  addVideoLog() {
    let videoLog: eTM_Video_Play_LogDTO = <eTM_Video_Play_LogDTO>{
      center_Level: this.center_Level,
      tier_Level: this.tier_Level,
      deptId: this.deptId,
      class_Level: this.route.snapshot.data['class_Level'],
      page_Name: this.route.snapshot.data['page_Name']
    };
    this.commonService.addVideoLog(videoLog).subscribe(res => {
      if (!res.success) {
        console.error(res.message);
      }
    })
  }
}
