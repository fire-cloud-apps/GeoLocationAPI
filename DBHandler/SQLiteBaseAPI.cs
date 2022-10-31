
using Microsoft.AspNetCore.Mvc;
using RepoDb;
using RepoDb.Enumerations;
using SqlKata;
using System.Diagnostics;

namespace GeoLocationAPI.DBHandler
{    
    /// <summary>
    /// Base API which is using Postgre-SQL as a database platform
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TController"></typeparam>
    public class SQLiteBaseAPI<TModel, TController> : ControllerBase where TModel : class
    {
        #region Variables
        readonly string _connectionString = string.Empty;
        protected ILogger<TController> _logger = null;
        protected IBaseAccess<TModel> _baseAccess = null;
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of ServiceStatus service
        /// </summary>
        /// <param name="logger">Log dependency injection</param>
        /// <param name="configuration">Configuration dependency injection</param>
        public SQLiteBaseAPI(ILogger<TController> logger, IConfiguration configuration,
                        IConnectionService connectionService, IHttpContextAccessor httpContext)
        {
            _connectionString = connectionService.GetDBConnection(configuration, httpContext);
            ConnectionString = _connectionString;
            _baseAccess = new SQLiteDataAccess<TModel>(_connectionString, new TraceDB());
            _logger = logger;
            _logger.LogInformation($"API {nameof(TController)} Initiated.");
        }

        public string ConnectionString { get; set; }


        public SQLiteBaseAPI() { }
        #endregion

        #region Basic Get Operations
        /// <summary>
        /// Gets all the items from TModel Table
        /// </summary>
        /// <returns>returns IEnumerable of Type Model</returns>
        [Route("getall")]
        [HttpGet]
        public async Task<IEnumerable<TModel>> GetAll()
        {
            IEnumerable<TModel> models = await _baseAccess.GetAllAsync();
            _logger.LogInformation($"GetAll API Executed Count : {models.Count()}");
            return models;
        }

        /// <summary>
        /// Gets By Id from Model Table
        /// </summary>
        /// <returns>returns IEnumerable of Model Model</returns>
        [Route("getbyId/{id}")]
        [HttpGet]
        public async Task<TModel> GetById(int id)
        {
            TModel model = await _baseAccess.GetByIdAsync(id);
            if (model != null)
            {
                _logger.LogInformation($"Get By ID API Executed : {model}");
            }
            return model;
        }

        /// <summary>
        /// Get the Model details by Batch and splits into Page.
        /// </summary>
        /// <param name="start">Page start value usually '0' </param>
        /// <param name="length">Page length value usually '10' </param>
        /// <param name="order">Ordering Ascending or Descending </param>
        /// <returns>returns no of Models</returns>
        [Route("GetByPage")]
        [HttpGet]
        public async Task<IEnumerable<TModel>> GetByPage(int start = 0, int length = 10, string order = "D")
        {
            Order idOrder = order.Equals("A") ? Order.Ascending : Order.Descending;
            var orderBy = OrderField.Parse(new { Id = idOrder });//Orders by 'Id';
            IEnumerable<TModel> models = await _baseAccess.GetByPagingAsync(orderBy, start, length);
            _logger.LogInformation($"GetByPage API Executed Count : {models.Count()}");
            return models;
        }

        [Route("GetByBatch")]
        [HttpPost]
        public async Task<MetaPagination<TModel>> GetByBatch
            (MetaPagingParams metaParams)
        {
            var metaPagination = new MetaPagination<TModel>();
            var dataList = await GetDataFromDB(metaParams);
            var count = await _baseAccess.GetRecordCountAsync();
            metaPagination.Total = count;
            metaPagination.TotalNotFiltered = count;
            metaPagination.Rows = dataList;
            return metaPagination;
        }
        private async Task<IEnumerable<TModel>> GetDataFromDB(MetaPagingParams metaParams)
        {
            metaParams.order = string.IsNullOrEmpty(metaParams.order) ? string.Empty : metaParams.order;

            var query = new Query(typeof(TModel).Name)
                .Limit(metaParams.limit)
                .Offset(metaParams.offset);
            query = metaParams.order.Equals("asc") ? query.OrderBy("Id") : query.OrderByDesc("Id");
            if (!string.IsNullOrEmpty(metaParams.search))
            {
                query.WhereRaw($"\"{metaParams.searchcolumn}\" ILIKE '%{metaParams.search}%'");
            }
            IEnumerable<TModel> taxData = await _baseAccess.ExecuteQuery(query);

            _logger.LogInformation($"{LogClassAndMethod()} API Executed Count : {taxData.Count()}");
            return taxData;
        }

        #endregion

        #region Basic CRUD Operation
        /// <summary>
        /// Create ServiceStatus based on the Model
        /// </summary>
        /// <param name="model">TModel model</param>
        /// <returns>returns ServiceStatus as json data</returns>
        [Route("create")]
        [HttpPost]
        public async Task<TModel> Create(TModel model)
        {
            TModel resultModel = await _baseAccess.CreateAsync(model);
            return resultModel;

        }
        /// <summary>
        /// Update ServiceStatus based on the Model
        /// </summary>
        /// <param name="model">ServiceStatus model</param>
        /// <returns>returns ServiceStatus as json data</returns>
        [Route("update")]
        [HttpPut]
        public async Task<TModel> Update(TModel model)
        {
            TModel resultModel = await _baseAccess.UpdateAsync(model);
            return resultModel;
        }

        /// <summary>
        /// Delete the ServiceStatus data by Id
        /// </summary>
        /// <param name="id">Unique Id of ServiceStatus</param>
        /// <returns>No of items removed.</returns>
        [Route("delete")]
        [HttpDelete]
        public async Task<ResultModel> Delete(int id)
        {
            int resultCount = await _baseAccess.DeleteAsync(id);

            return new ResultModel() { Id = id, Count = resultCount };
        }
        #endregion

        #region Basic Aggregation
        /// <summary>
        /// Gets all the items from TModel Table
        /// </summary>
        /// <returns>returns IEnumerable of Type Model</returns>
        [Route("count")]
        [HttpGet]
        public async Task<ResultModel> GetCount()
        {
            var recordCount = await _baseAccess.GetRecordCountAsync();
            ResultModel resultModel = new ResultModel()
            {
                Count = recordCount
            };
            _logger.LogInformation($"'GetCount' Result/Row Count : {resultModel.Count}");
            return resultModel;
        }
        #endregion

        #region Logging Extension
        /// <summary>
        /// Get Class Name and method for easy loging
        /// </summary>
        /// <returns>A string value with class name and method name</returns>
        public static string LogClassAndMethod()
        {
            var stackTrace = new StackTrace(true);
            string logMsg = string.Empty;
            foreach (var r in stackTrace.GetFrames())
            {
                logMsg = $"Filename: {r.GetFileName()} Method: {r.GetMethod()}"; break;
                //Line: r.GetFileLineNumber(),
                //Column: r.GetFileColumnNumber());
            }
            return logMsg;
        }
        #endregion
    }
}
