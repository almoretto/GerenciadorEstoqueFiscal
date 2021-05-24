#region --== Dependency Declaration ==--
using System.Linq;
using System.Collections.Generic;
using StockManagerCore.Data;
using StockManagerCore.Models;
using StockManagerCore.Services.Exceptions;
using StockManagerCore.Models.Enum;
#endregion

namespace StockManagerCore.Services
{
	public class UserService
	{
		#region --== Constructor for dependency injection ==--
		private readonly StockDBContext _context;

		//Constructor and dependency Injection to DbContext
		public UserService( StockDBContext context ) { _context = context; }
		#endregion

		#region --== Methods ==--

		//Querry to fetch all users from database 
		public List<User> GetUsers()
		{
			return _context.Users.ToList();
		}

		//Querry to Find a producta By Entity
		public User Find( int id )
		{
			User user = new User();

			try
			{
				user = _context.Users
					.Where( us => us.Id == id )
					.SingleOrDefault();
				if ( user.UserName == string.Empty )
				{
					throw new NotFoundException( "Entidade não encontrada" );
				}
			}
			catch ( DbComcurrancyException e)
			{
				throw new DbComcurrancyException(e.Message);
			}

			return user;
		}

		//Querry to find product by Name
		public List<User> FindByType( int type )
		{
			List<User> listUserType = new List<User>();
			if ( type <= 0 )
			{
				throw new RequiredFieldException( "Campo requerido para busca" );
			}
			listUserType = _context.Users.Where( us => us.UserType == (UserType)type ).ToList();
			if ( listUserType.Count < 1 )
			{
				throw new NotFoundException( "Nenhuma entidade encontrada" );
			}
			return listUserType;
		}

		#region --== CRUD ==--

		//Method to create a new product on database
		public string Create( User user )
		{
			string response = string.Empty;
			try
			{
				_context.Users.Add( user );
				_context.SaveChanges();
				var test = _context.Users.Where( t => t.UserName == user.UserName ).FirstOrDefault();
				response = "Id: " + test.Id.ToString() + " Usuário: " + test.UserName.ToUpper();
			}
			catch ( DbComcurrancyException ex )
			{
				if ( ex.InnerException != null )
				{
					response = ex.Message + "\n InnerError:" + ex.InnerException;
				}
				else
				{
					response = ex.Message;
				}
				throw new DbComcurrancyException( "Erro ao tentar criar usuário!\n" + response );
			}
			return response;
		}

		//Querry to find a specific product to edit
		public User FindToUdate( string name, int? id )
		{
			User usr = new User();

			if ( id.HasValue )
			{
				usr = _context.Users.Find( id );
				if ( usr.UserName == string.Empty )
				{
					throw new NotFoundException( "Id :" + id.ToString() + "Não encontrado" );
				}
				return usr;
			}
			else if ( name != "" )
			{
				usr = _context.Users.Where( c => c.UserName == name ).FirstOrDefault();
				if ( usr.UserName == string.Empty )
				{
					throw new NotFoundException( "Usuário :" + name + "Não encontrado" );
				}
				return usr;
			}
			throw new NotFoundException( "Insuficient Data to find entity!" );
		}

		//Method to update an edited product
		public string Update( User u )
		{
			string returnMsg = string.Empty;
			try
			{
				if ( u.UserName == string.Empty )
				{
					throw new DbComcurrancyException( "Entity could not be null or empty!" );
				}
				_context.Users.Update( u );
				_context.SaveChanges();
				returnMsg = "Update realizado com sucesso!";
			}
			catch ( DbComcurrancyException ex )
			{
				string msg = ex.Message;
				if ( ex.InnerException != null )
				{
					msg += "\n InnerException: " + ex.InnerException;
				}
				throw new DbComcurrancyException( "Não foi possivel atualizar veja mensagem: \n" + msg );
			}
		
			return returnMsg;
		}

		#endregion
		#endregion
	}
}