import {Component, OnDestroy, OnInit} from '@angular/core';
import {AppComponent} from '../../app.component';
import {ActivatedRoute, Router} from '@angular/router';
import {interval, Subscription} from 'rxjs';
import {AuthService} from "../../services/auth/auth.service";

@Component({
  selector: 'app-loading',
  templateUrl: './loading.component.html',
  styleUrls: ['./loading.component.scss']
})
export class LoadingComponent implements OnInit, OnDestroy {

  subscriptions: Subscription[] = []
  errorMessage?: string;
  hasError: boolean = false;
  subscriptionErrorTimer!: Subscription;

  // in seconds
  private TIME_BEFORE_REDIRECT: number = 10;
  secondBeforeRedirect: number = this.TIME_BEFORE_REDIRECT;

  constructor(private appComponent: AppComponent, private router: Router, private route: ActivatedRoute, private authService: AuthService) {
    this.appComponent.showNavbar = false;
    this.appComponent.showFooter = false;
  }

  ngOnInit(): void {

    if (this.authService.isLogged()) {
      this.router.navigate(["/"]);
    }

    this.subscriptions.push(
      this.route.queryParams
        .subscribe(params => {
          let hasParamState = params["state"] !== null;
          let state = params["state"];

          if (hasParamState) {
            this.authService.getUserToken(state)
              .then(r => {
                sessionStorage.setItem("token", r.token);
              })
              .catch((error) => {
                this.errorMessage = error.error.message;
                this.hasError = true;
              })
              .finally(() => {
                if (this.hasError) {

                  this.secondBeforeRedirect = 10;

                  this.subscriptionErrorTimer = interval(1000)
                    .subscribe(x => {
                      this.secondBeforeRedirect -= 1;
                    })

                  setTimeout(() => {
                    this.router.navigate(["/login"]);
                  }, this.TIME_BEFORE_REDIRECT * 1_000);
                } else {
                  this.router.navigate(["/"]);
                }
              });
            return;
          }
        })
    );

  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(s => s.unsubscribe());

    if (this.hasError) {
      this.subscriptionErrorTimer.unsubscribe();
    }
  }
}
