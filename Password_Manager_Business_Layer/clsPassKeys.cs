using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Password_Manager_Data_Layer;

namespace Password_Manager_Business_Layer
{
    public class clsPassKeys
    {
        enum enMode { Add,Update}
        enMode _Mode = enMode.Add;

        private int _KeyID {  get; set; }
        public int KeyID
        {
            get
            {
                return _KeyID;
            }
        }
        public int UserID { get; set; }
        public string Title {  get; set; }
        public string AccountUser { get; set; }
        public string Password { get; set; }
        public string URL { get; set; }
        public string ImagePath { get; set; }
        public clsPassKeys()
        {
            _KeyID = -1;
            UserID = -1;
            Title = "";
            AccountUser = "";
            Password = "";
            URL = "";
            ImagePath = "";
            _Mode = enMode.Add;
        }
        clsPassKeys(int keyID, int userID, string title, string accountUser, string password, string url, string imagePath)
        {
            this._KeyID = keyID;
            this.UserID = userID;
            this.Title = title;
            this.AccountUser = accountUser;
            this.Password = password;
            this.URL = url;
            this.ImagePath = imagePath;


            this._Mode = enMode.Update;
        }

        public static clsPassKeys Find(int keyID)
        {
            int userID = -1;
            string title = "", accountUser = "", password = "", url = "", imagePath = "";
            if(clsPassKeysData.GetPassKeyInfo(keyID, ref userID, ref title, ref accountUser, ref password, ref url, ref imagePath))
            {
                password = clsSecurityHelper.Decrypt(password,clsUser.Find(userID).UserName);
                return new clsPassKeys(keyID, userID, title, accountUser, password, url, imagePath);
            }
            else
                return null;
        }

        private bool _AddNewKey()
        {
            if (this.Password == "")
                return false;
            this._KeyID = clsPassKeysData.AddNewPassKey(this.UserID, this.Title, this.AccountUser, EncryptThePassword(this.Password), this.URL, this.ImagePath);
            return (this.KeyID != -1);
        }
        private bool _UpdateKeyInfo()
        {
            if (this.Password == "")
                return false;          
            return clsPassKeysData.UpdatePassKey(this._KeyID, this.UserID,this.Title,this.AccountUser, EncryptThePassword(this.Password),this.URL,this.ImagePath);
        }

        
        public bool Save()
        {
            switch(this._Mode)
            {
                case enMode.Add:
                    if (_AddNewKey())
                    {
                        _Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;
                case enMode.Update:
                            return _UpdateKeyInfo();

            }

            return false;   
        }

        public static bool Delete(int KeyID)
        {
            return clsPassKeysData.DeletePassKey(KeyID);
        }
        public static DataTable GetAllPassKeys()
        {
            return clsPassKeysData.GetAllPassKeys();
        }
        public static DataTable GetAllPassKeysByUserID(int userID)
        {
            return clsPassKeysData.GetAllPassKeysByUserID(userID);
        }

        public static bool DeleteAllKeys(int userID)
        {
            return clsPassKeysData.DeleteAllKeys(userID);
        }
        private string EncryptThePassword(string password)
        {
            clsUser _userInfo = clsUser.Find(this.UserID); 

            clsSecurityHelper sh = new clsSecurityHelper(_userInfo.UserName);
            string encPass = sh.Encrypt(password);
            return encPass;
        }
    }
}
