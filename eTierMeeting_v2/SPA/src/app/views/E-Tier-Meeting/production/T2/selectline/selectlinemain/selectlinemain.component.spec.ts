import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SelectlinemainComponent } from './selectlinemain.component';

describe('SelectlinemainComponent', () => {
  let component: SelectlinemainComponent;
  let fixture: ComponentFixture<SelectlinemainComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SelectlinemainComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SelectlinemainComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
