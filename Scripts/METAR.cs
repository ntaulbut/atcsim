using Flurl;
using Flurl.Http.Xml;
using Godot;
using System.Data;
using System.Threading.Tasks;
using System.Xml.Linq;

public class METAR
{
    private readonly string Station;
    public string RawText;
    public int Pressure;
    public float Temperature;
    public int WindDirection;
    public int WindSpeed;

    public static async Task<METAR> FromAWC(string icao)
    {
        var myClass = new METAR(icao);
        await myClass.PopulateFromAWC();
        return myClass;
    }

    private async Task PopulateFromAWC()
    {
        XDocument metarDocument = await "https://aviationweather.gov/api/data/metar"
            .SetQueryParams(new
            {
                format = "xml",
                hours = "1",
                ids = Station
            })
            .GetXDocumentAsync();
        XElement metar = metarDocument.Element("response").Element("data").Element("METAR");

        RawText = metar.Element("raw_text").Value;
        Pressure = Mathf.RoundToInt(metar.Element("altim_in_hg").Value.ToFloat() * 33.86389f);
        Temperature = metar.Element("temp_c").Value.ToFloat();
        WindDirection = metar.Element("wind_dir_degrees").Value.ToInt();
        WindSpeed = metar.Element("wind_speed_kt").Value.ToInt();

        GD.Print("Fetched METAR from AWC for " + Station);
    }

    private METAR(string station)
    {
        Station = station;
    }

    public async void Update()
    {
        await PopulateFromAWC();
    }
}
