import { Routes } from '@angular/router';
import {CreateProductsComponent } from './create-products/create-products.component';
import {ProductsComponent} from './products/products.component';
import {EditProductComponent} from './edit-product/edit-product.component'
export const routes: Routes = [

{
    path:"create",
    component: CreateProductsComponent
},
{
    path: 'edit/:id',
    component: EditProductComponent, // Edit page for products
  },
{ path: '', component: ProductsComponent },

];
