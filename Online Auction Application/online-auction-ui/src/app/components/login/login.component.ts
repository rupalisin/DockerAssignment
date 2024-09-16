import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  

  constructor(private authService : AuthService, private router: Router,
    private snackBar: MatSnackBar) { }

  ngOnInit() {
  }
  
  onLogin(loginForm: NgForm) {
    // console.log(loginForm.value);
    this.authService.loginUser(loginForm.value).subscribe(
      (response) => {
        // console.log(response);
        const token = response.token;
        
        localStorage.setItem('token', token)
        const user = this.authService.decodedToken();
        this.authService.setCurrentUser(user);
        this.snackBar.open('Login successful', 'Close', {
          duration: 3000,
        });
        // console.log("user:",user["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"])
        // console.log("login-role",user.role)
        if (user["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] === 'Admin') {
          this.router.navigate(['/admin']);
        } else {
          this.router.navigate(['/products']);
        }

      },
      (error) => {
        this.snackBar.open('Invalid Credentials', 'Close', {
          duration: 3000,
        });
      }
      
    );
  }
}
