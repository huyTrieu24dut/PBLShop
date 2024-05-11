using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PBLShop.Models;
using PBLShop.ViewModels;
using System.Security.Claims;

namespace PBLShop.Controllers
{
    public class UserController : Controller
    {
        private readonly WebShopContext _context;

        public UserController(WebShopContext context)
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
            if (ModelState.IsValid){
                var emailExists = _context.NguoiDungs
                    .Any(p => p.Email == p.Email);
                if (!emailExists)
                {
                    var gioiTinh = _context.GioiTinhs.FirstOrDefault(gt => gt.TenGioiTinh == model.GioiTinh);
                    NguoiDung user = new()
                    {
                        Email = model.Email,
                        MatKhau = model.MatKhau,
                        HoTen = model.HoTen,
                        MaGioiTinh = gioiTinh != null ? gioiTinh.MaGioiTinh : 1,
                        NgaySinh = model.NgaySinh,
                        DiaChi = model.DiaChi,
                        SoDienThoai = model.DienThoai,
                        MaVaiTro = 3,
                        TrangThai = true,
                    };
                    _context.NguoiDungs.Add(user);
                    _context.SaveChanges();
                }
                else
                {
                    ModelState.AddModelError("loi", "Email đã tồn tại");
                    return View(model);
                }
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
                var user = _context.NguoiDungs
                    .Include(p => p.MaVaiTroNavigation)
                    .FirstOrDefault(kh => kh.Email == model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("loi", "Sai Thông tin đăng nhập");
                    return View(model);
                }
                else
                {
                    if (user.MatKhau != model.MatKhau)
                    {
                        ModelState.AddModelError("loi", "Sai Thông tin đăng nhập");
                        return View(model);
                    }
                    else
                    {
                        var claims = new List<Claim> {
                            new Claim("MaNguoiDung", user.MaNguoiDung.ToString()),
                            new Claim(ClaimTypes.Name, user.HoTen),
                            new Claim(ClaimTypes.Email, user.Email),

                            new Claim(ClaimTypes.Role, "KhachHang")
                        };
                        if (user.MaVaiTroNavigation.TenVaiTro == "Admin")
                        {
                            claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                        }
                        else if (user.MaVaiTroNavigation.TenVaiTro == "Khách hàng")
                        {
                            claims.Add(new Claim(ClaimTypes.Role, "KhachHang"));
                        }
                        else
                        {
                            claims.Add(new Claim(ClaimTypes.Role, "Nhanvien"));
                        }
                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                        await HttpContext.SignInAsync(claimsPrincipal);

                        if (Url.IsLocalUrl(url))
                        {
                            TempData["SuccessMessage"] = "Đăng nhập thành công!";
                            return Redirect(url);
                        }
                        else
                        {
                            TempData["SuccessMessage"] = "Đăng nhập thành công!";
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
            }
            return View(model);
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
            var khachhang = _context.NguoiDungs
                .Include(p => p.MaGioiTinhNavigation)
                .FirstOrDefault(p => p.MaNguoiDung.ToString() == HttpContext.User.FindFirstValue("MaNguoiDung"));
            if (khachhang == null)
            {
                return Redirect("/404");
            }
            var khachhangvm = new NguoiDungVM
            {
                HoTen = khachhang.HoTen,
                Email = khachhang.Email,
                GioiTinh = khachhang.MaGioiTinhNavigation.TenGioiTinh,
                NgaySinh = (DateTime)khachhang.NgaySinh,
                SoDienThoai = khachhang.SoDienThoai,
                DiaChi = khachhang.DiaChi
            };
            return View(khachhangvm);
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
        [Authorize]
        public IActionResult Modify()
        {
            return Redirect("Profile");
        }
    }
}
