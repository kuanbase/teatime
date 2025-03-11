using Microsoft.AspNetCore.Mvc;
using TeaTime.DataAccess.UnitOfWork;
using TeaTime.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Update.Internal;
using Microsoft.EntityFrameworkCore;

namespace TeaTime.Areas.Admin.Controllers;

[Area("Admin")]
public class ProductController : Controller
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    private readonly IUnitOfWork _unitOfWork;

    public List<Product>? ViewProducts;

    public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
    {
        _unitOfWork = unitOfWork;
        _webHostEnvironment = webHostEnvironment;
        ViewProducts = GetViewProduct();
    }

    public IActionResult Index()
    {
        return View(ViewProducts);
    }

    public IActionResult Create()
    {
        ViewBag.CategoryList = GetSelectListItemGategories();
        return View();
    }

    [HttpPost]
    public IActionResult Create(Product product)
    {

        if (!ModelState.IsValid)
        {
            View(product);
        }

        _unitOfWork.Product.Add(product);

        _unitOfWork.Save();

        TempData["success"] = "添加成功!";

        return RedirectToAction("Index");
    }

    public IActionResult Edit(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        Product? obj = _unitOfWork.Product.Get(x => x.Id == id);
        if (obj == null)
        {
            return NotFound();
        }

        ViewBag.CategoryList = GetSelectListItemGategories();

        return View(obj);
    }

    [HttpPost]
    public IActionResult Edit(Product product)
    {
        if (!ModelState.IsValid)
        {
            return View(product);
        }

        _unitOfWork.Product.Update(product);
        _unitOfWork.Save();

        return RedirectToAction("Index");
    }

    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        Product? obj = _unitOfWork.Product.Get(x => x.Id == id);
        if (obj == null)
        {
            return NotFound();
        }

        return View(obj);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeletePost(int? id)
    {
        Product? obj = _unitOfWork.Product.Get(x => x.Id == id);
        if (obj == null)
        {
            return NotFound();
        }

        _unitOfWork.Product.Remove(obj);
        _unitOfWork.Save();

        return RedirectToAction("Index");
    }

    public IEnumerable<SelectListItem> GetSelectListItemGategories()
    {
        var list = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
        {
            Text = u.Name,
            Value = u.Id.ToString(),
        });

        return list;
    }

    public List<Product> GetViewProduct()
    {
        List<Product> products = _unitOfWork.Product.GetAll().ToList();
        List<Category> categories = _unitOfWork.Category.GetAll().ToList();

        var query = _unitOfWork.Category.GetAll();

        products.ForEach(product =>
        {
            int categoryId = product.CategoryId;
            var category = query.FirstOrDefault(category => category.Id == categoryId);

            if (category != null)
            {
                product.CategoryName = category.Name;
            } else
            {
                product.CategoryName = "未分類";
            }
        });

        return products;
    }

    private void UpdateViewProducts()
    {
        ViewProducts = GetViewProduct();
    }
}
