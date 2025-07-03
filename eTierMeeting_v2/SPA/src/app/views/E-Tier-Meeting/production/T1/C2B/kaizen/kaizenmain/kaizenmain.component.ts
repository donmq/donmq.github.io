import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from "@angular/router";
import { UntilDestroy } from '@ngneat/until-destroy';
import { ModalOptions } from 'ngx-bootstrap/modal';
import { environment } from '@environments/environment';
import { eTM_Video } from '@models/eTM_Video';
import { eTM_Video_Play_LogDTO } from '@models/eTM_Video_Play_LogDTO';
import { CommonService } from '@services/common.service';
import { CommonComponent } from '@commons/common/common';
import { LocalStorageConstants } from "@constants/storage.constants";

@UntilDestroy()
@Component({
  selector: 'app-kaizenmain',
  templateUrl: './kaizenmain.component.html',
  styleUrls: ['./kaizenmain.component.scss']
})
export class KaizenmainComponent extends CommonComponent implements OnInit, OnDestroy {
  @ViewChild('videoRef', { static: false }) videoRef: ElementRef;
  mediaUrl: string = environment.baseUrl;
  productionT1Videos: eTM_Video[] = [];
  video: string = '';
  previousLink: string = '';
  nextLink: string = '';
  bsModalConfig: ModalOptions = {
    backdrop: true,
    animated: true
  }

  constructor(
    private commonService: CommonService,
    private route: ActivatedRoute,
    private router: Router,
  ) {
    super(commonService, route);
  }

  ngOnInit(): void {
    this.deptId = this.route.snapshot.params['deptId'];
    this.center_Level = localStorage.getItem(LocalStorageConstants.CENTER_LEVEL);
    this.tier_Level = localStorage.getItem(LocalStorageConstants.TIER_LEVEL);
    this.previousLink = this.center_Level + "/" + this.tier_Level + '/efficiency/efficiencymain/CTB/' + this.deptId;
    this.nextLink = this.center_Level + "/" + this.tier_Level + '/modelpreparation/modelpreparationmain/CTB/' + this.deptId;
    this.productionT1Videos = this.route.snapshot.data['res'];
    setTimeout(() => {
      this.addMeetingLogPage();
    }, 1000);
  }

  async ngOnDestroy() {
    await this.updateMeetingLogPage();
  }

  loadVideo(video: string) {
    this.video = this.mediaUrl + video;
    return video;
  }

  stopVideo() {
    this.videoRef.nativeElement.pause();
    this.videoRef.nativeElement.currentTime = 0;
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
