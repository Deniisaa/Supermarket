namespace Tema3MVVM.Models
{
    public class User
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool Exists { get; set; }
        public string UserType { get; set; }
    }
}
