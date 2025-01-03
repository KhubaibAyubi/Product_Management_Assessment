import { Component, OnInit } from '@angular/core';
import { ProductsService } from './products.service';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';  
import { Products } from './products';
import { RouterModule } from '@angular/router'; 
import { DataTablesModule } from 'angular-datatables'
import { FormsModule } from '@angular/forms';
@Component({
  selector: 'app-products',
  standalone: true,
  imports: [CommonModule, HttpClientModule,RouterModule,DataTablesModule,FormsModule],  
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.css']  // Fixed typo
})
export class ProductsComponent implements OnInit {
  products: Products[] = [];
  filteredProducts: Products[] = [];
  paginatedProducts: Products[] = [];
  searchTerm: string = '';
  currentPage: number = 1;
  itemsPerPage: number = 5;
  totalPages: number = 1;
  showSuccessNotification: boolean = false;
  successMessage: string = '';
  constructor(private productService: ProductsService) {}

  ngOnInit(): void {
    this.getProductList();
  }
  getProductList(): void {
    this.productService.getProducts().subscribe(
      (response) => {
        this.products = response;
        this.filteredProducts = [...this.products];
        this.updatePagination();
      },
      (error) => {
        console.error('Error fetching products:', error);
      }
    );
  }

  searchProducts(): void {
    this.filteredProducts = this.products.filter((product) => {
      const searchLower = this.searchTerm.toLowerCase();

      // Check if the search term is included in the name, description, price, or created date
      return (
        product.name.toLowerCase().includes(searchLower) ||
        product.description.toLowerCase().includes(searchLower) ||
        product.price.toString().includes(searchLower) ||  // Ensure price is converted to string
        product.createdDate.toString().includes(searchLower) // Ensure createdDate is converted to string
      );
    });
    
    this.currentPage = 1; // Reset to the first page
    this.updatePagination();
  }

  updatePagination(): void {
    this.totalPages = Math.ceil(this.filteredProducts.length / this.itemsPerPage);
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    this.paginatedProducts = this.filteredProducts.slice(startIndex, endIndex);
  }

  previousPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.updatePagination();
    }
  }

  nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.updatePagination();
    }
  }

  deleteProduct(id: number): void {
    this.productService.deleteProduct(id).subscribe({
      next: () => {
        this.successMessage = 'Product deleted successfully.';
        this.showSuccessNotification = true;

        // Remove the deleted product
        this.products = this.products.filter((product) => product.id !== id);
        this.searchProducts(); // Reapply search and update pagination

        // Hide the success message after 3 seconds
        setTimeout(() => {
          this.showSuccessNotification = false;
        }, 3000);
      },
      error: (err: any) => {
        console.error('Error deleting product:', err);
      },
    });
  }
}