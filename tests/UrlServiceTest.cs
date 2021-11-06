using hey_url_challenge_code_dotnet.Models;
using hey_url_challenge_code_dotnet.Repository.Interfaces;
using hey_url_challenge_code_dotnet.Services;
using Moq;
using NUnit.Framework;
using Shyjus.BrowserDetection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace tests
{
  public class UrlServiceTest
  {
    private Mock<IUrlRepository> _repository;
    private Mock<IUnitOfWork> _unit;
    private Mock<IBrowserDetector> _browse;


    [SetUp]
    public void Setup()
    {
      _browse = new Mock<IBrowserDetector>();
      _repository = new Mock<IUrlRepository>();
      _unit = new Mock<IUnitOfWork>();

    }

    [Test]
    public void FindByShortUrl_WhenShortUrlIsNull_ShouldThrowException()
    {
      UrlService service = new UrlService(_repository.Object, _browse.Object, _unit.Object);
      _repository.Setup(x => x.FindByShortUrl(It.IsAny<string>())).Returns((Url)null);
      Assert.Throws<ArgumentNullException>(() =>
      {
        service.FindByShortUrl("ABCDE");
      });
    }

    [Test]
    public void FindByShortUrl_WhenShortUrlIsNotNullAndThereIsNoVisits_ShouldReturnAnObject()
    {
      UrlService service = new UrlService(_repository.Object, _browse.Object, _unit.Object);
      var url = new Url
      {
        Count = 0,
        CreatedAt = DateTime.Now,
        Id = Guid.NewGuid(),
        OriginalUrl = "http://www.google.com",
        ShortUrl = "ABCDE",
      };
      _repository.Setup(x => x.FindByShortUrl(It.IsAny<string>())).Returns(url);

      var result = service.FindByShortUrl("ABCDE");
      Assert.IsNotNull(result);
      Assert.AreEqual(result.Id, url.Id);
      Assert.AreEqual(result.OriginalUrl, url.OriginalUrl);
      Assert.AreEqual(result.ShortUrl, url.ShortUrl);
      Assert.AreEqual(result.CreatedAt, url.CreatedAt);
      Assert.AreEqual(result.Count, url.Count);
    }

    [Test]
    public void FindByShortUrl_WhenShortUrlIsNotNullAndThereAreVisits_ShouldReturnAnObject()
    {
      UrlService service = new UrlService(_repository.Object, _browse.Object, _unit.Object);
      var url = new Url
      {
        Count = 1,
        CreatedAt = DateTime.Now,
        Id = Guid.NewGuid(),
        OriginalUrl = "http://www.google.com",
        ShortUrl = "ABCDE",
      };
      var visit = new VisitLog
      {
        IdUrl = url.Id,
        Plataform = "Windows",
        Browse = "Chrome",
        Id = Guid.NewGuid(),
        ClickDate = DateTime.UtcNow
      };
      url.VisitLogs = new List<VisitLog>();
      url.VisitLogs.Add(visit);

      _repository.Setup(x => x.FindByShortUrl(It.IsAny<string>())).Returns(url);

      var result = service.FindByShortUrl("ABCDE");
      Assert.IsNotNull(result);
      Assert.AreEqual(result.Id, url.Id);
      Assert.AreEqual(result.OriginalUrl, url.OriginalUrl);
      Assert.AreEqual(result.ShortUrl, url.ShortUrl);
      Assert.AreEqual(result.CreatedAt, url.CreatedAt);
      Assert.AreEqual(result.Count, url.Count);
      Assert.AreEqual(result.VisitLogs.Count(), 1);
      Assert.IsTrue(result.VisitLogs.Where(x => x.Id.Equals(visit.Id)).Any());
    }

    [Test]
    public void FindAll_ShouldReturnAListOfUrls()
    {
      UrlService service = new UrlService(_repository.Object, _browse.Object, _unit.Object);
      var urlGoogle = new Url
      {
        Count = 1,
        CreatedAt = DateTime.Now,
        Id = Guid.NewGuid(),
        OriginalUrl = "http://www.google.com",
        ShortUrl = "ABCDE",
      };

      var urlInstagram = new Url
      {
        Count = 1,
        CreatedAt = DateTime.Now,
        Id = Guid.NewGuid(),
        OriginalUrl = "http://www.instagram.com",
        ShortUrl = "BCDEF",
      };

      var list = new List<Url>();
      list.Add(urlGoogle);
      list.Add(urlInstagram);

      _repository.Setup(x => x.FindAll()).Returns(list);

      var result = service.FindAll();

      Assert.IsNotNull(result);
      _repository.Verify(x => x.FindAll(), Times.Once);
      Assert.AreEqual(result.Count(), list.Count());
      foreach (var pair in result
        .Zip(list,(a, b) => new { result = a, list = b }))
      {
        Assert.AreEqual(pair.result.OriginalUrl, pair.list.OriginalUrl);
        Assert.AreEqual(pair.result.ShortUrl, pair.list.ShortUrl);
        Assert.AreEqual(pair.result.CreatedAt, pair.list.CreatedAt);
        Assert.AreEqual(pair.result.Count, pair.list.Count);
      }
    }

    [Test]
    public void Create_WhenEverythingIsOk_ShouldCreateUrl()
    {
      UrlService service = new UrlService(_repository.Object, _browse.Object, _unit.Object);
      var url = new Url
      {
        OriginalUrl = "http://www.google.com",
      };
      _repository.Setup(x => x.FindByShortUrl(It.IsAny<string>())).Returns((Url)null);
      _repository.Setup(x => x.Create(It.IsAny<Url>())).Returns(url);

      var result = service.Create(url);
      _repository.Verify(x => x.FindByShortUrl(It.IsAny<string>()), Times.Once);
      Assert.AreEqual(result.Id, url.Id);
      Assert.AreEqual(result.OriginalUrl, url.OriginalUrl);
      Assert.AreEqual(result.ShortUrl, url.ShortUrl);
      Assert.AreEqual(result.CreatedAt, url.CreatedAt);
      Assert.AreEqual(result.Count, 0);
      //asserting if the shorturls is ok
      Assert.AreEqual(result.ShortUrl.Length, 5);
      Assert.IsTrue(result.ShortUrl.All(char.IsUpper));
      Assert.IsTrue(result.ShortUrl.IndexOf(' ') == -1);
      Regex regex = new Regex("[^A-Z]");
      Assert.False(regex.IsMatch(result.ShortUrl));

    }

    [Test]
    public void Create_WhenUrlAlreadyExists_ShouldTryToGenerateAnotherOne()
    {
      UrlService service = new UrlService(_repository.Object, _browse.Object, _unit.Object);
      var url = new Url
      {
        Count = 1,
        CreatedAt = DateTime.Now,
        Id = Guid.NewGuid(),
        OriginalUrl = "http://www.google.com",
        ShortUrl = "ABCDE",
      };
      _repository.SetupSequence(x => x.FindByShortUrl(It.IsAny<string>())).Returns(url).Returns((Url)null);
      url = new Url
      {
        OriginalUrl = "http://www.google.com",
        ShortUrl = "BCDEF",
      };

      _repository.Setup(x => x.Create(It.IsAny<Url>())).Returns(url);
      var result = service.Create(url);
      _repository.Verify(x => x.FindByShortUrl(It.IsAny<string>()), Times.AtLeastOnce);
      Assert.IsNotNull(result);
      Assert.AreEqual(result.Id, url.Id);
      Assert.AreEqual(result.OriginalUrl, url.OriginalUrl);
      Assert.AreEqual(result.ShortUrl, url.ShortUrl);
      Assert.AreEqual(result.CreatedAt, url.CreatedAt);
      Assert.AreEqual(result.Count, url.Count);
      //asserting if the shorturls is ok
      Assert.AreEqual(result.ShortUrl.Length, 5);
      Assert.IsTrue(result.ShortUrl.All(char.IsUpper));
      Assert.IsTrue(result.ShortUrl.IndexOf(' ') == -1);
      Regex regex = new Regex("[^A-Z]");
      Assert.False(regex.IsMatch(result.ShortUrl));

    }

    [Test]
    public void SaveVisit_WhenEverythingIsOk_ShouldSaveVisit()
    {
      UrlService service = new UrlService(_repository.Object, _browse.Object, _unit.Object);
      var url = new Url
      {
        Count = 1,
        CreatedAt = DateTime.Now,
        Id = Guid.NewGuid(),
        OriginalUrl = "http://www.google.com",
        ShortUrl = "ABCDE",
      };
      var visit = new VisitLog
      {
        IdUrl = url.Id,
        Plataform = "Windows",
        Browse = "Chrome",
        Id = Guid.NewGuid(),
        ClickDate = DateTime.UtcNow
      };

      url.VisitLogs = new List<VisitLog>();
      url.VisitLogs.Add(visit);

      _browse.Setup(x => x.Browser.Name).Returns("Chrome");
      _browse.Setup(x => x.Browser.OS).Returns("Windows");

      _repository.Setup(x => x.FindByShortUrl(It.IsAny<string>())).Returns(url);
      _unit.Setup(x => x.SaveVisit(It.IsAny<Url>(), It.IsAny<VisitLog>())).Returns(url);

      var result = service.SaveVisit("ABCDE");

      _repository.Verify(x => x.FindByShortUrl(It.IsAny<string>()), Times.Once);
      _unit.Verify(x => x.SaveVisit(It.IsAny<Url>(), It.IsAny<VisitLog>()), Times.Once);
      Assert.AreEqual(result.Id, url.Id);
      Assert.AreEqual(result.OriginalUrl, url.OriginalUrl);
      Assert.AreEqual(result.ShortUrl, url.ShortUrl);
      Assert.AreEqual(result.CreatedAt, url.CreatedAt);
      Assert.AreEqual(result.Count, url.Count);
      Assert.IsTrue(result.VisitLogs.Where(x => x.Id.Equals(visit.Id)).Any());

    }

    [Test]
    public void SaveVisit_WhenUrlNotExist_ShouldThrowException()
    {
      UrlService service = new UrlService(_repository.Object, _browse.Object, _unit.Object);
      _browse.Setup(x => x.Browser.Name).Returns("Chrome");
      _browse.Setup(x => x.Browser.OS).Returns("Windows");
      _repository.Setup(x => x.FindByShortUrl(It.IsAny<string>())).Returns((Url)null);

      Assert.Throws<ArgumentNullException>(() =>
      {
        service.SaveVisit("XXXXX");
      });
      _repository.Verify(x => x.FindByShortUrl(It.IsAny<string>()), Times.Once);
      _unit.Verify(x => x.SaveVisit(It.IsAny<Url>(), It.IsAny<VisitLog>()), Times.Never);
    }
  }
}