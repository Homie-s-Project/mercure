import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ItemCardProductComponent } from './item-card-product.component';

describe('ItemCardProductComponent', () => {
  let component: ItemCardProductComponent;
  let fixture: ComponentFixture<ItemCardProductComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ItemCardProductComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ItemCardProductComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
