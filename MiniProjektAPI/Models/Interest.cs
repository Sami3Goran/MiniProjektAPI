namespace MiniProjektAPI.Models
{
    public class Interest
    {
        public int InterestId { get; set; }
        public string Title { get; set; }
        public string Descriptions { get; set; }

        public virtual ICollection<InterestLink> InterestLinks { get; set; }
    }
}
