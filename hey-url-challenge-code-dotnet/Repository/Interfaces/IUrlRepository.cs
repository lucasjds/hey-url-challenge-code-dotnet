using hey_url_challenge_code_dotnet.Models;
using hey_url_challenge_code_dotnet.Repository.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hey_url_challenge_code_dotnet.Repository.Interfaces
{
  public interface IUrlRepository : IBaseRepository<Url>
  {
    Url FindByShortUrl(string url);
  }
}
