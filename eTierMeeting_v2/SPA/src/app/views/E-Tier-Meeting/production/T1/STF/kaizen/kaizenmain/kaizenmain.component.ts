import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from "@angular/router";
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { ModalOptions } from 'ngx-bootstrap/modal';
import { environment } from '@environments/environment';
import { eTM_Video_Play_LogDTO } from '@models/eTM_Video_Play_LogDTO';
import { eTM_Video } from '@models/production/T1/STF/eTM_Video';
import { CommonService } from '@services/common.service';
import { ProductionT1STFKaizenService } from '@services/production/T1/STF/production-t1-stf-kaizen.service';
import { FunctionUtility } from '@utilities/function-utility';
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
  urlKaizenSystem: string = environment.ipKaizen;
  bsModalConfig: ModalOptions = {
    backdrop: true,
    animated: true
  }
  constructor(
    private productionT1KaizenService: ProductionT1STFKaizenService,
    private functionUtility: FunctionUtility,
    private commonService: CommonService,
    private route: ActivatedRoute
  ) {
    super(commonService, route);
  }

  ngOnInit(): void {
    this.deptId = this.route.snapshot.params['deptId'];
    this.center_Level = localStorage.getItem(LocalStorageConstants.CENTER_LEVEL);
    this.tier_Level = localStorage.getItem(LocalStorageConstants.TIER_LEVEL);
    this.previousLink = this.center_Level + "/" + this.tier_Level + '/efficiency/efficiencymain/STF/' + this.deptId;
    this.nextLink = this.center_Level + "/" + this.tier_Level + '/modelpreparation/modelpreparationmain/STF/' + this.deptId;
    this.getListVideo();
    setTimeout(() => {
      this.addMeetingLogPage();
    }, 1000);
  }

  async ngOnDestroy() {
    await this.updateMeetingLogPage();
  }

  redirectToKaizenSystem() {
    this.updateMeetingLogPage(true);
    window.open(this.urlKaizenSystem, '_blank')
  }

  getListVideo() {
    const date = this.functionUtility.getToDay();
    this.productionT1KaizenService.getListVideo(this.deptId, date)
      .pipe(untilDestroyed(this))
      .subscribe(res => {
        this.productionT1Videos = res;
      })
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
