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
                        chiTietDhVM.SoLuong = (int)item1.SoLuong;
                        donhang.chiTietDhVMs.Add(chiTietDhVM);
                    }
                }
                result.Add(donhang);
            }
            return View(result);
        }

        [Authorize(Roles = "Admin, NhanVien")]
        public IActionResult Update(int id, int maTrangThai)
        {
            var donhang = _context.DonHangs.Where(p => p.MaDh == id).FirstOrDefault();
            if (donhang == null)
            {
                TempData["Message"] = "Không tìm thấy đơn hàng";
                Redirect("/404");
            }
            else
            {
                if (donhang.MaTrangThai == 5)
                {
                    TempData["Message"] = "Đơn hàng đã bị hủy";
                    Redirect("/404");
                }
                donhang.MaTrangThai = maTrangThai;
                //_context.Update(donhang);
                _context.SaveChanges();

                var trangthai = new QuanLyDh
                {
                    MaDh = donhang.MaDh,
                    MaTrangThai = maTrangThai,
                    ThoiGian = DateTime.Now,
                    MaNv = Convert.ToInt32(HttpContext.User.FindFirstValue("MaNguoiDung")),
                };
                _context.Add(trangthai);
                _context.SaveChanges();
            }
            if (maTrangThai > 1 && maTrangThai < 4)
            {
                return RedirectToAction("ProgressingList", "DonHangAdmin");
            }
            else if (maTrangThai == 4)
            {
                return RedirectToAction("Index", "DonHangAdmin");
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
                        chiTietDhVM.SoLuong = (int)item1.SoLuong;
                        donhang.chiTietDhVMs.Add(chiTietDhVM);
                    }
                }
                result.Add(donhang);
            }
            return View(result);
        }

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
                        chiTietDhVM.SoLuong = (int)item1.SoLuong;
                        donhang.chiTietDhVMs.Add(chiTietDhVM);
                    }
                }
                result.Add(donhang);
            }
            return View(result);
        }
    }
}
