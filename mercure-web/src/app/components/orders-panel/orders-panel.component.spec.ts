import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OrdersPanelComponent } from './orders-panel.component';

describe('OrdersPanelComponent', () => {
  let component: OrdersPanelComponent;
  let fixture: ComponentFixture<OrdersPanelComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ OrdersPanelComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(OrdersPanelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
