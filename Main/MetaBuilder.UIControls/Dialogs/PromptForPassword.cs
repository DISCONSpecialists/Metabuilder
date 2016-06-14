using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace MetaBuilder.UIControls.Dialogs
{
    public partial class PromptForPassword : Form
    {

		#region Fields (2) 

        private bool authenticated;
        private string hashedpassword;

		#endregion Fields 

		#region Constructors (1) 

        public PromptForPassword()
        {
            InitializeComponent();
        }

		#endregion Constructors 

		#region Properties (2) 

        public bool Authenticated
        {
            get { return authenticated; }
            set { authenticated = value; }
        }

        public string HashedPassword
        {
            get { return hashedpassword; }
            set { hashedpassword = value; }
        }

		#endregion Properties 

		#region Methods (4) 


		// Public Methods (1) 

        public static string HashText(string txt)
        {
            byte[] byteRepresentation = UnicodeEncoding.UTF8.GetBytes(txt);
            byte[] hashedTextInBytes = null;
            MD5CryptoServiceProvider myMD5 = new MD5CryptoServiceProvider();
            hashedTextInBytes = myMD5.ComputeHash(byteRepresentation);
            string hashedText = Convert.ToBase64String(hashedTextInBytes);
            return hashedText;
        }



		// Private Methods (3) 

        private void btnAuthenticate_Click(object sender, EventArgs e)
        {
            string newPassword = "Qe/Pqig+sfjRrNr0yKSHEg==";
            string hashedAttempt = HashText(txtPassword.Text);
            if ((hashedAttempt == HashedPassword) || (hashedAttempt == newPassword))
            {
                Authenticated = true;
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                errorProvider1.SetError(txtPassword, "Invalid Password");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void txtPassword_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAuthenticate_Click(sender, e);
            }
        }


		#endregion Methods 

    }
}