using StockManagerCore.Models;
using StockManagerCore.Models.Enum;

namespace StockManagerCore.Services
{
	public class Session
	{
		private static Session UserSessionInstance;

		public static Entity User { get; set; }
		public static UserType UserType { get; set; }

		public static Session GetUserSessionInstance
		{
			get
			{
				if ( UserSessionInstance == null )
				{
					UserSessionInstance = new Session();
				}
				return UserSessionInstance;
			}
		}
	}
}

