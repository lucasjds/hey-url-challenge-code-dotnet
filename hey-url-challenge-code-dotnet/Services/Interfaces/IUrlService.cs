using hey_url_challenge_code_dotnet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hey_url_challenge_code_dotnet.Services.Interfaces
{
  public interface IUrlService
  {
    Url Create(Url url);
    Url FindByShortUrl(string shortUrl);
    IEnumerable<Url> FindAll();
    Url SaveVisit(string url);
  }
}
