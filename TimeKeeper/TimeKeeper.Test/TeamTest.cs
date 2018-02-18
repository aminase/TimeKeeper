﻿using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimeKeeper.DAL.Entities;
using TimeKeeper.DAL.Repository;

namespace TimeKeeper.Test
{
    [TestClass]
    public class TeamTest
    {
        UnitOfWork unit = new UnitOfWork();
        [TestMethod]
        public void CheckTeams()
        {
            int expected = 3;

            int numberOfTeams = unit.Teams.Get().Count();

            Assert.AreEqual(expected, numberOfTeams);
        }

        [TestMethod]
        public void AddTeam()
        {
            Team t = new Team()
            {
                Id = "OMG",
                Name="Omega",
                Description="Work on backend"
            };

            unit.Teams.Insert(t);

            Assert.IsTrue(unit.Save());
            Assert.IsNotNull(unit.Teams.Get("OMG"));
        }

        [TestMethod]
        public void UpdateTeam()
        {
            Team t = unit.Teams.Get().FirstOrDefault();
            string expected = "OM";

            t.Name = "OM";

            Assert.IsTrue(unit.Save());
            Assert.AreEqual(expected, unit.Teams.Get().FirstOrDefault().Name);
        }

        [TestMethod]
        public void DeleteTeam()
        {
            Team t = unit.Teams.Get("OMG");

            unit.Teams.Delete(t);
            unit.Save();

            Assert.IsNull(unit.Teams.Get("OMG"));
        }

        [TestMethod]
        public void CheckValidityForTeams()
        {
            Team t = new Team();
            Team t1 = unit.Teams.Get().FirstOrDefault();

            unit.Teams.Insert(t);
            t1.Name = "";

            Assert.IsFalse(unit.Save());
        }
    }
}