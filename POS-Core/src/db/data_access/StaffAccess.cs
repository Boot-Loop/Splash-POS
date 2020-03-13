using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

using Core.DB.Models;
using CoreApp = Core.Application;

namespace Core.DB.Access
{
	public class StaffAccess : DataAccess
	{

		private static readonly StaffAccess instance = new StaffAccess();

		private StaffAccess() { }

		public static StaffAccess singleton {
			get { return instance; }
		}

		public List<StaffModel> getStaffs() {
			SqlCommand command = new SqlCommand("SELECT * FROM dbo.Staff");
			return excuteObject<StaffModel>(command).ToList();
		}
		public void addStaff(StaffModel model) {
			using (SqlConnection connection = new SqlConnection(Constants.CONNECTION_STRING)) {
				string command_text = "INSERT INTO dbo.Staff (FirstName, LastName, UserName, Password, EMail, AccessLevel) VALUES (@FirstName, @LastName, @UserName, @Password, @EMail, @AccessLevel)";
				using (SqlCommand command = new SqlCommand(command_text)) {
					command.Connection = connection;
					command.Parameters.Add("@FirstName", System.Data.SqlDbType.NVarChar, 100).Value = model.FirstName.value;
					command.Parameters.Add("@LastName", System.Data.SqlDbType.NVarChar, 100).Value = !string.IsNullOrEmpty(model.LastName.value) ? model.LastName.value : (object)DBNull.Value;
					command.Parameters.Add("@UserName", System.Data.SqlDbType.VarChar, 100).Value = model.UserName.value;
					command.Parameters.Add("@Password", System.Data.SqlDbType.VarChar, 128).Value = model.Password.value;
					command.Parameters.Add("@EMail", System.Data.SqlDbType.VarChar, 100).Value = !string.IsNullOrEmpty(model.EMail.value) ? model.EMail.value : (object)DBNull.Value;
					command.Parameters.Add("@AccessLevel", System.Data.SqlDbType.TinyInt).Value = model.AccessLevel.value;
					try {
						connection.Open();
						command.ExecuteNonQuery();
						CoreApp.logger.log("Successfully staff added to database");
					}
					catch (Exception ex) { throw new Exception(ex.Message); }
					finally {
						try { connection.Close(); CoreApp.logger.log("Successfully connection closed"); }
						catch (Exception ex) { throw new Exception(ex.Message); }
					}
				}
			}
		}
		public void deleteStaff(int id) {
			using (SqlConnection connection = new SqlConnection(Constants.CONNECTION_STRING)) {
				string command_text = "DELETE FROM dbo.Staff WHERE ID = @ID";
				using (SqlCommand command = new SqlCommand(command_text)) {
					command.Connection = connection;
					command.Parameters.Add("@ID", System.Data.SqlDbType.Int).Value = id;
					try {
						connection.Open();
						command.ExecuteNonQuery();
						CoreApp.logger.log("Successfully staff deleted from database");
					}
					catch (Exception ex) { throw new Exception(ex.Message); }
					finally {
						try { connection.Close(); CoreApp.logger.log("Successfully connection closed"); }
						catch (Exception ex) { throw new Exception(ex.Message); }
					}
				}
			}
		}
		public void updateStaff(StaffModel model, int id) {
			using (SqlConnection connection = new SqlConnection(Constants.CONNECTION_STRING)) {
				string command_text = "UPDATE dbo.Staff SET FirstName = @FirstName, LastName = @LastName, UserName = @UserName, Password = @Password, EMail = @EMail, AccessLevel = @AccessLevel WHERE ID = @ID";
				using (SqlCommand command = new SqlCommand(command_text)) {
					command.Connection = connection;
					command.Parameters.Add("@FirstName", System.Data.SqlDbType.NVarChar, 100).Value = model.FirstName.value;
					command.Parameters.Add("@LastName", System.Data.SqlDbType.NVarChar, 100).Value = !string.IsNullOrEmpty(model.LastName.value) ? model.LastName.value : (object)DBNull.Value;
					command.Parameters.Add("@UserName", System.Data.SqlDbType.VarChar, 100).Value = model.UserName.value;
					command.Parameters.Add("@Password", System.Data.SqlDbType.VarChar, 128).Value = model.Password.value;
					command.Parameters.Add("@EMail", System.Data.SqlDbType.VarChar, 100).Value = !string.IsNullOrEmpty(model.EMail.value) ? model.EMail.value : (object)DBNull.Value;
					command.Parameters.Add("@AccessLevel", System.Data.SqlDbType.TinyInt).Value = model.AccessLevel.value;
					command.Parameters.Add("@ID", System.Data.SqlDbType.Int).Value = id;
					try {
						connection.Open();
						command.ExecuteNonQuery();
						CoreApp.logger.log("Successfully staff updated in database");
					}
					catch (Exception ex) { throw new Exception(ex.Message); }
					finally {
						try { connection.Close(); CoreApp.logger.log("Successfully connection closed"); }
						catch (Exception ex) { throw new Exception(ex.Message); }
					}
				}
			}
		}
		public StaffModel login(string password) {
			string command_text = "SELECT * FROM dbo.Staff WHERE Password = @Password";
			SqlCommand command = new SqlCommand();
			command.CommandType = CommandType.Text;
			command.CommandText = command_text;
			command.Parameters.Add("@Password", System.Data.SqlDbType.VarChar, 128).Value = password;

			List<StaffModel> logged_in_staffs = excuteObject<StaffModel>(command).ToList();
			return logged_in_staffs.Count == 1 ? logged_in_staffs[0] : throw new WrongPasswordError("Login Failed, Password maybe wrong");
		}
	}
}
