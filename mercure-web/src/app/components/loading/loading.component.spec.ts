import {ComponentFixture, TestBed} from '@angular/core/testing';

import {LoadingComponent} from './loading.component';
import {AppComponent} from '../../app.component';
import {AuthService} from "../../services/auth/auth.service";
import {RouterTestingModule} from "@angular/router/testing";

describe('LoadingComponent', () => {
  let component: LoadingComponent;
  let fixture: ComponentFixture<LoadingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [LoadingComponent],
      providers: [AppComponent, AuthService],
      imports: [RouterTestingModule]
    })
      .compileComponents();

    fixture = TestBed.createComponent(LoadingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
