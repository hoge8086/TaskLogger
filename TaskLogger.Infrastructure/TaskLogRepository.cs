using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskLogger.Business.Domain.Model;

namespace TaskLogger.Infrastructure
{
    public class TaskLogContext : DbContext
    {
        public TaskLogContext() : base("taskLogs") { }
        public DbSet<TaskLog> TaskLogs { get; set; }
    }

    public class TaskLogRepository : ITaskLogRepository, IDisposable
    {
        private readonly TaskLogContext context;

        public TaskLogRepository(TaskLogContext context)
        {
            this.context = context;
        }
        public TaskLogs FindAll()
        {
            var logs = context.TaskLogs.ToList();
            return new TaskLogs() { Logs =logs };
        }

        public void Add(TaskLog taskLog)
        {
            context.TaskLogs.Add(taskLog);
        }

        public void Remove(TaskLog taskLog)
        {
            context.TaskLogs.Remove(taskLog);
        }

        public void Update(TaskLog taskLog)
        {
            context.Entry(taskLog).State = EntityState.Modified;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        #region IDisposable Support
        private bool disposedValue = false; // 重複する呼び出しを検出するには

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: マネージ状態を破棄します (マネージ オブジェクト)。
                    context.Dispose();
                }

                // TODO: アンマネージ リソース (アンマネージ オブジェクト) を解放し、下のファイナライザーをオーバーライドします。
                // TODO: 大きなフィールドを null に設定します。

                disposedValue = true;
            }
        }

        // TODO: 上の Dispose(bool disposing) にアンマネージ リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします。
        // ~TaskLogRepository() {
        //   // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
        //   Dispose(false);
        // }

        // このコードは、破棄可能なパターンを正しく実装できるように追加されました。
        public void Dispose()
        {
            // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
            Dispose(true);
            GC.SuppressFinalize(this);
            // TODO: 上のファイナライザーがオーバーライドされる場合は、次の行のコメントを解除してください。
            // GC.SuppressFinalize(this);
        }
        #endregion
    }

}
