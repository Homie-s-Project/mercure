import { Component, OnInit, Renderer2 } from '@angular/core';
import { faGoogle, faMicrosoft } from '@fortawesome/free-brands-svg-icons';
import { AppComponent } from 'src/app/app.component';


@Component({
  selector: 'app-landing',
  templateUrl: './landing.component.html',
  styleUrls: ['./landing.component.scss']
})
export class LandingComponent implements OnInit{
  faGoogle = faGoogle;
  faMicrosoft = faMicrosoft;


  title: string = 'MERCURE';
  subTitle: string = "L'excellence pour vos compagnons fidèles";
  
  constructor(private renderer: Renderer2, private appComponent: AppComponent) { 
    this.renderer.addClass(document.body, 'landing-background');
    this.renderer.addClass(document.getElementById('app-container'), 'centered');
  }

  ngOnInit(): void {
    this.appComponent.showNavbar = false;
    this.appComponent.showFooter = false;
    let textDiv = document.getElementById("title-container");
    let buttonDiv = document.getElementById("auth-container");

    if(textDiv) {
      textDiv.style.animationPlayState = "running";
      textDiv.classList.remove("hide");
    }
  
    if(buttonDiv) {
      buttonDiv.style.animationPlayState = "running";
      buttonDiv.classList.remove("hide");
    }
  }
}
