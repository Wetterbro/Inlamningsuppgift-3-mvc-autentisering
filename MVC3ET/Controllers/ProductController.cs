using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC3ET.Data;
using MVC3ET.ViewModels;

namespace MVC3ET.Controllers
{
    /// <summary>
    /// Products controller, used to manage Products.
    /// </summary>
    [Authorize]
    public class  ProductController : Controller
    {
        
        private readonly ApplicationDbContext _context;
        
        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: Products
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.Include(p => p.Category).ToListAsync();
            return View(products);
        }
        // GET: Products/Details
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            var product = await _context.Products.Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
        // GET: Products/Create
        [Authorize]
        public IActionResult Create()
        {
            var categories = _context.Categories.ToList();
            var viewModel = new ProductCreateViewModel { Categories = categories };
            return View(viewModel);
        }
        
        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                _context.Products.Add(viewModel.Products);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            viewModel.Categories = _context.Categories.ToList();
            return View(viewModel);
        }
        
        // GET: Products/Edit
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            var categories = _context.Categories.ToList();
            var viewModel = new ProductCreateViewModel { Products = product, Categories = categories };
            return View(viewModel);
        }
        
        // POST: Products/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductCreateViewModel viewModel)
        {
            if (id != viewModel.Products.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Products.Update(viewModel.Products);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(viewModel.Products.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                      throw;  
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            viewModel.Categories = _context.Categories.ToList();
            return View(viewModel);
        }
        
        // GET: Products/Delete
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            var viewModel = new ProductCreateViewModel { Products = product, Categories = _context.Categories.ToList() };
            return View(viewModel);
        }

        // POST: Products/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
