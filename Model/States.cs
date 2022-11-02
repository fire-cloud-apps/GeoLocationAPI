namespace GeoLocationAPI.Model;

public class States
{
    public int id { get; set; }
    public string Name { get; set; }
    public int country_id { get; set; }
    public string country_code { get; set; }
    public string country_name { get; set; }
    public string state_code { get; set; }
    public object type { get; set; }
    public double latitude { get; set; }
    public double longitude { get; set; }
}
