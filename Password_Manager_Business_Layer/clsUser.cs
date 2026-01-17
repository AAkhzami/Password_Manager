using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Password_Manager_Data_Layer;

namespace Password_Manager_Business_Layer
{
    public class clsUser
    {
        enum enMode { Add, Update}
        enMode _Mode = enMode.Add;

        int _UserID {  get; set; }
        public int UserID
        {
            get
            {
                return _UserID;
            }
        }
        public string UserName { get; set; }
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Email { get; set; }

        public clsUser ()
        {
            this._UserID = -1;
            this.UserName = "";
            this.Password = "";
            this.CreatedAt = DateTime.Now;
            this.Email = "";
            this._Mode = enMode.Add;
        }
        clsUser(int userID, string userName, string Password, DateTime createdAt, string email)
        {
            this._UserID = userID;
            this.UserName = userName;
            this.Password = Password;
            this.CreatedAt = createdAt;
            this.Email = email;
            this._Mode = enMode.Update;
        }

        public static clsUser Find(int userID)
        {
            string userName = "", password = "", email = "";
            DateTime createdAt = DateTime.Now;  
            if(clsUserData.GetUserInfoByID(userID, ref userName, ref password, ref createdAt, ref email))
            {
                string decpassword = clsSecurityHelper.Decrypt(password, userName);
                return new clsUser(userID, userName, decpassword, createdAt, email);   
            }
            else
                return null;
        }
        public static clsUser Find(string userName)
        {
            int userID = -1;
            string password = "", email = "";
            DateTime createdAt = DateTime.Now;
            if (clsUserData.GetUserInfoByUserName(userName, ref userID, ref password, ref createdAt, ref email))
            {
                string decpassword = clsSecurityHelper.Decrypt(password, userName);
                return new clsUser(userID, userName, decpassword, createdAt, email);
            }
            else
                return null;
        }
        public static clsUser FindByUserNameAndPassword(string username, string password)
        {
            int userID = -1;
            string email = "";
            DateTime createdAt = DateTime.Now;
            string encpassword = clsSecurityHelper.Encrypt(password, username);
            if (clsUserData.FindByUserNameAndPassword(username, encpassword, ref userID, ref createdAt, ref email))
            {
                return new clsUser(userID, username, password, createdAt, email);
            }
            else
                return null;
        }
        private bool _AddNewUser()
        {
            if(this.UserName == "")
                return false;
            this._UserID = clsUserData.AddNewUser(this.UserName, clsSecurityHelper.Encrypt(this.Password,this.UserName), this.CreatedAt, this.Email);
            return this._UserID != -1;
        }
        private bool _UpdateUser()
        {
            if (this.UserName == "")
                return false;
            return clsUserData.UpdateUser(this._UserID,this.UserName, clsSecurityHelper.Encrypt(this.Password, this.UserName), this.CreatedAt, this.Email);
        }
        public bool Save()
        {
            switch(this._Mode)
            {
                case enMode.Add:
                    if(_AddNewUser())
                    {
                        this._Mode = enMode.Update;
                        return true;
                    }else
                        return false;
                case enMode.Update:
                    return _UpdateUser();

            }

            return false;
        }
        public static bool Delete(int userID)
        {
            if(!clsPassKeys.DeleteAllKeys(userID))
                return false;

            return clsUserData.DeleteUser(userID);
        }
        public static bool IsUserExist(string UserName)
        {
            return clsUserData.IsUserExist(UserName);
        }

    }
}
