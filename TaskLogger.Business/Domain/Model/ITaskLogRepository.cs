using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskLogger.Business.Domain.Model
{
    public interface ITaskLogRepository
    {
        TaskLogs FindAll();
        TaskLog FindByID(int id);
        TaskLogs FindWithinPeriod(Period period);
        void Add(TaskLog taskLog);
        void Remove(TaskLog taskLog);
        void Update(TaskLog taskLog);
        void Save();
    }

    public class MockTaskLogRepository : ITaskLogRepository
    {

        public void Add(TaskLog taskLog)
        {
            throw new NotImplementedException();
        }

        public TaskLogs FindAll()
        {
            return new TaskLogs()
                {
                    Logs = new List<TaskLog>()
                    {
                        new TaskLog(){TaskName="STEP1 設計 外仕査読", Start=DateTime.Parse("2000/1/1 8:20"), End=DateTime.Parse("2000/1/1 9:30"), DownTimeMinutes=10},    //60min
                        new TaskLog(){TaskName="STEP1 設計 調査", Start=DateTime.Parse("2000/1/1 9:40"), End=DateTime.Parse("2000/1/1 12:00"), DownTimeMinutes=20},       //120min
                        new TaskLog(){TaskName="STEP1 設計 検討", Start=DateTime.Parse("2000/1/1 13:00"), End=DateTime.Parse("2000/1/1 17:05"), DownTimeMinutes=30},      //215min
                        new TaskLog(){TaskName="STEP1 設計 外仕査読", Start=DateTime.Parse("2000/1/1 17:30"), End=DateTime.Parse("2000/1/1 18:15"), DownTimeMinutes=10},  //35min
                        new TaskLog(){TaskName="STEP1 設計 検討", Start=DateTime.Parse("2000/1/2 13:00"), End=DateTime.Parse("2000/1/2 13:35"), DownTimeMinutes=0},      //35min
                        new TaskLog(){TaskName="STEP1 設計 ドキュメント作成", Start=DateTime.Parse("2000/1/2 14:20"), End=DateTime.Parse("2000/1/2 17:05"), DownTimeMinutes=30},  //135min
                        new TaskLog(){TaskName="STEP1 設計 検討", Start=DateTime.Parse("2000/1/3 13:00"), End=DateTime.Parse("2000/1/3 13:35"), DownTimeMinutes=0},      //35min
                        //new TaskLog(){TaskName="STEP1 設計 外仕査読", Start=DateTime.Parse("2000/1/1 8:20"), WorkingMinutes=60, DownTimeMinutes=10},    //60min
                        //new TaskLog(){TaskName="STEP1 設計 調査", Start=DateTime.Parse("2000/1/1 9:40"), WorkingMinutes=120, DownTimeMinutes=20},       //120min
                        //new TaskLog(){TaskName="STEP1 設計 検討", Start=DateTime.Parse("2000/1/1 13:00"), WorkingMinutes=215, DownTimeMinutes=30},      //215min
                        //new TaskLog(){TaskName="STEP1 設計 外仕査読", Start=DateTime.Parse("2000/1/1 17:30"), WorkingMinutes=35, DownTimeMinutes=10},  //35min
                        //new TaskLog(){TaskName="STEP1 設計 検討", Start=DateTime.Parse("2000/1/2 13:00"), WorkingMinutes=35, DownTimeMinutes=0},      //35min
                        //new TaskLog(){TaskName="STEP1 設計 ドキュメント作成", Start=DateTime.Parse("2000/1/2 14:20"), WorkingMinutes=135, DownTimeMinutes=30},  //135min
                        //new TaskLog(){TaskName="STEP1 設計 検討", Start=DateTime.Parse("2000/1/3 13:00"), WorkingMinutes=35, DownTimeMinutes=0},      //35min
                    }
                };
        }

        public TaskLog FindByID(int id)
        {
            throw new NotImplementedException();
        }

        public TaskLogs FindWithinPeriod(Period period)
        {
            throw new NotImplementedException();
        }

        public void Remove(TaskLog taskLog)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void Update(TaskLog taskLog)
        {
            throw new NotImplementedException();
        }
    }
}
