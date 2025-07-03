import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ModalDirective, ModalOptions } from 'ngx-bootstrap/modal';
import { environment } from '@environments/environment';
import { eTM_Video } from '@models/eTM_Video';
import { eTM_Video_Play_LogDTO } from '@models/eTM_Video_Play_LogDTO';
import { CommonComponent } from '@commons/common/common';
import { ActivatedRoute } from '@angular/router';
import { CommonService } from "@services/common.service";
import { LocalStorageConstants } from "@constants/storage.constants";

@Component({
  selector: 'app-modelpreparationmain',
  templateUrl: './modelpreparationmain.component.html',
  styleUrls: ['./modelpreparationmain.component.scss']
})
export class ModelpreparationMainComponent extends CommonComponent implements OnInit, OnDestroy {
  @ViewChild('videoModal', { static: false }) videoModal: ModalDirective;
  @ViewChild('videoPlayerEl', { static: false }) videoPlayerEl: ElementRef;

  listModelPreparation: eTM_Video[] = [];
  baseUrl: string = environment.baseUrl;
  videoSrc: string = '';
  bsModalConfig: ModalOptions = {
    backdrop: true,
    animated: true
  }
  previousLink: string = '';
  nextLink: string = '';
  constructor(private commonService: CommonService,
    private route: ActivatedRoute) {
    super(commonService, route);
  }

  ngOnInit(): void {
    this.deptId = this.route.snapshot.params.deptId;
    this.center_Level = localStorage.getItem(LocalStorageConstants.CENTER_LEVEL);
    this.tier_Level = localStorage.getItem(LocalStorageConstants.TIER_LEVEL);
    this.listModelPreparation = this.route.snapshot.data['res'];
    this.previousLink = this.center_Level + "/" + this.tier_Level + '/kaizen/kaizenmain/UPF/' + this.deptId;
    this.nextLink = this.center_Level + "/" + this.tier_Level + '/safety/safetymain/UPF/' + this.deptId;
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
