using GeoLocationAPI.DBHandler;
using GeoLocationAPI.DBHandler.Interface;
using GeoLocationAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Handler = GeoLocationAPI.DBHandler.StateHandler;

namespace GeoLocationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StateController : SQLiteBaseAPI<States, StateController>
    {
        #region Variables
        //ICommand<Country> _commandCreateHandler;
        //ICommand<Country> _commandUpdateHandler;
        readonly string _connectionString = string.Empty;
        private readonly ILogger<StateController> _logger;
        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="configuration"></param>
        /// <param name="connectionService"></param>
        /// <param name="httpContext"></param>
        public StateController(ILogger<StateController> logger, IConfiguration configuration,
            IConnectionService connectionService, IHttpContextAccessor httpContext)
            : base(logger, configuration, connectionService, httpContext)
        {
            //If we need to write some business logic then only we need to create CreateHandler else we can use BaseAPI for CRUD.
            //_commandUpdateHandler = new ServiceTicketUpdateHandler(ConnectionString);
            _connectionString = configuration.GetValue<string>("DBSettings:ClientDB");
            _baseAccess = new SQLiteDataAccess<States>(_connectionString, new TraceDB());
            //For account alone we will use Account Database hence the logic was hot-coded.
            _logger = logger;
        }
        #endregion
        
        #region Get Details
        [Route("Details/{id}")]
        [HttpGet]
        public async Task<ActionResult<States>> GetDetails(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return  BadRequest();
            }

            return await GetById(int.Parse(id));
            //return await FilterById(m => m.Id == id);//
        }
        #endregion
        
        #region Get By Batch
        [Route("ByBatch")]
        [HttpPost]
        public async Task<ActionResult<IEnumerable<States>>> GetByPage(PageMetaData metaData)
        {
            IEnumerable<States> items;
            IActionQuery<States> query = new Handler.GetBatchHandler(_connectionString, _logger, metaData);
            items = await query.GetHandlerAsync(null);
            BatchResult<States> batchResult = new BatchResult<States>()
            {
                Items = items,
                TotalItems = GetCount().Result.Count
            };
            Console.WriteLine("Batch Executed");
    
            return Ok(batchResult);
        }
        #endregion
        
    }
}
