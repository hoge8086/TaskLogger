using System.Collections.Generic;

namespace TaskLogger.Business.Domain.Model
{
    public interface IReportTargetRepository
    {
        List<ReportTarget> FindAll();
        void Add(ReportTarget reportTarget);
        void Remove(string title);
        //void Update(ReportTarget reportTarget);
        void Save();
        //TODO:そのうち消してちゃんとやる
        void Save(List<ReportTarget> targets);
    }
}
