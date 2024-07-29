namespace GiftStore.Models
{
    public class UserPresentStatusClass
    {
        public decimal? UserId { get; set; }
        public decimal? PresentId { get; set; }
        public string ? ReciverAddress { get; set; }
        public string ? GiftName { get; set; }
        public decimal? GiftId { get; set; }
        public DateTime? RequestDate { get; set; }
        public string? ArrivedStatus { get; set; }
        public string? PaidStatus { get; set; }
        public string? RequestStatus { get; set; }
        public string? NotificationStatus { get; set; }

    }
}
