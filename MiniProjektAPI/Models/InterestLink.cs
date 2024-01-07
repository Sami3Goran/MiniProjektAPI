using System;
using System.ComponentModel.DataAnnotations;

namespace MiniProjektAPI.Models
{
    public class InterestLink
    {
        
        public int InterestLinkId { get; set; }
        public string Url { get; set; }

        public int PersonId { get; set; }
        public int InterestId { get; set; }

        public virtual Person Person { get; set; }
        public virtual Interest Interest { get; set; }
    }
}
