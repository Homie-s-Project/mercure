import { Component, OnInit } from '@angular/core';
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
  
  constructor(private appComponent: AppComponent) { }

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
