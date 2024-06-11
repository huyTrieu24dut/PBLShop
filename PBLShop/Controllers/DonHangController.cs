using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PBLShop.Models;
using PBLShop.ViewModels;
using System.Security.Claims;

namespace PBLShop.Controllers
{
    public class DonHangController : Controller
    {
        private readonly WebShopContext _context;

        public DonHangController(WebShopContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "KhachHang")]
        [HttpGet]
        public IActionResult Index(int? matrangthai)
        {
            var donhangs = _context.DonHangs
                .Include(p => p.MaNguoiDungNavigation)
                .Include(p => p.MaTrangThaiNavigation)
                .Include(p => p.HoaDon)
                .Where(p => p.MaNguoiDung.ToString() == HttpContext.User.FindFirstValue("MaNguoiDung"))
                .ToList();
            if (matrangthai != null)
            {
                donhangs = donhangs.Where(p => p.MaTrangThai == matrangthai).ToList();
            }

            List<DonHangVM> result = new List<DonHangVM>();
            foreach (var item in donhangs){
                var file = _context.HoaDons.Where(p => p.MaDh == item.MaDh).Select(p => p.FileHoaDon).FirstOrDefault();
                var donhang = new DonHangVM
                {
                    MaDh = item.MaDh,
                    TenKh = item.MaNguoiDungNavigation.HoTen,
                    TongTien = item.TongTien,
                    TrangThai = item.MaTrangThaiNavigation.TenTrangThai,
                    MaTrangThai = item.MaTrangThai ?? 0,
                    FileHoaDon = file,
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
                        chiTietDhVM.size = item1.MaKtNavigation.Size;
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

        [Authorize(Roles = "KhachHang")]
        [HttpGet]
        public IActionResult Checkout()
        {
            string customerId = HttpContext.User.FindFirstValue("MaNguoiDung");
            if (customerId != null)
            {
                var customer = _context.NguoiDungs.Where(p => p.MaNguoiDung == Convert.ToInt32(customerId)).FirstOrDefault();
                if (customer != null)
                {
                    var defaultInfo = new CheckoutVM
                    {
                        TenNguoiNhan = customer.HoTen,
                        DienThoai = customer.SoDienThoai,
                        DiaChi = customer.DiaChi,
                    };
                    return View(defaultInfo);
                }
                else
                {
                    TempData["Message"] = "Không tìm thấy khách hàng";
                    return RedirectToAction("/404");
                }
            }
            TempData["Message"] = "Không tìm thấy khách hàng";
            return RedirectToAction("/404");
        }

        [Authorize(Roles ="KhachHang")]
        [HttpPost]
        public IActionResult Checkout(CheckoutVM model)
        {
            if (ModelState.IsValid)
            {
                var cartItems = _context.ChiTietGhs
                    .Include(p => p.MaMauNavigation)
                    .Include(p => p.MaKtNavigation)
                    .Include(p => p.MaMauNavigation.MaSpNavigation)
                    .ToList();
                string customerId = HttpContext.User.FindFirstValue("MaNguoiDung");
                var KhachHang = _context.NguoiDungs.SingleOrDefault(kh => kh.MaNguoiDung.ToString() == customerId);

                int Total = 0;
                foreach (var item in cartItems)
                {
                    Total += (item.SoLuong * item.MaMauNavigation.MaSpNavigation.DonGia) ?? 0;
                }

                var donhang = new DonHang
                {
                    MaNguoiDung = Convert.ToInt32(customerId),
                    TenNguoiNhan = model.TenNguoiNhan,
                    DiaChi = model.DiaChi,
                    SdtnguoiNhan = model.DienThoai,
                    NgayDatHang = DateTime.Now,
                    MaPttt = model.MaPhuongThuc,
                    TongTien = Total,
                    MaTrangThai = 1
                };
                _context.Add(donhang);
                _context.SaveChanges();

                var ctdh = new List<ChiTietDh>();
                foreach (var item in cartItems)
                {
                    ctdh.Add(new ChiTietDh
                    {
                        MaDh = donhang.MaDh,
                        SoLuong = item.SoLuong,
                        MaMau = item.MaMau,
                        MaKt = item.MaKt,
                    });
                    var sanpham = _context.QuanLySanPhams.FirstOrDefault(p => p.MaMau == item.MaMau && p.MaKichThuoc == item.MaKt);
                    if (sanpham != null)
                    {
                        sanpham.SoLuong -= item.SoLuong;
                        _context.Update(item.MaMauNavigation.MaSpNavigation);
                    }
                    _context.Update(item.MaMauNavigation.MaSpNavigation);
                    var cartItem = _context.ChiTietGhs.FirstOrDefault(p => p.MaMau == item.MaMau && p.MaKt == item.MaKt);
                    if (cartItem != null)
                    {
                        _context.ChiTietGhs.Remove(cartItem);
                    }
                }
                _context.AddRange(ctdh);
                _context.SaveChanges();

                return RedirectToAction("Success", "DonHang");
            }

            return View(model);
        }

        [Authorize(Roles = "KhachHang")]
        public IActionResult Success()
        {
            return View();
        }

        [Authorize(Roles = "KhachHang")]
        public IActionResult Cancel(int id)
        {
            var donhang = _context.DonHangs.FirstOrDefault(p => p.MaDh == id);
            if (donhang != null)
            {
                donhang.MaTrangThai = 5;
                _context.Update(donhang);
                _context.SaveChanges();
                return RedirectToAction("Index", "DonHang");
            }
            return RedirectToAction("Index", "DonHang");
        }
    }
}
