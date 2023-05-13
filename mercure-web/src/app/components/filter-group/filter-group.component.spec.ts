import {ComponentFixture, TestBed} from '@angular/core/testing';

import {FilterGroupComponent} from './filter-group.component';
import {HttpClientTestingModule} from "@angular/common/http/testing";

describe('FilterGroupComponent', () => {
  let component: FilterGroupComponent;
  let fixture: ComponentFixture<FilterGroupComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FilterGroupComponent ],
      imports: [HttpClientTestingModule]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FilterGroupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
