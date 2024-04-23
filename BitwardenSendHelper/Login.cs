using BitwardenSendHelper.Utils;

namespace BitwardenSendHelper
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            // Check to see if the user filled in both userid and password.
            if (txtUserId.Text.Length == 0 && txtPassword.TextLength == 0)
            {
                MessageBox.Show("You must enter user id and password to continue.", "Missing Values");
            }

            else
            {
                
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

        public string UserId
        {
            get { return txtUserId.Text; }
        }

        public string Password
        {
            get { return txtPassword.Text; }
        }
    }
}
