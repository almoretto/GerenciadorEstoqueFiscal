using StockManagerCore.Models.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace StockManagerCore.Models
{
	public class User
	{

		[Key]
		public int Id { get; set; }
		public string UserName { get; set; }
		public UserType UserType { get; set; }
		public string Email { get; set; }
		public string Password { get; private set; }
		[NotMapped]
		private string Keychange;

		public User()
		{
		}

		public User( string userName )
		{
			UserName = userName;
			Keychange = "";
		}

		public bool SetPass( string password )
		{
			Keychange = password;
			Password = CryptoPass(Keychange);

			return true;
		}
		private string CryptoPass( string pass )
		{
			StringBuilder sb = new StringBuilder();
			return sb.ToString();
		}
		private string DeCryptPass( string pass )
		{
			StringBuilder sb = new StringBuilder();
			return sb.ToString();
		}
	}
}
