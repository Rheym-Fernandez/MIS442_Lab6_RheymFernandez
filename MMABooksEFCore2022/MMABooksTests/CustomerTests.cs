using System.Collections.Generic;
using System.Linq;
using System;

using NUnit.Framework;
using MMABooksEFClasses.MarisModels;
using Microsoft.EntityFrameworkCore;
//using MMABooksEFClasses.Models;

namespace MMABooksTests
{
    [TestFixture]
    public class CustomerTests
    {
        
        MMABooksContext dbContext;
        Customer? c;
        List<Customer>? customers;

        [SetUp]
        public void Setup()
        {
            dbContext = new MMABooksContext();
            dbContext.Database.ExecuteSqlRaw("call usp_testingResetData()");
        }

        [Test]
        public void GetAllTest()
        {
            //This asserts that when retrieving data, there should be a total of 696 results for the customer id.
            customers = dbContext.Customers.OrderBy(c => c.CustomerId).ToList();
            Assert.AreEqual(696, customers.Count);
            Assert.AreEqual(1, customers[0].CustomerId);
            PrintAll(customers);
        }

        [Test]
        public void GetByPrimaryKeyTest()
        {
            //This code gets the Customer with the ID of 1 
            //Assert.AreEqual("Molunguri, A", c.Name)) means that the ID should be Molunguri, A
            c = dbContext.Customers.Find(1);
            Assert.IsNotNull(c);
            Assert.AreEqual("Molunguri, A", c.Name);
            Console.WriteLine(c);
        }


        [Test]
        public void GetUsingWhere()
        {
            //Get a list of all of the customers who live in OR
            customers = dbContext.Customers.Where(c => c.StateCode.StartsWith("OR")).OrderBy(c => c.CustomerId).ToList();
            Assert.AreEqual(5, customers.Count);
            Assert.AreEqual("Swenson, Vi", customers[0].Name);
            PrintAll(customers);

        }

        [Test]
        public void GetWithInvoicesTest()
        {
            //Get the customer whose id is 20 and all of the invoices for that customer
            c = dbContext.Customers.Include("Invoices").Where(c => c.CustomerId == 20).SingleOrDefault();
            Assert.IsNotNull(c);
            Assert.AreEqual("Chamberland, Sarah", c.Name);
            Assert.AreEqual(3, c.Invoices.Count);
            Console.WriteLine(c);
        }

        [Test]
        public void GetWithJoinTest()
        {
            //Get a list of objects that include the customer id, name, statecode and statename
            var customers = dbContext.Customers.Join(
               dbContext.States,
               c => c.StateCode,
               s => s.StateCode,
               (c, s) => new { c.CustomerId, c.Name, c.StateCode, s.StateName }).OrderBy(r => r.StateName).ToList();
            Assert.AreEqual(696, customers.Count);
            // I wouldn't normally print here but this lets you see what each object looks like
            foreach (var c in customers)
            {
                Console.WriteLine(c);
            }
        }

        [Test]
        public void DeleteTest()
        {
            //First finds the CustomerId of 1 then removes it from the Customers table
            c = dbContext.Customers.Find(1);
            dbContext.Customers.Remove(c);
            dbContext.SaveChanges();
            Assert.IsNull(dbContext.Customers.Find(1));

        }

        [Test]
        public void CreateTest()
        {
            //THIS IS GIVING ME A DUPLICARE ENTRY ERROR.
            c = new Customer();
            c.CustomerId = 1;
            c.Name = "Molunguri, A";
            c.Address = "Test Address";
            c.City = "Portland";
            c.State = dbContext.States.Find("OR");
            c.ZipCode = "123456";
            dbContext.Customers.Add(c);
            dbContext.SaveChanges();
            Assert.IsNotNull(dbContext.Customers.Find("Molunguri, A"));
        }

        [Test]
        public void UpdateTest()
        {
            c = dbContext.Customers.Find(2);
            c.Address = "Test";
            dbContext.Customers.Update(c);
            dbContext.SaveChanges();
            c = dbContext.Customers.Find(2);
            Assert.AreEqual("Test", c.Address);
        }

        public void PrintAll(List<Customer> customers)
        {
            foreach (Customer c in customers)
            {
                Console.WriteLine(c);
            }
        }
        
    }
}