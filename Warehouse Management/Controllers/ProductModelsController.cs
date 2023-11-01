using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Warehouse_Management.Data;
using Warehouse_Management.Models;

namespace Warehouse_Management.Controllers
{
    public class ProductModelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductModelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ProductModel.Include(p => p.Category);
            ProductSummary();
            CategoriesSummary();
            


            return View(await applicationDbContext.ToListAsync());
        }



        [Authorize]
        public IActionResult Create()
        {
                     
            ViewData["CategoryId"] = new SelectList(_context.Set<CategoryModel>(), "Id", "Name");
            return View();
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Code,Name,Description,Category,Quantity,Measure,Price")] ProductModel productModel)
        {
            

            if (ModelState.IsValid)
            {
                productModel.Category = _context.CategoryModel.FirstOrDefault(p => p.Id.ToString() == productModel.Category.Name);
                _context.Add(productModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

              ViewData["CategoryId"] = new SelectList(_context.Set<CategoryModel>(), "Id", "Name", productModel.Category.Id);
            
            return View(productModel);
        }

        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ProductModel == null)
            {
                return NotFound();
            }

            var productModel = await _context.ProductModel.FindAsync(id);
            if (productModel == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Set<CategoryModel>(), "Id", "Name");
            return View(productModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,Name,Description,Category,Quantity,Measure,Price")] ProductModel productModel)
        {
            if (id != productModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    productModel.Category = _context.CategoryModel.FirstOrDefault(p => p.Id.ToString() == productModel.Category.Name);
                    _context.Update(productModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductModelExists(productModel.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Set<CategoryModel>(), "Id", "Name");
            return View(productModel);
        }

        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ProductModel == null)
            {
                return NotFound();
            }

            var productModel = await _context.ProductModel
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productModel == null)
            {
                return NotFound();
            }

            return View(productModel);
        }

        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ProductModel == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ProductModel'  is null.");
            }
            var productModel = await _context.ProductModel.FindAsync(id);
            if (productModel != null)
            {
                _context.ProductModel.Remove(productModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductModelExists(int id)
        {
          return (_context.ProductModel?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public void ProductSummary()
        {
            float summary = 0;
            float kg = 0;
            float pcs = 0;
            float litre = 0;
            var list = _context.ProductModel.ToList();
            foreach ( var item in list)
            {
                summary += item.Quantity * item.Price;
                if (item.Measure == "pcs")
                {
                    pcs += item.Quantity;
                }
                else if( item.Measure =="kg")
                {
                    kg += item.Quantity;
                }
                else if(item.Measure =="litre")
                {
                    litre += item.Quantity;
                }
            }
            ViewBag.Price = summary;
            ViewBag.Kg = kg;
            ViewBag.Pcs = pcs;
            ViewBag.Litre = litre;

        }


        public void CategoriesSummary()
        {
            
            var list = _context.ProductModel.ToList();
            var lista = _context.CategoryModel.ToList();          
            var Dictionary = new Dictionary<string, float>();
            var Kg = new Dictionary<string, float>();
            var Pcs = new Dictionary<string, float>();
            var Litre = new Dictionary<string, float>();            

            foreach(var category in lista)
            {
                float summary = 0;
                float kg = 0;
                float pcs = 0;
                float litre = 0;


                foreach (var item in list)
                {

                    
                    if (category.Name == item.Category.Name)
                    {
                        summary += item.Quantity * item.Price;
                        if (item.Measure == "pcs")
                        {
                            pcs += item.Quantity;
                        }
                        else if (item.Measure == "kg")
                        {
                            kg += item.Quantity;
                        }
                        else if (item.Measure == "litre")
                        {
                            litre += item.Quantity;
                        }
                    }




                }

                Dictionary.Add(category.Name, summary);
                Kg.Add(category.Name, kg);
                Pcs.Add(category.Name,pcs);
                Litre.Add(category.Name,litre);


            }
            ViewBag.CategoryValueSum = Dictionary;
            ViewBag.CategoryKgSum = Kg;
            ViewBag.CategoryPcsSum = Pcs;
            ViewBag.CategoryLitreSum = Litre;
            
        }


    }

        




    }

