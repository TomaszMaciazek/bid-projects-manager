import { Component } from '@angular/core';
import { MatIconRegistry } from "@angular/material/icon";
import { DomSanitizer } from '@angular/platform-browser';
import { ConfirmationService } from 'primeng/api';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  providers: [ConfirmationService]
})
export class AppComponent {
  title = 'bid-projects-manager-frontend';

  constructor(private matIconRegistry: MatIconRegistry, private domSanitizer: DomSanitizer) {
    this.matIconRegistry.addSvgIcon(
      `xlsx-file`,
      this.domSanitizer.bypassSecurityTrustResourceUrl("../assets/svg/xlsx-file.svg")
    );
  }
}
