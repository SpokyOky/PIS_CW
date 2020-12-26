using Logic.Models;
using Logic.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.Logics
{
    public class SaleLogic
    {
        private Database context;

        public SaleLogic(Database _context)
        {
            context = _context;
        }

        public List<Sale> Read(DateViewModel model)
        {
            List<Sale> result = new List<Sale>();

            if (model != null)
            {
                result.AddRange(context.Sales
                    .Include(rec => rec.TableProducts)
                    .ThenInclude(rec => rec.Product)
                    .ThenInclude(rec => rec.ProductGroup)
                    .ThenInclude(rec => rec.Suppliers)
                    .Include(rec => rec.Client)
                    .Where(rec => rec.Date >= model.dateFrom 
                    && rec.Date <= model.dateTo)
                    .Select(rec => rec));
            }
            else
            {
                result.AddRange(context.Sales
                    .Include(rec => rec.TableProducts)
                    .ThenInclude(rec => rec.Product)
                    .Include(rec => rec.Client));
            }

            return result;
        }
    }
}
