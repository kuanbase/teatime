using Microsoft.AspNetCore.Mvc;
using TeaTime.DataAccess.UnitOfWork;
using TeaTime.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Update.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Azure.Amqp.Framing;
using Microsoft.AspNetCore.Authorization;
using TeaTime.Utility;

namespace TeaTime.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = SD.Role_Admin)]
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
    public IActionResult Create(Product product, IFormFile? file)
    {
        if (!ModelState.IsValid)
        {
            View(product);
        }

        string wwwRootPath = _webHostEnvironment.WebRootPath;
        
        if (file != null)
        {
            string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string productPath = Path.Combine(wwwRootPath, @"images\product");
            
            using (var fileStream = new FileStream(Path.Combine(productPath, filename), FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            product.ImageUrl = @"\images\product\" + filename;
        }

        _unitOfWork.Product.Add(product);

        _unitOfWork.Save();

        TempData["success"] = "產品添加成功!";

        return RedirectToAction("Index");
    }

    public IActionResult Edit(int? id)
    {
        if (id == null || id == 0)
        {
            TempData["error"] = "不存在該產品";
            return RedirectToAction("Index");
        }

        Product? obj = _unitOfWork.Product.Get(x => x.Id == id);
        if (obj == null)
        {
            TempData["error"] = "不存在該產品";
            return RedirectToAction("Index");
        }

        ViewBag.CategoryList = GetSelectListItemGategories();

        return View(obj);
    }

    [HttpPost]
    public IActionResult Edit(Product product, IFormFile? file)
    {
        if (!ModelState.IsValid)
        {
            return View(product);
        }

        string wwwRootPath = _webHostEnvironment.WebRootPath;

        if (file != null)
        {
            string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string productPath = Path.Combine(wwwRootPath, @"images\product");

            if (!string.IsNullOrEmpty(product.ImageUrl))
            {
                var oldImagePath = Path.Combine(wwwRootPath, product.ImageUrl.Trim('\\'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            using (var fileStream = new FileStream(Path.Combine(productPath, filename), FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            product.ImageUrl = @"\images\product\" + filename;
        }

        _unitOfWork.Product.Update(product);
        _unitOfWork.Save();

        TempData["success"] = "產品修改成功";

        return RedirectToAction("Index");
    }

    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0)
        {
            TempData["error"] = "不存在該產品";
            return RedirectToAction("Index");
        }

        Product? obj = _unitOfWork.Product.Get(x => x.Id == id);
        if (obj == null)
        {
            TempData["error"] = "不存在該產品";
            return RedirectToAction("Index");
        }

        return View(obj);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeletePost(int? id)
    {
        Product? obj = _unitOfWork.Product.Get(x => x.Id == id);
        if (obj == null)
        {
            TempData["error"] = "不存在該產品";
            return RedirectToAction("Index");
        }

        _unitOfWork.Product.Remove(obj);
        _unitOfWork.Save();

        TempData["success"] = "刪除產品成功";

        return RedirectToAction("Index");
    }

    #region API CALLS
    [HttpGet]
    public IActionResult GetAll()
    {
        List<Product> objProductList = _unitOfWork.Product.GetAll().AsQueryable().Include(p => p.Category).ToList();
        return Json(new { data = objProductList});
    }
    #endregion

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
