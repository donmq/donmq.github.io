/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { QualitymainComponent } from './qualitymain.component';

describe('QualitymainComponent', () => {
  let component: QualitymainComponent;
  let fixture: ComponentFixture<QualitymainComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ QualitymainComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(QualitymainComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
