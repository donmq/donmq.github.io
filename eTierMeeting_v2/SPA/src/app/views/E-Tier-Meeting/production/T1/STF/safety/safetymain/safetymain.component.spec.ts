import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SafetymainComponent } from './safetymain.component';

describe('SafetymainComponent', () => {
  let component: SafetymainComponent;
  let fixture: ComponentFixture<SafetymainComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SafetymainComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SafetymainComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
