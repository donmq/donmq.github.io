/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { UploadDataHpA15Component } from './upload-data-hp-a15.component';

describe('UploadDataHpA15Component', () => {
  let component: UploadDataHpA15Component;
  let fixture: ComponentFixture<UploadDataHpA15Component>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UploadDataHpA15Component ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UploadDataHpA15Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
