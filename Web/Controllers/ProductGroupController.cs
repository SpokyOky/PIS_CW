using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Logic.Logics;
using Logic.Models;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class ProductGroupController : Controller
    {
        private ProductGroupLogic _productGroupLogic;

        public ProductGroupController(ProductGroupLogic productGroupLogic)
        {
            _productGroupLogic = productGroupLogic;
        }

        public IActionResult Index()
        {
            var productGroups = _productGroupLogic.Read(null);
            return View(productGroups);
        }

        public IActionResult Add()
        {
            return View("ProductGroup", new ProductGroup());
        }

        [HttpPost]
        public IActionResult Add(ProductGroup productGroup)
        {
            if (ModelState.IsValid)
            {
                _productGroupLogic.CreateOrUpdate(productGroup);
                return RedirectToAction("Index");
            }
            else
            {
                return View("ProductGroup", productGroup);
            }
        }

        public IActionResult Edit(int id)
        {
            return View("ProductGroup", _productGroupLogic.Read(new ProductGroup { Id = id }).First());
        }

        [HttpPost]
        public IActionResult Edit(ProductGroup productGroup)
        {
            if (ModelState.IsValid)
            {
                _productGroupLogic.CreateOrUpdate(productGroup);
                return RedirectToAction("Index");
            }
            else
            {
                return View("ProductGroup", productGroup);
            }
        }

        public IActionResult Delete(int id)
        {
            _productGroupLogic.Delete(new ProductGroup { Id = id });
            return RedirectToAction("Index");
        }
    }
}
