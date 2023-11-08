using System.Collections.Generic;
using System.Linq;
using System;

using NUnit.Framework;
using MMABooksEFClasses.MarisModels;
using Microsoft.EntityFrameworkCore;

namespace MMABooksTests
{
    [TestFixture]
    public class ProductTests
    {
        // ignore this warning about making dbContext nullable.
        // if you add the ?, you'll get a warning wherever you use dbContext
        MMABooksContext dbContext;
        Products? p;
        Invoicelineitems? il;
        List<Invoicelineitems> items;
        List<Products>? products;

        [SetUp]
        public void Setup()
        {
            dbContext = new MMABooksContext();
            dbContext.Database.ExecuteSqlRaw("call usp_testingResetData()");
        }

        [Test]
        public void GetAllTest()
        {
            //This asserts that when retrieving data, there should be a total of 16 results for the product code.
            products = dbContext.Products.OrderBy(p => p.ProductCode).ToList();
            Assert.AreEqual(16, products.Count);
            Assert.AreEqual("A4CS", products[0].ProductCode);
            PrintAll(products);
        }

        [Test]
        public void GetByPrimaryKeyTest()
        {
            //This code gets the product that has a code of A4VB
            //Assert.AreEqual("Ore", s.StateName) means that the code "A4VB" is equivalent to the product description of
            // "Murach's ASP.NET 4 Web Programming with VB 2010"
            p = dbContext.Products.Find("A4VB");
            Assert.IsNotNull(p);
            Assert.AreEqual("Murach's ASP.NET 4 Web Programming with VB 2010", p.Description);
            Console.WriteLine(p);
        }

        [Test]
        public void GetUsingWhere()
        {
            //Get a list of all of the products that have a unit price of 56.50
            //First line gets the code of all products that have a cost of 56.50; then outputting it to a list.
            //Based on the MYSQL query, I am asserting that there should be 7 products returned with a price of 56.5
            products = dbContext.Products.Where(p  => p.UnitPrice.Equals(56.5m)).OrderBy(p => p.ProductCode).ToList();
            Assert.AreEqual(7, products.Count);
            Assert.AreEqual("A4CS", products[0].ProductCode);
            PrintAll(products);
        }

        [Test]
        public void GetWithCalculatedFieldTest()
        {
            // get a list of objects that include the productcode, unitprice, quantity and inventoryvalue
            var products = dbContext.Products.Select(
            p => new { p.ProductCode, p.UnitPrice, p.OnHandQuantity, Value = p.UnitPrice * p.OnHandQuantity }).
            OrderBy(p => p.ProductCode).ToList();
            Assert.AreEqual(16, products.Count);
            foreach (var p in products)
            {
                Console.WriteLine(p);
            }
        }

        [Test]
        public void DeleteTest()
        {   //This deletes the Product with the description Murach's ASP.NET 4 Web Programming with C# 2010
            //Save Changes is necessary
            //Issue with the foreign key constraints
            p = dbContext.Products.Find("A4CS");
            dbContext.Products.Remove(p);
            dbContext.SaveChanges();
            Assert.IsNull(dbContext.Products.Find("A4CS"));
        }

        [Test]
        public void CreateTest()
        {

        }

        [Test]
        public void UpdateTest()
        {

        }

        public void PrintAll(List<Products> products)
        {
            foreach (Products p in products)
            {
                Console.WriteLine(p);
            }
        }

    }
}