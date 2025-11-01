namespace DAL.DAO
{
    public class UserDAO
    {
        public string? UserId { get; set; }
        public string? Username { get; set; }
        public required string Password { get; set; }
        public required string Email { get; set; }
        public string? MemberTypeId { get; set; }
        public int Level { get; set; }
        public int ExpPerLevel { get; set; }
        public int Coin { get; set; }
        public string? Status { get; set; }
    }
}
