using hey_url_challenge_code_dotnet.Models;
using HeyUrlChallengeCodeDotnet.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace hey_url_challenge_code_dotnet.Repository.Generic
{
  public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
  {
    protected ApplicationContext _context;
    protected DbSet<T> dataset;

    public BaseRepository(ApplicationContext context)
    {
      _context = context;
      dataset = context.Set<T>();
    }

    public T Create(T item)
    {
      try
      {
        dataset.Add(item);
        _context.SaveChanges();
        return item;
      }
      catch(Exception)
      {
        throw;
      }
    }

    public List<T> FindAll()
    {
      return dataset.ToList();
    }

    public T Update(T item)
    {
      var result = dataset.SingleOrDefault(x => x.Id.Equals(item.Id));
      if(result != null)
      {
        try
        {
          _context.Entry(result).CurrentValues.SetValues(item);
          _context.SaveChanges();
          return result;
        }
        catch (Exception)
        {
          throw;
        }
      }
      else
      {
        return null;
      }
    }

  }
}
