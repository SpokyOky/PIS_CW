using Logic;
using Logic.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Logic
{
    public class SampleData
    {
        public static void Initialize(Database context)
        {
            if (!context.Users.Any())
            {
                context.Users.Add(new User
                {
                    Login = "admin",
                    Password = "admin",
                    Role = "Маркетолог",
                    Name = "Иванов И.И."
                });
            }
            context.SaveChanges();

            if (!context.Divisions.Any())
            {
                context.Divisions.Add(new Division
                {
                    Name = "Основной отдел"
                });

                context.Divisions.Add(new Division
                {
                    Name = "Вспомогательный отдел"
                });
            }
            context.SaveChanges();

            if (!context.ProductGroups.Any())
            {
                context.ProductGroups.Add(new ProductGroup
                {
                    Name = "Группа 1",
                    Norm = 10
                });

                context.ProductGroups.Add(new ProductGroup
                {
                    Name = "Группа 2",
                    Norm = 15
                });
            }
            context.SaveChanges();

            if (!context.Clients.Any())
            {
                context.Clients.Add(new Client
                {
                    Name = "Клиент 1",
                    Type = "Тип 1"
                });
            }
            context.SaveChanges();

            if (!context.Products.Any())
            {
                var firstGroup = context.ProductGroups.FirstOrDefault();
                var lastGroup = context.ProductGroups.OrderBy(rec => rec.Id).LastOrDefault();

                var product1 = new Product
                {
                    Name = "Продукция 1",
                    AdvertData = "Реклама 1",
                    Price = 30,
                    Brand = "Марка 1",
                    Manufacturer = "Производитель 1",
                    City = "Город 1",
                    ProductGroup = firstGroup
                };
                context.Products.Add(product1);

                var product2 = new Product
                {
                    Name = "Продукция 2",
                    AdvertData = "Реклама 2",
                    Price = 40,
                    Brand = "Марка 2",
                    Manufacturer = "Производитель 2",
                    City = "Город 2",
                    ProductGroup = lastGroup
                };
                context.Products.Add(product2);

                var product3 = new Product
                {
                    Name = "Продукция 3",
                    AdvertData = "Реклама 2",
                    Price = 40,
                    Brand = "Марка 2",
                    Manufacturer = "Производитель 2",
                    City = "Город 2",
                    ProductGroup = lastGroup
                };
                context.Products.Add(product3);
                var product4 = new Product
                {
                    Name = "Продукция 4",
                    AdvertData = "Реклама 3",
                    Price = 40,
                    Brand = "Марка 3",
                    Manufacturer = "Производитель 1",
                    City = "Город 2",
                    ProductGroup = lastGroup
                };
                context.Products.Add(product4);

                var firstClient = context.Clients.FirstOrDefault();

                var tableProduct1 = new TableProduct
                {
                    Product = product1,
                    Amount = 10,
                    Sale = new Sale
                    {
                        Date = DateTime.Now.AddDays(-10),
                        Client = firstClient
                    }
                };
                context.TableProducts.Add(tableProduct1);
                var tableProduct2 = new TableProduct
                {
                    Product = product1,
                    Amount = 15,
                    Sale = new Sale
                    {
                        Date = DateTime.Now.AddDays(-9),
                        Client = firstClient
                    }
                };
                context.TableProducts.Add(tableProduct2);
                var tableProduct3 = new TableProduct
                {
                    Product = product2,
                    Amount = 5,
                    Sale = new Sale
                    {
                        Date = DateTime.Now.AddDays(-8),
                        Client = firstClient
                    }
                };
                context.TableProducts.Add(tableProduct3);
                var tableProduct4 = new TableProduct
                {
                    Product = product3,
                    Amount = 20,
                    Sale = new Sale
                    {
                        Date = DateTime.Now.AddDays(-7),
                        Client = firstClient
                    }
                };
                context.TableProducts.Add(tableProduct4);
                var tableProduct5 = new TableProduct
                {
                    Product = product4,
                    Amount = 10,
                    Sale = new Sale
                    {
                        Date = DateTime.Now.AddDays(-6),
                        Client = firstClient
                    }
                };
                context.TableProducts.Add(tableProduct5);
                var tableProduct6 = new TableProduct
                {
                    Product = product4,
                    Amount = 13,
                    Sale = new Sale
                    {
                        Date = DateTime.Now.AddDays(-5),
                        Client = firstClient
                    }
                };
                context.TableProducts.Add(tableProduct6);
                var tableProduct7 = new TableProduct
                {
                    Product = product4,
                    Amount = 18,
                    Sale = new Sale
                    {
                        Date = DateTime.Now.AddDays(-5),
                        Client = firstClient
                    }
                };
                context.TableProducts.Add(tableProduct7);
            }
            context.SaveChanges();

            if (!context.Suppliers.Any())
            {
                var supplier = new Supplier
                {
                    Name = "Поставщик 1",
                    INN = "123"
                };
                context.Suppliers.Add(supplier);

                var lastSupplier = new Supplier
                {
                    Name = "Поставщик 2",
                    INN = "456"
                };
                context.Suppliers.Add(lastSupplier);

                var firstGroup = context.ProductGroups.FirstOrDefault();
                var lastGroup = context.ProductGroups.OrderBy(rec => rec.Id).LastOrDefault();

                if (firstGroup != null)
                {
                    supplier.ProductGroups = new List<ProductGroup>();
                    supplier.ProductGroups.Add(firstGroup);

                    lastSupplier.ProductGroups = new List<ProductGroup>();
                    lastSupplier.ProductGroups.Add(lastGroup);
                }
            }

            context.SaveChanges();
        }
    }
}
