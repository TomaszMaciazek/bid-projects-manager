import { Component } from '@angular/core';
import { Router } from '@angular/router';


@Component({
  selector: 'app-basic-layout',
  templateUrl: './basic-layout.component.html',
  styleUrls: ['./basic-layout.component.scss']
})
export class BasicLayoutComponent {

    constructor(private router: Router){}

    addProject(){
      this.router.navigate(['/projects/edit', { id: 0 }]);
    }
}
