

namespace StockManagerCore.Models
{
    class User
    {

        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; private set; }

        private string Keychange;

        public User()
        {
        }

        public User(string userName)
        {
            UserName = userName;
            Keychange = "";
        }

        public void SetPass(string password)
        {
            Password = Keychange;
        }
    }
}
