import { Component } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductsService } from '../products/products.service';
import { CommonModule } from '@angular/common';
@Component({
  selector: 'app-edit-product',
  imports: [ReactiveFormsModule,CommonModule],
  templateUrl: './edit-product.component.html',
  styleUrl: './edit-product.component.css'
})
export class EditProductComponent {
  productForm: FormGroup;
  showSuccessNotification: boolean = false;
  productId: number | null = null;

  constructor(
    private fb: FormBuilder,
    private productService: ProductsService,
    private router: Router,
    private activatedRoute: ActivatedRoute
  ) {
    this.productForm = this.fb.group({
      name: ['', Validators.required],
      description: ['', Validators.required],
      price: ['', [Validators.required, Validators.min(0)]],
    });
  }
  ngOnInit(): void {
    // Fetch product by ID from route
    this.activatedRoute.paramMap.subscribe(params => {
      const id = params.get('id');
      console.log("ID", id);
      if (id) {
        this.productId = +id; // Convert the ID to a number
        this.getProductById(this.productId); // Fetch the product data
      }
    });
  }
  // Fetch product data for editing
  getProductById(id: number): void {
    this.productService.getProductById(id).subscribe({
      next: (product: any) => {
        // Ensure product data is available
        if (product) {
          console.log("ProductsforEdit",product);
          // Patch the form with the fetched data
          this.productForm.patchValue({
            name: product.name,
            description: product.description,
            price: product.price
          });
        }
      },
      error: (err: any) => {
        console.error('Error fetching product data:', err);
      }
    });
  }

  // Submit the form to update the product
  onSubmit(): void {
    if (this.productForm.valid) {
      // Make sure the productId exists
      if (this.productId) {
        // Create the product data object to send
        const productData = {
          id: this.productId, // Include the product ID
          name: this.productForm.value.name,
          description: this.productForm.value.description,
          price: this.productForm.value.price
        };
  
        // Call the service to update the product
        this.productService.updateProduct(productData).subscribe({
          next: (response: any) => {
            console.log('Product updated successfully:', response);
            this.showSuccessNotification = true;
            setTimeout(() => {
              this.showSuccessNotification = false;
              this.router.navigate(['/']); // Redirect after successful update
            }, 3000);
          },
          error: (err: any) => {
            console.error('Error updating product:', err);
          }
        });
      }
    } else {
      this.productForm.markAllAsTouched();
    }
  }
  
}
