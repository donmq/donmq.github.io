import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ModalOptions } from 'ngx-bootstrap/modal';
import { environment } from '@environments/environment';
import { eTM_Video } from '@models/eTM_Video';
import { eTM_Video_Play_LogDTO } from '@models/eTM_Video_Play_LogDTO';
import { CommonComponent } from '@commons/common/common';
import { ActivatedRoute } from '@angular/router';
import { CommonService } from "@services/common.service";
import { LocalStorageConstants } from "@constants/storage.constants";

@Component({
  selector: 'app-kaizenmain',
  templateUrl: './kaizenmain.component.html',
  styleUrls: ['./kaizenmain.component.scss']
})
export class KaizenmainComponent extends CommonComponent implements OnInit, OnDestroy {
  @ViewChild('videoRef', { static: false }) videoRef: ElementRef;
  mediaUrl: string = environment.baseUrl;
  video: string = '';
  previousLink: string = '';
  nextLink: string = '';
  urlKaizenSystem: string = environment.ipKaizen;
  bsModalConfig: ModalOptions = {
    backdrop: true,
    animated: true
  }
  productionT1UPFVideos: eTM_Video[] = [];
  constructor(private commonService: CommonService,
    private route: ActivatedRoute) {
    super(commonService, route);
  }

  ngOnInit(): void {
    this.deptId = this.route.snapshot.params['deptId'];
    this.center_Level = localStorage.getItem(LocalStorageConstants.CENTER_LEVEL);
    this.tier_Level = localStorage.getItem(LocalStorageConstants.TIER_LEVEL);
    this.previousLink = this.center_Level + "/" + this.tier_Level + '/efficiency/efficiencymain/UPF/' + this.deptId;
    this.nextLink = this.center_Level + "/" + this.tier_Level + '/modelpreparation/modelpreparationmain/UPF/' + this.deptId;
    this.productionT1UPFVideos = this.route.snapshot.data['res'];
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
