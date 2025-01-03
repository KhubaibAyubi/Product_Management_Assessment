import { Component } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ProductsService } from '../products/products.service';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
@Component({
  selector: 'app-create-products',
  imports: [ReactiveFormsModule,CommonModule],
  templateUrl: './create-products.component.html',
  styleUrl: './create-products.component.css'
})
export class CreateProductsComponent {
  productForm: FormGroup;
  showSuccessNotification = false; 
  constructor(private fb: FormBuilder, private productService: ProductsService,private router: Router) {
    this.productForm = this.fb.group({
      name: ['', Validators.required],
      description: ['', Validators.required],
      price: ['', [Validators.required, Validators.min(0)]],
    });
  }
  onSubmit() {
    if (this.productForm.valid) {
      this.productService.createProduct(this.productForm.value).subscribe({
        next: (response: any) => {
          console.log('Product created successfully:', response);
          this.showSuccessNotification = true; // Show success notification
          setTimeout(() => {
            this.showSuccessNotification = false; // Hide after 3 seconds
            this.productForm.reset();
            this.router.navigate(['/']);
          }, 3000);
     
        },
        error: (err: any) => {
          console.error('Error creating product:', err);
        },
      });
    } else {
      this.productForm.markAllAsTouched(); // Manually trigger validation
    }
  }
}