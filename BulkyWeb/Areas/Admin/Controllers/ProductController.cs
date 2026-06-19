using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]

    public class ProductController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _UnitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> objProductList = _UnitOfWork.Product.GetAll(includeProperties: "Category").ToList();

            return View(objProductList);
        }


        public IActionResult Upsert(int? id)
        {
            IEnumerable<SelectListItem> CategoryList = _UnitOfWork.Category.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            ViewBag.CategoryList = CategoryList;

            if (id == 0 || id == null)
            {
                // create
                Product product = new Product();
                return View(product);
            }
            else
            {
                Product? categoryFromDb =
                _UnitOfWork.Product.Get(u => u.Id == id);
                return View(categoryFromDb);
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Product obj, IFormFile? file)
        {
            // Repopulate CategoryList so view has values if we re-render the form
            ViewBag.CategoryList =
                _UnitOfWork.Category.GetAll()
                .Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });

            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;

                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString()
                        + Path.GetExtension(file.FileName);

                    string productpath = Path.Combine(
                        wwwRootPath,
                        "Images",
                        "Product"
                    );

                    if(!String.IsNullOrEmpty(obj.ImageUrl))
                    {
                        // delete the old image
                        var relativePath = obj.ImageUrl.Replace('\\', '/').TrimStart('/');
                        var oldImagePath = Path.Combine(wwwRootPath, relativePath);

                        if(System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);    
                        }
                    }

                    // Ensure directory exists
                    if (!Directory.Exists(productpath))
                    {
                        Directory.CreateDirectory(productpath);
                    }

                    using (var fileStream = new FileStream(
                        Path.Combine(productpath, fileName),
                        FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    obj.ImageUrl = "/Images/Product/" + fileName;
                }

                if (obj.Id == 0)
                {
                    _UnitOfWork.Product.Add(obj);
                    TempData["Success"] = "Product created successfully";
                }
                else
                {
                    Product productFromDb = _UnitOfWork.Product.Get(u => u.Id == obj.Id);
                
                    if (file == null)
                    {
                        obj.ImageUrl = productFromDb.ImageUrl;
                    }

                    _UnitOfWork.Product.Update(obj);

                    TempData["Success"] =
                        "Product updated successfully";
                }

                _UnitOfWork.save();

                return RedirectToAction("Index");
            }

            // Log validation errors to help debugging
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(error.ErrorMessage);
            }

            // If we get here ModelState is invalid — re-render the view with populated ViewBag
            return View(obj);
        }



        

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProductList = _UnitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = objProductList });
        }


        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productToBeDeleted = _UnitOfWork.Product.Get(u => u.Id == id);
            if(productToBeDeleted == null) 
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            if (!string.IsNullOrEmpty(productToBeDeleted.ImageUrl))
            {
                var relativePath = productToBeDeleted.ImageUrl.Replace('\\', '/').TrimStart('/');
                var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, relativePath);

                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            _UnitOfWork.Product.Remove(productToBeDeleted);
            _UnitOfWork.save();

            return Json(new { success = true, message = "Product deleted successfully" });
        }

    }
        #endregion
}

