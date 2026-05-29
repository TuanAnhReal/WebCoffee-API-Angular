import { Component, OnInit, inject, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { KhachHangService, KhachHangVm } from '../../services/khach-hang.service';

@Component({
  selector: 'app-khach-hang-list',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './khach-hang-list.component.html'
})
export class KhachHangListComponent implements OnInit {
  private khachHangService = inject(KhachHangService);
  private cdr = inject(ChangeDetectorRef);

  customers: KhachHangVm[] = [];
  filteredCustomers: KhachHangVm[] = [];
  paginatedCustomers: KhachHangVm[] = [];

  isLoading = true;
  totalCustomers = 0;
  vipCustomersCount = 0;
  newCustomersThisMonth = 0;
  searchTerm: string = '';
  currentPage = 1;
  itemsPerPage = 10;
  totalPages = 1;
  math = Math;

  ngOnInit(): void {
    this.loadCustomers();
  }

  loadCustomers(): void {
    this.isLoading = true;
    this.khachHangService.getAll().subscribe({
      next: (res) => {
        if (res.success) {
          this.customers = res.data.filter(k => k.maKH !== 'KH000' && k.tenKH !== 'Khách vãng lai');
          this.calculateStats();
          this.applyFilter();
        }
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error(err);
        this.isLoading = false;
      }
    });
  }

  calculateStats(): void {
    this.totalCustomers = this.customers.length;
    this.vipCustomersCount = this.customers.filter(k => k.diemTichLuy >= 500).length;

    const currentMonth = new Date().getMonth();
    const currentYear = new Date().getFullYear();
    this.newCustomersThisMonth = this.customers.filter(k => {
      if (!k.ngayTao) return false;
      const createdDate = new Date(k.ngayTao);
      return createdDate.getMonth() === currentMonth && createdDate.getFullYear() === currentYear;
    }).length;
  }

  applyFilter(): void {
    if (this.searchTerm) {
      const term = this.searchTerm.toLowerCase();
      this.filteredCustomers = this.customers.filter(k => 
        k.maKH.toLowerCase().includes(term) ||
        (k.tenKH && k.tenKH.toLowerCase().includes(term)) ||
        (k.sdtkh && k.sdtkh.includes(term))
      );
    } else {
      this.filteredCustomers = this.customers;
    }
    this.currentPage = 1;
    this.updatePagination();
  }

  updatePagination(): void {
    this.totalPages = Math.ceil(this.filteredCustomers.length / this.itemsPerPage) || 1;
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    this.paginatedCustomers = this.filteredCustomers.slice(startIndex, startIndex + this.itemsPerPage);
  }

  changePage(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.updatePagination();
    }
  }

  getPagesArray(): number[] {
    return Array.from({ length: this.totalPages }, (_, i) => i + 1);
  }

  getAvatarInitials(name: string): string {
    if (!name) return 'KH';
    const words = name.trim().split(' ');
    if (words.length >= 2) return (words[0][0] + words[words.length - 1][0]).toUpperCase();
    return name.substring(0, 2).toUpperCase();
  }

  deleteCustomer(customer: KhachHangVm): void {
    if (confirm(`Bạn có chắc chắn muốn xóa khách hàng ${customer.tenKH} không? Mọi điểm tích lũy sẽ bị mất.`)) {
      this.khachHangService.delete(customer.maKH).subscribe({
        next: (res) => {
          if (res.success) {
            this.loadCustomers();
          } else {
            alert(res.message);
          }
        }
      });
    }
  }
}