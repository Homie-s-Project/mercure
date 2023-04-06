import {Component, OnInit} from '@angular/core';
import {AppComponent} from '../../app.component';
import {ActivatedRoute, ParamMap} from '@angular/router';
import {Subscription} from 'rxjs';

@Component({
  selector: 'app-loading',
  templateUrl: './loading.component.html',
  styleUrls: ['./loading.component.scss']
})
export class LoadingComponent implements OnInit {
  state?: number;
  subscriptions: Subscription[] = []

  constructor(private appComponent: AppComponent, private route: ActivatedRoute){}

  ngOnInit(): void {
    this.appComponent.showNavbar = false;
    this.appComponent.showFooter = false;

    this.subscriptions.push(
      this.route.paramMap.subscribe((map: ParamMap) => {
        this.state = parseInt(map.get('state') || '');
        console.log(this.state);
      })
    );

  }
}
