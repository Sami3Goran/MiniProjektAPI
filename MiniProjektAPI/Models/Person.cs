﻿namespace MiniProjektAPI.Models
{
    public class Person
    {
        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }

        public virtual ICollection<Interest>? Interests { get; set; }
        public virtual ICollection<InterestLink>? InterestLinks { get; set; }

    }
}
