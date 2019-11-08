using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaskLogger.Business.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TaskLogger.Business.Tests
{
    [TestClass()]
    public class PartialPeriodTests
    {
        [TestMethod()]
        public void IsInTest()
        {
            var period = new PartialPeriod { StartDay = DateTime.Parse("2000/1/1"), EndDay = DateTime.Parse("2000/1/2") };
            Assert.AreEqual(true, period.IsIn(DateTime.Parse("2000/1/1 13:00")));
            Assert.AreEqual(true, period.IsIn(DateTime.Parse("2000/1/2 13:00")));
            Assert.AreEqual(true, period.IsIn(DateTime.Parse("2000/1/2 0:00")));
            Assert.AreEqual(false, period.IsIn(DateTime.Parse("2000/1/3 0:00")));
            
        }
    }
}