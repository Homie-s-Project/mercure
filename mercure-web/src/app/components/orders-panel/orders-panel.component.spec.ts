import {ComponentFixture, TestBed} from '@angular/core/testing';

import {OrdersPanelComponent} from './orders-panel.component';
import {OrderService} from "../../services/order/order.service";
import {HttpClientTestingModule} from "@angular/common/http/testing";

describe('OrdersPanelComponent', () => {
  let component: OrdersPanelComponent;
  let fixture: ComponentFixture<OrdersPanelComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      providers: [OrderService],
      imports: [HttpClientTestingModule]
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
