import {ComponentFixture, TestBed} from '@angular/core/testing';

import {ItemCartComponent} from './item-cart.component';
import {CartService} from "../../services/cart/cart.service";
import {HttpClientTestingModule} from "@angular/common/http/testing";
import {RouterTestingModule} from "@angular/router/testing";

describe('ItemCartComponent', () => {
  let component: ItemCartComponent;
  let fixture: ComponentFixture<ItemCartComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      providers: [ CartService ],
      declarations: [ ItemCartComponent ],
      imports: [ HttpClientTestingModule, RouterTestingModule ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ItemCartComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
