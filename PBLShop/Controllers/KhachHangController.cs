using Microsoft.AspNetCore.Mvc;
using PBLShop.Models;
using PBLShop.ViewModels;

namespace PBLShop.Controllers
{
    public class KhachHangController : Controller
    {
        private readonly WebShopContext _context;

        public KhachHangController(WebShopContext context)
        {
            _context = context;
        }
    }
}
