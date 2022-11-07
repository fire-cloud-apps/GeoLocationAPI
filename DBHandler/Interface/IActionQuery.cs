namespace GeoLocationAPI.DBHandler.Interface;

// <summary>
/// An Query execution with Error handler
/// </summary>
/// <typeparam name="TModel"></typeparam>
public interface IActionQuery<TModel> : IQuery<TModel>,  IError
{
    //Task<TModel> GetDetailHandlerAsync(TModel model);
}

public interface IQuery<TModel>
{
    string Message { get; set; }

    IEnumerable<TModel> GetHandler(TModel model);

    Task<IEnumerable<TModel>> GetHandlerAsync(TModel model);
}

public interface IError
{
    bool IsError { get; set; }
    string ErrorMessage { get; set; }
}