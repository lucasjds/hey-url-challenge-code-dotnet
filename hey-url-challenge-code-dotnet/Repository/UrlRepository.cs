using hey_url_challenge_code_dotnet.Models;
using hey_url_challenge_code_dotnet.Repository.Generic;
using hey_url_challenge_code_dotnet.Repository.Interfaces;
using HeyUrlChallengeCodeDotnet.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace hey_url_challenge_code_dotnet.Repository
{
  public class UrlRepository : BaseRepository<Url>, IUrlRepository
  {
    public UrlRepository(ApplicationContext context): base(context)
    {

    }

    public Url FindByShortUrl(string url)
    {
      return dataset.Include(x => x.VisitLogs).SingleOrDefault(x => x.ShortUrl.Equals(url));
    }
  }
}
