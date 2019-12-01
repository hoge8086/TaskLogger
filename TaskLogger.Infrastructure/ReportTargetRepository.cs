using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using TaskLogger.Business.Domain.Model;

namespace TaskLogger.Infrastructure
{
    public class ReportTargetRepository : IReportTargetRepository
    {
        public class XmlData
        {
            public List<ReportTarget> ReportTargets { get; set; }
            public XmlData() { }

            public  List<ReportTarget> Clone()
            {
                var targets = new List<ReportTarget>();
                targets.AddRange(ReportTargets);
                return targets;
            }
        }

        private XmlData Data = null;

        public void Add(ReportTarget reportTarget)
        {
            var found = Data.ReportTargets.FindIndex(x => x.Title == reportTarget.Title);
            if (found >= 0)
                throw new Exception("Cannot use a title that already exists");

            Data.ReportTargets.Add(reportTarget);
        }

        public void Remove(string title)
        {
            var found = Data.ReportTargets.FindIndex(x => x.Title == title);
            if (found >= 0)
                Data.ReportTargets.RemoveAt(found);
        }

        public List<ReportTarget> FindAll()
        {
            return Load().Clone();
        }

        public void Save(List<ReportTarget> targets)
        {
            Data = new XmlData();
            Data.ReportTargets = targets;
            Save();
        }
        public void Save()
        {
            try
            {
                var serializer = new XmlSerializer(typeof(XmlData));
                using (var sw = new System.IO.StreamWriter(@"config.xml", false, new System.Text.UTF8Encoding(false)))
                {
                    serializer.Serialize(sw, Data);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("faild save config.xml");
            }
        }

        private XmlData Load()
        {
            if (Data != null)
                return Data;

            try
            {
                var serializer = new XmlSerializer(typeof(XmlData));
                using (var sr = new System.IO.StreamReader(@"config.xml", new System.Text.UTF8Encoding(false)))
                {
                    var Data = (XmlData)serializer.Deserialize(sr);
                    if (Data != null)
                        return Data;
                }
            }
            catch (Exception ex) { }

            Data = new XmlData();
            Data.ReportTargets = new List<ReportTarget>();
            Data.ReportTargets.Add(new ReportTargetAllTask("新規", new DatePeriod(DateTime.Today)));
            return Data;
        }

        //public void Update(ReportTarget reportTarget)
        //{
        //    throw new NotImplementedException();
        //}
    }

}
