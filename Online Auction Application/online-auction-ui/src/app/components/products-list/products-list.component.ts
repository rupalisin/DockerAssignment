import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTableDataSource } from '@angular/material/table';
import { BehaviorSubject, Subscription, interval, switchMap } from 'rxjs';
import { BidService } from 'src/app/services/bid.service';
import { ProductService } from 'src/app/services/product.service';
import { AddProductDialogComponent } from '../add-product-dialog/add-product-dialog.component';

@Component({
  selector: 'app-products-list',
  templateUrl: './products-list.component.html',
  styleUrls: ['./products-list.component.css']
})
export class ProductsListComponent implements OnInit {

  dataSource: MatTableDataSource<any>;
  filteredDataSource: MatTableDataSource<any>;

  now: number;
  updateInterval: Subscription | undefined;




  constructor(private productService : ProductService, 
    private bidService: BidService,
    private snackBar: MatSnackBar,
    public dialog: MatDialog) {
    this.dataSource = new MatTableDataSource();
    this.filteredDataSource = new MatTableDataSource();

    this.now = new Date().getTime();

   }

   ngOnInit(): void {
    this.loadProducts();

    this.updateInterval = interval(1000).subscribe(() => {
      this.now = new Date().getTime();
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
  
  applyFilter(event: Event): void {
    const filterValue = (event.target as HTMLInputElement).value.trim().toLowerCase();
    this.filteredDataSource.data = this.dataSource.data.filter((product: any) => 
      product.name.toLowerCase().includes(filterValue) || 
      product.category.toLowerCase().includes(filterValue)
    );
  }



  sortData(event: any): void {
    const sortValue = event.value;
    this.filteredDataSource.data = [...this.filteredDataSource.data].sort((a: any, b: any) => {
      let comparison = 0;

      if (sortValue === 'priceAsc') {
        comparison = a.startingPrice - b.startingPrice;
      } else if (sortValue === 'priceDesc') {
        comparison = b.startingPrice - a.startingPrice;
      } else if (sortValue === 'timeAsc') {
        const aTimeRemaining = this.calculateTimeRemaining(a.auctionStartTime, a.auctionEndTime);
        const bTimeRemaining = this.calculateTimeRemaining(b.auctionStartTime, b.auctionEndTime);
        comparison = aTimeRemaining.localeCompare(bTimeRemaining);
      } else if (sortValue === 'timeDesc') {
        const aTimeRemaining = this.calculateTimeRemaining(a.auctionStartTime, a.auctionEndTime);
        const bTimeRemaining = this.calculateTimeRemaining(b.auctionStartTime, b.auctionEndTime);
        comparison = bTimeRemaining.localeCompare(aTimeRemaining);
      }

      return comparison;
    });
  }

  placeBid(element: any): void {
    const bidAmount = element.bidAmount;
    if (bidAmount > 0) {
      this.bidService.placeBid(element.id, bidAmount).subscribe(
        response => {
          console.log('Bid placed successfully', response);
          this.snackBar.open('Bid placed successfully', 'Close', {
            duration: 3000,
          });
          // Fetch the updated product data
          this.loadProducts();
        },
        error => {
          console.error('Error placing bid', error);
          this.snackBar.open('Error placing bid: ' + error.error, 'Close', {
            duration: 3000,
          });
        }
      );
    }
  }


  openAddProductDialog(): void {
    const dialogRef = this.dialog.open(AddProductDialogComponent, {
      width: '400px',
      data: {}
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        // Add the new product to the existing data sources
        const updatedProducts = [...this.dataSource.data, result];
        this.updateDataSources(updatedProducts);
        
        this.snackBar.open('Product added successfully', 'Close', { duration: 3000 });
      }
    });
  }

  loadProducts(): void {
    this.productService.getProducts().subscribe(
      (products: any) => {
        this.updateDataSources(products);
      },
      (error: any) => {
        console.error('Error fetching products', error);
        this.snackBar.open('Error fetching products', 'Close', { duration: 3000 });
      }
    );
  }

  updateDataSources(products: any[]): void {
    this.dataSource.data = products;
    this.filteredDataSource.data = products;
    this.dataSource._updateChangeSubscription();
    this.filteredDataSource._updateChangeSubscription();
  }
}