namespace DatingApp.API.Entities
{
    public partial class AppUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string LastName { get; set; }
        public byte[] Password { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}