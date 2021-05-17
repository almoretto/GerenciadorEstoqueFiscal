using System;
using System.Security.Cryptography;
using System.Text;

namespace StockManagerCore.Models.CryptoService
{
	/// <summary>
	/// Module for encript the system password and put them on database.
	/// Public methods for encriptation and comparisson.
	/// Private methods those who create hash and eventualy compares them
	/// </summary>
	public static class CypherModule
	{
		public static string MD5Return( string password ) //Public Metod to return encripted value
		{
			try
			{
				using ( MD5 md5Hash = MD5.Create() )
				{
					return HashReturn( md5Hash, password );
				}
			}
			catch ( CryptographicException ex )
			{
				string message = ex.Message;
				if ( ex.InnerException != null )
				{
					message += "\n" + ex.InnerException;
				}
				throw new CryptographicException( message );
			}
		}

		public static bool MD5Compare( string dbPass, string MD5Pass ) //Comparrison result return method
		{
			try
			{
				using ( MD5 md5Hash = MD5.Create() )
				{
					var Password = MD5Return( dbPass );
					if ( HashCheck( md5Hash, MD5Pass, Password ) )
					{
						return true;
					}
					else
					{
						return false;
					}
				}
			}
			catch ( CryptographicException ex )
			{
				string message = ex.Message;
				if ( ex.InnerException != null )
				{
					message += "\n" + ex.InnerException;
				}
				throw new CryptographicException( message );
			}
		}

		private static string HashReturn( MD5 md5Hash, string input ) //Private encriptor method 
		{
			StringBuilder sb = new StringBuilder();
			try
			{
				byte[] inputData = md5Hash.ComputeHash( Encoding.UTF8.GetBytes( input ) );

				for ( int i = 0; i < inputData.Length; i++ )
				{
					sb.Append( inputData[i].ToString( "x2" ) );
				}
			}
			catch ( CryptographicException ex )
			{
				string message = ex.Message;
				if ( ex.InnerException != null )
				{
					message += "\n" + ex.InnerException;
				}
				throw new CryptographicException( message );
			}
			return sb.ToString();
		}

		private static bool HashCheck( MD5 md5Hash, string input, string hash ) //Comparrison method
		{
			try
			{
				StringComparer scComparisson = StringComparer.OrdinalIgnoreCase;

				if ( 0 == scComparisson.Compare( input, hash ) )
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			catch ( CryptographicException ex )
			{
				string message = ex.Message;
				if ( ex.InnerException != null )
				{
					message += "\n" + ex.InnerException;
				}
				throw new CryptographicException( message );
			}
		}
	}
}
