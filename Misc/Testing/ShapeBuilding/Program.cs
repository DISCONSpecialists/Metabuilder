using System;
using MetaBuilder.UIControls;

namespace ShapeBuilding
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            TextToHierarchy.VisualHierarchyBuilder vhb = new ShapeBuilding.TextToHierarchy.VisualHierarchyBuilder();
            vhb.ShowDialog();
            return;

            FSDLayout.Form1 ff = new ShapeBuilding.FSDLayout.Form1();
            ff.ShowDialog();
            return;
            MetaBuilder.UIControls.CoreInjector ci = new CoreInjector();
            ci.InjectCoreVariables();
            for (int i = 0; i < 10000; i++)
            {
                MetaBuilder.Meta.MetaBase mb = MetaBuilder.Meta.Loader.CreateInstance("Function");
                mb.Set("Name", "Deon");
                //mb.MachineName = "DEONF";
                mb.MachineName = "DEONF";
                mb.WorkspaceName = MetaBuilder.Core.Variables.Instance.CurrentWorkspaceName;
                mb.WorkspaceTypeId = MetaBuilder.Core.Variables.Instance.CurrentWorkspaceTypeId;
                mb.SaveToRepository(Guid.NewGuid(), "Client");
            }
            return;
            /*Example x = new Example();
            x.ShowDialog(this);
            return;
            MARS.Tester t = new ShapeBuilding.MARS.Tester();
            return;*/

            Form1 f1 = new Form1();
            f1.ShowDialog();
            return;
            //Form1 f1 = new Form1();
            //f1.ShowDialog(this);
            /*
            ShapesToXML stoXml = new ShapesToXML();
            stoXml.ShowDialog(this);
            return;*/

            /*
            NetSpell.SpellChecker.Dictionary.Word w = new NetSpell.SpellChecker.Dictionary.Word("noo");
            NetSpell.SpellChecker.Dictionary.WordDictionary wDict = new NetSpell.SpellChecker.Dictionary.WordDictionary();
            */


            /*
           */
            /*    MetaBuilder.WinUI.ApplicationForms.UserLogin ulogin = new MetaBuilder.WinUI.ApplicationForms.UserLogin();
                ulogin.StartWithWorkspaces = true;
                Application.Run(ulogin);
                return;*/
            /* Application.Run(new DBExplorer.DataBaseExplorer(DBExplorer.DataBaseExplorer.DBExplorer_StartupOptions.All));
             return;
             Application.Run(new RelMan.RelationshipManager());
             return;
             Application.Run(new MyPrintDialog());
             return;

             Form1 f1 = new Form1();
             f1.ShowDialog(this);
            
             DBExplorer.DataBaseExplorer frm1 = new DBExplorer.DataBaseExplorer(DBExplorer.DataBaseExplorer.DBExplorer_StartupOptions.All);
             frm1.ShowDialog(this);
             */

            /*
            Repository.ObjectManager oman = new ShapeBuilding.Repository.ObjectManager();
            Application.Run(oman);*/
            /*   UserPermissions up = new UserPermissions();
               Application.Run(up);*/
        }
    }
}
