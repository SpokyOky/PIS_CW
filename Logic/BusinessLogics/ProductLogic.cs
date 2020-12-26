using Logic.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.Logics
{
    public class ProductLogic
    {
        private Database context;

        public ProductLogic(Database _context)
        {
            context = _context;
        }

        public void CreateOrUpdate(Product product)
        {
            Product tempProduct = product.Id.HasValue ? null : new Product();

            if (product.Id.HasValue)
            {
                tempProduct = context.Products.FirstOrDefault(rec => rec.Id == product.Id);
            }

            if (product.Id.HasValue)
            {
                if (tempProduct == null)
                {
                    throw new Exception("Элемент не найден");
                }

                tempProduct.Id = product.Id;
                tempProduct.Name = product.Name;
                tempProduct.Brand = product.Brand;
                tempProduct.Manufacturer = product.Manufacturer;
                tempProduct.AdvertData = product.AdvertData;
                tempProduct.City = product.City;
                tempProduct.ProductGroup = product.ProductGroup;
            }
            else
            {
                context.Products.Add(product);
            }

            context.SaveChanges();
        }

        public void Delete(Product product)
        {
            Product element = context.Products.FirstOrDefault(rec => rec.Id == product.Id.Value);

            if (element != null)
            {
                context.Products.Remove(element);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }

        public List<Product> Read(Product product)
        {
            List<Product> result = new List<Product>();

            if (product != null)
            {
                result.AddRange(context.Products
                    .Include(rec => rec.ProductGroup)
                    .Include(rec => rec.TableProducts).ThenInclude(rec => rec.Sale)
                    .Where(rec => (rec.Id == product.Id) || (rec.City == product.City))
                    .Select(rec => rec));
            }
            else
            {
                result.AddRange(context.Products
                    .Include(rec => rec.ProductGroup)
                    .Include(rec => rec.TableProducts).ThenInclude(rec => rec.Sale));
            }

            return result;
        }

        public void RevaluateProduct(Product product, double newPrice)
        {
            var products = context.Products
                .Where(rec => rec.Id == product.Id)
                .ToList();

            foreach (var _product in products)
            {
                _product.Price = newPrice;
            }

            context.SaveChanges();
        }
    }
}
