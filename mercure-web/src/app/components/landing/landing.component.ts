import { AfterViewInit, Component, OnDestroy, OnInit, Renderer2 } from '@angular/core';
import { faGoogle, faMicrosoft } from '@fortawesome/free-brands-svg-icons';
import { AppComponent } from 'src/app/app.component';


@Component({
  selector: 'app-landing',
  templateUrl: './landing.component.html',
  styleUrls: ['./landing.component.scss']
})
export class LandingComponent implements OnInit, AfterViewInit, OnDestroy{
  faGoogle = faGoogle;
  faMicrosoft = faMicrosoft;


  title: string = 'MERCURE';
  subTitle: string = "L'excellence pour vos compagnons fidèles";
  
  constructor(private renderer: Renderer2, private appComponent: AppComponent) { 
    
  }

  ngOnInit(): void {
    this.appComponent.showNavbar = false;
    this.appComponent.showFooter = false;
  }

  ngAfterViewInit(): void{
    this.renderer.addClass(document.body, 'landing-background');
    this.renderer.addClass(document.getElementById('app-container'), 'centered');
  }

  ngOnDestroy(): void {
    this.renderer.removeClass(document.body, 'landing-background');
    this.renderer.removeClass(document.getElementById('app-container'), 'centered');
  }
}