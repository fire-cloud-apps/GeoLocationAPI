using Newtonsoft.Json;

namespace GeoLocationAPI.Model
{    
    // Root myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(myJsonResponse);
    public class Countries
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Iso3 { get; set; }
        public string Iso2 { get; set; }
        public int Numeric_Code { get; set; }
        public string phone_code { get; set; }
        public string capital { get; set; }
        public string currency { get; set; }
        public string currency_name { get; set; }
        public string currency_symbol { get; set; }
        public string tld { get; set; }
        public string native { get; set; }
        public string region { get; set; }
        public string subregion { get; set; }
        public string timezones { get; set; }
        //public List<Timezone> timezones { get; set; }
        //public Translations translations { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string emoji { get; set; }
        public string emojiU { get; set; }
    }

    public class Timezone
    {
        public string zoneName { get; set; }
        public int gmtOffset { get; set; }
        public string gmtOffsetName { get; set; }
        public string abbreviation { get; set; }
        public string tzName { get; set; }
    }

    public class Translations
    {
        public string kr { get; set; }

        [JsonProperty("pt-BR")]
        public string PtBR { get; set; }
        public string pt { get; set; }
        public string nl { get; set; }
        public string hr { get; set; }
        public string fa { get; set; }
        public string de { get; set; }
        public string es { get; set; }
        public string fr { get; set; }
        public string ja { get; set; }
        public string it { get; set; }
        public string cn { get; set; }
        public string tr { get; set; }
    }


}
