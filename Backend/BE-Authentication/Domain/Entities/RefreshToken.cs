namespace Domain.Entities;

public class RefreshToken
{   
    public Guid Id { get; set; }
    public string UserId { get; set; } = null!;
    public string Token { get; set; } = null!;
    public DateTime ExpiryDate { get; set; }
}
