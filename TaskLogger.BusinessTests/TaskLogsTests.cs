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
    public class TaskLogsTests
    {
        TaskLogs logs = new TaskLogs()
        {
            Logs = new List<TaskLog>()
            {
                new TaskLog(){TaskName="STEP1 設計 外仕査読", Start=DateTime.Parse("2000/1/1 8:20"), WorkingMinutes=60, DownTimeMinutes=10},    //60min
                new TaskLog(){TaskName="STEP1 設計 調査", Start=DateTime.Parse("2000/1/1 9:40"), WorkingMinutes=120, DownTimeMinutes=20},       //120min
                new TaskLog(){TaskName="STEP1 設計 検討", Start=DateTime.Parse("2000/1/1 13:00"), WorkingMinutes=215, DownTimeMinutes=30},      //215min
                new TaskLog(){TaskName="STEP1 設計 外仕査読", Start=DateTime.Parse("2000/1/1 17:30"), WorkingMinutes=35, DownTimeMinutes=10},  //35min
                new TaskLog(){TaskName="STEP1 設計 検討", Start=DateTime.Parse("2000/1/2 13:00"), WorkingMinutes=35, DownTimeMinutes=0},      //35min
                new TaskLog(){TaskName="STEP1 設計 ドキュメント作成", Start=DateTime.Parse("2000/1/2 14:20"), WorkingMinutes=135, DownTimeMinutes=30},  //135min
                new TaskLog(){TaskName="STEP1 設計 検討", Start=DateTime.Parse("2000/1/3 13:00"), WorkingMinutes=35, DownTimeMinutes=0},      //35min
            }
        };

        [TestMethod()]
        public void CreateReport_OneLog_Test()
        {
            var report = logs.CreateReport(new ReportTarget() {
                    Period = new DatePeriod { Date = DateTime.Parse("2000/1/1") },
                    TargetTasks = new List<TaskSearchMethod>()
                    {
                        new TaskSearchMethod(){ TaskKeyword="STEP1 設計 検討", searchMethod=TaskSearchMethod.Method.PerfectMatch}
                    }
                });
            Assert.AreEqual(1, report.Items.Count);
            Assert.AreEqual(215, report.FindItem("STEP1 設計 検討").TotalMinutes);
        }
        [TestMethod()]
        public void CreateReport_TwoLog_Test()
        {
            var report = logs.CreateReport(new ReportTarget() {
                    Period = new DatePeriod { Date = DateTime.Parse("2000/1/1") },
                    TargetTasks = new List<TaskSearchMethod>()
                    {
                        new TaskSearchMethod(){ TaskKeyword="STEP1 設計 外仕査読", searchMethod=TaskSearchMethod.Method.PerfectMatch}
                    }
                });
            Assert.AreEqual(1, report.Items.Count);
            Assert.AreEqual(95, report.FindItem("STEP1 設計 外仕査読").TotalMinutes);
        }
        [TestMethod()]
        public void CreateReport_2DaysPeriod_Test()
        {
            var report = logs.CreateReport(new ReportTarget() {
                    Period = new PartialPeriod{ StartDay= DateTime.Parse("2000/1/1"), EndDay=DateTime.Parse("2000/1/2")},
                    TargetTasks = new List<TaskSearchMethod>()
                    {
                        new TaskSearchMethod(){ TaskKeyword="STEP1 設計 検討", searchMethod=TaskSearchMethod.Method.PerfectMatch}
                    }
                });
            Assert.AreEqual(1, report.Items.Count);
            Assert.AreEqual(250, report.FindItem("STEP1 設計 検討").TotalMinutes);
        }
        [TestMethod()]
        public void CreateReport_WholePeriod_Test()
        {
            var report = logs.CreateReport(new ReportTarget() {
                    Period = new WholePeriod(),
                    TargetTasks = new List<TaskSearchMethod>()
                    {
                        new TaskSearchMethod(){ TaskKeyword="STEP1 設計 検討", searchMethod=TaskSearchMethod.Method.PerfectMatch}
                    }
                });
            Assert.AreEqual(1, report.Items.Count);
            Assert.AreEqual(285, report.FindItem("STEP1 設計 検討").TotalMinutes);
        }

        [TestMethod()]
        public void CreateReport_FirstMatch_Test()
        {
            var report = logs.CreateReport(new ReportTarget() {
                    Period = new WholePeriod(),
                    TargetTasks = new List<TaskSearchMethod>()
                    {
                        new TaskSearchMethod(){ TaskKeyword="STEP1", searchMethod=TaskSearchMethod.Method.FirstMatch}
                    }
                });
            Assert.AreEqual(1, report.Items.Count);
            Assert.AreEqual(635, report.FindItem("STEP1").TotalMinutes);
        }

        [TestMethod()]
        public void CreateReport_PartialMatch_Test()
        {
            var report = logs.CreateReport(new ReportTarget() {
                    Period = new WholePeriod(),
                    TargetTasks = new List<TaskSearchMethod>()
                    {
                        new TaskSearchMethod(){ TaskKeyword="調査", searchMethod=TaskSearchMethod.Method.PartialMatch}
                    }
                });
            Assert.AreEqual(1, report.Items.Count);
            Assert.AreEqual(120, report.FindItem("調査").TotalMinutes);
        }
        [TestMethod()]
        public void CreateReport_RegexpMatch_Test()
        {
            var report = logs.CreateReport(new ReportTarget() {
                    Period = new WholePeriod(),
                    TargetTasks = new List<TaskSearchMethod>()
                    {
                        new TaskSearchMethod(){ TaskKeyword="調査|外仕査読", searchMethod=TaskSearchMethod.Method.RegexpMatch}
                    }
                });
            Assert.AreEqual(1, report.Items.Count);
            Assert.AreEqual(215, report.FindItem("調査|外仕査読").TotalMinutes);
        }
        [TestMethod()]
        public void CreateReport_2Search_Test()
        {
            var report = logs.CreateReport(new ReportTarget() {
                    Period = new WholePeriod(),
                    TargetTasks = new List<TaskSearchMethod>()
                    {
                        new TaskSearchMethod(){ TaskKeyword="STEP1 設計 検討", searchMethod=TaskSearchMethod.Method.PerfectMatch},
                        new TaskSearchMethod(){ TaskKeyword="STEP1", searchMethod=TaskSearchMethod.Method.FirstMatch}
                    }
                });
            Assert.AreEqual(2, report.Items.Count);
            Assert.AreEqual(285, report.FindItem("STEP1 設計 検討").TotalMinutes);
            Assert.AreEqual(635, report.FindItem("STEP1").TotalMinutes);
        }
    }
}