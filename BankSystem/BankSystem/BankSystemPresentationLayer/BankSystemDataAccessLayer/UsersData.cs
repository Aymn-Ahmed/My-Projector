using System;
using System.Data;
using System.Data.SqlClient;


namespace BankSystemDataAccessLayer
{
    public class UsersData
    {
        static public bool GetUserByUserId(int userId, ref int Personid, ref string FirstName, ref string LastName
            , ref string Email, ref string phone, ref string Username, ref string Password,
            ref int Permissions)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = "select * from ViewUsersDetails;";
            SqlCommand command = new SqlCommand(Query, connection);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    isFound = true;
                    Personid = (int)reader["PersonID"];
                    FirstName = (string)reader["Firstname"];
                    LastName = (string)reader["Lastname"];
                    Email = (string)reader["Email"];
                    phone = (string)reader["Phone"];
                    Username = (string)reader["Username"];
                    Password = (string)reader["Password"];
                    Permissions = (int)reader["Permissions"];
                }
                else
                {
                    isFound = false;
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                isFound = false;
            }
            finally
            {
                connection.Close();
            }
            return isFound;

        }

        static public bool GetUserByUserName(ref int userId, ref int Personid, 
            string Username, ref string Password, ref int Permissions)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"select * from Users
                            where Username = @UserName ";
            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue ("@UserName" , Username);
            //command.Parameters.AddWithValue ("@PassWord" , Password);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    userId = (int)reader["UserID"];
                    Personid = (int)reader["PersonID"];
                    Password = (string)reader["Password"];
                    Permissions = (int)reader["Permissions"];
                }
                else
                {
                    isFound = false;
                }

                reader.Close();
            }
            catch (Exception ex)      
            {
                
                
            }
            finally
            {
                connection.Close();
            }
            return isFound;

        }



        static public bool AddNewPerson(ref int userId, ref int Personid, string Username, string Password,
           int Permissions)
        {
            bool IsTrue = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "";
            if (Personid == -1)
            {
                query = @"INSERT INTO [dbo].[Person]
                      ([Firstname],[Lastname],[Email],[Phone])
                      VALUES
                         (@FirstName,@LastName,@Email,@phone);
                INSERT INTO [dbo].[Users]
                    ([Username],[Password],[Permissions],[PersonID])
                      VALUES
                   (@Username,@Password,@Permissions, (select top 1 SCOPE_IDENTITY() from Person));
                         select top 1 SCOPE_IDENTITY() from Users;";

            }

            SqlCommand command = new SqlCommand(query, connection);

           
            command.Parameters.AddWithValue("Username", Username);
            command.Parameters.AddWithValue("Password", Password);
            command.Parameters.AddWithValue("Permissions", Permissions);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    IsTrue = true;
                    userId = insertedID;
                    Personid = PersonsData.GetPersonIDForUser(userId);
                }
                else
                {
                    IsTrue = false;
                }
            }

            catch (Exception ex)
            {
                IsTrue = false;
            }

            finally
            {
                connection.Close();
            }

            return IsTrue;



        }


        static public bool UpdateUser(int User_ID, int Person_ID, string UserName,  string Password,
                int Permission)
        {

            int rowAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"UPDATE [dbo].[Users]
                          SET [Username] = @Username
                             ,[Password] = @Password
                             ,[Permissions] = @Permissions
                          WHERE UserID = @User_ID;
                             UPDATE [dbo].[Person]
                          SET [Firstname] = @Firstname
                             ,[Lastname] = @Lastname
                             ,[Email] = @Email
                             ,[Phone] = @Phone
                          WHERE PersonId = @Person_ID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@Username", UserName);
            command.Parameters.AddWithValue("@Person_ID", Person_ID);
            command.Parameters.AddWithValue("@User_ID", User_ID);
            command.Parameters.AddWithValue("@Password", Password);
            command.Parameters.AddWithValue("@Permissions", Permission);

            try
            {
                connection.Open();
                rowAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                return false;
            }

            finally
            {
                connection.Close();
            }
            return (rowAffected > 0);

        }


        public static DataTable GetListUsers()
        {
            DataTable table = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = "select * from ViewUsersDetails;";
            SqlCommand command = new SqlCommand(Query, connection);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    table.Load(reader);
                }
                reader.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
            return table;

        }

        static public bool is_UserExist(int ID)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = "select found=1 from Users where UserID = @ID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ID", ID);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                isFound = reader.HasRows;
                reader.Close();
            }
            catch (Exception ex)
            {
                isFound = false;
            }
            finally
            {
                connection.Close();
            }
            return isFound;
        }

        static public bool Delete_User(int ID)
        {
            int RowAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"DELETE FROM [dbo].[Users] WHERE UserID = @ID;
                         DELETE FROM [dbo].[Users] WHERE PersonID =
                         (select PersonID from Users where UserID = @ID);";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ID", ID);
            try
            {
                connection.Open();
                RowAffected = command.ExecuteNonQuery();

            }
            catch
            {

            }
            finally
            {
                connection.Close();
            }
            return (RowAffected > 0);
        }

    }
}
