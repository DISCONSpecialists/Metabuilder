using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using MetaBuilder.Core;

namespace MetaBuilder.UIControls.Common
{
    public partial class About : Form
    {

		#region Constructors (1) 

        public About()
        {
            InitializeComponent();
            StringBuilder sbInstallType = new StringBuilder();
            if (Variables.Instance.IsServer)
            {
               sbInstallType.Append("Server Edition" + Environment.NewLine);
               //btnRegister.Visible = false;
            }
            if (Variables.Instance.IsDeveloperEdition)
            {
                sbInstallType.Append("Developer Edition" + Environment.NewLine);
                //btnRegister.Visible = false;
            }
            if (Variables.Instance.IsDesktopEdition)
            {
                sbInstallType.Append("Desktop Edition" + Environment.NewLine);
                //btnRegister.Visible = false;
            }
            if (Variables.Instance.IsDemo)
            {
                sbInstallType.Append("Demo Version - " + Variables.Instance.DemoDaysLeft.ToString() + " days left");
                //btnRegister.Visible = true;
            }
            lblInstallType.Text = sbInstallType.ToString();
            lblVersion.Text = "V " + FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location).FileVersion.ToString();
        }

		#endregion Constructors 

		#region Methods (1) 


		// Private Methods (1) 

        private void btnRegister_Click(object sender, EventArgs e)
        {
            RegForm rform = new RegForm();
            rform.Show(MetaBuilder.UIControls.GraphingUI.DockingForm.DockForm);
            Close();
        }


		#endregion Methods 

    }
}