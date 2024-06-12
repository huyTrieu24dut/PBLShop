using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PBLShop.Models;
using PBLShop.ViewModels;
using System.Security.Claims;

namespace PBLShop.Controllers
{
    public class DonHangAdminController : Controller
    {
        private readonly WebShopContext _context;

        public DonHangAdminController(WebShopContext context)
        {
            _context = context;
        }


        [Authorize(Roles = "Admin, NhanVien")]
        public IActionResult Index()
        {
            var donhangs = _context.DonHangs
                .Include(p => p.MaNguoiDungNavigation)
                .Include(p => p.MaTrangThaiNavigation)
                .Include(p => p.MaPtttNavigation)
                .ToList();

            donhangs = donhangs.Where(p => p.MaTrangThai == 4).ToList();

            List<DonHangVM> result = new List<DonHangVM>();
            foreach (var item in donhangs)
            {
                var donhang = new DonHangVM
                {
                    MaDh = item.MaDh,
                    MaKh = item.MaNguoiDung,
                    TenKh = item.MaNguoiDungNavigation.HoTen,
                    TenNguoiNhan = item.TenNguoiNhan,
                    SoDienThoai = item.SdtnguoiNhan,
                    DiaChi = item.DiaChi,
                    TongTien = item.TongTien,
                    TrangThai = item.MaTrangThaiNavigation.TenTrangThai,
                    PhuongThuc = item.MaPtttNavigation.TenPt,
                    NgayDat = item.NgayDatHang
                };
                var chitietdhs = _context.ChiTietDhs
                    .Include(p => p.MaMauNavigation)
                    .Include(p => p.MaKtNavigation)
                    .Include(p => p.MaMauNavigation.MaSpNavigation)
                    .Where(p => p.MaDh == item.MaDh)
                    .ToList();
                foreach (var item1 in chitietdhs)
                {
                    if (item1.MaMauNavigation != null && item1.MaMauNavigation.MaSpNavigation != null)
                    {
                        var chiTietDhVM = new ChiTietDhVM();
                        chiTietDhVM.TenSp = item1.MaMauNavigation.MaSpNavigation.TenSp;
                        chiTietDhVM.MauSp = item1.MaMauNavigation.TenMau;
                        chiTietDhVM.size = item1.MaKtNavigation.Size; // null conditional operator
                        chiTietDhVM.DonGia = item1.MaMauNavigation.MaSpNavigation.DonGia;
                        chiTietDhVM.SoLuong = item1.SoLuong ?? 0;
                        chiTietDhVM.HinhAnh = item1.MaMauNavigation.AnhSp ?? "";
                        donhang.chiTietDhVMs.Add(chiTietDhVM);
                    }
                }
                var hoaDon = _context.HoaDons.Where(p => p.MaDh == donhang.MaDh).FirstOrDefault();
                if (hoaDon != null)
                {
                    donhang.MaHoaDon = hoaDon.MaHd;
                }
                else
                {
                    donhang.MaHoaDon = 0;
                }
                result.Add(donhang);
            }
            return View(result);
        }

        [Authorize(Roles = "Admin, NhanVien")]
        public IActionResult Update(int id, int maTrangThai)
        {
            int maTrangThaiBanDau = 0;
            HoaDon hd = new HoaDon();
            var donhang = _context.DonHangs.Where(p => p.MaDh == id).FirstOrDefault();
            if (donhang == null)
            {
                TempData["Message"] = "Không tìm thấy đơn hàng";
                Redirect("/404");
            }
            else
            {
                maTrangThaiBanDau = donhang.MaTrangThai ?? 0;
                if (donhang.MaTrangThai == 5)
                {
                    TempData["Message"] = "Đơn hàng đã bị hủy";
                    Redirect("/404");
                }
                donhang.MaTrangThai = maTrangThai;
                _context.SaveChanges();

                var trangthai = new QuanLyDh
                {
                    MaDh = donhang.MaDh,
                    MaTrangThai = maTrangThai,
                    ThoiGian = DateTime.Now,
                    MaNv = Convert.ToInt32(HttpContext.User.FindFirstValue("MaNguoiDung")),
                };
                _context.Add(trangthai);

                if (maTrangThai == 4)
                {
                    var hoadon = new HoaDon
                    {
                        MaDh = donhang.MaDh,
                        NgayHoanThanh = DateTime.Now,
                    };
                    _context.Add(hoadon);
                    hoadon.FileHoaDon = $"Invoice_{hoadon.MaHd}";
                    hd = hoadon;

                }
                _context.SaveChanges();
            }
            if (maTrangThai == 4 && donhang != null)
            {
                return RedirectToAction("GenerateInvoicePdf", "HoaDon", new { id = hd.MaHd });
            }
            if (maTrangThaiBanDau > 1 && maTrangThaiBanDau < 4)
            {
                return RedirectToAction("ProgressingList", "DonHangAdmin");
            }
            return RedirectToAction("ReceiveList", "DonHangAdmin");
        }

        [Authorize(Roles = "Admin, NhanVien")]
        public IActionResult UpdateInfo(int? id)
        {
            if (id == null)
            {
                TempData["Message"] = "Không tìm thấy đơn hàng";
                Redirect("/404");
            }
            var donhang = _context.DonHangs.Where(p => p.MaDh == id).FirstOrDefault();
            if (donhang == null)
            {
                TempData["Message"] = "Không tìm thấy đơn hàng";
                Redirect("/404");
            }
            else
            {
                var result = new List<lichSuDhVM>();
                var lichSu = _context.QuanLyDhs
                    .Include(p => p.MaDhNavigation)
                    .Include(p => p.MaDhNavigation.MaNguoiDungNavigation)
                    .Include(p => p.MaNvNavigation)
                    .Where(p => p.MaDh == donhang.MaDh).ToList();
                foreach (var ls in lichSu)
                {
                    result.Add(new lichSuDhVM
                    {
                        MaDh = ls.MaDh,
                        TenKh = ls.MaDhNavigation.MaNguoiDungNavigation.HoTen,
                        TenNv = ls.MaNvNavigation.HoTen,
                    });
                }

            }
            return View();
        }

        [Authorize(Roles = "Admin, NhanVien")]
        public IActionResult ReceiveList()
        {
            var donhangs = _context.DonHangs
                .Include(p => p.MaNguoiDungNavigation)
                .Include(p => p.MaTrangThaiNavigation)
                .Include(p => p.MaPtttNavigation)
                .ToList();

            donhangs = donhangs.Where(p => p.MaTrangThai == 1).ToList();

            List<DonHangVM> result = new List<DonHangVM>();
            foreach (var item in donhangs)
            {
                var donhang = new DonHangVM
                {
                    MaDh = item.MaDh,
                    MaKh = item.MaNguoiDung,
                    TenKh = item.MaNguoiDungNavigation.HoTen,
                    TenNguoiNhan = item.TenNguoiNhan,
                    SoDienThoai = item.SdtnguoiNhan,
                    DiaChi = item.DiaChi,
                    TongTien = item.TongTien,
                    TrangThai = item.MaTrangThaiNavigation.TenTrangThai,
                    PhuongThuc = item.MaPtttNavigation.TenPt,
                    NgayDat = item.NgayDatHang
                };
                var chitietdhs = _context.ChiTietDhs
                    .Include(p => p.MaMauNavigation)
                    .Include(p => p.MaKtNavigation)
                    .Include(p => p.MaMauNavigation.MaSpNavigation)
                    .Where(p => p.MaDh == item.MaDh)
                    .ToList();
                foreach (var item1 in chitietdhs)
                {
                    if (item1.MaMauNavigation != null && item1.MaMauNavigation.MaSpNavigation != null)
                    {
                        var chiTietDhVM = new ChiTietDhVM();
                        chiTietDhVM.TenSp = item1.MaMauNavigation.MaSpNavigation.TenSp;
                        chiTietDhVM.MauSp = item1.MaMauNavigation.TenMau;
                        chiTietDhVM.size = item1.MaKtNavigation.Size; // null conditional operator
                        chiTietDhVM.DonGia = item1.MaMauNavigation.MaSpNavigation.DonGia;
                        chiTietDhVM.SoLuong = item1.SoLuong ?? 0;
                        chiTietDhVM.HinhAnh = item1.MaMauNavigation.AnhSp ?? "";
                        donhang.chiTietDhVMs.Add(chiTietDhVM);
                    }
                }
                result.Add(donhang);
            }
            return View(result);
        }

        [Authorize(Roles = "Admin, NhanVien")]
        public IActionResult ProgressingList()
        {
            var donhangs = _context.DonHangs
                .Include(p => p.MaNguoiDungNavigation)
                .Include(p => p.MaTrangThaiNavigation)
                .Include(p => p.MaPtttNavigation)
                .ToList();

            donhangs = donhangs.Where(p => p.MaTrangThai == 2 || p.MaTrangThai == 3).ToList();

            List<DonHangVM> result = new List<DonHangVM>();
            foreach (var item in donhangs)
            {
                var donhang = new DonHangVM
                {
                    MaDh = item.MaDh,
                    MaKh = item.MaNguoiDung,
                    TenKh = item.MaNguoiDungNavigation.HoTen,
                    TenNguoiNhan = item.TenNguoiNhan,
                    SoDienThoai = item.SdtnguoiNhan,
                    DiaChi = item.DiaChi,
                    TongTien = item.TongTien,
                    TrangThai = item.MaTrangThaiNavigation.TenTrangThai,
                    PhuongThuc = item.MaPtttNavigation.TenPt,
                    NgayDat = item.NgayDatHang
                };
                var chitietdhs = _context.ChiTietDhs
                    .Include(p => p.MaMauNavigation)
                    .Include(p => p.MaKtNavigation)
                    .Include(p => p.MaMauNavigation.MaSpNavigation)
                    .Where(p => p.MaDh == item.MaDh)
                    .ToList();
                foreach (var item1 in chitietdhs)
                {
                    if (item1.MaMauNavigation != null && item1.MaMauNavigation.MaSpNavigation != null)
                    {
                        var chiTietDhVM = new ChiTietDhVM();
                        chiTietDhVM.TenSp = item1.MaMauNavigation.MaSpNavigation.TenSp;
                        chiTietDhVM.MauSp = item1.MaMauNavigation.TenMau;
                        chiTietDhVM.size = item1.MaKtNavigation.Size; // null conditional operator
                        chiTietDhVM.DonGia = item1.MaMauNavigation.MaSpNavigation.DonGia;
                        chiTietDhVM.SoLuong = item1.SoLuong ?? 0;
                        chiTietDhVM.HinhAnh = item1.MaMauNavigation.MaSpNavigation.AnhSp ?? "";
                        donhang.chiTietDhVMs.Add(chiTietDhVM);
                    }
                }
                result.Add(donhang);
            }
            return View(result);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Statistic(int type)
        { 
            var chitietDHs = _context.ChiTietDhs
                .Include(p => p.MaDhNavigation)
                .Include(p => p.MaDhNavigation.HoaDon)
                .Include(p => p.MaMauNavigation)
                .Include(p => p.MaMauNavigation.MaSpNavigation)
                .Include(p => p.MaMauNavigation.MaSpNavigation.MaDmNavigation)
                .Where(p => p.MaDhNavigation.HoaDon != null)
                .ToList()
                .AsEnumerable();
            int hoaDons = 0;
            switch (type)
            {
                case 1:
                    hoaDons = _context.HoaDons.Where(p => p.NgayHoanThanh.Date == DateTime.Today).ToList().Count();
                    chitietDHs = chitietDHs.Where(p => p.MaDhNavigation.HoaDon.NgayHoanThanh.Date == DateTime.Today);
                    TempData["Message"] = "Ngày " + DateTime.Today.ToString("dd/MM/yyyy");
                    TempData["status"] = "Day";
                    break;
                case 2:
                    hoaDons = _context.HoaDons.Where(p => p.NgayHoanThanh.Month == DateTime.Today.Month
                        && p.NgayHoanThanh.Year == DateTime.Today.Year).ToList().Count();
                    chitietDHs = chitietDHs.Where(p => p.MaDhNavigation.HoaDon.NgayHoanThanh.Month == DateTime.Today.Month 
                        && p.MaDhNavigation.HoaDon.NgayHoanThanh.Year == DateTime.Today.Year);
                    TempData["Message"] = "Tháng " + DateTime.Today.ToString("MM/yyyy");
                    TempData["status"] = "Month";
                    break;
                case 3:
                    hoaDons = _context.HoaDons.Where(p => p.NgayHoanThanh.Year == DateTime.Today.Year).ToList().Count();
                    chitietDHs = chitietDHs.Where(p => p.MaDhNavigation.HoaDon.NgayHoanThanh.Year == DateTime.Today.Year);
                    TempData["Message"] = "Năm " + DateTime.Today.Year;
                    TempData["status"] = "Year";
                    break;
            }
            var chitiets = new List<ChiTietDh>();

            var danhMucs = _context.DanhMucs.Where(p => p.MaDmcha != null).ToList();
            var sanPhams = _context.SanPhams.Select(p => p).ToList();
            
            var productsData = new List<ProductData>();
            var turnoverData = new List<TurnoverData>();
            var growthData = new List<GrowthData>();
            var viewModel = new ChartDataVM();

            viewModel.TotalInvoice = hoaDons;
            foreach (var danhMuc in danhMucs)
            {
                int soluong = 0;
                foreach (var chitietDH in chitietDHs)
                {
                    
                    if (chitietDH.MaMauNavigation.MaSpNavigation.MaDm == danhMuc.MaDm)
                    {
                        soluong += chitietDH.SoLuong ?? 0;
                    }
                }
                if (soluong > 0)
                {
                    productsData.Add(new ProductData
                    {
                        Name = danhMuc.TenDanhMuc,
                        Quantity = soluong,
                    });
                    viewModel.TotalProduct += soluong;
                }
            }
            foreach (var sanPham in sanPhams)
            {
                int doanhthu = 0;
                foreach (var chitietDH in chitietDHs)
                {
                    if (chitietDH.MaMauNavigation.MaSp == sanPham.MaSp)
                    {
                        doanhthu += (sanPham.DonGia * chitietDH.SoLuong) ?? 0;
                    }
                }
                if (doanhthu > 0)
                {
                    turnoverData.Add(new TurnoverData
                    {
                        Product = sanPham.TenSp,
                        Revenue = doanhthu,
                    });
                    viewModel.TotalRevenue += doanhthu;
                }
            }

            //for (int i = 1; i <= 12; i++)
            //{

            //}

            viewModel.ProductsData = productsData;
            viewModel.TurnoverData = turnoverData;
            viewModel.GrowthData = growthData;

            return View(viewModel);
        }
    }
}
