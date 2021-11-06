using hey_url_challenge_code_dotnet.Models;
using hey_url_challenge_code_dotnet.Repository.Generic;
using hey_url_challenge_code_dotnet.Repository.Interfaces;
using HeyUrlChallengeCodeDotnet.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hey_url_challenge_code_dotnet.Repository.UnitOfWork
{
  public class UnitOfWork : IUnitOfWork
  {
    public IBaseRepository<VisitLog> _visitLogRepository;
    public IUrlRepository _urlRepository;
    private ApplicationContext _context;

    public UnitOfWork(ApplicationContext context, IUrlRepository urlRepository, IBaseRepository<VisitLog> visitLogRepository)
    {
      _visitLogRepository = visitLogRepository;
      _context = context;
      _urlRepository = urlRepository;

    }

    public IBaseRepository<VisitLog> VisitLogRepository
    {
      get
      {
        if(_visitLogRepository == null)
        {
          _visitLogRepository = new BaseRepository<VisitLog>(_context);
        }
        return _visitLogRepository;
      }
    }

    public IUrlRepository UrlRepository
    {
      get
      {
        if (_urlRepository == null)
        {
          _urlRepository = new UrlRepository(_context);
        }
        return _urlRepository;
      }
    }

    public Url SaveVisit(Url urlModel, VisitLog visitLog)
    {
      UrlRepository.Update(urlModel);
      VisitLogRepository.Create(visitLog);
      return UrlRepository.FindByShortUrl(urlModel.ShortUrl);
    }
  }
}
