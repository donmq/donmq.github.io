import { Component, OnDestroy, OnInit } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { environment } from '@environments/environment';
import { CaptionConstants, MessageConstants } from '@constants/system.constants';
import { eTM_HSE_Score_Image } from '@models/production/T2/CTB/eTM_HSE_Score_Image';
import { EvaluetionCategory } from '@models/production/T2/CTB/sefetyViewModel';
import { NgSnotifyService } from '@services/ng-snotify.service';
import { ProductionT2SafetyService } from '@services/production/T2/CTB/production-t2-safety.service';
import { CommonComponent } from '@commons/common/common';
import { ActivatedRoute } from '@angular/router';
import { CommonService } from "@services/common.service";
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { LocalStorageConstants } from "@constants/storage.constants";

@UntilDestroy()
@Component({
  selector: 'app-safetymain',
  templateUrl: './safetymain.component.html',
  styleUrls: ['./safetymain.component.scss']
})
export class SafetymainComponent extends CommonComponent implements OnInit, OnDestroy {
  factory: string = '';
  localTranslation: any = {
    title: '',
    category: '',
    weight: '',
    achievement: ''
  }
  previousLink: string = '';
  nextLink: string = '';
  building: string = '';
  month: number = 0;
  year: number = 0;
  evaluetions: EvaluetionCategory[] = [];
  lines: string[] = [];
  images: eTM_HSE_Score_Image[] = [];
  imageDetail: eTM_HSE_Score_Image = <eTM_HSE_Score_Image>{};
  totalImage: number = 0;
  hideButton: boolean = true;
  mediaUrl: string = environment.baseUrl;

  constructor(
    private productionT2SafetyService: ProductionT2SafetyService,
    private spinner: NgxSpinnerService,
    private snotify: NgSnotifyService,
    private commonService: CommonService,
    private route: ActivatedRoute
  ) {
    super(commonService, route);
  }

  ngOnInit(): void {
    this.deptId = this.route.snapshot.params['deptId'];
    this.building = this.deptId;
    this.center_Level = localStorage.getItem(LocalStorageConstants.CENTER_LEVEL);
    this.tier_Level = localStorage.getItem(LocalStorageConstants.TIER_LEVEL);
    this.previousLink = this.center_Level + "/" + this.tier_Level + '/kaizen/kaizenmain/CTB/' + this.building;
    this.nextLink = this.center_Level + "/" + this.tier_Level + '/quality/qualitymain/CTB/' + this.building;
    this.factory = localStorage.getItem(LocalStorageConstants.FACTORY);
    this.getLocalTranslation(this.factory);
    this.getData();
    setTimeout(() => {
      this.addMeetingLogPage();
    }, 1000);
  }

  async ngOnDestroy() {
    await this.updateMeetingLogPage();
  }

  getLocalTranslation(factory: string) {
    if (factory === 'SHC' || factory === 'CB') {
      this.localTranslation = {
        title: 'Kết quả HSE tuần xưởng hàng tháng',
        category: 'Hạng mục kiểm tra / đánh giá',
        weight: 'Điểm chuẩn',
        achievement: 'Điểm đạt được'
      }
    }
    if (factory === 'TSH') {
      this.localTranslation = {
        title: 'Hasil Walkthrough HSE Per Bulan',
        category: 'Kategori Evaluasi',
        weight: 'Bobot',
        achievement: 'Pencapaian'
      }
    }
    // if (factory === 'SPC') {
    //   this.localTranslation = {
    //     title: 'လစဥ်HSEစက်ရုံလှည့်ပတ်စစ်ဆေးရေးရလဒ်',
    //     category: 'အကဲဖြတ်ခြင်း',
    //     weight: 'စံချိန်ရမှတ်',
    //     achievement: 'အောင်မြင်မှုရလဒ်'
    //   }
    // }
  }

  getData() {
    this.spinner.show();
    this.productionT2SafetyService.getData(this.building).pipe(untilDestroyed(this))
      .subscribe({
        next: (res) => {
          this.month = res.month;
          this.year = res.year;
          this.evaluetions = res.evaluetions;
          this.lines = res.lines;
          this.spinner.hide();
        }, error: () => {
          this.snotify.error(MessageConstants.UN_KNOWN_ERROR, CaptionConstants.ERROR);
          this.spinner.hide();
        }
      })
  }

  getDetailScoreUnPass(hseScoreID: number) {
    this.spinner.show();
    let index: number = 1;
    this.productionT2SafetyService.getDetailScoreUnPass(hseScoreID).pipe(untilDestroyed(this))
      .subscribe({
        next: (res) => {
          this.images = res;
          this.images.forEach(x => {
            x.index = index;
            x.image_Path = `${this.mediaUrl}${x.image_Path}`;
            x.noImage = !x.image_Path ? true : false;
            index++;
          });
          this.totalImage = this.images?.length;
          this.imageDetail = this.totalImage > 0 ? this.images[0] : <eTM_HSE_Score_Image>{ noImage: true };
          this.spinner.hide();
        }, error: () => {
          this.snotify.error(MessageConstants.UN_KNOWN_ERROR, CaptionConstants.ERROR);
          this.spinner.hide();
        }
      })
  }

  prevImage(index: number) {
    this.imageDetail = index < 2 ? this.images[this.totalImage - 1] : this.images?.find(x => x.index == index - 1);
  }

  nextImage(index: number) {
    this.imageDetail = index < this.totalImage ? this.images?.find(x => x.index == index + 1) : this.images[0];
  }
}
