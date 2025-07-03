import { ComponentFixture, TestBed } from '@angular/core/testing';

import { KaizenmainComponent } from './kaizenmain.component';

describe('KaizenmainComponent', () => {
  let component: KaizenmainComponent;
  let fixture: ComponentFixture<KaizenmainComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ KaizenmainComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(KaizenmainComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
