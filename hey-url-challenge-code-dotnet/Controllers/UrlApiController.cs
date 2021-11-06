using hey_url_challenge_code_dotnet.Models;
using JsonApiDotNetCore.Configuration;
using JsonApiDotNetCore.Controllers;
using JsonApiDotNetCore.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hey_url_challenge_code_dotnet.Controllers
{
  public class UrlApiController : JsonApiController<Url , Guid>
  {
    public UrlApiController(IJsonApiOptions jsonApiOptions, 
      ILoggerFactory logger, IResourceService<Url, Guid> resource)
      :base(jsonApiOptions, logger, resource)
    {

    }
  }
}
