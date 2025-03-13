using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeaTime.DataAccess.Data;
using TeaTime.DataAccess.Repository;
using TeaTime.DataAccess.UnitOfWork;
using TeaTime.Models;
using TeaTime.Utility;

namespace TeaTime.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<Category> categories = _unitOfWork.Category.GetAll().ToList();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (category.Name == category.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "類別名稱不能跟顯示順序一致");
                return View(category);
            }

            if (!ModelState.IsValid)
            {
                return View(category);
            }

            _unitOfWork.Category.Add(category);

            _unitOfWork.Save();

            TempData["success"] = "類別添加成功!";

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                TempData["error"] = "不存在該類別";
                return RedirectToAction("Index");
            }

            Category? catrgoryFromDb = _unitOfWork.Category.Get(x => x.Id == id);
            if (catrgoryFromDb == null)
            {
                TempData["error"] = "不存在該類別";
                return RedirectToAction("Index");
            }

            return View(catrgoryFromDb);
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            _unitOfWork.Category.Update(category);
            _unitOfWork.Save();

            TempData["success"] = "類別修改成功";

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                TempData["error"] = "不存在該類別";
                return RedirectToAction("Index");
            }

            Category? categoryFromDb = _unitOfWork.Category.Get(x => x.Id == id);
            if (categoryFromDb == null)
            {
                TempData["error"] = "不存在該類別";
                return RedirectToAction("Index");
            }

            return View(categoryFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            Category? categoryFromDb = _unitOfWork.Category.Get(x => x.Id == id);
            if (categoryFromDb == null)
            {
                TempData["error"] = "不存在該類別";
                return RedirectToAction("Index");
            }

            IQueryable<Product> query = _unitOfWork.Product.GetAll().AsQueryable();

            var product = query.FirstOrDefault(x => x.CategoryId == id);

            if (product != null)
            {
                TempData["error"] = "刪除失敗，仍有產品引用該類別";
                return RedirectToAction("Index");
            }

            _unitOfWork.Category.Remove(categoryFromDb);
            _unitOfWork.Save();

            TempData["success"] = "類別刪除成功！";
            return RedirectToAction("Index");
        }
    }
}
