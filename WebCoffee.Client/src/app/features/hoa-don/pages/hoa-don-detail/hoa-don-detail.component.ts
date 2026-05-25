import { Component, OnInit, inject, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, ActivatedRoute, Router } from '@angular/router';
import { HoaDonService, HoaDonVm } from '../../services/hoa-don.service';

@Component({
  selector: 'app-hoa-don-detail',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './hoa-don-detail.component.html'
})
export class HoaDonDetailComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private hoaDonService = inject(HoaDonService);
  private cdr = inject(ChangeDetectorRef);

  soHd: string = '';
  invoice!: HoaDonVm;
  isLoading = true;

  ngOnInit(): void {
    this.soHd = this.route.snapshot.paramMap.get('id') || '';
    this.loadInvoiceDetail();
  }

  loadInvoiceDetail(): void {
    if (!this.soHd) return;

    this.isLoading = true;
    this.hoaDonService.getById(this.soHd).subscribe({
      next: (res) => {
        if (res.success && res.data) {
          this.invoice = res.data;
        }
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error(err);
        alert('Không tìm thấy hóa đơn!');
        this.router.navigate(['/admin/hoa-don']);
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });
  }

  get totalItemsCount(): number {
    if (!this.invoice || !this.invoice.chiTietHoaDons) return 0;
    return this.invoice.chiTietHoaDons.reduce((sum, item) => sum + item.slsp, 0);
  }

  get subTotal(): number {
    if (!this.invoice || !this.invoice.chiTietHoaDons) return 0;
    return this.invoice.chiTietHoaDons.reduce((sum, item) => sum + (item.donGia * item.slsp), 0);
  }

  printInvoice(): void {
    window.print();
  }
}