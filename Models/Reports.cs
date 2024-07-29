namespace GiftStore.Models
{
    public class Reports
    {
        public string Username { get; set; }
        public string ReciverAddress { get; set; }
        public string GiftName { get; set; }
        public DateTime? RequestDate { get; set; }
        public decimal? Price { get; set; }
        public string PaidStatus { get; set; }
        public decimal? Profits { get; set; }


    }
}
