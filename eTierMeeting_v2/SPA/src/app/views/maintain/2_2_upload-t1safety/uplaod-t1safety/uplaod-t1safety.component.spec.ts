import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UplaodT1safetyComponent } from './uplaod-t1safety.component';

describe('UplaodT1safetyComponent', () => {
  let component: UplaodT1safetyComponent;
  let fixture: ComponentFixture<UplaodT1safetyComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UplaodT1safetyComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UplaodT1safetyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
