import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeptclassificationListComponent } from './deptclassification-list.component';

describe('DeptclassificationListComponent', () => {
  let component: DeptclassificationListComponent;
  let fixture: ComponentFixture<DeptclassificationListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DeptclassificationListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DeptclassificationListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
