import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AppComponent } from 'src/app/app.component';
import { ProductComponent } from './product.component';
import { ProductService } from 'src/app/services/product/product.service';
import { ActivatedRoute } from '@angular/router';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

describe('ProductComponent', () => {
  let component: ProductComponent;
  let fixture: ComponentFixture<ProductComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProductComponent ],
      providers: [ 
        AppComponent, 
        ProductService, 
        {
          provide: ActivatedRoute, 
          useValue: {
            snapshot: {
              paramMap: {
                get(): number {
                  return 1;
                }
              }
            }
          }
        } 
      ],
      imports: [BrowserAnimationsModule]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProductComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
