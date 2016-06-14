using System;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using MetaBuilder.BusinessLogic;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Graphing.Utilities;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Containers
{
    [Serializable]
    public class Symbol : BaseDocument
    {
        #region Fields (1) 

        private BindingInfo shapeBindingInfo;

        #endregion Fields 

        #region Constructors (1) 

        public Symbol()
        {
            FileType = FileTypeList.Symbol;
            Initialise();
            GraphNode newNode = new GraphNode();
            Add(newNode);
        }

        #endregion Constructors 

        #region Properties (1) 

        public BindingInfo ShapeBindingInfo
        {
            get
            {
                if (shapeBindingInfo != null)
                    return shapeBindingInfo;

                if (Count > 0)
                {
                    foreach (object o in this)
                    {
                        if (o is GraphNode)
                        {
                            GraphNode n = o as GraphNode;
                            if (n.HasBindingInfo)
                                shapeBindingInfo = n.BindingInfo;
                            return shapeBindingInfo;
                        }
                    }
                }
                return null;
            }
            set { shapeBindingInfo = value; }
        }

        #endregion Properties 

        #region Methods (4) 

        // Public Methods (3) 

        /// <summary>
        /// Just grabs the first available shapecontainer
        /// </summary>
        /// <returns></returns>
        public GraphNode GetShapeContainer()
        {
            foreach (GoObject o in this)
            {
                if (o is GraphNode)
                    return o as GraphNode;
            }
            return null;
        }

        public bool Load(string Filename)
        {
            if (File.Exists(Filename))
            {
                FileStream fstream = File.OpenRead(Filename);
                IFormatter formatter = new BinaryFormatter();
                GraphNode node = (GraphNode) formatter.Deserialize(fstream);
                fstream.Close();
                node.EditMode = true;
                node.UnlockShapes();
                node.Selectable = true;
                node.Editable = true;
                node.Resizable = true;
                node.DragsNode = true;
                node.Movable = true;
                shapeBindingInfo = node.BindingInfo;
                Clear();
                Add(node);
                return true;
            }
            return false;
        }


        public GraphNode GetObjectToSave()
        {
            GraphNode retval = null;
            if (Count > 0)
            {
                PointF leftTopMostPoint = new PointF(2000, 2000);
                foreach (GoObject obj in CopyArray())
                {
                    if (obj is GraphNode)
                    {
                        if (ShapeBindingInfo != null)
                            ((GraphNode) obj).BindingInfo = ShapeBindingInfo;
                        retval = obj.Copy() as GraphNode;
                        retval.LockShapes();
                        foreach (GoObject o in retval)
                        {
                            o.Resizable = false;
                            o.Reshapable = false;
                            o.Deletable = false;
                            o.DragsNode = true;
                        }
                        leftTopMostPoint = GetLeftTopMostPoint(leftTopMostPoint, retval);
                        retval.FormatPorts();
                        retval.Position =
                            new PointF(retval.Position.X - leftTopMostPoint.X,
                                       retval.Position.Y - leftTopMostPoint.Y);
                        retval.Selectable = true;
                        retval.Editable = false;
                        retval.DragsNode = true;
                        retval.Movable = true;
                        retval.Resizable = false;
                        retval.AutoRescales = false;
                        retval.BindingInfo = ShapeBindingInfo;
                    }
                }
            }
            return retval;
        }

        public void Save(string filename)
        {
            GraphNode node = GetObjectToSave();
            StorageManipulator.FileSystemManipulator.SaveObject(node, filename);
            node.Name = filename;
        }


        // Private Methods (1) 

        /// <summary>
        /// Compares a GoObject's top left point to an existing point and returns the top left point of both
        /// </summary>
        /// <param name="leftTopMostPoint"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static PointF GetLeftTopMostPoint(PointF leftTopMostPoint, GoObject obj)
        {
            if (obj.Position.X < leftTopMostPoint.X)
            {
                leftTopMostPoint.X = obj.Position.X;
            }
            if (obj.Position.Y < leftTopMostPoint.Y)
            {
                leftTopMostPoint.Y = obj.Position.Y;
            }
            return leftTopMostPoint;
        }

        #endregion Methods 
    }
}