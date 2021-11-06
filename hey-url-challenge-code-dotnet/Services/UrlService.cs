using hey_url_challenge_code_dotnet.Models;
using hey_url_challenge_code_dotnet.Repository.Interfaces;
using hey_url_challenge_code_dotnet.Services.Interfaces;
using Shyjus.BrowserDetection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hey_url_challenge_code_dotnet.Services
{
  public class UrlService : IUrlService
  {
    private readonly IUrlRepository _repository;
    private readonly IBrowserDetector _browserDetector;
    private readonly IUnitOfWork _unitOfWork;

    public UrlService(IUrlRepository repository, IBrowserDetector browserDetector, IUnitOfWork unitOfWork)
    {
      _repository = repository;
      _browserDetector = browserDetector;
      _unitOfWork = unitOfWork;
    }

    public Url Create(Url url)
    {
      url.ShortUrl = url.GenerateShortenUrl();
      while (_repository.FindByShortUrl(url.ShortUrl) != null)
      {
        url.ShortUrl = url.GenerateShortenUrl();
      }
      url.CreatedAt = DateTime.UtcNow;
      return _repository.Create(url);
    }

    public Url FindByShortUrl(string shortUrl)
    {
      var urlModel = _repository.FindByShortUrl(shortUrl);
      if (urlModel == null)
        throw new ArgumentNullException($"{shortUrl} not found");
      return urlModel;
    }

    public IEnumerable<Url> FindAll()
    {
      return _repository.FindAll();
    }

    public Url SaveVisit(string url)
    {
      var urlModel = FindByShortUrl(url);
      var visit = new VisitLog
      {
        IdUrl = urlModel.Id,
        Plataform = _browserDetector.Browser.OS,
        Browse = _browserDetector.Browser.Name,
        Id = Guid.NewGuid(),
        ClickDate = DateTime.UtcNow
      };
      urlModel.Count++;
      return _unitOfWork.SaveVisit(urlModel, visit);
    }
  }
}
