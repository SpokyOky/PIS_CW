using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Logic;
using Logic.Models;
using Logic.Logics;

namespace Web.Controllers
{
    public class ProductController : Controller
    {
        private ProductLogic _productLogic;
        private ProductGroupLogic _productGroupLogic;

        public ProductController(ProductLogic productLogic, ProductGroupLogic productGroupLogic)
        {
            _productLogic = productLogic;
            _productGroupLogic = productGroupLogic;
        }

        public IActionResult Index()
        {
            var products = _productLogic.Read(null);
            return View(products);
        }


        public IActionResult Add()
        {
            ViewBag.ProductGroups = GetProductGroups();
            return View("Product", new Product());
        }

        [HttpPost]
        public IActionResult Add(Product product)
        {
            int productGroupId;

            if (Int32.TryParse(Request.Form["productGroup"].ToString(), out productGroupId))
            {
                product.ProductGroup = _productGroupLogic.Read(new ProductGroup { Id = productGroupId }).First();
            }

            if (product.ProductGroup == null)
            {
                ModelState.AddModelError("ProductGroup", "Необходимо выбрать категорию товара.");
            }

            if (ModelState.IsValid)
            {
                _productLogic.CreateOrUpdate(product);
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.ProductGroups = GetProductGroups(product.ProductGroup != null ? product.ProductGroup.Id.Value : 1);
                return View("Product", product);
            }
        }

        public IActionResult Edit(int id)
        {
            var product = _productLogic.Read(new Product { Id = id }).First();
            ViewBag.ProductGroups = GetProductGroups(product.ProductGroup != null ? product.ProductGroup.Id.Value : 1);
            return View("Product", product);
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            int productGroupId;

            if (Int32.TryParse(Request.Form["productGroup"].ToString(), out productGroupId))
            {
                product.ProductGroup = _productGroupLogic.Read(new ProductGroup { Id = productGroupId }).First();
            }

            if (product.ProductGroup == null)
            {
                ModelState.AddModelError("ProductGroup", "Необходимо выбрать категорию товара.");
            }

            if (ModelState.IsValid)
            {
                _productLogic.CreateOrUpdate(product);
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.ProductGroups = GetProductGroups(product.ProductGroup != null ? product.ProductGroup.Id.Value : 1);
                return View("Product", product);
            }
        }

        public IActionResult Delete(int id)
        {
            _productLogic.Delete(new Product { Id = id });
            return RedirectToAction("Index");
        }

        private SelectList GetProductGroups(int selectedId = 1)
        {
            var productGroups = _productGroupLogic.Read(null);
            var list = new SelectList(productGroups, "Id", "Name");
            var item = list.FirstOrDefault(rec => Convert.ToInt32(rec.Value) == selectedId);

            if (item != null)
            {
                item.Selected = true;
            }

            return list;
        }
    }
}
