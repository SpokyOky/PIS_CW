using Logic.Logics;
using Logic.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Web.Controllers
{
    public class SupplierController : Controller
    {
        private SupplierLogic _supplierLogic;
        private ProductGroupLogic _productGroupLogic;

        public SupplierController(SupplierLogic supplierLogic, ProductGroupLogic productGroupLogic)
        {
            _supplierLogic = supplierLogic;
            _productGroupLogic = productGroupLogic;
        }

        public IActionResult Index()
        {
            var suppliers = _supplierLogic.Read(null);
            return View(suppliers);
        }

        public IActionResult Add()
        {
            return View("Add", new Supplier());
        }

        [HttpPost]
        public IActionResult Add(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                _supplierLogic.CreateOrUpdate(supplier);
                return RedirectToAction("Index");
            }
            else
            {
                return View("Add", supplier);
            }
        }

        public IActionResult Edit(int id)
        {
            var supplier = _supplierLogic.Read(new Supplier { Id = id }).First();
            ViewBag.ProductGroups = GetProductGroups(supplier);
            return View("Supplier", supplier);
        }

        [HttpPost]
        public IActionResult Edit(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                _supplierLogic.CreateOrUpdate(supplier);
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.ProductGroups = GetProductGroups(supplier);
                return View("Supplier", supplier);
            }
        }

        public IActionResult AddPG(int id)
        {
            var supplier = _supplierLogic.Read(new Supplier { Id = id }).First();
            ViewBag.ProductGroups = GetProductGroups(null);
            return View("AddPG", supplier);
        }

        [HttpPost]
        public IActionResult AddPG(Supplier supplier)
        {
            int productGroupId;

            if (Int32.TryParse(Request.Form["productGroup"].ToString(), out productGroupId))
            {
                supplier.ProductGroups = new List<ProductGroup>();
                supplier.ProductGroups.Add(_productGroupLogic.Read(new ProductGroup { Id = productGroupId }).First());
            }

            if (ModelState.IsValid)
            {
                _supplierLogic.CreateOrUpdate(supplier);
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.ProductGroups = GetProductGroups(null);
                return View("AddPG", supplier);
            }
        }

        public IActionResult RemovePG(int id)
        {
            var supplier = _supplierLogic.Read(new Supplier { Id = id }).First();
            ViewBag.ProductGroups = GetProductGroups(supplier);
            return View("RemovePG", supplier);
        }

        [HttpPost]
        public IActionResult RemovePG(Supplier supplier)
        {
            int productGroupId;

            if (Int32.TryParse(Request.Form["productGroup"].ToString(), out productGroupId))
            {
                supplier.ProductGroups = new List<ProductGroup>();
                supplier.ProductGroups.Remove(_productGroupLogic.Read(new ProductGroup { Id = productGroupId }).First());
            }

            if (ModelState.IsValid)
            {
                _supplierLogic.CreateOrUpdate(supplier);
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.ProductGroups = GetProductGroups(supplier);
                return View("RemovePG", supplier);
            }
        }

        public IActionResult Delete(int id)
        {
            _supplierLogic.Delete(new Supplier { Id = id });
            return RedirectToAction("Index");
        }

        private SelectList GetProductGroups(Supplier supplier)
        {
            var _productGroups = _productGroupLogic.Read(null);

            if (supplier != null)
            {
                var productGroups = new List<ProductGroup>();
                foreach (var pg in _productGroups)
                {
                    foreach (var sup in pg.Suppliers)
                    {
                        if (sup == supplier)
                        {
                            productGroups.Add(pg);
                            break;
                        }
                    }
                }

                return new SelectList(productGroups, "Id", "Name");
            }
            else
            {
                return new SelectList(_productGroups, "Id", "Name");
            }           
        }
    }
}
