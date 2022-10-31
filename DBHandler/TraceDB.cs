using RepoDb;
using RepoDb.Enumerations;

namespace GeoLocationAPI.DBHandler
{
    public class TraceDB : BaseTrace
    {
        public override void BeforeBatchQuery(CancellableTraceLog log)
        {
            Console.WriteLine($"BeforeBatchQuery: {log.Statement}, TotalTime: {log.ExecutionTime.TotalSeconds} second(s)");
            //base.BeforeBatchQuery(log);
        }

        public override void AfterBatchQuery(TraceLog log)
        {
            Console.WriteLine($"AfterBatchQuery: {log.Statement}, TotalTime: {log.ExecutionTime.TotalSeconds} second(s)");

        }
    }
}
