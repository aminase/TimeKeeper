﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimeKeeper.API.Controllers;
using TimeKeeper.API.Helper;
using TimeKeeper.API.Models;
using TimeKeeper.DAL.Entities;
using TimeKeeper.DAL.Repository;

namespace TimeKeeper.Test
{
    [TestClass]
    public class EmployeeTest
    {
        UnitOfWork unit = new UnitOfWork();

        [TestInitialize]
        public void InitializeHttpContext()
        {
            HttpContext.Current = new HttpContext(
                new HttpRequest("", "http://tempuri.org", ""),
                new HttpResponse(new StringWriter())
            );
        }

        [TestMethod]
        public void EmployeeCheck()
        {
            int expected = 2;

            int numberOfEmployees = unit.Employees.Get().Count();

            Assert.AreEqual(expected, numberOfEmployees);
        }

        [TestMethod]
        public void EmployeeAdd()
        {
            Employee e = new Employee()
            {
                FirstName = "Helen",
                LastName = "Dale",
                BirthDate = new DateTime(1991, 05, 02),
                Email = "helen@gmail.com",
                Phone = "+387629598836",
                Salary = 45.5m,
                BeginDate = new DateTime(1989, 06, 03),
                Status = EmployeeStatus.Active
            };

            unit.Employees.Insert(e);

            Assert.IsTrue(unit.Save());
            Assert.IsNotNull(unit.Employees.Get());
        }

        [TestMethod]
        public void EmployeeUpdate()
        {
            Employee e = unit.Employees.Get().FirstOrDefault();
            string expected = "Helen";

            e.FirstName = "Helen";
            unit.Employees.Update(e, 1);

            Assert.IsTrue(unit.Save());
            Assert.AreEqual(expected, unit.Employees.Get().FirstOrDefault().FirstName);
        }

        [TestMethod]
        public void EmployeeDelete()
        {
            Employee e = unit.Employees.Get(3);

            unit.Employees.Delete(e);
            unit.Save();

            Assert.IsNull(unit.Employees.Get(3));
        }

        [TestMethod]
        public void EmployeeCheckValidity()
        {
            Employee e = new Employee();
            Employee e1 = unit.Employees.Get().FirstOrDefault();

            unit.Employees.Insert(e);
            e1.FirstName = "";

            Assert.IsFalse(unit.Save());
        }

        //Tests for controller
        [TestMethod]
        public void EmployeeControllerGet()
        {
            var controller = new EmployeesController();
            var h = new Header();

            var response = controller.Get(h);
            var result = (OkNegotiatedContentResult<List<EmployeeModel>>)response;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public void EmployeeControllerGetById()
        {
            var controller = new EmployeesController();

            var response = controller.Get(2);
            var result = (OkNegotiatedContentResult<EmployeeModel>)response;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);

        }

        [TestMethod]
        public void EmployeeControllerPost()
        {
            var controller = new EmployeesController();
            var mf = new ModelFactory();
            var e = new Employee()
            {
                FirstName = "Daria",
                LastName = "Eleska",
                BirthDate = new DateTime(1982, 05, 02),
                Email = "daria@gmail.com",
                Phone = "+38762959889",
                Salary = 85.5m,
                BeginDate = new DateTime(2018, 06, 03),
                Status = EmployeeStatus.Active
            };

            var response = controller.Post(mf.Create(e));
            var result = (OkNegotiatedContentResult<EmployeeModel>)response;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);

        }

        [TestMethod]
        public void EmployeeControllerPut()
        {
            var controller = new EmployeesController();
            var mf = new ModelFactory();
            var e = new Employee()
            {
                Id = 2,
                FirstName = "Annie",
                LastName = unit.Employees.Get(2).LastName,
                BirthDate = new DateTime(1988, 06, 03),
                Email = unit.Employees.Get(2).Email,
                Phone = "+38762123836",
                Salary = 65.5m,
                BeginDate = new DateTime(2017, 07, 03),
                Status = EmployeeStatus.Leaver
            };

            var response = controller.Put(mf.Create(e), 2);
            var result = (OkNegotiatedContentResult<EmployeeModel>)response;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public void EmployeeControllerDelete()
        {
            var controller = new EmployeesController();

            var response = controller.Delete(1);
            var result = (OkResult)response;

            Assert.IsNotNull(result);
        }
    }
}

