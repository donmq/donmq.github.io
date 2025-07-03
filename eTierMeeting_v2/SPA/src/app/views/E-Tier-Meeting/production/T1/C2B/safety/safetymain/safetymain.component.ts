import { Component, ElementRef, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { UntilDestroy } from '@ngneat/until-destroy';
import { ModalDirective, ModalOptions } from 'ngx-bootstrap/modal';
import { environment } from '@environments/environment';
import { eTM_Video } from '@models/eTM_Video';
import { eTM_Video_Play_LogDTO } from '@models/eTM_Video_Play_LogDTO';
import { CommonComponent } from '@commons/common/common';
import { ActivatedRoute } from '@angular/router';
import { CommonService } from "@services/common.service";
import { LocalStorageConstants } from "@constants/storage.constants";

@UntilDestroy()
@Component({
  selector: 'app-safetymain',
  templateUrl: './safetymain.component.html',
  styleUrls: ['./safetymain.component.scss']
})
export class SafetymainComponent extends CommonComponent implements OnInit, OnDestroy {
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
    private commonService: CommonService,
    private route: ActivatedRoute
  ) {
    super(commonService, route);
  }

  ngOnInit(): void {
    this.todayData = this.route.snapshot.data['res'];
    this.deptId = this.route.snapshot.params.deptId;
    this.center_Level = localStorage.getItem(LocalStorageConstants.CENTER_LEVEL);
    this.tier_Level = localStorage.getItem(LocalStorageConstants.TIER_LEVEL);
    this.previousLink = this.center_Level + "/" + this.tier_Level + '/modelpreparation/modelpreparationmain/CTB/' + this.deptId;
    this.nextLink = this.center_Level + "/" + this.tier_Level + '/quality/qualitymain/CTB/' + this.deptId;
    setTimeout(() => {
      this.addMeetingLogPage();
    }, 1000);
  }

  async ngOnDestroy() {
    await this.updateMeetingLogPage();
  }

  showVideoModal(item: eTM_Video) {
    this.videoSrc = `${this.baseUrl}${item.video_Path}`;
    this.videoModal.show();
  }

  pauseVideo() {
    this.videoPlayerEl.nativeElement.pause();
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
