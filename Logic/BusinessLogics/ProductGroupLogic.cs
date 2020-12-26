using Logic.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.Logics
{
    public class ProductGroupLogic
    {
        private Database context;

        public ProductGroupLogic(Database _context)
        {
            context = _context;
        }

        public void CreateOrUpdate(ProductGroup productGroup)
        {
            ProductGroup tempProductGroup = productGroup.Id.HasValue ? null : new ProductGroup();

            if (productGroup.Id.HasValue)
            {
                tempProductGroup = context.ProductGroups.FirstOrDefault(rec => rec.Id == productGroup.Id);
            }

            if (productGroup.Id.HasValue)
            {
                if (tempProductGroup == null)
                {
                    throw new Exception("Элемент не найден");
                }

                tempProductGroup.Id = productGroup.Id;
                tempProductGroup.Name = productGroup.Name;
                tempProductGroup.Norm = productGroup.Norm;
            }
            else
            {
                context.ProductGroups.Add(productGroup);
            }

            context.SaveChanges();
        }

        public void Delete(ProductGroup productGroup)
        {
            ProductGroup element = context.ProductGroups.FirstOrDefault(rec => rec.Id == productGroup.Id.Value);

            if (element != null)
            {
                context.ProductGroups.Remove(element);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }

        public List<ProductGroup> Read(ProductGroup productGroup)
        {
            List<ProductGroup> result = new List<ProductGroup>();

            if (productGroup != null)
            {
                result.AddRange(context.ProductGroups
                    .Include(rec => rec.Suppliers)
                    .Include(rec => rec.Products)
                        .ThenInclude(rec => rec.TableProducts)
                    .Where(rec => rec.Id == productGroup.Id)
                    .Select(rec => rec));
            }
            else
            {
                result.AddRange(context.ProductGroups
                    .Include(rec => rec.Suppliers)
                    .Include(rec => rec.Products)
                        .ThenInclude(rec => rec.TableProducts));
            }

            return result;
        }

        public void RevaluateGroup(ProductGroup productGroup, double multiplyPrice)
        {
            var products = context.Products
                .Include(prod => prod.ProductGroup)
                .Where(rec => rec.ProductGroup.Id == productGroup.Id)
                .ToList();

            foreach (var _product in products)
            {
                _product.Price *= multiplyPrice;
            }

            context.SaveChanges();
        }
    }
}
