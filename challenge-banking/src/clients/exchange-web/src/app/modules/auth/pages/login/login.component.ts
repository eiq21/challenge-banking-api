import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthenticationService } from '@app/core/auth/authentication.service';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit {
  credentials!: FormGroup;
  isLoading = false;
  constructor(
    private router: Router,
    private fb: FormBuilder,
    private authService: AuthenticationService
  ) {}

  ngOnInit(): void {
    this.credentials = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
    });
  }

  async login() {
    this.isLoading = true;
    console.log('credentials::::', this.credentials);
    const login$ = this.authService.login(this.credentials.value);
    login$
      .pipe(
        finalize(() => {
          this.credentials.markAsPristine();
          this.isLoading = false;
        })
      )
      .subscribe(
        (result) => {
          console.log(`${result.username} successfully logged in`);
          this.router.navigateByUrl('/home', { replaceUrl: true });
        },
        (error) => {
          console.log('error:', error);
          this.isLoading = false;
        }
      );
  }

  get email() {
    return this.credentials.get('email');
  }

  get password() {
    return this.credentials.get('password');
  }
}
