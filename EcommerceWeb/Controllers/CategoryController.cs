using Microsoft.AspNetCore.Mvc;
using ECommerce.Models;
using ECommerce.DataAccess.Data;
using ECommerce.DataAccess.Repository.IRepository;

namespace EcommerceWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepo;
        public CategoryController(ICategoryRepository categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }
        public IActionResult Index()
        {
            var list = _categoryRepo.GetAll().ToList();
            return View(list);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category obj)
        {

            if(obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "The DisplayOrder cannot exactly match the Name");
            }

            if(ModelState.IsValid)
            {
                _categoryRepo.Add(obj);
                _categoryRepo.Save();
                TempData["success"] = "Category created successfully";
                return RedirectToAction("Index");
            }

            return View();
           
        }

        public IActionResult Edit(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }

            var category = _categoryRepo.Get(x => x.Id == id);
            if(category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(Category obj)
        {

            if (ModelState.IsValid)
            {
                _categoryRepo.Update(obj);
                _categoryRepo.Save();
                TempData["success"] = "Category updated successfully";
                return RedirectToAction("Index");
            }

            return View();

        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var category = _categoryRepo.Get(x => x.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
                  if(id == null || id == 0)
            {
                return NotFound();
            }

            var category = _categoryRepo.Get(x => x.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            _categoryRepo.Remove(category);
            _categoryRepo.Save();
            TempData["success"] = "Category deleted successfully";

            return RedirectToAction("Index");

        }
    }
}
