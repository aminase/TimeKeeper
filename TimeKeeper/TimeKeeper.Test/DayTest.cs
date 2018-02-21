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
    public class DayTest
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
        public void CheckAllDays()
        {
            int numberOfDays = unit.Calendar.Get().Count();

            int expected = 2;

            Assert.AreEqual(expected, numberOfDays);
        }

        [TestMethod]
        public void GetDayById()
        {
            Day d = unit.Calendar.Get(1);

            string expected = unit.Calendar.Get().FirstOrDefault().Date.ToString();

            Assert.AreEqual(expected, d.Date.ToString());
        }

        [TestMethod]
        public void AddDay()
        {
            Day d = new Day()
            {
                Date = DateTime.Today,
                Hours = 4,
                Type = DayType.WorkingDay,
                Employee = unit.Employees.Get(1)
            };

            unit.Calendar.Insert(d);

            Assert.IsTrue(unit.Save());
            Assert.IsNotNull(unit.Calendar.Get(3));
        }

        [TestMethod]
        public void UpdateDay()
        {
            Day d = unit.Calendar.Get(3);
            DateTime expected = new DateTime(2018, 1, 5);
            d.Date = new DateTime(2018, 1, 5);

            Assert.IsTrue(unit.Save());
            Assert.AreEqual(expected, unit.Calendar.Get(3).Date);
        }

        [TestMethod]
        public void DeleteDay()
        {
            Day d = unit.Calendar.Get(3);

            unit.Calendar.Delete(d);
            unit.Save();

            Assert.IsNull(unit.Calendar.Get(3));
        }

        [TestMethod]
        public void CheckValidityForDay()
        {
            Day d = new Day();

            unit.Calendar.Insert(d);

            Assert.IsFalse(unit.Save());
        }

        //Test for controller
        [TestMethod]
        public void ControllerGetAllDays()
        {
            var controller = new DaysController();
            var h = new Header();

            var response = controller.Get(h);
            var result = (OkNegotiatedContentResult<List<CalendarModel>>)response;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public void ControllerGetDayBy()
        {
            var controller = new DaysController();

            var response = controller.Get(1);
            var result = (OkNegotiatedContentResult<CalendarModel>)response;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public void ControllerPostDay()
        {
            var controller = new DaysController();
            Day d = new Day()
            {
                Date = DateTime.Today,
                Hours = 8,
                Type = DayType.SickLeave,
                //Employee = unit.Employees.Get(2)
            };

            var response = controller.Post(d);
            var result = (OkNegotiatedContentResult<Day>)response;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public void ControllerPutDay()
        {
            var controller = new DaysController();
            Day d = unit.Calendar.Get(4);
            d.Date = new DateTime(2018, 02, 16);

            var response = controller.Put(d,4);
            var result = (OkNegotiatedContentResult<Day>)response;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public void ControllerDeleteDay()
        {
            var controller = new DaysController();

            var response = controller.Delete(4);
            var result = (OkResult)response;

            Assert.IsNotNull(result);
        }
    }
}