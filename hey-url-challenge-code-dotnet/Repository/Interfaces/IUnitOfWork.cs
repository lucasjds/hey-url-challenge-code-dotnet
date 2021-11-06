using hey_url_challenge_code_dotnet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hey_url_challenge_code_dotnet.Repository.Interfaces
{
  public interface IUnitOfWork
  {
    Url SaveVisit(Url urlModel, VisitLog visitLog);
  }
}
