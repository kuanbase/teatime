using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeaTime.DataAccess.UnitOfWork;
using TeaTime.Models;

namespace TeaTime.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> productList = 
                _unitOfWork.Product.GetAll().AsQueryable().Include(p => p.Category);
            return View(productList);
        }

        public IActionResult Details(int productId)
        {
            ShoppingCart cart = new ShoppingCart()
            {
                Product = _unitOfWork.Product.Get(u => u.Id == productId),
                Count = 1,
                ProductId = productId,
            };
            
            return View(cart);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;

            if (claimsIdentity is null)
            {
                return NotFound();
            }

            string userId = claimsIdentity!.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            shoppingCart.ApplicationUserId = userId;

            var cartFromDb = _unitOfWork.ShoppingCart.Get(u =>
            u.Ice == shoppingCart.Ice
            && u.ApplicationUserId == shoppingCart.ApplicationUserId
            && u.Sweetness == shoppingCart.Sweetness
            && u.Product!.Id == shoppingCart.ProductId
            );

            //var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u == shoppingCart);

            //var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Equals(shoppingCart));
            //var list = _unitOfWork.ShoppingCart.dbSet.Where(u => u.Equals(shoppingCart)).FirstOrDefault();
            if (cartFromDb is null)
            {
                _unitOfWork.ShoppingCart.Add(shoppingCart);
            } 
            else
            {
                cartFromDb.Count += shoppingCart.Count;
                _unitOfWork.ShoppingCart.Update(cartFromDb);
            }

            //_unitOfWork.ShoppingCart.Add(shoppingCart);
            _unitOfWork.Save();

            TempData["success"] = "加入購物車成功！";

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
