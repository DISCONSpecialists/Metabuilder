using System;
using System.Drawing;
using System.Windows.Forms;
using MetaBuilder.DataAccessLayer;
using MetaBuilder.BusinessLogic;
using MetaBuilder.Graphing.Containers;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Tools
{
    [Serializable]
    internal class QuickPanelButton : GoButton
    {
        public GraphView MyView;
        private readonly GoContextMenu cMenu;
        private GoShape s = new GoRectangle();
        //changes border color to visually display association selectedness
        public void setPenColor(bool on)
        {
            if (on)
                s.Brush = Brushes.Green;//new SolidBrush(Color.Green);
            //s.FillSimpleGradient(Color.LightGreen, Color.Green, GoObject.MiddleLeft);
            else
                s.Brush = Brushes.Red;//new SolidBrush(Color.Red);
            //s.FillSimpleGradient(Color.Red, Color.DarkRed, GoObject.MiddleLeft);

        }
        public QuickPanelButton(string text, GraphView view)
        {
            Text = text;
            Visible = true;
            AutoRescales = false;
            Deletable = false;

            MyView = view;
            Size = new SizeF(55, 25);

            Background = null;
            //s.Pen.Color = Color.Black;
            //s.Pen.Width = 1;
            //s.PenColor = Color.Black;

            //s.PenWidth = 1;
            Background = s;

            Label.Bold = false;
            Label.FontSize = 8f;
            //Label.BackgroundColor = Color.Transparent;
            Label.TransparentBackground = true;
            Label.TextColor = Color.White;
            Label.AutoResizes = false;
            Label.AutoRescales = false;
            Label.Width = 55;

            //remove the original button thing which looks lame, allows us more control over objects shape then :P
            cMenu = new GoContextMenu(MyView);
            Action += button_Clicked;

            s.Brush = Brushes.Red;//new SolidBrush(Color.Red);

            //s.FillSimpleGradient(Color.Red, Color.DarkRed, GoObject.MiddleTop);
        }

        private void button_Clicked(object sender, GoInputEventArgs evt)
        {
            QuickPanel p = (Parent as QuickPanel);
            cMenu.MenuItems.Clear();
            switch (Text)
            {
                //Class
                case "Child Class":
                    if (p != null)
                    {
                        cMenu.MenuItems.Add("Current : " + p.CurrentClass);
                        cMenu.MenuItems.Add("-");
                        foreach (string allClass in p.AllClasses)
                        {
                            cMenu.MenuItems.Add(allClass, menuItem_Click);
                        }
                        cMenu.Show(MyView, evt.MouseEventArgs.Location);
                    }
                    break;
                //Association
                case "Association":
                    if (p != null && p.AllowedAssociations != null)
                    {
                        if (p.AllowedAssociations != null && p.AllowedAssociations.Count > 0)
                        {
                            if (p.Association != null)
                            {
                                cMenu.MenuItems.Add("Current : " + p.Association.Name);
                                cMenu.MenuItems.Add("-");
                            }
                            foreach (AssociationType type in p.AllowedAssociations)
                            {
                                cMenu.MenuItems.Add(type.Name, menuItem_Click);
                            }
                            cMenu.Show(MyView, evt.MouseEventArgs.Location);
                        }
                    }
                    break;
                case "Expand":
                    if (p.thisParentNode.MetaObject == null || p.thisParentNode.MetaObject.pkid == 0)
                    {
                        Visible = false;
                        return;
                    }
                    MyView.ViewController.RED(p.thisParentNode, false, null);
                    break;
                case "R-E:D":
                    if (p.thisParentNode.MetaObject == null || p.thisParentNode.MetaObject.pkid == 0)
                    {
                        Visible = false;
                        return;
                    }
                    MyView.ViewController.RED(p.thisParentNode, true, null);
                    break;
                default:
                    addObject(p.Association != null);
                    break;
            }
        }

        private void menuItem_Click(object sender, EventArgs e)
        {
            if (!(sender is MenuItem))
                return;
            if (!(Parent is QuickPanel))
                return;
            QuickPanel p = Parent as QuickPanel;
            switch (Text)
            {
                case "Child Class":
                    //change currentclass if its different
                    if (p.CurrentClass != (sender as MenuItem).Text)
                    {
                        p.CurrentClass = (sender as MenuItem).Text;
                        p.Association = null;
                        //rebuild associations based on choice
                        p.SetAllowedAssociationsThread();
                    }
                    break;
                case "Association":
                    //change association
                    if (p.Association == null || p.Association.Name != (sender as MenuItem).Text)
                        p.Association = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.AssociationTypeProvider.GetByName((sender as MenuItem).Text);
                    break;
            }
        }

        private void addObject(bool defaulted)
        {
            if (!(Parent is QuickPanel))
                return;
            QuickPanel p = Parent as QuickPanel;
            if (p.Association != null)
            {
                p.AddNode(p.CurrentClass, MyView, p.thisParentNode, p.Association);
            }
            else
            {
                if (defaulted)
                {
                    MessageBox.Show("The default association has not been set." + Environment.NewLine + "Use the association button to select one.", "Unable to create default node", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                //default association
                foreach (ClassAssociation classAssociation in DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ClassAssociationProvider.GetByChildClass(p.CurrentClass))
                {
                    if (classAssociation.ParentClass != p.ParentClass)
                        continue;

                    if (classAssociation.IsDefault)
                    {
                        p.Association = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.AssociationTypeProvider.GetBypkid(classAssociation.AssociationTypeID);
                        break;
                    }
                }
                if (!defaulted)
                    addObject(true);
            }
        }
    }
}