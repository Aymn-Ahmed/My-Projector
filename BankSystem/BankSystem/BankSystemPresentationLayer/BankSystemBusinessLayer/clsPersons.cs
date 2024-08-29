using BankSystemDataAccessLayer;
using System;
using System.Data;

namespace BankSystemBusinessLayer
{
    public class clsPersons
    {
        private enum enModePR { AddNew = 0, Update = 1 };
        private enModePR ModePR = enModePR.AddNew;

        public int PersonID { set; get; }
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public string Email { set; get; }
        public string Phone { set; get; }

        public clsPersons()
        {
            this.PersonID = -1;
            this.FirstName = "";
            this.LastName = "";
            this.Email = "";
            this.Phone = "";
            ModePR = enModePR.AddNew;
        }

        protected clsPersons(int personID, string firstName, string lastName, string email, string phone)
        {
            this.PersonID = personID;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email;
            this.Phone = phone;
            //  ModePR = enModePR.Update;
        }

        public static clsPersons Find(int ID)
        {
            string FirstName = "", LastName = "", Email = "", Phone = "";

            if (PersonsData.GetPersonById(ID, ref FirstName, ref LastName, ref Email, ref Phone))
            {
                return new clsPersons(ID, FirstName, LastName, Email, Phone);
            }
            else
            {
                return null;
            }
        }


        private bool _AddNewUser()
        {
            this.PersonID = PersonsData.AddNewPerson(this.FirstName, this.LastName, this.Email, this.Phone);

            return (this.PersonID != -1);
        }

        private bool _UpdatePerson()
        {
            return
                PersonsData.UpdatePerson(this.PersonID, this.FirstName, this.LastName, this.Email, this.Phone);
        }


        public bool Save()
        {
            switch (ModePR)
            {
                case enModePR.AddNew:
                    if (_AddNewUser())
                    {
                        ModePR = enModePR.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enModePR.Update:
                    return _UpdatePerson();

            }
            return false;
        }


        static public bool DeletePerson(int ID)
        {
            return PersonsData.DeletePerson(ID);
        }

        static public bool isPersonExists(int ID)
        {
            return PersonsData.IsPersonExist(ID);
        }




    }
}
