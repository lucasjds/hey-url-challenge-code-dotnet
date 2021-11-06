using System;
using System.Collections.Generic;
using hey_url_challenge_code_dotnet.Models;
using hey_url_challenge_code_dotnet.Services.Interfaces;
using hey_url_challenge_code_dotnet.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shyjus.BrowserDetection;

namespace HeyUrlChallengeCodeDotnet.Controllers
{
  [Route("/")]
  public class UrlsController : Controller
  {
    private readonly ILogger<UrlsController> _logger;
    private readonly IUrlService _urlService;


    public UrlsController(ILogger<UrlsController> logger, IUrlService urlService)
    {
      _urlService = urlService;
      _logger = logger;
    }

    public IActionResult Index()
    {
      var model = new HomeViewModel();
      model.Urls = _urlService.FindAll();
      model.NewUrl = new();
      return View(model);
    }

    [Route("/{url}")]
    public IActionResult Visit(string url)
    {
      try
      {
        _urlService.SaveVisit(url);
        return new OkObjectResult($"{url} visited");
      }
      catch(ArgumentNullException ex)
      {
        return new NotFoundObjectResult(ex.Message);
      }
    } 

    [HttpPost]
    public IActionResult Create(HomeViewModel homeViewModel)
    {
      if (ModelState.IsValid)
      {
        _urlService.Create(homeViewModel.NewUrl);
        return RedirectToAction("Index");
      }
      return View("Index", homeViewModel);
    }

    [Route("urls/{url}")]
    public IActionResult Show(string url)
    {
      try
      {
        var urlModel = _urlService.FindByShortUrl(url);
        return View(new ShowViewModel
        {
          Url = urlModel,
          DailyClicks = urlModel.FilterListVisitByClickDates(),
          BrowseClicks = urlModel.FilterListVisitByBrowses(), 
          PlatformClicks =  urlModel.FilterListVisitByPlataforms()
        });
      }
      catch (ArgumentNullException ex)
      {
        return new NotFoundObjectResult(ex.Message);
      }

    }
  }
}