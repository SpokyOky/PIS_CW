using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Logic.Logics;
using Logic.Models;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class OperationRevaluationController : Controller
    {
        private ProductGroupLogic _productGroupLogic;
        private ProductLogic _productLogic;

        public OperationRevaluationController(ProductGroupLogic productGroupLogic, ProductLogic productLogic)
        {
            _productGroupLogic = productGroupLogic;
            _productLogic = productLogic;
        }

        public IActionResult Index()
        {
            var productGroups = _productGroupLogic.Read(null);
            var productPrices = new Dictionary<Product, double>();

            foreach (var productGroup in productGroups)
            {
                foreach (var product in productGroup.Products)
                {
                    productPrices.Add(product, Math.Round(productGroup.Products.Average(rec => rec.Price), 2));
                }
            }

            ViewBag.ProductPrices = productPrices;

            return View(productGroups);
        }

        [HttpPost]
        public IActionResult Revaluate()
        {
            var form = Request.Form;
            var productGroups = _productGroupLogic.Read(null);
            var products = _productLogic.Read(null);

            foreach (var pair in form)
            {
                if (pair.Key.EndsWith("-product"))
                {
                    var id = Convert.ToInt32(pair.Key.Split("-")[0]);
                    var product = products.FirstOrDefault(rec => rec.Id == id);

                    if (pair.Value.Any(rec => rec != ""))
                    {
                        _productLogic.RevaluateProduct(product, Convert.ToDouble(pair.Value));
                    }
                }
            }

            foreach (var pair in form)
            {
                if (pair.Key.EndsWith("-group"))
                {
                    var id = Convert.ToInt32(pair.Key.Split("-")[0]);
                    var productGroup = productGroups.FirstOrDefault(rec => rec.Id == id);
                    try
                    {
                        _productGroupLogic.RevaluateGroup(productGroup, Convert.ToDouble(pair.Value.ToString().Replace('.',',')));
                    } catch (Exception) { }
                }
            }

            return RedirectToAction("Index");
        }
    }
}
