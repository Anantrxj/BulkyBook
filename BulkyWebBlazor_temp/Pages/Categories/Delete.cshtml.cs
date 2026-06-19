using BulkyWebBlazor_temp.Data;
using BulkyWebBlazor_temp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace BulkyWebBlazor_temp.Pages.Categories
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _db;


        [BindProperty]
        public Category Category { get; set; }


        public DeleteModel(ApplicationDbContext db)
        {
            _db = db;
        }


        public IActionResult OnGet(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }


            Category = _db.Categories.Find(id);


            if (Category == null)
            {
                return NotFound();
            }


            return Page();
        }


        public IActionResult OnPost()
        {
            Category? obj = _db.Categories.Find(Category.Id);


            if (obj == null)
            {
                return NotFound();
            }


            _db.Categories.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "Category deleted successfully";


            return RedirectToPage("Index");
        }
    }
}