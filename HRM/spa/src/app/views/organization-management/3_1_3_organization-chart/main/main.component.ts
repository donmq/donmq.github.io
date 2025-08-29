import { Component, ElementRef, OnDestroy, OnInit, ViewChild, effect } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ClassButton, IconButton } from '@constants/common.constants';
import { LocalStorageConstants } from '@constants/local-storage.constants';
import { INodeDto, OrganizationChartParam, OrganizationChart_MainMemory } from '@models/organization-management/3_1_3_organization-chart';
import { ModalService } from '@services/modal.service';
import { S_3_1_3_OrganizationChart } from '@services/organization-management/s_3_1_3_organization-chart.service';
import { InjectBase } from '@utilities/inject-base-app';
import { KeyValuePair } from '@utilities/key-value-pair';
import html2canvas, { Options } from 'html2canvas';
import jsPDF from 'jspdf';
import { PanZoomAPI, PanZoomConfig, PanZoomConfigOptions, PanZoomModel } from 'ngx-panzoom';
import { Subscription } from 'rxjs';
@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.scss']
})
export class MainComponent extends InjectBase implements OnInit, OnDestroy {
  @ViewChild('divRef') divRef: ElementRef;
  private panZoomAPI: PanZoomAPI;
  private apiSubscription: Subscription;
  private modelChangedSubscription: Subscription;
  private panZoomConfigOptions: PanZoomConfigOptions = {
    zoomLevels: 4,
    scalePerZoomLevel: 4,
    zoomStepDuration: 0.2,
    freeMouseWheelFactor: 0.0004,
    zoomToFitZoomLevelFactor: 0.9,
    zoomButtonIncrement: 0.4,
    dragMouseButton: 'left'
  };
  private initPosition: PanZoomModel = null
  initialised: boolean = false

  panzoomConfig: PanZoomConfig;
  iconButton = IconButton;
  classButton = ClassButton;

  param: OrganizationChartParam = <OrganizationChartParam>{};

  factoryList: KeyValuePair[] = [];
  divisionList: KeyValuePair[] = [];
  levelList: KeyValuePair[] = [];
  departmentList: KeyValuePair[] = [];
  data: INodeDto[] = []
  isEnabledPDF: boolean = false
  optionPDF: Partial<Options> = {
    scale: 2,
    useCORS: true,
    allowTaint: true,
    scrollY: -window.scrollY
  }
  pageLayout: string = 'a4'
  defaultLevel: string = '7'
  title: string = '';
  programCode: string = '';

  constructor(
    private service: S_3_1_3_OrganizationChart,
    private modalService: ModalService
  ) {
    super();
    this.programCode = this.route.snapshot.data['program'];
    this.translateService.onLangChange.pipe(takeUntilDestroyed()).subscribe(() => {
      this.title = this.functionUtility.getTitle(this.route.snapshot.data['program'])
      this.loadData()
    });
    effect(() => {
      this.param = this.service.paramSearch().param;
      this.initPosition = this.service.paramSearch().position
      this.data = this.service.paramSearch().data
      this.panzoomConfig = this.initPosition
        ? this.initPanzoomConfig(this.initPosition)
        : this.panzoomConfig = this.initPanzoomConfig();
      this.apiSubscription = this.panzoomConfig.api.subscribe((api: PanZoomAPI) =>
        this.panZoomAPI = api
      );
      this.modelChangedSubscription = this.panzoomConfig.modelChanged.subscribe((model: PanZoomModel) =>
        this.onModelChanged(model)
      );
      this.loadData()
    });
  }
  private loadData() {
    this.retryGetDropDownList()
    if (!this.functionUtility.checkEmpty(this.param.factory)) {
      this.getDepartment()
      if (this.data.length > 0) {
        if (!this.functionUtility.checkFunction('Search'))
          this.clear()
        else
          this.getData()
      }
    }
  }
  initPanzoomConfig(initPosition?: PanZoomModel): PanZoomConfig {
    return initPosition
      ? {
        ...new PanZoomConfig(this.panZoomConfigOptions),
        initialZoomLevel: initPosition.zoomLevel,
        initialPanX: initPosition.pan.x,
        initialPanY: initPosition.pan.y
      }
      : {
        ...new PanZoomConfig(this.panZoomConfigOptions)
      }
  }
  onModelChanged(model: PanZoomModel): void {
    if (this.initialised)
      this.initPosition = model
  }
  ngOnDestroy(): void {
    this.service.setParamSearch(<OrganizationChart_MainMemory>{ param: this.param, position: this.initPosition, data: this.data });
    this.apiSubscription.unsubscribe();
    this.modelChangedSubscription.unsubscribe();
  }
  ngOnInit(): void {
    this.title = this.functionUtility.getTitle(this.route.snapshot.data['program'])
    this.route.data.subscribe(
      (role) => {
        this.filterList(role.dataResolved)
      }).unsubscribe();;
  }
  retryGetDropDownList() {
    this.service.getDropDownList(this.param.division)
      .subscribe({
        next: (res) => {
          this.filterList(res)
        },
        error: () => {
          this.snotifyService.error(
            this.translateService.instant('System.Message.UnknowError'),
            this.translateService.instant('System.Caption.Error')
          );
        }
      });
  }
  filterList(keys: KeyValuePair[]) {
    this.factoryList = structuredClone(keys.filter((x: { key: string; }) => x.key == "F")).map(x => <KeyValuePair>{ key: x.key = x.value.substring(0, x.value.indexOf('-')), value: x.value })
    this.divisionList = structuredClone(keys.filter((x: { key: string; }) => x.key == "D")).map(x => <KeyValuePair>{ key: x.key = x.value.substring(0, x.value.indexOf('-')), value: x.value })
    this.levelList = structuredClone(keys.filter((x: { key: string; }) => x.key == "L")).map(x => <KeyValuePair>{ key: x.key = x.value.substring(0, x.value.indexOf('-')), value: x.value })
  }
  search = () => {
    this.initPosition = null
    this.initialised = false
    this.data = []
    this.getData(true)
  }
  getData = (isSearch?: boolean) => {
    this.spinnerService.show();
    this.service
      .getChartData(this.param)
      .subscribe({
        next: (res) => {
          this.spinnerService.hide();
          this.data = res;
          this.isEnabledPDF = res.length > 0 ? true : false
          if (isSearch)
            this.functionUtility.snotifySuccessError(true,'System.Message.SearchOKMsg')

          if (!this.initPosition)
            setTimeout(() => {
              this.onResetViewClicked();
            }, 300);
          this.initialised = !!this.panzoomConfig
        },
        error: () => this.functionUtility.snotifySystemError()
      });
  };
  clear() {
    this.param = <OrganizationChartParam>{
      level: this.defaultLevel
    };
    this.data = []
    this.isEnabledPDF = false
  }
  pdf(element: any) {
    this.spinnerService.show();
    if (this.panZoomAPI.model.zoomLevel <= 0.5) {
      this.optionPDF.scale = 8
      this.pageLayout = 'a1'
    }
    else if (0.5 < this.panZoomAPI.model.zoomLevel && this.panZoomAPI.model.zoomLevel <= 1) {
      this.optionPDF.scale = 6
      this.pageLayout = 'a2'
    }
    else if (1 < this.panZoomAPI.model.zoomLevel && this.panZoomAPI.model.zoomLevel <= 1.5) {
      this.optionPDF.scale = 4
      this.pageLayout = 'a3'
    }
    else {
      this.optionPDF.scale = 2
      this.pageLayout = 'a4'
    }
    html2canvas(element, this.optionPDF)
      .then((canvas) => {
        let image: string = canvas.toDataURL('image/jpg', 1.0);
        let doc: jsPDF = new jsPDF('l', 'px', this.pageLayout);
        let pageWidth: number = doc.internal.pageSize.getWidth();
        let pageHeight: number = doc.internal.pageSize.getHeight();
        let widthRatio: number = pageWidth / canvas.width;
        let heightRatio: number = pageHeight / canvas.height;
        let ratio: number = widthRatio > heightRatio ? heightRatio : widthRatio;
        let canvasWidth: number = canvas.width * ratio * 1;
        let canvasHeight: number = canvas.height * ratio * 1;
        let marginX: number = (pageWidth - canvasWidth) / 2;
        let marginY: number = (pageHeight - canvasHeight) / 2;
        doc.addImage(image, 'PNG', marginX, marginY, canvasWidth, canvasHeight);
        return doc;
      })
      .then((doc) => {
        const fileName = this.functionUtility.getFileNameExport(this.programCode, 'Download')
        doc.save(`${fileName}.pdf`)
        this.spinnerService.hide();
      });
  }
  excel() {
    this.spinnerService.show();
    this.service
      .downloadExcel(this.param)
      .subscribe({
        next: (result) => {
          this.spinnerService.hide();
          const fileName = this.functionUtility.getFileNameExport(this.programCode, 'Download')
          this.functionUtility.exportExcel(result.data, fileName);
        },
        error: () => this.functionUtility.snotifySystemError()
      });
  }
  detail(item: INodeDto) {
    this.modalService.open(item);
  }
  deleteProperty(name: string) {
    delete this.param[name]
  }
  onZoomInClicked(): void {
    this.panZoomAPI.zoomIn('viewCenter');
  }
  onZoomOutClicked(): void {
    this.panZoomAPI.zoomOut('viewCenter');
  }
  async onResetViewClicked() {
    await this.goCenter()
    await this.goZoom()
  }
  async goCenter() {
    return new Promise((resolve) => {
      resolve(this.panZoomAPI.centerContent());
    });
  }
  async goZoom() {
    const width = this.divRef.nativeElement.clientWidth
    const height = this.divRef.nativeElement.clientHeight
    return new Promise((resolve) => {
      setTimeout(() => {
        resolve(this.panZoomAPI.changeZoomLevel(2, { x: width / 2, y: height / 2 }));
      }, 300);
    });
  }
  getDepartment(onFactoryChange?: boolean) {
    if (this.param.division && this.param.factory) {
      this.spinnerService.show();
      this.service
        .getDepartmentList(this.param)
        .subscribe({
          next: (res) => {
            this.spinnerService.hide();
            this.departmentList = res;
            if (onFactoryChange)
              this.deleteProperty('department')
          },
          error: () => this.functionUtility.snotifySystemError()
        });
    }
  }
  onDivisionChange() {
    this.retryGetDropDownList()
    this.deleteProperty('factory')
    this.deleteProperty('department')
  }
}
