import { Component, OnDestroy, OnInit, Renderer2 } from '@angular/core';
import { faGoogle, faMicrosoft } from '@fortawesome/free-brands-svg-icons';
import { faArrowLeft } from '@fortawesome/free-solid-svg-icons';
import { AppComponent } from 'src/app/app.component';


@Component({
  selector: 'app-landing',
  templateUrl: './landing.component.html',
  styleUrls: ['./landing.component.scss']
})
export class LandingComponent implements OnDestroy{
  faGoogle = faGoogle;
  faMicrosoft = faMicrosoft;
  faArrowLeft = faArrowLeft;

  title: string = 'MERCURE';

  constructor(private renderer: Renderer2, private appComponent: AppComponent) {
    this.appComponent.showNavbar = false;
    this.appComponent.showFooter = false;

    this.renderer.addClass(document.body, 'landing-background');
  }

  ngOnDestroy(): void {
    this.renderer.removeClass(document.body, 'landing-background');
  }
}
