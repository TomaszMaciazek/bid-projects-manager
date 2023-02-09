import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { KeycloakService } from 'keycloak-angular';


@Component({
  selector: 'app-basic-layout',
  templateUrl: './basic-layout.component.html',
  styleUrls: ['./basic-layout.component.scss']
})
export class BasicLayoutComponent {

    constructor(private router: Router, private keycloakService: KeycloakService){}

    addProject(){
      this.router.navigate(['/projects/edit', { id: 0 }]);
    }

    logout(){
      this.keycloakService.logout().then(() => this.keycloakService.clearToken());
    }

    public get isAdmin(){
      return this.keycloakService.isUserInRole("Administrator");
    }

    public get isEditor(){
      return this.keycloakService.isUserInRole("Editor");
    }
}
