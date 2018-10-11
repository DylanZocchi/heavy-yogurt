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
        private UserManager<IdentityUser> _userManager;

        public CategoryController(ApplicationContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Category
        public async Task<ActionResult> Index()
        {
            var name = _userManager.GetUserName(User);
            return View(await _context.Categories
                .AsNoTracking()
                .OrderBy(c => c.CategoryName)
                .Where(u => u.UserName == name)
                .ToListAsync());
        }


        // GET: Category/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Category category)
        {
            // if not unit testing method
            if (_userManager != null)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                category.UserName = user.UserName;
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (_context.Categories.FirstOrDefault(c => c.CategoryName == category.CategoryName && c.UserName == category.UserName) == null)
            {
                try
                {
                    _context.Categories.Add(category);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                ViewBag.message = "Category already exists.";
                return View();
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