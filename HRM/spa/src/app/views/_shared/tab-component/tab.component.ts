import { style, animate, AnimationBuilder, AnimationPlayer } from '@angular/animations';
import { afterRender, AfterViewInit, Component, ElementRef, input, model, OnDestroy, OnInit, TemplateRef, Type, ViewChild } from '@angular/core';
import { IconButton } from '@constants/common.constants';
import { TabsetComponent } from 'ngx-bootstrap/tabs';
export interface TabComponentModel {
  id: string;
  title: string;
  isEnable: boolean,
  content: ComponentTemplate | TemplateRef<any>
}
export interface ComponentTemplate {
  component: Type<any>,
  inputs: Record<string, unknown>
}
export interface TabStyle {
  timing: number,
  isHoverEffect: boolean,
  indicatorColor: string
}
export interface TabParameter {
  [key: string]: {
    left: number;
    width: number;
  };
}
@Component({
  selector: 'tab-component',
  templateUrl: './tab.component.html',
  styleUrl: './tab.component.scss',
})
export class TabComponent implements OnInit, AfterViewInit, OnDestroy {
  @ViewChild('tabset', { static: true }) tabSets?: TabsetComponent;
  @ViewChild('header', { read: ElementRef }) header: ElementRef;
  @ViewChild('indicator') indicator: ElementRef;
  defaultStyle: TabStyle = <TabStyle>{
    timing: 400,
    isHoverEffect: true,
    indicatorColor: 'rgb(32, 168, 216)'
  }
  tabs = input.required<TabComponentModel[]>()
  tabStyle = input<TabStyle>(this.defaultStyle)
  tabId = model<string>()
  get _tabId(): string { return !this.tabId() && this.tabs().length > 0 ? this.tabs()[0].id : this.tabId() }
  set _tabId(id: string) { this.tabId.update(x => x = id) }

  iconButton = IconButton;

  resizeObserver: ResizeObserver | null = null;
  tabPos: TabParameter = {};
  allowDetect: boolean = true;

  player: AnimationPlayer;
  constructor(
    private builder: AnimationBuilder
  ) {
    afterRender(() => { if (!this.tabId() && this.tabs().length > 0) this._tabId = this._tabId; })
  }
  ngOnInit(): void {
    if (this.tabStyle().timing == undefined)
      this.tabStyle().timing = this.defaultStyle.timing
    if (this.tabStyle().isHoverEffect == undefined)
      this.tabStyle().isHoverEffect = this.defaultStyle.isHoverEffect
    if (this.tabStyle().indicatorColor == undefined)
      this.tabStyle().indicatorColor = this.defaultStyle.indicatorColor
  }
  ngAfterViewInit(): void {
    this.setDefaultTab();
    this.updateTab()
    this.detectResizeEvent();
  }
  ngOnDestroy(): void {
    this.resizeObserver?.disconnect();
    this.resizeObserver = null;
  }
  setDefaultTab(id?: string) {
    const index = id == null
      ? this.tabSets.tabs.findIndex(f => f.id == this._tabId)
      : this.tabSets.tabs.findIndex(f => f.id == id);
    if (index >= 0)
      this.tabSets.tabs[index].active = true;
  }
  updateTab(onResize: boolean = false) {
    //Tab Position Update
    if (!this.header || !this.tabs()) return;
    const tabHeaders = this.header.nativeElement.querySelectorAll('.nav-background');
    let left = 0;
    tabHeaders.forEach((header: HTMLElement, index: number) => {
      if (index < this.tabs().length) {
        const tabId = this.tabs()[index].id;
        this.tabPos[tabId] = { left, width: header.offsetWidth };
        left += header.offsetWidth;
      }
    });
    //Animation Update
    this.callAnimate(this.tabPos[this._tabId].left, this.tabPos[this._tabId].width, onResize ? 0 : this.tabStyle().timing)
  }
  callAnimate(left: number, width: number, period: number) {
    const myAnimation = this.builder.build([
      animate(period, style({ left: `${left}px`, width: `${width}px` }))
    ])
    this.player = myAnimation.create(this.indicator.nativeElement);
    this.player.play();
  }
  detectResizeEvent(): void {
    this.resizeObserver = new ResizeObserver(_ =>
      this.allowDetect ? this.updateTab(true) : (this.updateTab(), this.allowDetect = true)
    );
    this.resizeObserver.observe(this.header.nativeElement);
  }
  onTabChange(id: string) {
    this.allowDetect = false
    this._tabId = id
    this.updateTab()
  }
  getBorderImage() { return `linear-gradient(to left, transparent 40%, ${this.tabStyle().indicatorColor} 50%, transparent 60%)`; }
}
