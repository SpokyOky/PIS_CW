using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FusionCharts.DataEngine;
using FusionCharts.Visualization;
using Logic.Logics;
using Logic.Models;
using Logic.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.Controllers
{
    public class DataController : Controller
    {
        ProductGroupLogic _productGroupLogic;
        ProductLogic _productLogic;
        SupplierLogic _supplierLogic;
        ExportLogic _exportLogic;
        SaleLogic _saleLogic;

        public DataController(ProductGroupLogic productGroupLogic, ProductLogic productLogic, SupplierLogic supplierLogic, ExportLogic exportLogic, SaleLogic saleLogic)
        {
            _productGroupLogic = productGroupLogic;
            _productLogic = productLogic;
            _supplierLogic = supplierLogic;
            _exportLogic = exportLogic;
            _saleLogic = saleLogic;
        }

        public IActionResult Supplier()
        {
            try
            {
                var form = Request.Form;
                Supplier supplier = null;

                if (form.ContainsKey("supplierName") && form["supplierName"][0].Length > 0)
                {
                    var name = form["supplierName"];
                    supplier = _supplierLogic.Read(new Supplier { Name = name }).FirstOrDefault();

                }
                else if (form.ContainsKey("supplierINN") && form["supplierINN"][0].Length > 0)
                {
                    var inn = form["supplierINN"];
                    supplier = _supplierLogic.Read(new Supplier { INN = inn }).FirstOrDefault();
                }
                if (supplier != null)
                {
                    ViewBag.Supplier = supplier;
                    var productGroups = new List<ProductGroup>();
                    foreach (var pg in supplier.ProductGroups)
                    {
                        productGroups.Add(pg);
                    }
                    ViewBag.ProductGroups = productGroups;
                }
            }
            catch (Exception) { }

            return View("Supplier");
        }

        public IActionResult ProductSupplierCity()
        {
            try
            {
                var suppliers = _supplierLogic.Read(null);
                var productsList = new List<(Supplier, IEnumerable<IGrouping<string, Product>>)>();
                foreach (var sup in suppliers)
                {
                    var products = new List<Product>();
                    foreach (var pg in sup.ProductGroups)
                    {
                        foreach (var pr in pg.Products)
                        {
                            products.Add(pr);
                        }
                    }
                    productsList.Add((sup, products.GroupBy(pr => pr.City)));
                }
                ViewBag.ProductsList = productsList;
            }
            catch (Exception) { }

            return View("ProductSupplierCity");
        }

        public IActionResult TopSales()
        {
            try
            {
                var products = _productLogic.Read(null);
                var topSales = new List<(Product, double)>();
                if (products != null)
                {
                    foreach (var product in products)
                    {
                        var sumSale = 0.0;
                        if (product.TableProducts != null)
                        {
                            foreach (var tableProduct in product.TableProducts)
                            {
                                if (tableProduct.Sale != null)
                                {
                                    sumSale += tableProduct.Amount * product.Price;
                                }
                            }
                            topSales.Add((product, sumSale));
                        }
                    }
                }
                
                var sortedTop = topSales.OrderByDescending(rec => rec.Item2).ToList();
                if (sortedTop.Count() > 10)
                {
                    sortedTop.RemoveRange(10, sortedTop.Count() - 10);
                }
                ViewBag.TopSales = sortedTop;
            }
            catch (Exception) { }

            return View("TopSales");
        }

        public IActionResult ProductSales()
        {
            return View("ProductSales");
        }

        [HttpPost]
        public IActionResult ProductSales(DateViewModel model)
        {
            if (model.dateFrom != null && model.dateTo != null)
            {
                if (model.dateFrom <= model.dateTo)
                {
                    var sales = _saleLogic.Read(model);
                    var products = _productLogic.Read(null);
                    var result = new Dictionary<string, int>();
                    foreach (var product in products)
                    {
                        result[product.Name] = 0;
                    }
                    if (products != null)
                    {
                        foreach (var sale in sales)
                        {
                            foreach (var tp in sale.TableProducts)
                            {
                                foreach (var product in products)
                                {
                                    if (tp.Product == product)
                                    {
                                        result[product.Name] += tp.Amount;
                                    }
                                }
                            }
                        }
                        ViewBag.Result = result;
                        ViewBag.Products = products;
                    }   
                }
            }
            return View("ProductSales", model);
        }

        public IActionResult SalesSum()
        {
            return View("SalesSum");
        }

        [HttpPost]
        public IActionResult SalesSum(DateViewModel model)
        {
            try
            {
                var sales = _saleLogic.Read(null);
                var suppliers = _supplierLogic.Read(null);
                if (sales != null || sales.Count > 0)
                {
                    DateTime min = sales[0].Date;
                    DateTime max = sales[0].Date;
                    foreach (var sale in sales)
                    {
                        if (sale.Date < min.Date)
                        {
                            min = sale.Date;
                        }
                        if (sale.Date > max.Date)
                        {
                            max = sale.Date;
                        }
                    }

                    var periodCount = max - min;
                    var weeks = Convert.ToInt32(periodCount.TotalDays / 7);
                    if (weeks == 0)
                    {
                        weeks++;
                    }
                    var sums = new List<List<double>>();
                    for (int i = 0; i < weeks; i++)
                    {
                        sums.Add(new List<double>());
                    }
                    var periods = new List<string>();

                    var tempDate = min.Date;
                    for (int i = 0; i < weeks; i++)
                    {
                        foreach (var supplier in suppliers)
                        {
                            var sum = 0.0;
                            if (supplier.ProductGroups != null)
                                foreach (var pg in supplier.ProductGroups)
                                    if (pg.Products != null)
                                        foreach (var product in pg.Products)
                                            if (product.TableProducts != null)
                                                foreach (var tp in product.TableProducts)
                                                    if (tp.Sale != null)
                                                        if (tp.Sale.Date >= tempDate && tp.Sale.Date < tempDate.AddDays(7))
                                                            sum += tp.Amount * product.Price;

                            sums[i].Add(sum);
                        }
                        string period = tempDate.Date.ToString("yyyy-MM-dd");
                        tempDate = tempDate.AddDays(7);
                        period += " - " + tempDate.Date.ToString("yyyy-MM-dd");
                        periods.Add(period);
                    }

                    var result = new List<List<string>>();
                    var headerList = new List<string>();
                    headerList.Add("");
                    for (int i = 0; i < suppliers.Count; i++)
                    {
                        headerList.Add(suppliers[i].Name);
                    }
                    result.Add(headerList);
                    for (int i = 0; i < weeks; i++)
                    {
                        var tempList = new List<string>();
                        tempList.Add(periods[i]);
                        for (int j = 0; j < sums[i].Count; j++)
                        {
                            tempList.Add(sums[i][j].ToString());
                        }
                        result.Add(tempList);
                    }

                    ViewBag.Result = result;
                    if (model.saveWord)
                    {
                        ExportLogic.CreateDocCross(new Logic.HelperModels.WordInfo
                        {
                            FileName = "Cross.docx",
                            Title = "Перекрестный Покупки/Поставищики"
                        }, result);
                    }
                }
            }
            catch (Exception) { }

            return View("SalesSum", model);
        }
    }
}