import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeliverymainComponent } from './deliverymain.component';

describe('DeliverymainComponent', () => {
  let component: DeliverymainComponent;
  let fixture: ComponentFixture<DeliverymainComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DeliverymainComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DeliverymainComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
