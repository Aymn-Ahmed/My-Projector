using BankSystemBusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace BankSystemPresentationLayer
{
    public partial class LoginScreen : Form
    {
        public LoginScreen()
        {
            InitializeComponent();
            
        }

        private enum enLoginStatus { eSuccess = 0, eFailedUserNotFound = 2,
            eFailedPasswordWrong = 3, eUserEmpty = 4, ePasswordEmpty = 5};



        private void chkshowpass_CheckedChanged(object sender, EventArgs e)
        {
            if (chkshowpass.Checked)
            {
                txtPassword.PasswordChar = '\0';
            }

            else
            {
                txtPassword.PasswordChar = '*';
            }
        }

        private void txtUserName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUserName.Text.Trim()))
            {
                e.Cancel = true;
              
                errorProvider1.SetError(txtUserName, "Input Username");
            }

            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtUserName,"");
            }
        }

        private void txtPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPassword.Text.Trim()))
            {
                e.Cancel = true;

                errorProvider1.SetError(txtPassword, "Input Password");
            }

            else
            {

                e.Cancel = false;
                errorProvider1.SetError(txtPassword, "");
            }
        }
        private enLoginStatus _LoginAccount()
        {
            string Username = txtUserName.Text.Trim();
            string Password = txtPassword.Text.Trim ( ) ;
            

            if (string.IsNullOrEmpty(Username))
            {
                return enLoginStatus.eUserEmpty;
            }

            if (string.IsNullOrEmpty(Password))
            {
                return enLoginStatus.ePasswordEmpty;
            }

            clsUsers User = clsUsers.FindUserByUsername(Username);



            if ( User == null )
            {
                return enLoginStatus.eFailedUserNotFound;
            }


            if (!(User.Username == Username))
            {
                return enLoginStatus.eFailedUserNotFound;
            }

            if (!(User.Password == Password))
            {
                return enLoginStatus.eFailedPasswordWrong;
            }

            //clsGlobalUser.CurrentUser = User;
            return enLoginStatus.eSuccess;

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            
            enLoginStatus loginStatus = _LoginAccount();

            if (loginStatus == enLoginStatus.eSuccess)
            {
                MessageBox.Show("Login success");
            }
            else
            {

                MessageBox.Show("Failed");
            }
            
            

        }

  
    }
    }

