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
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var users = _context.NguoiDungs
                .Include(p => p.MaGioiTinhNavigation)
                .Where(p => p.MaVaiTro == 3 && p.TrangThai == true).ToList();
            List<NguoiDungVM> result = new List<NguoiDungVM>();
            foreach (var kh in users)
            {
                result.Add(new NguoiDungVM
                {
                    ID = kh.MaNguoiDung,
                    HoTen = kh.HoTen,
                    Email = kh.Email,
                    GioiTinh = kh.MaGioiTinhNavigation.TenGioiTinh,
                    NgaySinh = kh.NgaySinh,
                    SoDienThoai = kh.SoDienThoai,
                    DiaChi = kh.DiaChi
                });
            }
            return View(users);
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
                    .Any(p => p.Email == model.Email);
                if (!emailExists)
                {
                    NguoiDung user = new()
                    {
                        Email = model.Email,
                        MatKhau = model.MatKhau,
                        HoTen = model.HoTen,
                        MaGioiTinh = model.MaGioiTinh,
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
                else if(user.TrangThai)
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
                        if (user.MaVaiTro == 2)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                        }
                        else if (user.MaVaiTro == 3)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, "KhachHang"));
                        }
                        else
                        {
                            claims.Add(new Claim(ClaimTypes.Role, "NhanVien"));
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
                            if (user.MaVaiTro == 3)
                                return RedirectToAction("Index", "Home");
                            else
                                return RedirectToAction("Statistic", "DonHangAdmin");
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("loi", "Tài khoản đã bị xóa hoặc bị chặn");
                    return View(model);
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
            var nguoiDung = _context.NguoiDungs
                .Include(p => p.MaGioiTinhNavigation)
                .Include(p => p.MaVaiTroNavigation)
                .FirstOrDefault(p => p.MaNguoiDung.ToString() == HttpContext.User.FindFirstValue("MaNguoiDung"));
            if (nguoiDung == null)
            {
                return Redirect("/404");
            }
            var khachhangvm = new NguoiDungVM
            {
                ID = nguoiDung.MaNguoiDung,
                HoTen = nguoiDung.HoTen,
                Email = nguoiDung.Email,
                GioiTinh = nguoiDung.MaGioiTinhNavigation.TenGioiTinh,
                NgaySinh = nguoiDung.NgaySinh,
                SoDienThoai = nguoiDung.SoDienThoai,
                DiaChi = nguoiDung.DiaChi,
                vaiTro = nguoiDung.MaVaiTroNavigation.TenVaiTro,
            };
            return View(khachhangvm);
        }
        [Route("/AccessDenied")]
        public IActionResult AccessDenied()
        {
            return View();
        }


        [HttpGet]
        [Authorize]
        public IActionResult Update(int id)
        {
            var user = _context.NguoiDungs.FirstOrDefault(p => p.MaNguoiDung == id);
            if (user != null)
            {
                var nd = new ThongTinNguoiDung
                {
                    ID = user.MaNguoiDung,
                    Email = user.Email,
                    HoTen = user.HoTen,
                    MaGioiTinh = (int)user.MaGioiTinh,
                    NgaySinh = user.NgaySinh,
                    DiaChi = user.DiaChi,
                    DienThoai = user.SoDienThoai,
                    MaVaiTro = user.MaVaiTro,
                };
                return View(nd);
            }
            else
            {
                TempData["Message"] = "Không tìm thấy nhân viên";
                RedirectToAction("/404");
            }
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Update(int id, ThongTinNguoiDung model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.NguoiDungs.FirstOrDefault(p => p.MaNguoiDung == id);
                if (user == null)
                {
                    TempData["Message"] = "Không tìm thấy nhân viên";
                    RedirectToAction("/404");
                }

                var emailExists = _context.NguoiDungs.Any(p => p.Email == model.Email && p.MaNguoiDung != id);
                if (!emailExists)
                {
                    user.Email = model.Email;
                    user.HoTen = model.HoTen;
                    user.MaGioiTinh = model.MaGioiTinh;
                    user.NgaySinh = model.NgaySinh;
                    user.DiaChi = model.DiaChi;
                    user.SoDienThoai = model.DienThoai;

                    _context.SaveChanges();
                }
                else
                {
                    ModelState.AddModelError("loi", "Email đã tồn tại");
                    return View(model);
                }
            }
            return RedirectToAction("Profile", "User");
        }
    }
}
