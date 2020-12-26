using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using FusionCharts.DataEngine;
using FusionCharts.Visualization;
using Logic.Logics;
using Logic.Models;
using Logic.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class OperationSalesAnalysisController : Controller
    {
        private ProductGroupLogic _productGroupLogic;
        private SaleLogic _saleLogic;
        private ProductLogic _productLogic;

        public OperationSalesAnalysisController(ProductGroupLogic productGroupLogic, SaleLogic saleLogic, ProductLogic productLogic)
        {
            _productGroupLogic = productGroupLogic;
            _saleLogic = saleLogic;
            _productLogic = productLogic;
        }

        public IActionResult Index()
        {
            return View("Index");
        }

        [HttpPost]
        public IActionResult Index(DateViewModel model)
        {
            if (model.dateFrom != null)
            {
                model.dateTo = model.dateFrom.Date.AddDays(7);
                model.dateFrom = model.dateFrom.Date;

                var sales = _saleLogic.Read(model);
                var productsList = new List<(Sale, IEnumerable<IGrouping<string, Product>>)>();
                foreach (var sale in sales)
                {
                    var products = new List<Product>();
                    foreach (var tp in sale.TableProducts)
                    {
                        products.Add(tp.Product);
                    }
                    productsList.Add((sale, products.GroupBy(pr => pr.ProductGroup.Name)));
                }
                ViewBag.ProductsList = productsList;
                if (model.saveWord)
                {
                    ExportLogic.CreateDocAnalysis(new Logic.HelperModels.WordInfo
                    {
                        FileName = "Analysis.docx",
                        Title = "Ведомость продаж"
                    }, productsList);
                }
            }
            return View("Index", model);
        }

        public IActionResult Diagram()
        {
            DataTable ChartData = new DataTable();

            ChartData.Columns.Add("Стоимость", typeof(double));
            ChartData.Columns.Add("Спрос", typeof(int));

            var sales = _saleLogic.Read(null);
            var products = _productLogic.Read(null);
            var spros = new List<int>();
            var prices = new List<double>();
            if (products != null)
            {
                foreach (var product in products)
                {
                    prices.Add(product.Price);
                    spros.Add(0);
                }
            }
           
            if (sales != null)
            {
                foreach (var sale in sales)
                {
                    if (sale.TableProducts != null)
                    {
                        foreach (var tp in sale.TableProducts)
                        {
                            foreach (var product in products)
                            {
                                if (tp.Product == product)
                                {
                                    spros[products.IndexOf(product)] += tp.Amount;
                                }
                            }
                        }
                    }
                }
            }
            

            for (int i = 0; i < products.Count(); i++)
            {
                ChartData.Rows.Add(prices[i], spros[i]);
            }

            StaticSource source = new StaticSource(ChartData);
            DataModel model = new DataModel();
            model.DataSources.Add(source);
            Charts.ColumnChart column = new Charts.ColumnChart("first_chart");
            column.Width.Pixel(700);
            column.Height.Pixel(400);
            column.Data.Source = model;
            column.Caption.Text = "Диаграмма цена/спрос";
            column.Legend.Show = false;
            column.XAxis.Text = "Стоимость";
            column.YAxis.Text = "Спрос";
            column.ThemeName = FusionChartsTheme.ThemeName.FUSION;

            return View("Diagram", column.Render());
        }
    }
}