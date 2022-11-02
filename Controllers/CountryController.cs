using GeoLocationAPI.DBHandler;
using GeoLocationAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Principal;

namespace GeoLocationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : SQLiteBaseAPI<Countries, CountryController>
    {
        #region Variables
        //ICommand<Country> _commandCreateHadler;
        //ICommand<Country> _commandUpdateHadler;
        readonly string _connectionString = string.Empty;
        private readonly ILogger<CountryController> _logger;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor to handle Country Data
        /// </summary>
        /// <param name="logger">MS Logger</param>
        /// <param name="configuration"></param>
        /// <param name="connectionService"></param>
        /// <param name="httpContext"></param>
        public CountryController(ILogger<CountryController> logger, IConfiguration configuration,
            IConnectionService connectionService, IHttpContextAccessor httpContext)
            : base(logger, configuration, connectionService, httpContext)
        {
            //If we need to write some business logic then only we need to create CreateHandler else we can use BaseAPI for CRUD.
            //_commandUpdateHadler = new ServiceTicketUpdateHandler(ConnectionString);
            _connectionString = configuration.GetValue<string>("DBSettings:ClientDB");
            _baseAccess = new SQLiteDataAccess<Countries>(_connectionString, new TraceDB());
            //For account alone we will use Account Database hence the logic was hotcoded.
            _logger = logger;
        }
        #endregion
        //Ref: https://github.com/dr5hn/countries-states-cities-database
    }
}
