using BankSystemDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystemBusinessLayer
{
    public class clsUsers 
    {
        public enum enPermissions
        {
            eAll = -1, pListClients = 1, pAddNewClient = 2, pDeleteClient = 4,
            pUpdateClients = 8, pFindClient = 16, pTranactions = 32, pManageUsers = 64,
            pShowLogInRegister = 128
        };
        public enum enModeUser { AddNew = 0, Update = 1 };
        public enModeUser ModeUser = enModeUser.AddNew;
        public string Username { get; set; }
        public string Password { get; set; }
        public int Permissions { get; set; }
        private int PersonID { get; set; }
        public int UserID { get; set; }

        private clsUsers(int UsersId, string username, string password, int permissions, int PersonId)
            
        {
            UserID = UsersId;
            PersonID = PersonId;
            Username = username;
            Password = password;
            Permissions = permissions;
            ModeUser = enModeUser.Update;
        }
        public clsUsers()
        {
            UserID = -1;
            Username = "";
            Password = "";
            Permissions = 0;
            ModeUser = enModeUser.AddNew;
        }

        //public static clsUsers FindUserByID(int userID)
        //{
        //    int Person_ID = -1, permission = 0;
        //    string FirstName = "", LastName = "",
        //        Email = "", Password = "", Phone = "", UserName = "";
        //    if (UsersData.GetUserByUserId(userID, ref Person_ID, ref FirstName, ref LastName, ref Email, ref Phone,
        //       ref UserName, ref Password, ref permission))
        //    {
        //        return new clsUsers(userID, FirstName, LastName, Email, Phone, UserName, Password
        //            , permission, Person_ID);
        //    }
        //    else
        //    {
        //        return null;
        //    }

        //}


        public static clsUsers FindUserByUsername(string username)
        {

            int Person_ID = 0, permission = 0, User_ID = 0;
            string  Password = "";
            
            if (UsersData.GetUserByUserName(ref User_ID, ref Person_ID,
                username, ref Password, ref permission))
            {
               return new clsUsers (User_ID, username, Password
                    , permission, Person_ID);
            }
            return null;


        }



        private bool _AddNewUser()
        {
            int _UserID = -1, _PersonID = -1;

            if (UsersData.AddNewPerson(ref _UserID, ref _PersonID, Username, Password, Permissions))
            {
                UserID = _UserID;
                PersonID = _PersonID;
                ModeUser = enModeUser.Update;
                return true;
            }
            else
            {
                return false;
            }
        }


        private bool _UpdateUser()
        {
            return UsersData.UpdateUser(this.UserID, this.PersonID, this.Username,this.Password,  this.Permissions);
        }

        public static DataView ListUser()
        {
            DataView test = UsersData.GetListUsers().DefaultView;
            return test;

        }

        static public bool DeleteUser(int ID)
        {
            return UsersData.Delete_User(ID);
        }

        static public bool IsUserExist(int ID)
        {
            return UsersData.is_UserExist(ID);
        }

        public bool Save()
        {
            switch (ModeUser)
            {
                case enModeUser.AddNew:
                    if (_AddNewUser())
                    {
                        ModeUser = enModeUser.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enModeUser.Update:
                    return _UpdateUser();

            }
            return false;
        }

        bool CheckAccessPermission(enPermissions Permission)
        {
            if (this.Permissions == (int)enPermissions.eAll)
            {
                return true;
            }

            if (((int)Permission & this.Permissions) == (int)Permission)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

    }
}
