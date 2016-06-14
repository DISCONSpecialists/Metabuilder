using System;
using System.Collections.Generic;
using System.Text;
using Northwoods.Go;
using System.Drawing;
using MetaBuilder.Graphing.Containers;
using MetaBuilder.BusinessLogic;
using MetaBuilder.Meta;
using MetaBuilder.DataAccessLayer;
using System.Windows.Forms;

namespace MetaBuilder.Graphing.Shapes
{
    public class LinkController
    {
        #region Points and paths
        public void InsertPoint_Command(GoLabeledLink lnk, Object sender, EventArgs e)
        {
            GraphView v = GoContextMenu.FindView(sender as MenuItem) as GraphView;
            //GoView v = lnk.View;
            if (v != null)
            {
                PointF menupt = v.LastInput.DocPoint;
                GoStroke s = lnk.RealLink;
                int i = s.GetSegmentNearPoint(menupt);
                if (s.PointsCount > 3)
                {
                    if (i < 1)
                        i = 1; // don't add to first segment
                    else if (i >= s.PointsCount - 2)
                        i = s.PointsCount - 3; // don't add to last segment
                }
                PointF closest;
                if (GoStroke.NearestPointOnLine(s.GetPoint(i), s.GetPoint(i + 1), menupt, out closest))
                {
                    v.StartTransaction();
                    s.InsertPoint(i + 1, closest);
                    if (lnk.Orthogonal) // when orthogonal, gotta insert two points
                        s.InsertPoint(i + 1, closest);
                    s.AddSelectionHandles(v.Selection, lnk); // assume selected
                    v.FinishTransaction("inserted point into link stroke");
                }
            }
        }
        public void RemoveSegment_Command(GoLabeledLink lnk, Object sender, EventArgs e)
        {
            GraphView v = GoContextMenu.FindView(sender as MenuItem) as GraphView;
            //GoView v = lnk.View;
            if (v != null)
            {
                PointF menupt = v.LastInput.DocPoint;
                GoStroke s = lnk.RealLink;
                int i = s.GetSegmentNearPoint(menupt);
                v.StartTransaction();
                if (lnk.Orthogonal)
                {
                    // will have at least 7 points
                    // don't remove either first two or last two segments
                    i = Math.Max(i, 2);
                    i = Math.Min(i, lnk.RealLink.PointsCount - 5);
                    if (i > -1)
                    {
                        PointF a = s.GetPoint(i);
                        PointF b = s.GetPoint(i + 1);
                        s.RemovePoint(i);
                        // to maintain orthogonality, gotta remove two points
                        if (s.PointsCount > i)
                        {
                            s.RemovePoint(i);
                            // now fix up following point to maintain orthogonality
                            PointF c = s.GetPoint(i);
                            if (a.X == b.X)
                            {
                                c.Y = a.Y;
                            }
                            else
                            {
                                c.X = a.X;
                            }
                            s.SetPoint(i, c);
                        }
                    }
                }
                else
                {
                    // will have at least 3 points
                    i = Math.Max(i, 1); // don't remove point 0
                    i = Math.Min(i, lnk.RealLink.PointsCount - 2); // don't remove last point
                    s.RemovePoint(i);
                }
                s.AddSelectionHandles(v.Selection, lnk); // assume selected
                v.FinishTransaction("removed point from link stroke");
            }
        }
        #endregion

        public ObjectAssociation GetAssociation(QLink lnk, bool FullyLoaded)
        {
            if (lnk.FromNode is IMetaNode && lnk.ToNode is IMetaNode)
            {
                MetaBase mbFrom = (lnk.FromNode as IMetaNode).MetaObject;
                MetaBase mbTo = (lnk.ToNode as IMetaNode).MetaObject;
                if (mbFrom != null && mbTo != null)
                {
                    TList<ClassAssociation> possibleAssocs = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ClassAssociationProvider.GetByParentClass(mbFrom._ClassName);
                    foreach (ClassAssociation assoc in possibleAssocs)
                    {
                        if (assoc.ChildClass == mbTo._ClassName && assoc.AssociationTypeID == (int)lnk.AssociationType)
                        {
                            ObjectAssociation oassoc = new ObjectAssociation();
                            oassoc.CAid = assoc.CAid;
                            oassoc.ObjectID = mbFrom.pkid;
                            oassoc.ObjectMachine = mbFrom.MachineName;
                            oassoc.ChildObjectID = mbTo.pkid;
                            oassoc.ChildObjectMachine = mbTo.MachineName;
                            if (FullyLoaded)
                            {
                                ObjectAssociationKey oakey = new ObjectAssociationKey(oassoc);
                                ObjectAssociation oaLoaded = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.Get(oakey);
                                return oaLoaded;
                            }
                            return oassoc;
                        }
                    }
                }
            }
            return null;
        }

        public static ObjectAssociation GetAssociation(MetaBase mbFrom, MetaBase mbTo, LinkAssociationType type)
        {
            if (mbFrom != null && mbTo != null)
            {
                TList<ClassAssociation> possibleAssocs = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ClassAssociationProvider.GetByParentClass(mbFrom._ClassName);
                foreach (ClassAssociation assoc in possibleAssocs)
                {
                    if (assoc.ChildClass == mbTo._ClassName && assoc.AssociationTypeID == (int)type)
                    {
                        //ObjectAssociation oassoc = new ObjectAssociation();
                        //oassoc.CAid = assoc.CAid;
                        //oassoc.ObjectID = mbFrom.pkid;
                        //oassoc.ObjectMachine = mbFrom.MachineName;
                        //oassoc.ChildObjectID = mbTo.pkid;
                        //oassoc.ChildObjectMachine = mbTo.MachineName;

                        //ObjectAssociationKey oakey = new ObjectAssociationKey(oassoc);
                        ObjectAssociation oaLoaded = DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.GetByCAidObjectIDChildObjectIDObjectMachineChildObjectMachine(assoc.CAid, mbFrom.pkid, mbTo.pkid, mbFrom.MachineName, mbTo.MachineName);
                        if (oaLoaded != null)
                            return oaLoaded;
                    }
                }
            }
            return null;
        }

        public static void InverseMarkForDelete(ObjectAssociation mainAssociation)
        {
            if (mainAssociation != null)
            {
                if (DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ClassAssociationProvider.GetByCAid(mainAssociation.CAid).AssociationTypeID == (int)AssociationTypeList.Mapping)
                {
                    //get dbtrigger asssociation
                    foreach (ObjectAssociation invAss in DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.GetByObjectIDObjectMachine(mainAssociation.ChildObjectID, mainAssociation.ChildObjectMachine))
                    {
                        if (DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ClassAssociationProvider.GetByCAid(invAss.CAid).AssociationTypeID == (int)AssociationTypeList.Mapping)
                        {
                            if (invAss.ChildObjectID == mainAssociation.ObjectID && invAss.ChildObjectMachine == mainAssociation.ObjectMachine) // && invAss.Machine == "DB-TRIGGER"
                            {
                                invAss.VCStatusID = (int)VCStatusList.MarkedForDelete;
                                DataRepository.Connections[Core.Variables.Instance.ClientProvider].Provider.ObjectAssociationProvider.Save(invAss);
                                return;
                            }
                        }
                    }
                }
            }
        }

        public static ObjectAssociation SaveAssociation(MetaBase mbFrom, MetaBase mbTo, LinkAssociationType type, string Provider)
        {
            if (mbFrom == null || mbTo == null)
                return null;
            if (mbFrom.pkid == 0 || mbTo.pkid == 0)
                return null;
            TList<ClassAssociation> possibleAssocs = DataRepository.Connections[Provider].Provider.ClassAssociationProvider.GetByParentClass(mbFrom._ClassName);
            foreach (ClassAssociation assoc in possibleAssocs)
            {
                if (assoc.ChildClass == mbTo._ClassName && assoc.AssociationTypeID == (int)type)
                {
                    ObjectAssociation oassoc = new ObjectAssociation();
                    oassoc.CAid = assoc.CAid;
                    oassoc.ObjectID = mbFrom.pkid;
                    oassoc.ObjectMachine = mbFrom.MachineName;
                    oassoc.ChildObjectID = mbTo.pkid;
                    oassoc.ChildObjectMachine = mbTo.MachineName;
                    oassoc.State = VCStatusList.None;

                    try
                    {
                        DataRepository.Connections[Provider].Provider.ObjectAssociationProvider.Save(oassoc);
                    }
                    catch (Exception ex)
                    {
                        //the objects are missing from database
                        Core.Log.WriteLog(ex.ToString());
                    }
                    return oassoc;
                }
            }
            return null;
        }
    }
}