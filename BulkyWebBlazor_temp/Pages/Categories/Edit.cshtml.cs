using BulkyWebBlazor_temp.Data;
using BulkyWebBlazor_temp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkyWebBlazor_temp.Pages.Categories
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        [BindProperty]
        public Category Category { get; set; }
        public EditModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public void OnGet(int ? id)
        {
            if(id!= null || id!= 0)
            {
                 Category = _db.Categories.Find(id) ;
            }
        }
        public IActionResult OnPost()
        
        {
            if (ModelState.IsValid)
            {
                _db.Categories.Update(Category);
                _db.SaveChanges();
                TempData["Success"] = "Category updated successfully";

                return RedirectToPage("Index");
            }
            else
            {
                return Page();
            }

        }
    }
}
