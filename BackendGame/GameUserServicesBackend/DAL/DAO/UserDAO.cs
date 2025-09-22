namespace DAL.DAO
{
    public class UserDAO
    {
        public string? userId;
        public string? username;
        public required string password;
        public required string email;
        public string? memberTypeId;
        public int level;
        public int expPerLevel;
        public int coin;
        public string? status;
    }
}
