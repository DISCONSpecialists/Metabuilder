using System;
using System.Drawing;
using System.Windows.Forms;

namespace MetaBuilder.UIControls.GraphingUI
{
    public sealed partial class CustomBox : Form
    {
        //TODO : Store all strings in resources
        private Icon _Icon;

        #region Constructors
        public CustomBox()
        {
            InitializeComponent();
            iconBox.Paint += (iconBox_Paint);
        } //Method Calls
        public CustomBox(string message, string caption, Icon icon, MessageBoxButtons btns)
        {
            InitializeComponent();
            iconBox.Paint += (iconBox_Paint);

            Text = caption;
            labelMessage.Text = message;
            _Icon = icon;

            switch (btns)
            {
                case MessageBoxButtons.OK:
                    buttonCancel.Visible = false;
                    labelCancel.Visible = false;
                    buttonNo.Visible = false;
                    labelNO.Visible = false;
                    break;
                case MessageBoxButtons.YesNo:
                    buttonOK.Text = "Yes";
                    buttonCancel.Visible = false;
                    labelCancel.Visible = false;
                    break;
                case MessageBoxButtons.YesNoCancel:
                    buttonOK.Text = "Yes";
                    break;
            }

        } //Object Calls
        #endregion

        #region Events
        private void iconBox_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawIcon(_Icon, 2, 2);
        }
        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (buttonOK.Text == "Yes")
                DialogResult = DialogResult.Yes;
            else
                DialogResult = DialogResult.OK;
        }
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
        private void buttonNo_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
        }

        //public void Show(string message)
        //{
        //    labelMessage.Text = message;
        //    buttonCancel.Visible = false;
        //    labelCancel.Visible = false;
        //    buttonNo.Visible = false;
        //    labelNO.Visible = false;
        //    StartPosition = FormStartPosition.CenterParent;
        //    FormBorderStyle = FormBorderStyle.FixedToolWindow;
        //    ShowDialog();
        //}

        //public void Show(string message, string caption, Icon icon, MessageBoxButtons btns)
        //{
        //    iconBox.Paint += (iconBox_Paint);

        //    Text = caption;
        //    labelMessage.Text = message;
        //    _Icon = icon;

        //    switch (btns)
        //    {
        //        case MessageBoxButtons.OK:
        //            buttonCancel.Visible = false;
        //            labelCancel.Visible = false;
        //            buttonNo.Visible = false;
        //            labelNO.Visible = false;
        //            break;
        //        case MessageBoxButtons.YesNo:
        //            buttonOK.Text = "Yes";
        //            buttonCancel.Visible = false;
        //            labelCancel.Visible = false;
        //            break;
        //        case MessageBoxButtons.YesNoCancel:
        //            buttonOK.Text = "Yes";
        //            break;
        //    }
        //    StartPosition = FormStartPosition.CenterParent;
        //    FormBorderStyle = FormBorderStyle.FixedToolWindow;
        //    ShowDialog();
        //}

        #endregion

        #region Method Results (Create static reference in variables - Call required method)

        public DialogResult ShowGraphViewClosingSaveDialog(string FileType)
        {
            Text = "Save changes?";
            labelMessage.Text = "The " + FileType + " has changed. Do you want to save changes?";
            _Icon = SystemIcons.Question;

            //switch (MessageBoxButtons.YesNoCancel)
            //{
            //    case MessageBoxButtons.OK:
            //        buttonCancel.Visible = false;
            //        labelCancel.Visible = false;
            //        buttonNo.Visible = false;
            //        labelNO.Visible = false;
            //        break;
            //    case MessageBoxButtons.YesNo:
            //        buttonOK.Text = "Yes";
            //        buttonCancel.Visible = false;
            //        labelCancel.Visible = false;
            //        break;
            //    case MessageBoxButtons.YesNoCancel:
                    buttonOK.Text = "Yes";
            //        break;
            //}

            return ShowDialog();
        }

        #endregion
    }
}