
﻿using GeoLocationAPI.DBHandler;
 using GeoLocationAPI.DBHandler.Interface;
 using GeoLocationAPI.Model;
using Microsoft.AspNetCore.Mvc;
 using Handler = GeoLocationAPI.DBHandler.CityHandler;
 
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
   
    #region Get Details
    [Route("Details/{id}")]
    [HttpGet]
    public async Task<ActionResult<Cities>> GetDetails(string id)
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
    public async Task<ActionResult<IEnumerable<Cities>>> GetByPage(PageMetaData metaData)
    {
        IEnumerable<Cities> items;
        IActionQuery<Cities> query = new Handler.GetBatchHandler(_connectionString, _logger, metaData);
        items = await query.GetHandlerAsync(null);
        BatchResult<Cities> batchResult = new BatchResult<Cities>()
        {
            Items = items,
            TotalItems = GetCount().Result.Count
        };
        Console.WriteLine("Batch Executed");
    
        return Ok(batchResult);
    }
    #endregion
}