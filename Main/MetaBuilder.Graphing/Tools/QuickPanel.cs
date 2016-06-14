using System.Collections.Generic;
using System;
using System.Drawing;
using System.Windows.Forms;
using MetaBuilder.BusinessFacade.MetaHelper;
using MetaBuilder.BusinessLogic;
using MetaBuilder.DataAccessLayer;
using MetaBuilder.Graphing.Containers;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Graphing.Shapes.General;
using MetaBuilder.Graphing.Shapes.Nodes;
using MetaBuilder.Meta;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Tools
{
    [Serializable]
    public class QuickPanel : GoListGroup
    {
        public List<string> AllClasses;
        public string CurrentClass;
        public string ParentClass;
        private AssociationType association;
        public AssociationType Association
        {
            get
            {
                return association;
            }
            set
            {
                association = value;
                if (associationButton != null)
                    associationButton.setPenColor(association != null);
            }
        }
        private QuickPanelButton associationButton;
        public TList<AssociationType> AllowedAssociations;
        private readonly GraphView MyView;
        public readonly GraphNode thisParentNode;

        public QuickPanel(string className, GraphView view, GraphNode parentNode)
        {
            MyView = view;
            thisParentNode = parentNode;
            InitializePanel();
            ParentClass = className;
            CurrentClass = className;
            AllClasses = new List<string>();
            foreach (ClassAssociation ca in DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ClassAssociationProvider.GetByParentClass(ParentClass))
            {
                if (ca.IsActive == false)
                    continue;

                if (!AllClasses.Contains(ca.ChildClass))
                    AllClasses.Add(ca.ChildClass);
            }
            addChildClassBox();
            addAssociationBox();
            addAddBox();
            addAddChildrenBox();
        }

        private void addAddChildrenBox()
        {
            //Dont add these buttons when the node has not been saved
            if (thisParentNode.MetaObject == null || thisParentNode.MetaObject.pkid == 0)
                return;

            QuickPanelButton btn = new QuickPanelButton("Expand", MyView);
            btn.DragsNode = true;
            Add(btn);
            if (thisParentNode.MetaObject == null || thisParentNode.MetaObject.pkid == 0)
                btn.setPenColor(false);
            else
                btn.setPenColor(true);

            btn = new QuickPanelButton("R-E:D", MyView);
            btn.DragsNode = true;
            Add(btn);
            if (thisParentNode.MetaObject == null || thisParentNode.MetaObject.pkid == 0)
                btn.setPenColor(false);
            else
                btn.setPenColor(true);
        }

        private void InitializePanel()
        {
            //Width = 81;
            //Height = 27;
            Location = new PointF(thisParentNode.Right + 5, thisParentNode.Top);
            Shadowed = true;
            Resizable = false;
            Reshapable = false;
            Editable = false;
            Deletable = true;
            Visible = true;
            Printable = false;
            Copyable = false;
            AutoRescales = false;
            Selectable = true;

            SkipsUndoManager = true;
            //BorderPenColor = Color.SteelBlue;
            //BorderPenWidth = 1;

            //Width += 5;
            //Height += 5;

            //FillSimpleGradient(Color.RoyalBlue);
            //PenColor = Color.Transparent;
        }

        private void addChildClassBox()
        {
            QuickPanelButton btn = new QuickPanelButton("Child Class", MyView);
            btn.DragsNode = true;
            Add(btn);
            btn.setPenColor(true);
        }

        private void addAssociationBox()
        {
            associationButton = new QuickPanelButton("Association", MyView);
            associationButton.DragsNode = true;
            Add(associationButton);
            SetAllowedAssociationsThread();
        }

        private void addAddBox()
        {
            QuickPanelButton btn = new QuickPanelButton("Add Object", MyView);
            btn.DragsNode = true;
            Add(btn);
            btn.setPenColor(true);
        }

        public void SetAllowedAssociationsThread()
        {
            //var t = new System.Threading.Thread(SetAllowedAssociations)
            //{
            //    IsBackground = true
            //};
            //t.Start();
            SetAllowedAssociations();
        }
        public void SetAllowedAssociations()
        {
            AllowedAssociations = new TList<AssociationType>();
            List<ClassAssociation> classAssociations = AssociationManager.Instance.GetAssociationsForParentAndChildClasses(ParentClass, CurrentClass);
            foreach (ClassAssociation t in classAssociations)
            {
                //Application.DoEvents();
                AllowedAssociations.Add(DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.AssociationTypeProvider.GetBypkid(t.AssociationTypeID));
                if (t.IsDefault)
                    Association = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.AssociationTypeProvider.GetBypkid(t.AssociationTypeID);
            }
        }

        public void AddNode(string child, GraphView currentView, GraphNode pNode, AssociationType ass)
        {
            GraphNode newNodeObject = (GraphNode)Core.Variables.Instance.ReturnShape(child);
            if (newNodeObject == null)
            {
                MessageBox.Show("Cannot find a shape for " + child.ToUpper() + ". Please open a stencil with the required shape in order to add a node.", "Stencil Cache");
                return;
            }

            //additive height of child nodes based on X/Y Values
            GraphNode newNode = (GraphNode)newNodeObject.Copy();
            newNode.MetaObject = Loader.CreateInstance(child);
            GoObject lastObject = findLastObject(pNode);
            if (lastObject == null)
                newNode.Position = new PointF(pNode.Right - calculateIndent(), pNode.Bottom);
            else //we dont calculate the indent on the last node because it should logically be indented from before and therefore use the left side of it
                newNode.Position = new PointF(lastObject.Left, lastObject.Bottom);
            currentView.StartTransaction();
            currentView.Document.Add(newNode);
            newNode.BindToMetaObjectProperties();
            linkNodes(pNode, newNode, currentView, ass);
            currentView.FinishTransaction("Add Node From Quick Panel");
        }

        //Finds the last child object favouring the Y Axis
        private GoObject findLastObject(GraphNode node)
        {
            //always go down to the right IE +run/+climb
            float maxX = 0;
            float maxY = 0;

            GoObject rO = null;
            foreach (GoObject o in thisParentNode.Destinations)
            {
                if (o is GraphNode) //exclude links to collapsiblenoderecorditems
                {
                    if (o.Location.X > maxX)
                    {
                        maxX = o.Location.X;
                        rO = o;
                    }
                    //increases in Y value overrides that of X
                    if (o.Location.Y > maxY)
                    {
                        maxY = o.Location.Y;
                        rO = o;
                    }
                }
            }
            return rO;
        }

        private int calculateIndent()
        {
            //Enum Conversion!
            //switch (Core.Variables.Instance.DefaultFromPort)
            //{ 
            //    case QuickPortHelper.QuickPortLocation.BottomLeft.ToString():
            //        return 80;
            //    case QuickPortHelper.QuickPortLocation.BottomRight.ToString():
            //        return 40;
            //    case QuickPortHelper.QuickPortLocation.Bottom.ToString():
            //        return 60;
            //    default:
            //        return 60;
            //}
            //Return the indent requried based on the defaultfromport
            return 60; //this is the indent needed for Bottom
        }

        private static void linkNodes(GoNode pNode, GoNode cNode, GoView currentView, IAssociationType ass)
        {
            MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation defaultFrom = (MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation)Enum.Parse(typeof(MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation), Core.Variables.Instance.DefaultFromPort);
            MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation defaultTo = (MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation)Enum.Parse(typeof(MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation), Core.Variables.Instance.DefaultToPort);
            //int CAid = DataRepository.ClassAssociationProvider.GetByParentClass(ParentClass).First(p => p.ChildClass == CurrentClass && p.AssociationTypeID == Association.pkid).CAid;
            QLink l = QLink.CreateLink(pNode, cNode, ass.pkid, defaultFrom, defaultTo);
            if (l != null)
                currentView.Document.Add(l);
        }

    }
}