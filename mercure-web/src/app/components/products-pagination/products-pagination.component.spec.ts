import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductsPaginationComponent } from './products-pagination.component';
import {HttpClientTestingModule} from "@angular/common/http/testing";

describe('ProductsPaginationComponent', () => {
  let component: ProductsPaginationComponent;
  let fixture: ComponentFixture<ProductsPaginationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProductsPaginationComponent ],
      imports: [ HttpClientTestingModule ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProductsPaginationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
