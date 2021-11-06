using JsonApiDotNetCore.Resources.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;

namespace hey_url_challenge_code_dotnet.Models
{
  public class Url : BaseEntity
  {
    [Attr]
    public string ShortUrl { get; set; }
    [Attr, Url, Required]
    public string OriginalUrl { get; set; }
    [Attr]
    public int Count { get; set; }
    [Attr]
    public DateTime CreatedAt { get; set; }
    public ICollection<VisitLog> VisitLogs { get; set; }

    public string FormatDate()
    {
      CultureInfo culture = new CultureInfo("en-us");
      return CreatedAt.ToString("MMMM dd, yyyy", culture);
    }

    public string GenerateShortenUrl()
    {
      Random random = new Random();
      const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
      return new string(Enumerable.Repeat(letters, 5)
                                  .Select(x => x[random.Next(x.Length)]).ToArray());
    }

    public Dictionary<string, int> FilterListVisitByClickDates()
    {
      return VisitLogs?.GroupBy(x => x.ClickDate.ToShortDateString())
                       .Select(x => new { Name = x.Key, Count = x.Count() })
                       .ToDictionary(x => x.Name, y => y.Count);
    }

    public Dictionary<string, int> FilterListVisitByBrowses()
    {
      return VisitLogs?.GroupBy(x => x.Browse)
                       .Select(x => new { Name = x.Key, Count = x.Count() })
                       .ToDictionary(x => x.Name, y => y.Count);
    }

    public Dictionary<string, int> FilterListVisitByPlataforms()
    {
      return VisitLogs?.GroupBy(x => x.Plataform)
                       .Select(x => new { Name = x.Key, Count = x.Count() })
                       .ToDictionary(x => x.Name, y => y.Count);
    }
  }
}
