using System;
using System.Collections.Generic;
using Northwoods.Go;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using MetaBuilder.Core;
using System.Collections.ObjectModel;

namespace MetaBuilder.Graphing.Shapes
{
    [Serializable]
    public class AllocationHandle : GoCollapsibleHandle
    {
        [NonSerialized]
        ContextMenuStrip m = new ContextMenuStrip();

        [NonSerialized]
        private Collection<AllocationItem> items;
        public Collection<AllocationItem> Items
        {
            get
            {
                if (items == null)
                    items = new Collection<AllocationItem>();
                return items;
            }
            set
            {
                items = value;
                SetStyle();
            }
        }

        public void SetStyle()
        {
            if (Items.Count == 0)
                Style = GoCollapsibleHandleStyle.PlusMinus;
            else
                Style = GoCollapsibleHandleStyle.TriangleRight;
        }
        public AllocationHandle()
        {
        }

        public override bool Printable
        {
            get { return false; }
            set { base.Printable = value; }
        }

        public override bool OnSingleClick(GoInputEventArgs evt, GoView view)
        {
            Style = GoCollapsibleHandleStyle.ChevronUp;
            MetaBuilder.Graphing.Containers.GraphView actualView = view as MetaBuilder.Graphing.Containers.GraphView;
            if (actualView == null)
            {
                Style = GoCollapsibleHandleStyle.TriangleRight;
                return true;
            }

            if (evt.Control || Items.Count == 0)
            {
                actualView.OnEditAllocation(this, EventArgs.Empty);
            }
            else
            {
                if (Items.Count == 1)
                {
                    AllocationItem item = Items[0];
                    item.View = actualView;
                    ToolStripMenuItem i = new ToolStripMenuItem();
                    i.Text = item.ToString();
                    i.Tag = item;
                    i.Image = Core.Variables.Instance.GetIcon(item.Path);
                    i.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                    i_Click(i, EventArgs.Empty);
                }
                else
                {
                    m.Items.Clear();

                    //MenuItem xi = new MenuItem();
                    //xi.Text = "Allocated Diagrams";
                    //m.MenuItems.Add(xi);

                    foreach (AllocationItem item in Items)
                    {
                        ToolStripMenuItem i = new ToolStripMenuItem();
                        item.View = actualView;
                        i.Text = item.ToString();
                        //visually missing if not absolute and not relative
                        if (!File.Exists(item.Path) && !File.Exists(strings.GetPath((item.View.Document as MetaBuilder.Graphing.Containers.NormalDiagram).Name) + strings.GetFileNameOnly(item.Path)))
                        {
                            i.ForeColor = Color.Red;
                            i.ToolTipText = "This file is missing. Click it to replace it.";
                        }

                        i.Tag = item;
                        i.Image = Core.Variables.Instance.GetIcon(item.Path);
                        i.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                        i.Click += new EventHandler(i_Click);

                        m.Items.Add(i);
                    }

                    m.Show(actualView.Parent.Parent.Parent, evt.ViewPoint);
                }
            }

            SetStyle();
            //return base.OnSingleClick(evt, view);

            return true;
        }

        private void i_Click(object sender, EventArgs e)
        {
            AllocationItem i = (sender as ToolStripMenuItem).Tag as AllocationItem;

            #region File Location
            string path = i.Path; //absolute
            if (!File.Exists(path))
            {
                if (i.View.Document is MetaBuilder.Graphing.Containers.NormalDiagram)
                {
                    //relative
                    path = strings.GetPath((i.View.Document as MetaBuilder.Graphing.Containers.NormalDiagram).Name) + strings.GetFileNameOnly(i.Path);
                    if (!File.Exists(path))
                    {
                        //absolute
                        path = i.Path;
                        //browse path for new file
                        OpenFileDialog ofd = new OpenFileDialog();
                        ofd.InitialDirectory = strings.GetPath(i.Path);

                        if (i.Type == "GraphFile")
                            ofd.Filter = "MetaBuilder Diagram (*.mdgm)|*.mdgm;*.dgm";
                        else
                            ofd.Filter = "All Files (*.*)|*.*|MetaBuilder Diagram (*.mdgm)|*.mdgm;*.dgm";

                        ofd.FilterIndex = 0;
                        ofd.Title = "File Not Found! Select a file to replace it.";
                        ofd.Multiselect = false;

                        if (ofd.ShowDialog() == DialogResult.OK)
                        {
                            if (ofd.FileName.Length > 0)
                            {
                                //replaced
                                path = ofd.FileName;
                                //replaced (for save)
                                i.Path = ofd.FileName;
                            }
                        }
                    }
                    else
                    {
                        //relative (for save)
                        i.Path = path;
                    }
                }
            }
            #endregion

            if (i.Type == "GraphFile")
            {
                i.View.OnOpenDiagramFromAnother(path, e);
            }
            else
            {
                try
                {
                    Process.Start(path);
                }
                catch (Exception ex)
                {
                    Core.Log.WriteLog("Cannnot open (" + path + ") because of " + Environment.NewLine + ex.ToString());
                }
            }
        }

        public override GoObject CopyObject(GoCopyDictionary env)
        {
            GoObject obj = base.CopyObject(env);
            (obj as AllocationHandle).Items = new Collection<AllocationItem>();
            return obj;
        }
        public override void CopyObjectDelayed(GoCopyDictionary env, GoObject newobj)
        {
            (newobj as AllocationHandle).Items = new Collection<AllocationItem>();
            base.CopyObjectDelayed(env, newobj);
        }

        [Serializable]
        public class AllocationItem
        {
            [NonSerialized]
            private MetaBuilder.Graphing.Containers.GraphView view;
            public MetaBuilder.Graphing.Containers.GraphView View
            {
                get { return view; }
                set { view = value; }
            }

            public string WriteString
            {
                get { return Unique + ";" + Path + ";" + Type; }
            }

            public string Type
            {
                get { return Path.EndsWith("dgm") ? "GraphFile" : "Other"; }
            }

            private string path;
            public string Path
            {
                get { return path; }
                set { path = value; }
            }

            private string unique;
            public string Unique
            {
                get { return unique; }
                set { unique = value; }
            }

            public AllocationItem(string path, string unique)
            {
                Unique = unique;
                Path = path;
            }
            public override string ToString()
            {
                //if (Type == "GraphFile")
                return Core.strings.GetFileNameOnly(Path);

                return Path;
            }

            //private bool isRelative;
            //public bool IsRelative
            //{
            //    get { return isRelative; }
            //    set { isRelative = value; }
            //}

        }

    }
}