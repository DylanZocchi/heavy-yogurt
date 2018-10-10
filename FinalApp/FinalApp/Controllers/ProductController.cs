using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalApp.Data;
using FinalApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;

namespace FinalApp.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private UserManager<IdentityUser> _userManager;
        private ApplicationContext _context;

        public ProductController(ApplicationContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var name = _userManager.GetUserName(User);
            
            return View(await _context.Products
                .AsNoTracking()
                .Include(x => x.Category)
                .Where(u => u.UserName == name)
                .OrderBy(p => p.ExpirationDate)
                .ToListAsync());
        }

        [HttpGet]
        public ActionResult Create()
        {           
            return View(PopulateViewModel(null)); ;
        }

        [Route("Edit/{id:int}")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            } 
           
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(PopulateViewModel(product));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("ProductId,CategoryId,ProductName,PurchaseDate,ExpirationDate")] Product product)
        {
            // if not unit testing method
            if (_userManager != null)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                product.UserName = user.UserName;
            }
            ModelState.Remove("product.PurchaseDate");
            if (!ModelState.IsValid)
            {
                return View(PopulateViewModel(product));
            }
            try
            {
                _context.Products.Add(product);
                {
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        public async Task<ActionResult> Delete(Product product)
        {
            var c = await _context.Products.FindAsync(product.ProductId);
            _context.Products.Remove(c);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", _context.Products);
        }


        [HttpPost]
        [Route("Edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, [Bind("ProductId,Category,CategoryId,ProductName,PurchaseDate,ExpirationDate")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }
            if (product.PurchaseDate > DateTime.Now || product.ExpirationDate <= DateTime.Now)
            {
                return View(PopulateViewModel(product));
            }
            var user = await _userManager.GetUserAsync(HttpContext.User);
            product.UserName = user.UserName;
            ModelState.Remove("product.PurchaseDate");
            _context.Update(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", _context.Products);
        }

        public ProductViewModel PopulateViewModel(Product product)
        {
            List<Category> Categories = _context.Categories.OrderBy(c => c.CategoryName).ToList();
            var productViewModel = new ProductViewModel(product, Categories);
            return productViewModel;
        }
    }
}