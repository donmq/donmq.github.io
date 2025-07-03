import { Component, Input } from '@angular/core';

@Component({
  selector: 'chart-legend',
  templateUrl: './chart-legend.component.html',
  styleUrls: ['./chart-legend.component.scss']
})
export class ChartLegendComponent {
  @Input() items: ChartLegendItem[] = [];
}

export interface ChartLegendItem {
  color: string;
  text: string;
}