import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EfficiencymainComponent } from './efficiencymain.component';

describe('EfficiencymainComponent', () => {
  let component: EfficiencymainComponent;
  let fixture: ComponentFixture<EfficiencymainComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EfficiencymainComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EfficiencymainComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
