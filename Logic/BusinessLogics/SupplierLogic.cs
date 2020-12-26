using Logic.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.Logics
{
    public class SupplierLogic
    {
        private Database context;

        public SupplierLogic(Database _context)
        {
            context = _context;
        }

        public void CreateOrUpdate(Supplier supplier)
        {
            Supplier tempSupplier = supplier.Id.HasValue ? null : new Supplier();

            if (supplier.Id.HasValue)
            {
                tempSupplier = context.Suppliers.FirstOrDefault(rec => rec.Id == supplier.Id);
            }

            if (supplier.Id.HasValue)
            {
                if (tempSupplier == null)
                {
                    throw new Exception("Элемент не найден");
                }

                tempSupplier.Id = supplier.Id;
                tempSupplier.Name = supplier.Name;
                tempSupplier.INN = supplier.INN;
                tempSupplier.ProductGroups = supplier.ProductGroups;
                tempSupplier.Consignments = supplier.Consignments;
            }
            else
            {
                context.Suppliers.Add(supplier);
            }

            context.SaveChanges();
        }

        public void Delete(Supplier supplier)
        {
            Supplier element = context.Suppliers.FirstOrDefault(rec => rec.Id == supplier.Id.Value);

            if (element != null)
            {
                context.Suppliers.Remove(element);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }

        public List<Supplier> Read(Supplier supplier)
        {
            List<Supplier> result = new List<Supplier>();

            if (supplier != null)
            {
                result.AddRange(context.Suppliers
                    .Include(rec => rec.ProductGroups)
                    .ThenInclude(rec => rec.Products)
                    .Where(rec => (rec.Id == supplier.Id) 
                    || (rec.Name == supplier.Name) 
                    || (rec.INN == supplier.INN))
                    .Select(rec => rec));
            }
            else
            {
                result.AddRange(context.Suppliers
                    .Include(rec => rec.ProductGroups)
                    .ThenInclude(rec => rec.Products)
                    .ThenInclude(rec => rec.TableProducts)
                    .ThenInclude(rec => rec.Sale));
            }

            return result;
        }
    }
}
