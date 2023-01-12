import { APP_INITIALIZER, NgModule } from '@angular/core';

import { KeycloakAngularModule, KeycloakService } from 'keycloak-angular';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { SharedModule } from './modules/shared/shared.module';
import { BasicLayoutComponent } from './common/basic-layout/basic-layout.component';
import { NgxSpinnerModule } from 'ngx-spinner';
import { HttpClientModule } from '@angular/common/http';
import { ToastrModule } from 'ngx-toastr';
import { BrowserModule } from '@angular/platform-browser';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

// function initializeKeycloak(keycloak: KeycloakService) {
//   return () =>
//     keycloak.init({
//       config: {
//         url: 'http://localhost:8080',
//         clientId: 'app-angular'
//       },
//       initOptions: {
//         onLoad: 'check-sso',
//         silentCheckSsoRedirectUri:
//           window.location.origin + '/assets/silent-check-sso.html'
//       }
//     });
// }

@NgModule({
  declarations: [AppComponent, BasicLayoutComponent],
  imports: [
    AppRoutingModule,
    BrowserModule,
    KeycloakAngularModule,
    BrowserAnimationsModule,
    SharedModule,
    NgxSpinnerModule,
    ToastrModule.forRoot(),
    HttpClientModule,
    NgbModule
  ],
  providers: [
    // {
    //   provide: APP_INITIALIZER,
    //   useFactory: initializeKeycloak,
    //   multi: true,
    //   deps: [KeycloakService]
    // }
  ],
  bootstrap: [AppComponent]
})
export class AppModule {}