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
            var task = new TaskLog() { Start = DateTime.Parse("8:20"), End = DateTime.Parse("9:20") };
            Assert.AreEqual(60, task.WorkingMinutes);
        }
        [TestMethod()]
        public void WorkingMinutes0Test()
        {
            var task = new TaskLog() { Start = DateTime.Parse("8:20"), End = DateTime.Parse("8:20") };
            Assert.AreEqual(0, task.WorkingMinutes);
        }
        [TestMethod()]
        public void WorkingMinutes1DayOverTest()
        {
            var task = new TaskLog() { Start = DateTime.Parse("2000/1/1 8:20"), End = DateTime.Parse("2000/1/2 8:21") };
            Assert.AreEqual(24 * 60 + 1, task.WorkingMinutes);
        }
    }
}