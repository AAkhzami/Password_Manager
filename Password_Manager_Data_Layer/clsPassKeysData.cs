using DVLD_Data_Layer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Password_Manager_Data_Layer
{
    public class clsPassKeysData
    {
        public static bool GetPassKeyInfo(int KeyID, ref int UserID,ref string Title, ref string AccountUser, ref string Password, ref string URL, ref string ImagePath)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = @"select * from PassKeys where KeyID = @KeyID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@KeyID", KeyID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    isFound = true;
                    UserID = (int)reader["UserID"];
                    Title = (string)reader["Title"];
                    
                    AccountUser = (string)reader["AccountUser"];

                    Password = (string)reader["Password"];
                    URL = (string)reader["URL"];

                    if (reader["ImagePath"] != DBNull.Value)
                        ImagePath = (string)reader["ImagePath"];
                    else
                        ImagePath = "";

                }
                reader.Close();
            }
            catch (Exception ex)
            { isFound = false; }
            finally
            {
                connection.Close();
            }
            return isFound;
        }
        public static int AddNewPassKey(int UserID, string Title, string AccountUser, string Password, string URL, string ImagePath)
        {
            int PassKeyID = -1;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = @"INSERT INTO PassKeys
                             (UserID,Title,AccountUser,Password,URL,ImagePath)
                             VALUES
                             (@UserID,@Title,@AccountUser,@Password,@URL,@ImagePath);
                             select SCOPE_IDENTITY();";
            SqlCommand command = new SqlCommand(query, connection);


            command.Parameters.AddWithValue("@UserID", UserID);
            command.Parameters.AddWithValue("@Title", Title);
            
            command.Parameters.AddWithValue("@AccountUser", AccountUser);

            command.Parameters.AddWithValue("@Password", Password);
            command.Parameters.AddWithValue("@URL", URL);

            if (ImagePath == "")
                command.Parameters.AddWithValue("@ImagePath", DBNull.Value);
            else
                command.Parameters.AddWithValue("@ImagePath", ImagePath);

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int newID))
                {
                    PassKeyID = newID;
                }
            }
            catch (Exception ex)
            {
                PassKeyID = -1;
            }
            finally
            {
                connection.Close();
            }

            return PassKeyID;
        }
        public static bool UpdatePassKey(int KeyID,int UserID, string Title, string AccountUser, string Password, string URL, string ImagePath)
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = @"UPDATE PassKeys
                            SET UserID = @UserID,
                            Title = @Title,
                            Password = @Password,
                            AccountUser = @AccountUser,
                            URL = @URL
                            WHERE KeyID = @KeyID";

            SqlCommand command = new SqlCommand(query, connection);


            command.Parameters.AddWithValue("@UserID", UserID);
            command.Parameters.AddWithValue("@Title", Title);

            command.Parameters.AddWithValue("@AccountUser", AccountUser);
            command.Parameters.AddWithValue("@Password", Password);
            command.Parameters.AddWithValue("@URL", URL);
            command.Parameters.AddWithValue("@KeyID", KeyID);

            if (ImagePath != "")
                command.Parameters.AddWithValue("@ImagePath", DBNull.Value);
            else
                command.Parameters.AddWithValue("@ImagePath", ImagePath);

            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            { return false; }
            finally
            {
                connection.Close();
            }

            return rowsAffected > 0;
        }
        public static bool DeletePassKey(int KeyID)
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = @"DELETE FROM PassKeys
                             WHERE  KeyID = @KeyID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@KeyID", KeyID);

            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            { return false; }
            finally
            {
                connection.Close();
            }

            return rowsAffected > 0;
        }
        public static DataTable GetAllPassKeys()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = "select * from PassKeys";
            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    dt.Load(reader);

                }
            }
            catch (Exception ex)
            { }
            finally
            {
                connection.Close();
            }
            return dt;
        }
        public static DataTable GetAllPassKeysByUserID(int UserID)
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = "select * from PassKeys where UserID = @UserID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@UserID",UserID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if(reader.HasRows)
                {
                    dt.Load(reader);

                }
            }
            catch (Exception ex)
            { }
            finally
            {
                connection.Close();
            }
            return dt;
        }
        public static bool DeleteAllKeys(int UserID)
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = @"DELETE FROM PassKeys
                             WHERE  UserID = @UserID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            { return false; }
            finally
            {
                connection.Close();
            }

            return rowsAffected > 0;
        }
    }

}
