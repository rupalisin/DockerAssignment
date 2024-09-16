import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { ProductService } from 'src/app/services/product.service';

@Component({
  selector: 'app-bought-products',
  templateUrl: './bought-products.component.html',
  styleUrls: ['./bought-products.component.css']
})
export class BoughtProductsComponent implements OnInit {
  displayedColumns: string[] = ['id', 'name', 'description', 'category', 'boughtPrice'];
  dataSource: MatTableDataSource<any> = new MatTableDataSource<any>();

  constructor(private productService: ProductService) { }

  ngOnInit(): void {
    this.getBoughtProducts();
  }

  getBoughtProducts(): void {
    this.productService.getProductsBoughtByUser().subscribe(
      (products) => {
        this.dataSource.data = products;
      },
      (error) => {
        console.error('Error fetching products:', error);
      }
    );
  }
}
