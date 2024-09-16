import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { ProductsListComponent } from './components/products-list/products-list.component';
import { AddedProductsComponent } from './components/added-products/added-products.component';
import { BoughtProductsComponent } from './components/bought-products/bought-products.component';
import { MyBidsComponent } from './components/my-bids/my-bids.component';
import { AdminDashboardComponent } from './components/admin/admin-dashboard/admin-dashboard.component';
import { UserListComponent } from './components/admin/user-list/user-list.component';
import { ProductListComponent } from './components/admin/product-list/product-list.component';

const routes: Routes = [
  
  {path: 'login' , component: LoginComponent},
  {path: 'products' , component: ProductsListComponent},
  { path: 'added-products', component: AddedProductsComponent },
  { path: 'bought-products', component: BoughtProductsComponent },
  { path: 'my-bids', component: MyBidsComponent },
  { path: 'admin', component: AdminDashboardComponent },
  { path: 'admin/products', component: ProductListComponent },
  { path: 'admin/users', component: UserListComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
