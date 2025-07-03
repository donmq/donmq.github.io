import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UploadT1safetyAddComponent } from './upload-t1safety-add.component';

describe('UploadT1safetyAddComponent', () => {
  let component: UploadT1safetyAddComponent;
  let fixture: ComponentFixture<UploadT1safetyAddComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UploadT1safetyAddComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UploadT1safetyAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
