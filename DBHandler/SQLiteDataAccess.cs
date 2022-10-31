using RepoDb;
using SqlKata.Compilers;
using SqlKata;
using System.Data.SQLite;
using System.Linq.Expressions;
using RepoDb.Interfaces;

namespace GeoLocationAPI.DBHandler
{

    /// <summary>
    /// Use this class to access all the basic functions available in the SQLite
    /// </summary>
    /// <typeparam name="TModel">A Model/Entity type</typeparam>
    public class SQLiteDataAccess<TModel> : IBaseAccess<TModel> where TModel : class
    {
        string _conString = string.Empty;
        BaseTrace _baseTrace = null;
        SqliteCompiler _sqliteCompiler;

        #region Constructor
        public SQLiteDataAccess(string connectionString, BaseTrace baseTrace = null)
        {
            _conString = connectionString;
            _baseTrace = baseTrace;
            RepoDb.SQLiteBootstrap.Initialize();
            _sqliteCompiler = new SqliteCompiler();
        }
        #endregion

        #region IBaseAccess - Base Generic CRUD Operation 
        public TModel Create(TModel model)
        {
            using (var connection = new SQLiteConnection(_conString))
            {
                var id = connection.Insert(model);
            }
            return model;
        }
        public async Task<TModel> CreateAsync(TModel model)
        {
            using (var connection = new SQLiteConnection(_conString))
            {
                var id = await connection.InsertAsync(model);
            }
            return model;
        }
        public TModel Update(TModel model)
        {
            using (var connection = new SQLiteConnection(_conString))
            {
                var id = connection.Update<TModel>(model);
            }
            return model;
        }
        public async Task<TModel> UpdateAsync(TModel model)
        {
            using (var connection = new SQLiteConnection(_conString))
            {
                var id = await connection.UpdateAsync(model);
            }
            return model;
        }
        public int Delete(object id)
        {
            //string value = typeof(TModel).Name;
            int noOfRows = 0;
            using (var connection = new SQLiteConnection(_conString))
            {
                noOfRows = connection.Delete<TModel>(id);
            }
            return noOfRows;
        }
        public async Task<int> DeleteAsync(object id)
        {
            int noOfRows = 0;
            using (var connection = new SQLiteConnection(_conString))
            {
                noOfRows = await connection.DeleteAsync<TModel>(id);
            }
            return noOfRows;
        }
        #endregion

        #region GET Basic Operation
        public TModel GetById(object id)
        {
            TModel model;
            using (var connection = new SQLiteConnection(_conString))
            {
                model = connection.Query<TModel>(id).FirstOrDefault();
            }
            return model;
        }
        public async Task<TModel> GetByIdAsync(object id)
        {
            TModel model;
            using (var connection = new SQLiteConnection(_conString))
            {
                var result = await connection.QueryAsync<TModel>(id);
                model = result.FirstOrDefault();
            }
            return model;
        }
        public IEnumerable<TModel> GetAll()
        {
            IEnumerable<TModel> modelList = null;
            using (var connection = new SQLiteConnection(_conString))
            {
                modelList = connection.QueryAll<TModel>();
            }
            return modelList;
        }

        public async Task<IEnumerable<TModel>> GetAllAsync()
        {
            IEnumerable<TModel> modelList = null;
            using (var connection = new SQLiteConnection(_conString))
            {
                modelList = await connection.QueryAllAsync<TModel>();
            }
            return modelList;
        }

        #endregion

        #region Get By Condition
        public IEnumerable<TModel> GetByCondition(Expression condition)
        {
            IEnumerable<TModel> model = null;

            using (var connection = new SQLiteConnection(_conString))
            {
                model = connection.Query<TModel>(what: condition);
            }
            return model;
        }

        public async Task<IEnumerable<TModel>> GetByConditionAsync(Expression condition)
        {
            IEnumerable<TModel> model = null;

            using (var connection = new SQLiteConnection(_conString))
            {
                model = await connection.QueryAsync<TModel>(condition);
            }
            return model;
        }

        public IEnumerable<TModel> GetByCondition(QueryField[] whereCondition)
        {
            IEnumerable<TModel> model = null;
            //Ref: https://repodb.net/class/queryfield
            using (var connection = new SQLiteConnection(_conString))
            {
                model = connection.Query<TModel>(where: whereCondition);
            }
            return model;
        }
        public async Task<IEnumerable<TModel>> GetByConditionAsync(QueryField[] whereCondition)
        {
            IEnumerable<TModel> model = null;
            //Ref: https://repodb.net/class/queryfield
            using (var connection = new SQLiteConnection(_conString))
            {
                model = await connection.QueryAsync<TModel>(where: whereCondition);
            }
            return model;
        }

        #endregion

        #region Get Paging - Operation

        public IEnumerable<TModel> GetByPaging(IEnumerable<OrderField> orderBy, int page = 0, int rowsPerBatch = 10)
        {
            IEnumerable<TModel> modelList = null;
            using (var connection = new SQLiteConnection(_conString))
            {
                modelList = connection.BatchQuery<TModel>
                    (
                    page: page,
                    rowsPerBatch: rowsPerBatch,
                    orderBy: orderBy,
                    where: (object)null,//In this where is a object
                                        // if no where condition assign eg. where: (object)null
                    trace: _baseTrace
                    );
            }
            return modelList;
        }
        public IEnumerable<TModel> GetByPaging(IEnumerable<OrderField> orderBy, Expression<Func<TModel, bool>> filterCondition, int page = 0, int rowsPerBatch = 10)
        {
            IEnumerable<TModel> modelList = null;
            using (var connection = new SQLiteConnection(_conString))
            {
                modelList = connection.BatchQuery<TModel>
                       (
                       page: page,
                       rowsPerBatch: rowsPerBatch,
                       orderBy: orderBy,
                       where: filterCondition,//In this where is a expression.
                                              //eg. if we have where condition eg. where: e => e.Name == true
                       trace: _baseTrace
                       );
            }
            return modelList;
        }

        public async Task<IEnumerable<TModel>> GetByPagingAsync(IEnumerable<OrderField> orderBy, int page = 0, int rowsPerBatch = 10)
        {
            IEnumerable<TModel> modelList = null;
            using (var connection = new SQLiteConnection(_conString))
            {
                modelList = await connection.BatchQueryAsync<TModel>
                    (
                    page: page,
                    rowsPerBatch: rowsPerBatch,
                    orderBy: orderBy,
                    where: (object)null,//In this where is a object
                                        // if no where condition assign eg. where: (object)null
                    trace: _baseTrace
                    );
            }
            return modelList;
        }
        public async Task<IEnumerable<TModel>> GetByPagingAsync(IEnumerable<OrderField> orderBy, Expression<Func<TModel, bool>> filterCondition, int page = 0, int rowsPerBatch = 10)
        {
            IEnumerable<TModel> modelList = null;
            using (var connection = new SQLiteConnection(_conString))
            {
                modelList = await connection.BatchQueryAsync<TModel>
                       (
                       page: page,
                       rowsPerBatch: rowsPerBatch,
                       orderBy: orderBy,
                       where: filterCondition,//In this where is a expression.
                                              //eg. if we have where condition eg. where: e => e.Name == true

                       trace: _baseTrace
                       );
                //await connection.BatchQueryAsync<TModel>()
            }
            return modelList;
        }

        #endregion

        #region Basic Search
        public async Task<IEnumerable<TModel>> ExecuteQuery(Query query)
        {
            IEnumerable<TModel> model = null;
            try
            {
                SqlResult sqlResult = _sqliteCompiler.Compile(query);
                #region DEBUG
                Console.WriteLine($"ExecuteQuery : {sqlResult.ToString()}");
                #endregion
                using (var connection = new SQLiteConnection(_conString))
                {
                    model = await connection.ExecuteQueryAsync<TModel>(sqlResult.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return model;
        }
        public async Task<IEnumerable<TModel>> ExecuteQuery<T>(Query query) where T : class
        {
            IEnumerable<TModel> model = null;
            try
            {
                SqlResult sqlResult = _sqliteCompiler.Compile(query);
                #region DEBUG
                Console.WriteLine($"ExecuteQuery : {sqlResult.ToString()}");
                #endregion
                using (var connection = new SQLiteConnection(_conString))
                {
                    model = await connection.ExecuteQueryAsync<TModel>(sqlResult.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return model;
        }
        #endregion

        #region Scalar Operation
        public long GetRecordCount()
        {
            long noOfRecords = 0;
            using (var connection = new SQLiteConnection(_conString))
            {
                noOfRecords = connection.CountAll<TModel>();
            }
            return noOfRecords;
        }

        public async Task<long> GetRecordCountAsync()
        {
            long noOfRecords = 0;
            using (var connection = new SQLiteConnection(_conString))
            {
                noOfRecords = await connection.CountAllAsync<TModel>();
            }
            return noOfRecords;
        }
        #endregion

        #region IDangerExecution Implementation

        public async Task<long> TruncateAsync()
        {
            long noOfRecords = 0;
            using (var connection = new SQLiteConnection(_conString))
            {
                noOfRecords = await connection.TruncateAsync<TModel>();
            }
            return noOfRecords;
        }

        public int DeleteAll()
        {
            int noOfRowsDeleted = 0;
            using (var connection = new SQLiteConnection(_conString))
            {
                noOfRowsDeleted = connection.DeleteAll<TModel>();
            }
            return noOfRowsDeleted;
        }

        public int Truncate()
        {
            int noOfRowsTruncated = 0;
            using (var connection = new SQLiteConnection(_conString))
            {
                noOfRowsTruncated = connection.Truncate<TModel>();
            }
            return noOfRowsTruncated;
        }

        public bool CreteTable(string sqlQuery)
        {
            return ExecuteSQLQuery(sqlQuery);
        }

        public bool DeleteTable(string sqlQuery)
        {
            return ExecuteSQLQuery(sqlQuery);
        }

        private bool ExecuteSQLQuery(string sqlQuery)
        {
            bool result = false;
            using (var connection = new SQLiteConnection(_conString))
            {
                var value = connection.ExecuteNonQuery(sqlQuery);
                result = true;
            }
            return result;
        }
        #endregion
    }

    /// <summary>
    /// Trace class which captures traces in console, which can be extended to any trace implementation.
    /// </summary>
    public class BaseTrace : ITrace
    {
        #region After Query Execution
        public void AfterAverage(TraceLog log)
        {

        }

        public void AfterAverageAll(TraceLog log)
        {

        }

        public virtual void AfterBatchQuery(TraceLog log)
        {

        }

        public void AfterCount(TraceLog log)
        {

        }

        public void AfterCountAll(TraceLog log)
        {

        }

        public void AfterDelete(TraceLog log)
        {

        }

        public void AfterDeleteAll(TraceLog log)
        {

        }

        public void AfterExecuteNonQuery(TraceLog log)
        {

        }

        public void AfterExecuteQuery(TraceLog log)
        {

        }

        public void AfterExecuteReader(TraceLog log)
        {

        }

        public void AfterExecuteScalar(TraceLog log)
        {

        }

        public void AfterExists(TraceLog log)
        {

        }

        public void AfterInsert(TraceLog log)
        {

        }

        public void AfterInsertAll(TraceLog log)
        {

        }

        public void AfterMax(TraceLog log)
        {

        }

        public void AfterMaxAll(TraceLog log)
        {

        }

        public void AfterMerge(TraceLog log)
        {

        }

        public void AfterMergeAll(TraceLog log)
        {

        }

        public void AfterMin(TraceLog log)
        {

        }

        public void AfterMinAll(TraceLog log)
        {

        }

        public void AfterQuery(TraceLog log)
        {
            Console.WriteLine($"AfterQuery: {log.Statement}, TotalTime: {log.ExecutionTime.TotalSeconds} second(s)");
        }

        public void AfterQueryAll(TraceLog log)
        {

        }

        public void AfterQueryMultiple(TraceLog log)
        {

        }

        public void AfterSum(TraceLog log)
        {

        }

        public void AfterSumAll(TraceLog log)
        {

        }

        public void AfterTruncate(TraceLog log)
        {

        }

        public void AfterUpdate(TraceLog log)
        {
            Console.WriteLine($"AfterUpdate: {log.Statement}, TotalTime: {log.ExecutionTime.TotalSeconds} second(s)");
        }

        public void AfterUpdateAll(TraceLog log)
        {

        }

        #endregion

        #region Before Execution

        public void BeforeAverage(CancellableTraceLog log)
        {

        }

        public void BeforeAverageAll(CancellableTraceLog log)
        {

        }

        public virtual void BeforeBatchQuery(CancellableTraceLog log)
        {
            Console.WriteLine($"BeforeBatchQuery: {log.Statement}, TotalTime: {log.ExecutionTime.TotalSeconds} second(s)");
        }

        public void BeforeCount(CancellableTraceLog log)
        {

        }

        public void BeforeCountAll(CancellableTraceLog log)
        {

        }

        public void BeforeDelete(CancellableTraceLog log)
        {

        }

        public void BeforeDeleteAll(CancellableTraceLog log)
        {

        }

        public void BeforeExecuteNonQuery(CancellableTraceLog log)
        {

        }

        public void BeforeExecuteQuery(CancellableTraceLog log)
        {
            Console.WriteLine($"BeforeExecuteQuery: {log.Statement}, TotalTime: {log.ExecutionTime.TotalSeconds} second(s)");
        }

        public void BeforeExecuteReader(CancellableTraceLog log)
        {

        }

        public void BeforeExecuteScalar(CancellableTraceLog log)
        {

        }

        public void BeforeExists(CancellableTraceLog log)
        {

        }

        public void BeforeInsert(CancellableTraceLog log)
        {

        }

        public void BeforeInsertAll(CancellableTraceLog log)
        {

        }

        public void BeforeMax(CancellableTraceLog log)
        {

        }

        public void BeforeMaxAll(CancellableTraceLog log)
        {

        }

        public void BeforeMerge(CancellableTraceLog log)
        {

        }

        public void BeforeMergeAll(CancellableTraceLog log)
        {

        }

        public void BeforeMin(CancellableTraceLog log)
        {

        }

        public void BeforeMinAll(CancellableTraceLog log)
        {

        }

        public void BeforeQuery(CancellableTraceLog log)
        {
            Console.WriteLine($"BeforeQuery: {log.Statement}, TotalTime: {log.ExecutionTime.TotalSeconds} second(s)");
        }

        public void BeforeQueryAll(CancellableTraceLog log)
        {
            Console.WriteLine($"BeforeQueryAll: {log.Statement}, TotalTime: {log.ExecutionTime.TotalSeconds} second(s)");
        }

        public virtual void BeforeQueryMultiple(CancellableTraceLog log)
        {
            Console.WriteLine($"BeforeQueryMultiple: {log.Statement}, TotalTime: {log.ExecutionTime.TotalSeconds} second(s)");
        }

        public void BeforeSum(CancellableTraceLog log)
        {

        }

        public void BeforeSumAll(CancellableTraceLog log)
        {

        }

        public void BeforeTruncate(CancellableTraceLog log)
        {

        }

        public void BeforeUpdate(CancellableTraceLog log)
        {

        }

        public void BeforeUpdateAll(CancellableTraceLog log)
        {

        }
        #endregion
    }
}
