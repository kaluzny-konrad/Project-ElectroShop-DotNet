namespace ElectroShop.Shared.Domain
{
    public class WishlistElement
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public DateTime AddedDate { get; set; }
    }
}
