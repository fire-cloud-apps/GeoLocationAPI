namespace GeoLocationAPI.DBHandler;

/// <summary>
/// Used as a response data result to Client side.
/// </summary>
/// <typeparam name="TModel">Model Type</typeparam>
public class BatchResult<TModel>
{
    public IEnumerable<TModel> Items { get; set; }
    public long TotalItems { get; set; }
}