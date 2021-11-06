using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hey_url_challenge_code_dotnet.Models
{
  public class VisitLog : BaseEntity
  {
    public Guid IdUrl { get; set; }
    public string Plataform { get; set; }
    public string Browse { get; set; }
    public DateTime ClickDate { get; set; }
    public Url Url { get; set; }
  }
}
