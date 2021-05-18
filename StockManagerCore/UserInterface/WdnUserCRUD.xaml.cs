using StockManagerCore.Models;
using StockManagerCore.Services;
using System.Collections.Generic;
using System.Windows;
using System.Linq;

namespace StockManagerCore.UserInterface
{
	public partial class WdnUserCRUD : Window
	{

		private readonly UserService _userServices;
		private User User { get; set; }
		private List<User> UserList { get; set; }
		public int UserId { get; set; }
		public WdnUserCRUD( UserService userService )
		{
			_userServices = userService;
			UserList = _userServices.GetUsers();
			txtPassword.IsEnabled = false;
			txtPassConfirmation.IsEnabled = false;
			foreach ( User u in UserList )
			{
				cmbUserList.Items.Add( u.Name );
			}
			InitializeComponent();
		}

		private void btnListUsers_Click( object sender, RoutedEventArgs e )
		{
			IEnumerable<object> list = from u
								in UserList
									   select new
									   {
										   Nome = u.Name,
										   Usuario = u.UserName,
										   Permissao = u.UserType,
										   Status = u.Status
									   };
			WdwGenericGridData userGrid = new WdwGenericGridData( list.ToList(), "Lista de Usuários" );
			userGrid.ShowDialog();
		}

		private void btnUserSearch_Click( object sender, RoutedEventArgs e )
		{
			User user = UserList
								.Where( u => u.Name == cmbUserList.SelectedItem.ToString() )
								.FirstOrDefault();

			FillForm( user );
		}

		private void FillForm( User u )
		{
			UserId = u.Id;
			txtName.Text = u.Name;
			txtUsername.Text = u.UserName;
			txtEmail.Text = u.Email;
			txtPassword.IsEnabled = true;
			txtPassConfirmation.IsEnabled = true;
		}

		private void btnChangePassword_Click( object sender, RoutedEventArgs e )
		{
			if ( txtPassword.Text.Trim() == string.Empty )
			{
				MessageBox.Show( "Necessário digitar senha",
												"Erro de senha",
												MessageBoxButton.OK,
												MessageBoxImage.Error );
				return;
			}
			if ( txtPassConfirmation.Text.Trim() == string.Empty || txtNewPass.Text.Trim() == string.Empty )
			{
				MessageBox.Show( "Necessário digitar senha nova",
													"Erro de senha",
													MessageBoxButton.OK,
													MessageBoxImage.Error );
				return;
			}
			if ( txtNewPass.Text!=txtPassConfirmation.Text )
			{
				MessageBox.Show( "Nova senha e Confirmação não coincidem",
														"Erro de senha",
														MessageBoxButton.OK,
														MessageBoxImage.Error );
				return;
			}
			User changePass = new User();
			changePass = _userServices.Find( UserId );
			if ( changePass.CheckPass( txtPassword.Text ) )
			{
				changePass.SetPass( txtPassConfirmation.Text.Trim() );
			}
			
		}
	}
}
