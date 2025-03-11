using Microsoft.AspNetCore.Mvc;
using TeaTime.DataAccess.UnitOfWork;
using TeaTime.Models;
using TeaTime.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TeaTime.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<Product> products = _unitOfWork.Product.GetAll().ToList();
            List<ProductVM> productVMs = new();

            var CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });

            foreach (var product in products)
            {
                ProductVM productVM = new ProductVM
                {
                    Product = product,
                    CategoryList = CategoryList,
                };

                productVMs.Add(productVM);
            }

            return View(productVMs);
        }

        public IActionResult Create()
        {
            ProductVM product = new()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                })
            };

            return View(product);
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {

            if (!ModelState.IsValid)
            {
                return View(product);
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

            ProductVM productVM = new ProductVM
            {
                Product = obj,
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.ToString(),
                })
            };

            return View(productVM);
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View();
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
    }
}
