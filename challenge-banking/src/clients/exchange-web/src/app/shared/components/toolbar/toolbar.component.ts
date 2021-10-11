import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '@app/core/auth/authentication.service';

@Component({
  selector: 'app-toolbar',
  templateUrl: './toolbar.component.html',
  styleUrls: ['./toolbar.component.scss'],
})
export class ToolbarComponent implements OnInit {
  constructor(private authService: AuthenticationService) {}

  ngOnInit(): void {}

  logout(): void {
    this.authService.logout();
  }
}
