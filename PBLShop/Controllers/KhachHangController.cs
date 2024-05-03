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
            var emailExists = _context.KhachHangs.Any(p => p.Email == model.Email);
            if (!emailExists)
            {
                var kh = _context.KhachHangs.ToList();
                KhachHang khachhang = new()
                {
                    MaKh = "KH" + (kh.Count + 1).ToString(),
                    MatKhau = model.MatKhau,
                    HoTen = model.HoTen,
                    GioiTinh = model.GioiTinh,
                    NgaySinh = model.NgaySinh,
                    DiaChi = model.DiaChi,
                    DienThoai = model.DienThoai,
                    Email = model.Email
                };
                _context.KhachHangs.Add(khachhang);
                _context.SaveChanges();
            }
            else
            {
                ModelState.AddModelError("loi", "Email đã tồn tại");
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
                var khachhang = _context.KhachHangs.FirstOrDefault(kh => kh.Email == model.Email);
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
        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult Profile()
        {
            var khachhang = _context.KhachHangs.FirstOrDefault(p => p.MaKh == HttpContext.User.FindFirstValue("MaKhachHang"));
            return View(khachhang);
        }
    }
}
