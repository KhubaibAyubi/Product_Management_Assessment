import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Products } from './products';

@Injectable({
  providedIn: 'root',  // Ensures service is globally available
})
export class ProductsService {
  private apiUrl = 'https://localhost:7218/products';  // Use your API URL
  private CreateapiUrl = 'https://localhost:7218/postproducts';
  private EditapiUrl = 'https://localhost:7218';
  constructor(private http: HttpClient) {}

  getProducts(): Observable<Products[]> {
    return this.http.get<Products[]>(this.apiUrl);
  }
  createProduct(product: any): Observable<any> {
    return this.http.post(this.CreateapiUrl, product);
  } 
  getProductById(id: number): Observable<any> {
    console.log("ServiceId",id);
    return this.http.get<any>(`${this.EditapiUrl}/getproductbyId/${id}`);
  }
  updateProduct(productData: any): Observable<any> {
    console.log("productData",productData);
    return this.http.put<any>(`${this.EditapiUrl}/UpdateProducts`, productData);
  }
  // Delete product by ID
  deleteProduct(id: number): Observable<any> {
    return this.http.delete(`${this.EditapiUrl}/DeleteProduct/${id}`);
  }
}
