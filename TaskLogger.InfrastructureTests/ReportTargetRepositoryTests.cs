using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaskLogger.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskLogger.Business.Domain.Model;

namespace TaskLogger.Infrastructure.Tests
{
    [TestClass()]
    public class ReportTargetRepositoryTests
    {
        [TestMethod()]
        public void FindAllTest()
        {
            var rep = new ReportTargetRepository();
            var targets = rep.FindAll();
            targets.Add(new ReportTarget("ABC", new WholePeriod(), new AllTaskSpecify()));
            targets.Add(new ReportTarget("abc",  new DatePeriod(DateTime.Today), new IndividualTaskSpecify(new List<TaskSearchMethod>())));
            rep.Save();

        }
    }
}