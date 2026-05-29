import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { forkJoin } from 'rxjs';

import { HoaDonService } from '../../services/hoa-don.service';
import { SanPhamService, SanPhamVm } from '../../../san-pham/services/san-pham.service';
import { KhachHangService, KhachHangVm } from '../../../khach-hang/services/khach-hang.service';
import { KhuVucBanService, KhuVucVm, BanVm } from '../../../khuvuc-ban/services/khu-vuc-ban.service';
// import { AuthService } from '../../../auth/services/auth.service'; // Dùng service Auth thực tế của bạn

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
  private khachHangService = inject(KhachHangService);
  private khuVucBanService = inject(KhuVucBanService);
  private router = inject(Router);

  invoiceForm!: FormGroup;
  
  // Dữ liệu master
  products: SanPhamVm[] = [];
  filteredProducts: SanPhamVm[] = [];
  customers: KhachHangVm[] = [];
  areas: KhuVucVm[] = [];
  filteredTables: BanVm[] = [];
  
  // Giỏ hàng & Trạng thái
  cart: CartItem[] = [];
  isLoading = true;
  isSubmitting = false;
  activeCategory = 'Tất cả';

  customerNameDisplay = 'Khách vãng lai';
  currentAreaSurcharge = 0;
  currentUser: any = null; // Chứa thông tin NV đang login

  ngOnInit(): void {
    this.initForm();
    this.loadMasterData();
    this.loadCurrentUser(); // Lấy thông tin người tạo bill
  }

  initForm(): void {
    this.invoiceForm = this.fb.group({
      sdtKH: [''],
      maKH: [''],
      soKV: ['', Validators.required],
      soBan: ['', Validators.required],
      giamGiaHD: [0, Validators.min(0)],
      ghiChuHD: ['']
    });
  }

  loadCurrentUser(): void {
    // GIẢ LẬP: Lấy từ API api/auth/me
    // this.authService.getMe().subscribe(res => this.currentUser = res.data);
    this.currentUser = {
      maNV: 'NV01',
      hoTen: 'Nguyễn Văn Admin',
      role: 'Nhân viên phục vụ'
    };
  }

  loadMasterData(): void {
    forkJoin({
      products: this.sanPhamService.getAll(),
      customers: this.khachHangService.getAll(),
      areas: this.khuVucBanService.getAllNested()
    }).subscribe({
      next: ({ products, customers, areas }) => {
        if (products.success) {
          this.products = products.data.filter(p => p.trangThai === 'Đang bán');
          this.filteredProducts = this.products;
        }
        if (customers.success) this.customers = customers.data;
        if (areas.success) this.areas = areas.data;
        
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Lỗi tải dữ liệu:', err);
        this.isLoading = false;
      }
    });
  }

  // --- LOGIC TÌM KHÁCH HÀNG QUA SĐT ---
  onPhoneChange(event: Event): void {
    const phone = (event.target as HTMLInputElement).value.trim();
    const found = this.customers.find(c => c.sdtkh === phone);
    
    if (found) {
      this.invoiceForm.patchValue({ maKH: found.maKH });
      this.customerNameDisplay = found.tenKH;
    } else {
      this.invoiceForm.patchValue({ maKH: '' });
      this.customerNameDisplay = 'Khách vãng lai';
    }
  }

  // --- LOGIC KHU VỰC -> BÀN ---
  onAreaChange(event: Event): void {
    const soKV = (event.target as HTMLSelectElement).value;
    const area = this.areas.find(a => a.soKV === soKV);
    
    if (area) {
      this.filteredTables = area.bans.filter(b => b.trangThaiBan === 'Trống'); // Chỉ hiển thị bàn trống
      this.currentAreaSurcharge = area.phuThuKV || 0; // Gán phụ thu
      this.invoiceForm.patchValue({ soBan: '' }); // Reset chọn bàn
    } else {
      this.filteredTables = [];
      this.currentAreaSurcharge = 0;
    }
  }

  // --- LOGIC BỘ LỌC SẢN PHẨM ---
  filterByCategory(categoryName: string): void {
    this.activeCategory = categoryName;
    if (categoryName === 'Tất cả') {
      this.filteredProducts = this.products;
    } else {
      this.filteredProducts = this.products.filter(p => p.tenLoaiSp && p.tenLoaiSp.includes(categoryName));
    }
  }

  onSearchProduct(event: Event): void {
    const keyword = (event.target as HTMLInputElement).value.toLowerCase();
    this.filteredProducts = this.products.filter(p => 
      p.tenSp.toLowerCase().includes(keyword) && 
      (this.activeCategory === 'Tất cả' || (p.tenLoaiSp && p.tenLoaiSp.includes(this.activeCategory)))
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

  increaseQty(item: CartItem): void { item.quantity++; }
  decreaseQty(item: CartItem): void {
    if (item.quantity > 1) item.quantity--;
    else this.removeFromCart(item);
  }
  removeFromCart(item: CartItem): void {
    const index = this.cart.indexOf(item);
    if (index > -1) this.cart.splice(index, 1);
  }

  // --- LOGIC TÍNH TOÁN TIỀN ---
  get subTotal(): number {
    return this.cart.reduce((sum, item) => sum + (item.product.giaSp * item.quantity), 0);
  }

  get vatAmount(): number { return this.subTotal * 0.1; }

  get totalAmount(): number {
    const giamGia = this.invoiceForm.get('giamGiaHD')?.value || 0;
    // Tổng = Tiền hàng + VAT + Phụ thu - Giảm giá
    return this.subTotal + this.vatAmount + this.currentAreaSurcharge - Number(giamGia);
  }

  // --- GỬI API ---
  onSubmit(): void {
    if (this.invoiceForm.invalid) {
      this.invoiceForm.markAllAsTouched();
      alert('Vui lòng chọn Khu vực và Bàn!'); 
      return;
    }

    if (this.cart.length === 0) {
      alert('Vui lòng chọn ít nhất 1 sản phẩm!'); return;
    }
    if (this.invoiceForm.invalid) {
      this.invoiceForm.markAllAsTouched();
      alert('Vui lòng chọn Khu vực và Bàn!'); return;
    }

    this.isSubmitting = true;
    const formValues = this.invoiceForm.getRawValue();

    const payload = {
      maKH: formValues.maKH || null, 
      soBan: formValues.soBan,
      maNV_PV: this.currentUser.maNV, // Gắn cứng mã NV Order
      maNV_PC: null,
      giamGiaHD: Number(formValues.giamGiaHD) || 0,
      phuThu: this.currentAreaSurcharge, // Kéo phụ thu vào
      thueVAT: this.vatAmount,
      trangThaiHD: "Chờ pha chế", // Trạng thái mặc định khi vừa order xong
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
          alert('Tạo hóa đơn thành công! Đã chuyển xuống quầy Pha chế.');
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