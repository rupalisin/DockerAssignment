import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Subscription } from 'rxjs';
import { AdminService } from 'src/app/services/admin.service';
@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css']
})
export class ProductListComponent implements OnInit {
  products: any[] = [];
  displayedColumns: string[] = ['id', 'name', 'description', 'startingPrice', 'auctionDuration', 'category', 'reservedPrice', 'highestBid', 'sold', 'actions'];

  now: number;
  updateInterval: Subscription | undefined;

  constructor(private adminService: AdminService, private snackBar: MatSnackBar) {
    this.now = new Date().getTime();
  }

  ngOnInit(): void {
    this.adminService.getAllProducts().subscribe(products => {
      this.products = products;
    });
  }

  ngOnDestroy(): void {
    if (this.updateInterval) {
      this.updateInterval.unsubscribe();
    }
  }

  calculateTimeRemaining(startTime: string, endTime: string): string {
    const start = new Date(startTime).getTime();
    const end = new Date(endTime).getTime();
    const now = this.now; 
    
  
    const timeRemaining = end - start;
  
    if (timeRemaining < 0) {
      return 'Closed';
    }
  
    const hours = Math.floor(timeRemaining / (1000 * 60 * 60));
    const minutes = Math.floor((timeRemaining % (1000 * 60 * 60)) / (1000 * 60));
    const seconds = Math.floor((timeRemaining % (1000 * 60)) / 1000);
  
    return `${hours}h ${minutes}m ${seconds}s`;
  }

  deleteProduct(productId: number): void {
    this.adminService.deleteProduct(productId).subscribe(() => {
      this.products = this.products.filter(p => p.id !== productId);
      this.snackBar.open('Product deleted successfully', 'Close', { duration: 3000 });
    });
  }
}
