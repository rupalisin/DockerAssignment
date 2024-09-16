import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.css']
})
export class NavBarComponent implements OnInit {

  isLoggedIn: boolean = false;
  userName: string | undefined;
  isAdmin: boolean = false;


  constructor(private authService: AuthService,
    private userService : UserService) { }

  ngOnInit(): void {
    this.authService.getCurrentUser().subscribe(user => {
      if (user) {
        this.isLoggedIn = true;
        this.fetchUserDetails(user.UserId);
      } else {
        this.isLoggedIn = false;
      }
    });
  }

  fetchUserDetails(userId: string): void {
    this.userService.getUserById(userId).subscribe(
      (userDetails: any) => {
        this.userName = userDetails.name;
        this.isAdmin = userDetails.role === 'Admin';
        console.log("user name -> ",this.userName)
        console.log("role -> ", userDetails.role)
      },
      error => {
        console.error('Failed to fetch user details', error);
      }
    );
  }

  refreshUserDetails(): void {
    this.authService.getCurrentUser().subscribe(user => {
      if (user) {
        this.fetchUserDetails(user.UserId);
      }
    });
  }
  logout(): void {
    this.authService.logout();
  }

}
