import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeptclassificationEditComponent } from './deptclassification-edit.component';

describe('DeptclassificationEditComponent', () => {
  let component: DeptclassificationEditComponent;
  let fixture: ComponentFixture<DeptclassificationEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DeptclassificationEditComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DeptclassificationEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
