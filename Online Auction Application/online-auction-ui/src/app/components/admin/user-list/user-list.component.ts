import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AdminService } from 'src/app/services/admin.service';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent implements OnInit {
  users: any[] = [];
  displayedColumns: string[] = ['name', 'email', 'actions'];

  constructor(private adminService: AdminService, private snackBar: MatSnackBar, public dialog: MatDialog) {}

  ngOnInit(): void {
    this.adminService.getAllUsers().subscribe(users => {
      this.users = users;
      console.log("users->", users)
    });
  }

  suspendUser(userId: string): void {
    this.adminService.suspendUser(userId).subscribe(() => {
      this.snackBar.open('User suspended successfully', 'Close', { duration: 3000 });
    });
  }
}
