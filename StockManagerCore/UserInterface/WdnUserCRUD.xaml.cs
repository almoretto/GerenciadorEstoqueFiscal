using StockManagerCore.Models;
using StockManagerCore.Services;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using StockManagerCore.Models.Enum;

namespace StockManagerCore.UserInterface
{
	public partial class WdnUserCRUD : Window
	{

		private readonly UserService _userServices;
		private User UserToShow { get; set; } = null;
		private List<User> UserList { get; set; }
		public int UserId { get; set; }
		public WdnUserCRUD( UserService userService )
		{
			_userServices = userService;
			UserList = _userServices.GetUsers();
			txtPassword.IsEnabled = false;
			txtNewPass.IsEnabled = false;
			txtPassConfirmation.IsEnabled = false;
			foreach ( User u in UserList )
			{
				cmbUserList.Items.Add( u.Name );
			}
			InitializeComponent();
		}

		#region --== Action Buttons ==--
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
			UserToShow = UserList
								.Where( u => u.Name == cmbUserList.SelectedItem.ToString() )
								.FirstOrDefault();
			if ( UserToShow == null )
			{
				MessageBox.Show( "Usuário não encontrado",
												"Manutenção de usuários",
												 MessageBoxButton.OK,
												 MessageBoxImage.Error );
				ClearForm();
				return;
			}
			FillForm( UserToShow );
		}

		private void btnChangePassword_Click( object sender, RoutedEventArgs e )
		{
			if ( UserId == -1 || UserToShow == null )
			{
				MessageBox.Show( "Necessário selecionar o usuário",
													"Erro de senha",
													MessageBoxButton.OK,
													MessageBoxImage.Error );
				return;
			}
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
			if ( txtNewPass.Text != txtPassConfirmation.Text )
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
				if ( changePass.SetPass( txtPassConfirmation.Text.Trim() ) )
				{
					MessageBox.Show(
										"Senha Alterada com Sucesso",
										"Alteração de senha",
										MessageBoxButton.OK,
										MessageBoxImage.Information );
					ClearForm();
				}
				else
				{
					MessageBox.Show(
										"Senha não pode ser alterada, tente novamente!",
										"Alteração de senha",
										MessageBoxButton.OK,
										MessageBoxImage.Exclamation );
				}
			}
		}

		private void btnSave_Click( object sender, RoutedEventArgs e )
		{
			txtNewPass.IsEnabled = false;
			txtPassword.IsEnabled = false;
			txtPassConfirmation.IsEnabled = false;
			if ( UserToShow == null )
			{
				MessageBox.Show( "Busque novamente o usuário para alterar!",
												"Manutenção de usuários",
												 MessageBoxButton.OK,
												 MessageBoxImage.Error );
				ClearForm();
				return;
			}
			UserToShow.Name = txtName.Text.Trim().ToUpper();
			UserToShow.UserName = txtUsername.Text.Trim().ToLower();
			if ( txtEmail.Text.Trim().Contains( "@" )
				&& ( txtEmail.Text.Trim().Contains( ".com" ) || txtEmail.Text.Trim().Contains( ".net" ) ) )
			{ UserToShow.Email = txtEmail.Text.Trim().ToLower(); }
			if ( rbtAdmin.IsChecked == true )
			{ UserToShow.UserType = UserType.Administrador; }
			else
			{ UserToShow.UserType = UserType.Usuario; }
			if ( rbtActive.IsChecked == true )
			{ UserToShow.Status = true; }
			else
			{ UserToShow.Status = false; }

			string update = _userServices.Update( UserToShow );
			if ( update == string.Empty )
			{
				MessageBox.Show( "Usuário não pode ser atualizado",
												"Atualização de Usuário",
												MessageBoxButton.OK,
												MessageBoxImage.Error );
				return;
			}
			MessageBox.Show( update,
												"Atualização de Usuário",
												MessageBoxButton.OK,
												MessageBoxImage.Information );
		}

		private void btnClear_Click( object sender, RoutedEventArgs e )
		{
			ClearForm();
		}

		private void btnAddUser_Click( object sender, RoutedEventArgs e )
		{
			if ( txtName.Text.Trim() != string.Empty )
			{
				MessageBox.Show( "Necessário preencher nome",
												"Cadastro de Usuários",
												MessageBoxButton.OK,
												MessageBoxImage.Exclamation );
				return;
			}
			if ( txtEmail.Text.Trim() != string.Empty )
			{
				MessageBox.Show( "Necessário preencher email",
												"Cadastro de Usuários",
												MessageBoxButton.OK,
												MessageBoxImage.Exclamation );
				return;
			}
			if ( txtUsername.Text.Trim() != string.Empty )
			{
				MessageBox.Show( "Necessário preencher usuário",
												"Cadastro de Usuários",
												MessageBoxButton.OK,
												MessageBoxImage.Exclamation );
				return;
			}
			if ( !txtNewPass.Text.Trim().Equals( txtPassConfirmation.Text.Trim() ) )
			{
				MessageBox.Show( "Senha Nova e Confirmação não coincidem digite novamente.",
												"Cadastro de Usuários",
												MessageBoxButton.OK,
												MessageBoxImage.Exclamation );
				return;
			}
			string create = _userServices.Create( UserToShow );
			if ( create == string.Empty )
			{
				MessageBox.Show( "Usuário não pode ser criado",
													"Cadastro de Usuário",
													MessageBoxButton.OK,
													MessageBoxImage.Error );
				return;
			}
			MessageBox.Show( create,
													"Cadastro de Usuário",
													MessageBoxButton.OK,
													MessageBoxImage.Information );
		}

		private void btnDeleteUser_Click( object sender, RoutedEventArgs e )
		{
			if ( cmbUserList.SelectedItem == null )
			{
				MessageBox.Show( "Necessário selecionar para deletar",
												"Manutenção de usuários",
												 MessageBoxButton.OK,
												 MessageBoxImage.Information );
			}
			UserToShow = UserList
									.Where( u => u.Name == cmbUserList.SelectedItem.ToString() )
									.FirstOrDefault();
			if ( UserToShow == null )
			{
				MessageBox.Show( "Usuário não encontrado",
												"Manutenção de usuários",
												 MessageBoxButton.OK,
												 MessageBoxImage.Error );
				ClearForm();
				return;
			}
			FillForm( UserToShow );
			MessageBoxResult mr = MessageBox.Show(
													"Desativar usuário: "
														+ UserToShow.Name,
													"Confirmação de Desativação",
													MessageBoxButton.YesNo,
													MessageBoxImage.Question );
			if ( mr == MessageBoxResult.Yes )
			{
				UserToShow.Status = false;
				UserToShow.UserType = UserType.Usuario;
				string delete = _userServices.Update( UserToShow );
				MessageBox.Show( delete,
												"Desativação de usuários",
												 MessageBoxButton.OK,
												 MessageBoxImage.Information );
				ClearForm();
			}
			else
			{
				MessageBox.Show( "Desativação Cancelada",
													"Desativação de usuários",
													 MessageBoxButton.OK,
													 MessageBoxImage.Information );
				return;
			}
		}

		private void btnExit_Click( object sender, RoutedEventArgs e )
		{
			ClearForm();
			this.Close();
		}
		#endregion

		#region --== Local Methods ==--
		private void FillForm( User u )
		{
			UserId = u.Id;
			txtName.Text = u.Name;
			txtUsername.Text = u.UserName;
			txtEmail.Text = u.Email;
			txtPassword.IsEnabled = true;
			txtNewPass.IsEnabled = true;
			txtPassConfirmation.IsEnabled = true;
		}
		private void ClearForm()
		{
			UserId = -1;
			UserToShow = null;
			cmbUserList.SelectedItem = null;
			txtEmail.Text = string.Empty;
			txtName.Text = string.Empty;
			txtNewPass.Text = string.Empty;
			txtPassConfirmation.Text = string.Empty;
			txtPassword.Text = string.Empty;
			txtUsername.Text = string.Empty;
			rbtUser.IsChecked = false;
			rbtAdmin.IsChecked = false;
			rbtActive.IsChecked = false;
			rbtInactive.IsChecked = false;
			txtNewPass.IsEnabled = false;
			txtPassConfirmation.IsEnabled = false;
			txtPassword.IsEnabled = false;
		}
		#endregion		
	}
}
