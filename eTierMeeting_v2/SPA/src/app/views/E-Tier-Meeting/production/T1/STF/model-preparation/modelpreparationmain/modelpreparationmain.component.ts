import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { ModalDirective, ModalOptions } from 'ngx-bootstrap/modal';
import { environment } from '@environments/environment';
import { eTM_Video_Play_LogDTO } from '@models/eTM_Video_Play_LogDTO';
import { ProductionT1Video } from '@models/production-t1-video';
import { eTM_Video } from '@models/production/T1/STF/eTM_Video';
import { ProductionT1STFModelpreparationService } from '@services/production/T1/STF/production-t1-stf-modelpreparation.service';
import { CommonComponent } from '@commons/common/common';
import { ActivatedRoute } from '@angular/router';
import { CommonService } from "@services/common.service";
import { LocalStorageConstants } from "@constants/storage.constants";

@UntilDestroy()
@Component({
  selector: 'app-modelpreparationmain',
  templateUrl: './modelpreparationmain.component.html',
  styleUrls: ['./modelpreparationmain.component.scss']
})
export class ModelPreparationmainComponent extends CommonComponent implements OnInit, OnDestroy {
  @ViewChild('videoModal', { static: false }) videoModal: ModalDirective;
  @ViewChild('videoPlayerEl', { static: false }) videoPlayerEl: ElementRef;

  modelPreparation: eTM_Video[] = [];
  baseUrl: string = environment.baseUrl;
  videoSrc: string = '';
  bsModalConfig: ModalOptions = {
    backdrop: true,
    animated: true
  }
  previousLink: string = '';
  nextLink: string = '';

  constructor(
    private productionT1ModelpreparationService: ProductionT1STFModelpreparationService,
    private commonService: CommonService,
    private route: ActivatedRoute
  ) {
    super(commonService, route);
  }

  ngOnInit() {
    this.deptId = this.route.snapshot.params.deptId;
    this.center_Level = localStorage.getItem(LocalStorageConstants.CENTER_LEVEL);
    this.tier_Level = localStorage.getItem(LocalStorageConstants.TIER_LEVEL);
    this.previousLink = this.center_Level + "/" + this.tier_Level + '/kaizen/kaizenmain/STF/' + this.deptId;
    this.nextLink = this.center_Level + "/" + this.tier_Level + '/safety/safetymain/STF/' + this.deptId;
    this.getModelPreparation();
    setTimeout(() => {
      this.addMeetingLogPage();
    }, 1000);
  }

  async ngOnDestroy() {
    await this.updateMeetingLogPage();
  }

  getModelPreparation() {
    this.productionT1ModelpreparationService
      .getModelPreparation(this.deptId)
      .pipe(untilDestroyed(this))
      .subscribe(res => this.modelPreparation = res);
  }

  showVideoModal(item: ProductionT1Video) {
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
