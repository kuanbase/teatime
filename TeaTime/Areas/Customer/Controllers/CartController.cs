using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using TeaTime.DataAccess.UnitOfWork;
using TeaTime.Models.ViewModel;

namespace TeaTime.Areas.Customer.Controllers;

[Area("Customer")]
[Authorize]
public class CartController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    public ShoppingCartVM? ShoppingCartVM;

    public CartController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        var claimsIdentity = User.Identity as ClaimsIdentity;

        if (claimsIdentity == null)
        {
            Console.WriteLine("警告:\t" + claimsIdentity + "為空");
            return Index();
        }

        var userId = claimsIdentity!.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        ShoppingCartVM = new()
        {
            ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId, includeProperties: "Product"),
            OrderTotal = 0
        };

        foreach (var cart in ShoppingCartVM.ShoppingCartList)
        {
            if (cart.Product == null)
            {
                continue;
            }

            ShoppingCartVM.OrderTotal += (cart.Product!.Price * cart.Count);
        }
        
        return View(ShoppingCartVM);
    }

    public IActionResult Summary()
    {
        return View();
    }

    public IActionResult Plus(int cartId)
    {
        var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);

        cartFromDb.Count += 1;

        _unitOfWork.ShoppingCart.Update(cartFromDb);

        _unitOfWork.Save();

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Minus(int cartId)
    {
        var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);

        if (cartFromDb.Count <= 1)
        {
            _unitOfWork.ShoppingCart.Remove(cartFromDb);
        }
        else
        {
            cartFromDb.Count -= 1;
            _unitOfWork.ShoppingCart.Update(cartFromDb);
        }

        _unitOfWork.Save();
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Remove(int cartId)
    {
        var cartFormDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);
        _unitOfWork.ShoppingCart.Remove(cartFormDb);
        _unitOfWork.Save();

        return RedirectToAction(nameof(Index));
    }
}
