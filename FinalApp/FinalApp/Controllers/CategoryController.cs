using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalApp.Data;
using FinalApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace FinalApp.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        public ApplicationContext _context { get; set; }
        public CategoryController(ApplicationContext context)
        {
                _context = context;
        }

        // GET: Category
        public async Task<ActionResult> Index()
        {
            return View(await _context.Categories
                .AsNoTracking()
                .OrderBy(c => c.CategoryName)
                .ToListAsync());
        }


        // GET: Category/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Category category)
        {
            try
            {
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch
            {
                return View("Index");
            }
        }

        public async Task<ActionResult> Delete(Category category)
        {
            var c = await _context.Categories.FindAsync(category.CategoryId);
            if (!_context.Products.Any(p => p.Category == category))
            {
                _context.Categories.Remove(c);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", _context.Categories);
            }
            else
            {
                ViewBag.Error = "You may not delete categories with products in them.";
                return View("Index", await _context.Categories
                .AsNoTracking()
                .OrderBy(cat => cat.CategoryName)
                .ToListAsync());
            }
        }
    }
}