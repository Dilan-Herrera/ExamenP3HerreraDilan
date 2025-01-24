using Newtonsoft.Json;

namespace ExamenP3HerreraDilan.Models;

public class Aeropuerto
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("country")]
    public string Country { get; set; }

    [JsonProperty("location")]
    public Location Location { get; set; }

    [JsonProperty("contact_info")]
    public ContactInfo ContactInfo { get; set; }
}

public class Location
{
    [JsonProperty("latitude")]
    public double Latitude { get; set; }

    [JsonProperty("longitude")]
    public double Longitude { get; set; }
}

public class ContactInfo
{
    [JsonProperty("email")]
    public string Email { get; set; }
}
