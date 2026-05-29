import { Component, OnInit, inject, signal, computed } from '@angular/common';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { forkJoin } from 'rxjs';
import { environment } from '../../../../../environments/environment.development';

@Component({
  selector: 'app-doanh-thu',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './doanh-thu.component.html'
})
export class DoanhThuComponent implements OnInit {
  private http = inject(HttpClient);
  private apiUrl = environment.apiUrl;

  dangTai = true;
  tuKhoaTimKiem = '';
  loaiBoLoc = signal<'ngay' | 'thang' | 'nam'>('ngay');

  // Khởi tạo các tín hiệu Signal lưu dữ liệu gốc an toàn từ API
  danhSachHoaDon = signal<any[]>([]);
  danhSachSanPham = signal<any[]>([]);

  ngOnInit(): void {
    this.taiDuLieuThongKe();
  }

  taiDuLieuThongKe(): void {
    this.dangTai = true;
    forkJoin({
      hoaDons: this.http.get<any>(`${this.apiUrl}/Authorization/HoaDons` || `${this.apiUrl}/HoaDons`),
      sanPhams: this.http.get<any>(`${this.apiUrl}/SanPhams`)
    }).subscribe({
      next: (ketQua) => {
        const hDons = ketQua.hoaDons?.success ? ketQua.hoaDons.data : (Array.isArray(ketQua.hoaDons) ? ketQua.hoaDons : []);
        const sPhams = ketQua.sanPhams?.success ? ketQua.sanPhams.data : (Array.isArray(ketQua.sanPhams) ? ketQua.sanPhams : []);

        this.danhSachHoaDon.set(hDons);
        this.danhSachSanPham.set(sPhams);
        this.dangTai = false;
      },
      error: (loi) => {
        console.error('Lỗi kết nối API Thống kê:', loi);
        this.dangTai = false;
      }
    });
  }

  // 1. Phễu lọc hóa đơn hợp lệ (Chỉ tính đơn đã hoàn tất dòng tiền)
  hoaDonHopLe = computed(() => {
    return this.danhSachHoaDon().filter(hd => 
      hd.trangThaiHD === 'Đã thanh toán' || hd.trangThaiHD === 'Hoàn thành'
    );
  });

  // 2. Sửa lỗi múi giờ: Bộ lọc thời gian chuẩn hóa theo định dạng địa phương YYYY-MM-DD
  hoaDonTheoBoLoc = computed(() => {
    const danhSach = this.hoaDonHopLe();
    const bayGio = new Date();
    
    // Đảm bảo lấy đúng ngày hệ thống local không bị lệch UTC khi sang ngày mới
    const homNayChuoi = bayGio.toLocaleDateString('en-CA'); // Kết quả luôn là "YYYY-MM-DD" đúng múi giờ VN
    const thangNayChuoi = homNayChuoi.substring(0, 7);
    const namNayChuoi = homNayChuoi.substring(0, 4);

    return danhSach.filter(hd => {
      if (!hd.tgVao) return false;
      const ngayHD = hd.tgVao.split('T')[0];

      if (this.loaiBoLoc() === 'ngay') return ngayHD === homNayChuoi;
      if (this.loaiBoLoc() === 'thang') return ngayHD.startsWith(thangNayChuoi);
      return ngayHD.startsWith(namNayChuoi);
    });
  });

  // 3. Khối tính toán chỉ số chỉ tiêu KPI động
  tongDoanhThu = computed(() => {
    return this.hoaDonTheoBoLoc().reduce((tong, hd) => tong + (hd.tongTien || 0), 0);
  });

  tongDonHang = computed(() => {
    return this.hoaDonTheoBoLoc().length;
  });

  giaTriDonTrungBinh = computed(() => {
    const doanhThu = this.tongDoanhThu();
    const donHang = this.tongDonHang();
    return donHang > 0 ? Math.round(doanhThu / donHang) : 0;
  });

  // 4. Thuật toán so sánh tăng trưởng thông minh (Ngày vs Hôm qua, Tháng vs Tháng trước, Năm vs Năm trước)
  tyLeTangTruongDoanhThu = computed(() => {
    const danhSach = this.hoaDonHopLe();
    const kyNay = this.tongDoanhThu();
    let kyTruoc = 0;

    const bayGio = new Date();

    if (this.loaiBoLoc() === 'ngay') {
      const homQua = new Date();
      homQua.setDate(homQua.getDate() - 1);
      const chuoiHomQua = homQua.toLocaleDateString('en-CA');
      kyTruoc = danhSach.filter(hd => hd.tgVao && hd.tgVao.split('T')[0] === chuoiHomQua).reduce((t, hd) => t + (hd.tongTien || 0), 0);
    } 
    else if (this.loaiBoLoc() === 'thang') {
      bayGio.setMonth(bayGio.getMonth() - 1);
      const chuoiThangTruoc = bayGio.toLocaleDateString('en-CA').substring(0, 7);
      kyTruoc = danhSach.filter(hd => hd.tgVao && hd.tgVao.split('T')[0].startsWith(chuoiThangTruoc)).reduce((t, hd) => t + (hd.tongTien || 0), 0);
    } 
    else {
      const chuoiNamTruoc = (bayGio.getFullYear() - 1).toString();
      kyTruoc = danhSach.filter(hd => hd.tgVao && hd.tgVao.split('T')[0].startsWith(chuoiNamTruoc)).reduce((t, hd) => t + (hd.tongTien || 0), 0);
    }

    if (kyTruoc === 0) return kyNay > 0 ? 100 : 0;
    return Math.round(((kyNay - kyTruoc) / kyTruoc) * 100);
  });

  // 5. Thống kê chi tiết món bán chạy KHỚP CHUẨN theo phễu bộ lọc thời gian
  thongKeMonBanChay = computed(() => {
    const banDoSanPham = new Map<string, { maSp: string; tenSp: string; tenLoaiSp: string; hinhAnh: string; soLuongBan: number; tongDoanhThuMon: number }>();

    // SỬA LỖI NGHIỆP VỤ: Đổi từ hoaDonHopLe sang hoaDonTheoBoLoc để đồng bộ thời gian lọc
    this.hoaDonTheoBoLoc().forEach(hd => {
      const chiTiet = hd.chiTietHoaDons ?? [];
      chiTiet.forEach((ct: any) => {
        const sp = this.danhSachSanPham().find(p => p.maSp === ct.maSP);
        const duLieuHienTai = banDoSanPham.get(ct.maSP) || {
          maSp: ct.maSP,
          tenSp: sp?.tenSp || 'Sản phẩm ' + ct.maSP,
          tenLoaiSp: sp?.tenLoaiSp || 'Khác',
          hinhAnh: sp?.hinhAnh || null,
          soLuongBan: 0,
          tongDoanhThuMon: 0
        };

        duLieuHienTai.soLuongBan += (ct.slsp ?? 0);
        duLieuHienTai.tongDoanhThuMon += (ct.thanhTien ?? 0);
        banDoSanPham.set(ct.maSP, duLieuHienTai);
      });
    });

    return Array.from(banDoSanPham.values()).sort((a, b) => b.soLuongBan - a.soLuongBan);
  });

  tenMonBanChayNhat = computed(() => {
    const danhSach = this.thongKeMonBanChay();
    return danhSach.length > 0 ? danhSach[0].tenSp : 'Chưa có dữ liệu';
  });

  monBanChayHienThi = computed(() => {
    const tuKhoa = this.tuKhoaTimKiem.trim().toLowerCase();
    if (!tuKhoa) return this.thongKeMonBanChay();
    return this.thongKeMonBanChay().filter(m => 
      m.tenSp.toLowerCase().includes(tuKhoa) || m.tenLoaiSp.toLowerCase().includes(tuKhoa)
    );
  });

  // 6. Tính toán Tỷ trọng ngành hàng (Đồ uống vs Thức ăn) KHỚP CHUẨN theo bộ lọc
  tyTrongDoanhThu = computed(() => {
    let doUong = 0; 
    let thucAn = 0; 
    let khac = 0;

    this.thongKeMonBanChay().forEach(m => {
      if (m.tenLoaiSp === 'Cà phê' || m.tenLoaiSp === 'Trà trái cây') {
        doUong += m.tongDoanhThuMon;
      } else if (m.tenLoaiSp === 'Bánh ngọt') {
        thucAn += m.tongDoanhThuMon;
      } else {
        khac += m.tongDoanhThuMon;
      }
    });

    const tong = doUong + thucAn + khac || 1;
    return {
      doUong,
      thucAn,
      khac,
      phanTramDoUong: Math.round((doUong / tong) * 100),
      phanTramThucAn: Math.round((thucAn / tong) * 100),
      gocXoayThucAn: Math.round((doUong / tong) * 360) // Tính góc xoay SVG donut mượt mà
    };
  });

  // 7. Thuật toán xử lý Đồ thị Xu hướng Doanh thu 7 ngày gần nhất (Không hardcode Thứ cố định)
  toadoDoThiTuyen = computed(() => {
    const mangDoanhThu7Ngay = new Array(7).fill(0);
    const danhSach = this.hoaDonHopLe();
    
    // Tạo danh sách chuỗi 7 ngày gần đây tính ngược từ hôm nay
    const mangChuoi7Ngay: string[] = [];
    for (let i = 6; i >= 0; i--) {
      const d = new Date();
      d.setDate(d.getDate() - i);
      mangChuoi7Ngay.push(d.toLocaleDateString('en-CA'));
    }

    danhSach.forEach(hd => {
      if (!hd.tgVao) return;
      const ngayHD = hd.tgVao.split('T')[0];
      const indexNgay = mangChuoi7Ngay.indexOf(ngayHD);
      if (indexNgay !== -1) {
        mangDoanhThu7Ngay[indexNgay] += (hd.tongTien || 0);
      }
    });

    const maxRevenue = Math.max(...mangDoanhThu7Ngay) || 1;
    
    // Ánh xạ doanh thu thực sang ma trận Y-axis của khung SVG (Cao 300px, chừa lề đáy 250px)
    const toaDoY = mangDoanhThu7Ngay.map(tien => 250 - Math.round((tien / maxRevenue) * 200));

    // Tính chuỗi định vị 7 tọa độ X dàn đều màn hình (Khoảng cách bước nhảy 155px)
    return {
      chuoiPath: `M 20,${toaDoY[0]} L 175,${toaDoY[1]} L 330,${toaDoY[2]} L 485,${toaDoY[3]} L 640,${toaDoY[4]} L 795,${toaDoY[5]} L 950,${toaDoY[6]}`,
      diemY: toaDoY,
      nhanNgay: mangChuoi7Ngay.map(n => n.substring(5, 10).replace('-', '/')) // Trả về định dạng MM/DD ngắn gọn
    };
  });

  // 8. Chiều cao đồ thị Lưu lượng khách theo khung giờ KHỚP CHUẨN theo phễu lọc
  chieuCaoCotGioCaoDiem = computed(() => {
    const khungGio = [7, 8, 9, 10, 11, 12, 13, 15, 18, 20];
    const thongKeGio = new Array(khungGio.length).fill(0);

    this.hoaDonTheoBoLoc().forEach(hd => {
      if (hd.tgVao) {
        const gio = new Date(hd.tgVao).getHours();
        const indexKhung = khungGio.indexOf(gio);
        if (indexKhung !== -1) {
          thongKeGio[indexKhung]++;
        }
      }
    });

    const maxDon = Math.max(...thongKeGio) || 1;
    return thongKeGio.map(don => Math.max(15, Math.round((don / maxDon) * 100)));
  });

  thayDoiBoLoc(kieu: 'ngay' | 'thang' | 'nam'): void {
    this.loaiBoLoc.set(kieu);
  }

  // Nghiệp vụ thật: Xuất file báo cáo định dạng CSV/Excel tải về máy
  xuatBaoCaoExcel(): void {
    if (this.hoaDonTheoBoLoc().length === 0) {
      alert('Không có dữ liệu hóa đơn trong kỳ này để xuất báo cáo!');
      return;
    }
    
    let noiDungCSV = 'Mã Hóa Đơn,Số Bàn,Thời Gian Vào,Tổng Tiền,Trạng Thái\n';
    this.hoaDonTheoBoLoc().forEach(hd => {
      noiDungCSV += `${hd.soHD},${hd.soBan || 'Mang đi'},${hd.tgVao},${hd.tongTien},${hd.trangThaiHD}\n`;
    });

    const blob = new Blob([new Uint8Array([0xEF, 0xBB, 0xBF]), noiDungCSV], { type: 'text/csv;charset=utf-8;' });
    const link = document.createElement('a');
    link.href = URL.createObjectURL(blob);
    link.setAttribute('download', `BaoCao_DoanhThu_${this.loaiBoLoc()}_WebCoffee.csv`);
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
  }
}