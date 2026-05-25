import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { HoaDonService } from '../../services/hoa-don.service';
import { SanPhamService, SanPhamVm } from '../../../san-pham/services/san-pham.service';
import { NhanVienService, NhanVienVm } from '../../../nhan-vien/services/nhan-vien.service';
import { forkJoin } from 'rxjs';

// Interface cho Giỏ hàng
export interface CartItem {
  product: SanPhamVm;
  quantity: number;
  giamGia: number;
}

@Component({
  selector: 'app-hoa-don-create',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule],
  templateUrl: './hoa-don-create.component.html'
})
export class HoaDonCreateComponent implements OnInit {
  private fb = inject(FormBuilder);
  private hoaDonService = inject(HoaDonService);
  private sanPhamService = inject(SanPhamService);
  private nhanVienService = inject(NhanVienService);
  private router = inject(Router);

  invoiceForm!: FormGroup;
  
  // Dữ liệu master
  products: SanPhamVm[] = [];
  filteredProducts: SanPhamVm[] = [];
  employees: NhanVienVm[] = [];
  
  // Giỏ hàng
  cart: CartItem[] = [];
  
  // Trạng thái UI
  isLoading = true;
  isSubmitting = false;
  searchKeyword = '';
  activeCategory = 'Tất cả';

  ngOnInit(): void {
    this.initForm();
    this.loadMasterData();
  }

  initForm(): void {
    this.invoiceForm = this.fb.group({
      maKH: [''],
      soBan: ['Bàn 01'],
      maNV_PV: ['', Validators.required],
      maNV_PC: ['', Validators.required],
      giamGiaHD: [0, Validators.min(0)],
      ghiChuHD: ['']
    });
  }

  loadMasterData(): void {
    forkJoin({
      products: this.sanPhamService.getAll(),
      employees: this.nhanVienService.getAll()
    }).subscribe({
      next: ({ products, employees }) => {
        if (products.success) {
          // Lọc chỉ lấy sản phẩm đang bán
          this.products = products.data.filter(p => p.trangThai === 'Đang bán');
          this.filteredProducts = this.products;
        }
        if (employees.success) {
          // Lọc chỉ lấy nhân viên đang làm việc
          this.employees = employees.data.filter(e => e.trangThaiNV === 'Đang làm');
        }
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Lỗi tải dữ liệu:', err);
        this.isLoading = false;
      }
    });
  }

  // --- LOGIC BỘ LỌC SẢN PHẨM ---
  filterByCategory(categoryName: string): void {
    this.activeCategory = categoryName;
    if (categoryName === 'Tất cả') {
      this.filteredProducts = this.products;
    } else {
      this.filteredProducts = this.products.filter(p => p.tenLoaiSp.includes(categoryName));
    }
  }

  onSearchProduct(event: Event): void {
    const keyword = (event.target as HTMLInputElement).value.toLowerCase();
    this.filteredProducts = this.products.filter(p => 
      p.tenSp.toLowerCase().includes(keyword) && 
      (this.activeCategory === 'Tất cả' || p.tenLoaiSp.includes(this.activeCategory))
    );
  }

  // --- LOGIC GIỎ HÀNG ---
  addToCart(product: SanPhamVm): void {
    const existingItem = this.cart.find(item => item.product.maSp === product.maSp);
    if (existingItem) {
      existingItem.quantity++;
    } else {
      this.cart.push({ product, quantity: 1, giamGia: 0 });
    }
  }

  increaseQty(item: CartItem): void {
    item.quantity++;
  }

  decreaseQty(item: CartItem): void {
    if (item.quantity > 1) {
      item.quantity--;
    } else {
      this.removeFromCart(item);
    }
  }

  removeFromCart(item: CartItem): void {
    const index = this.cart.indexOf(item);
    if (index > -1) {
      this.cart.splice(index, 1);
    }
  }

  // --- LOGIC TÍNH TOÁN TIỀN ---
  get subTotal(): number {
    return this.cart.reduce((sum, item) => sum + (item.product.giaSp * item.quantity), 0);
  }

  get vatAmount(): number {
    return this.subTotal * 0.1; // VAT 10%
  }

  get totalAmount(): number {
    const giamGia = this.invoiceForm.get('giamGiaHD')?.value || 0;
    return this.subTotal + this.vatAmount - Number(giamGia);
  }

  // --- GỬI API ---
  onSubmit(): void {
    if (this.cart.length === 0) {
      alert('Vui lòng chọn ít nhất 1 sản phẩm để tạo hóa đơn!');
      return;
    }

    if (this.invoiceForm.invalid) {
      this.invoiceForm.markAllAsTouched();
      alert('Vui lòng chọn nhân viên phục vụ và pha chế!');
      return;
    }

    this.isSubmitting = true;
    const formValues = this.invoiceForm.value;

    // Build payload theo đúng chuẩn API Swagger
    const payload = {
      maKH: formValues.maKH || 'Khách vãng lai',
      soBan: formValues.soBan,
      maNV_PV: formValues.maNV_PV,
      maNV_PC: formValues.maNV_PC,
      giamGiaHD: Number(formValues.giamGiaHD) || 0,
      phuThu: 0,
      thueVAT: this.vatAmount,
      ghiChuHD: formValues.ghiChuHD,
      chiTietHoaDons: this.cart.map(item => ({
        maSP: item.product.maSp,
        slsp: item.quantity,
        donGia: item.product.giaSp,
        giamGia: item.giamGia
      }))
    };

    this.hoaDonService.create(payload).subscribe({
      next: (res) => {
        if (res.success) {
          alert('Tạo hóa đơn thành công!');
          this.router.navigate(['/admin/hoa-don']);
        }
        this.isSubmitting = false;
      },
      error: (err) => {
        console.error(err);
        alert('Lỗi tạo hóa đơn. Vui lòng thử lại!');
        this.isSubmitting = false;
      }
    });
  }
}