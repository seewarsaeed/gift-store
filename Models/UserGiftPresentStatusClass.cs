namespace GiftStore.Models
{
    public class UserGiftPresentStatusClass
    {
        public decimal? PresentId { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? GiftName { get; set; }
        public decimal? GiftPrice { get; set; }
        public DateTime? RequestDate { get; set; }
        public string? ReciverAddress { get; set; }
        public string? ArrivedStatus { get; set; }
        public string? PaidStatus { get; set; }
        public string? RequestStatus { get; set; }
        public string? NotificationStatus { get; set; }

    }
}
