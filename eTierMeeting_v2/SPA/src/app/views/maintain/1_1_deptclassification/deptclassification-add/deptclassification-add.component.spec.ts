import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeptclassificationAddComponent } from './deptclassification-add.component';

describe('DeptclassificationAddComponent', () => {
  let component: DeptclassificationAddComponent;
  let fixture: ComponentFixture<DeptclassificationAddComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DeptclassificationAddComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DeptclassificationAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
