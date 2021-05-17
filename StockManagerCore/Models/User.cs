#region --== Using includes ==--
using StockManagerCore.Models.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using StockManagerCore.Models.CryptoService;
using System.Windows;
using System;
#endregion

namespace StockManagerCore.Models
{
	public class User
	{

		#region --== Properties ==--
		[Key]
		public int Id { get; set; }
		public string UserName { get; set; }
		public UserType UserType { get; set; }
		public string Email { get; set; }
		private string MD5Password { get; set; }
		#endregion

		#region --== Private Props ==--
		[NotMapped]
		private string Keychange;
		#endregion

		#region --== Constructors ==--
		public User() { }

		public User( string userName, UserType userType, string email, string openPass )
		{
			UserName = userName;
			UserType = userType;
			Email = email;
			Keychange = "";
			SetPass( openPass );
		}
		#endregion

		#region --== Public Methods ==--
		public bool SetPass( string password )
		{
			try
			{
				Keychange = password;
				MD5Password = CypherPass( Keychange );
				return true;
			}
			catch ( ApplicationException ex  )
			{
				MessageBox.Show( 
						ex.Message, 
						"Erro ao gravar senha!", 
						MessageBoxButton.OK, 
						MessageBoxImage.Error );
			}
			return false;
		}
		public bool CheckPass( string passKey )
		{
			if ( CypherCheck( passKey ) )
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		public bool ChangePass(string passkey, string newPass)
		{
			if ( CheckPass( passkey ) )
			{
				return SetPass( newPass );
			}
			else
			{
				MessageBox.Show(
						"Não foi possivel trocar a senha!\n Verifique a senha atual e tente novamente!",
						"Erro ao gravar senha!",
						MessageBoxButton.OK,
						MessageBoxImage.Error );
				return false;
			}
						
		}
		#endregion
	
		#region --== Private Methods ==--
		private string CypherPass( string password )
		{ return CypherModule.MD5Return( password ); }
		private bool CypherCheck( string passKey )
		{ return CypherModule.MD5Compare( passKey, MD5Password ); }
		#endregion
	}
}
