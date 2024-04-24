using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PBLShop.Models;
using PBLShop.ViewModels;
using System.Security.Claims;

namespace PBLShop.Controllers
{
    public class KhachHangController : Controller
    {
        private readonly PblshopContext _context;

        public KhachHangController(PblshopContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(DangKyVM model)
        {
            if (ModelState.IsValid)
            {
                KhachHang khachhang = new KhachHang();
                khachhang.MaKh = model.MaKh;
                khachhang.MatKhau = model.MatKhau;
                khachhang.HoTen = model.HoTen;
                khachhang.GioiTinh = model.GioiTinh;
                khachhang.NgaySinh = model.NgaySinh;
                khachhang.DiaChi = model.DiaChi;
                khachhang.DienThoai = model.DienThoai;
                khachhang.Email = model.Email;

                _context.KhachHangs.Add(khachhang);
                _context.SaveChanges();
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Login(string? url)
        {
            ViewBag.ReturnUrl = url;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(DangNhapVM model, string? url)
        {
            ViewBag.ReturnUrl = url;
            if (ModelState.IsValid)
            {
                var khachhang = _context.KhachHangs.FirstOrDefault(kh => kh.MaKh == model.MaKh);
                if (khachhang == null)
                {
                    ModelState.AddModelError("loi", "Sai Thông tin đăng nhập");
                }
                else
                {
                    if(khachhang.MatKhau != model.MatKhau)
                    {
                        ModelState.AddModelError("loi", "Sai Thông tin đăng nhập");
                    }
                    else
                    {
                        var clams = new List<Claim> {
                            new Claim("MaKhachHang", khachhang.MaKh),
                            new Claim(ClaimTypes.Name, khachhang.HoTen),
                            new Claim(ClaimTypes.Email, khachhang.Email),

                            new Claim(ClaimTypes.Role, "KhachHang")
                        };

                        var claimsIdentity = new ClaimsIdentity(clams, CookieAuthenticationDefaults.AuthenticationScheme);
                        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                        await HttpContext.SignInAsync(claimsPrincipal);

                        if (Url.IsLocalUrl(url))
                        {
                            return Redirect(url);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
            }
            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult Profile()
        {
            return View();
        }
    }
}
