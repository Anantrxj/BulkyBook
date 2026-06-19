using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;


        public CategoryController(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }


        public IActionResult Index()
        {
            List<Category> objCategoryList = _UnitOfWork.Category.GetAll().ToList();

            return View(objCategoryList);
        }


        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError(
                    "Name",
                    "Display Order cannot exactly match Name"
                );
            }


            if (ModelState.IsValid)
            {
                _UnitOfWork.Category.Add(obj);
                _UnitOfWork.save();

                TempData["Success"] = "Category created successfully";

                return RedirectToAction("Index");
            }

            return View(obj);
        }


        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }


            Category? categoryFromDb =
                _UnitOfWork.Category.Get(u=>u.Id==id);


            if (categoryFromDb == null)
            {
                return NotFound();
            }


            return View(categoryFromDb);
        }


        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _UnitOfWork.Category.Update(obj);

                _UnitOfWork.save();

                TempData["Success"] =
                    "Category updated successfully";

                return RedirectToAction("Index");
            }

            return View(obj);
        }



        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }


            Category? categoryFromDb =
                _UnitOfWork.Category.Get(u=>u.Id==id);


            if (categoryFromDb == null)
            {
                return NotFound();
            }


            return View(categoryFromDb);
        }



        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category? obj = _UnitOfWork.Category.Get(u => u.Id == id);


            if (obj == null)
            {
                return NotFound();
            }


            _UnitOfWork.Category.Remove(obj);

            _UnitOfWork.save();


            TempData["Success"] =
                "Category deleted successfully";


            return RedirectToAction("Index");
        }
    }
}