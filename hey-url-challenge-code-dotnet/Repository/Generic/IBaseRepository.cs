using hey_url_challenge_code_dotnet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hey_url_challenge_code_dotnet.Repository.Generic
{
  public interface IBaseRepository<T> where T : BaseEntity
  {
    T Create(T item);
    List<T> FindAll();
    T Update(T item);
  }
}
