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
    public class TaskLogTests
    {
        [TestMethod()]
        public void WorkingMinutes60Test()
        {
            var task = new TaskLog() { Start = DateTime.Parse("8:20"), WorkingMinutes=60};
            Assert.AreEqual(DateTime.Parse("9:20"), task.End);
        }
        [TestMethod()]
        public void WorkingMinutes0Test()
        {
            var task = new TaskLog() { Start = DateTime.Parse("8:20"), WorkingMinutes=0};
            Assert.AreEqual(DateTime.Parse("8:20"), task.End);
        }
        [TestMethod()]
        public void WorkingMinutes1DayOverTest()
        {
            var task = new TaskLog() { Start = DateTime.Parse("2000/1/1 8:20"), WorkingMinutes=24 * 60 + 1};
            Assert.AreEqual(DateTime.Parse("2000/1/2 8:21"), task.End);
        }
    }
}