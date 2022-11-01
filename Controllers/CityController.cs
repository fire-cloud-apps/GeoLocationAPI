using GeoLocationAPI.DBHandler;
using GeoLocationAPI.Model;
using Microsoft.AspNetCore.Mvc;

namespace GeoLocationAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CityController : SQLiteBaseAPI<Cities, CityController>
{
    #region Variables
    //ICommand<Country> _commandCreateHandler;
    //ICommand<Country> _commandUpdateHandler;
    readonly string _connectionString = string.Empty;
    private readonly ILogger<CityController> _logger;
    #endregion

    #region Constructor
    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="configuration"></param>
    /// <param name="connectionService"></param>
    /// <param name="httpContext"></param>
    public CityController(ILogger<CityController> logger, IConfiguration configuration,
        IConnectionService connectionService, IHttpContextAccessor httpContext)
        : base(logger, configuration, connectionService, httpContext)
    {
        //If we need to write some business logic then only we need to create CreateHandler else we can use BaseAPI for CRUD.
        //_commandUpdateHandler = new ServiceTicketUpdateHandler(ConnectionString);
        _connectionString = configuration.GetValue<string>("DBSettings:ClientDB");
        _baseAccess = new SQLiteDataAccess<Cities>(_connectionString, new TraceDB());
        //For account alone we will use Account Database hence the logic was hot-coded.
        _logger = logger;
    }
    #endregion
    //Ref: https://github.com/dr5hn/countries-states-cities-database
}