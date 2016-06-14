using System;
using System.Drawing;
using MetaBuilder.Graphing.Shapes.Nodes;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Shapes.General
{
    public class QuickPortHelper
    {
        public enum QuickPortLocation
        {
            Top,
            Bottom,
            Right,
            Left,
            //Single, //Used on nodes which only have a single port
            //Around, //Used on nodes which have a dynamic port
            TopLeft, TopRight,
            RightTop, RightBottom,
            BottomLeft, BottomRight,
            LeftTop, LeftBottom,
            Circumferential
        }

        public static QuickPortLocation tryMinimizeLocation(QuickPortLocation location)
        {
            //switch (location)
            //{
            //    case QuickPortLocation.TopLeft:
            //    case QuickPortLocation.TopRight:
            //        return QuickPortLocation.Top;
            //        break;
            //    case QuickPortLocation.BottomLeft:
            //    case QuickPortLocation.BottomRight:
            //        return QuickPortLocation.Bottom;
            //        break;
            //    case QuickPortLocation.LeftTop:
            //    case QuickPortLocation.LeftBottom:
            //        return QuickPortLocation.Left;
            //        break;
            //    case QuickPortLocation.RightTop:
            //    case QuickPortLocation.RightBottom:
            //        return QuickPortLocation.Right;
            //        break;
            //}
            return location;
        }

        public static QuickPortLocation GetPortEnumFromString(string port)
        {
            QuickPortLocation rtype = (QuickPortLocation)Enum.Parse(typeof(QuickPortLocation), port);
            return rtype;
        }

        public static GoPort ReturnPort(QuickPortLocation location, GoNode node)
        {
            //node is CollapsibleNode || 
            if (node is CollapsingRecordNodeItem)
            {
                //node.Ports.GetEnumerator().Reset = true;
                //try
                //{
                //    return node.Ports.GetEnumerator().Current as GoPort;
                //}
                //catch
                //{
                foreach (GoObject o in node)
                {
                    if (o is GoPort)
                        return o as GoPort;
                }
                return null;
                //}
            }

            foreach (GoPort p in node.Ports)
            {
                if (p is QuickPort)
                    if ((p as QuickPort).PortPosition == location)
                        return p;
            }

            //foreach (var p in from QuickPort p in node.Ports
            //                  where p.PortPosition == location
            //                  select p)
            //{
            //    return p;
            //}

            //Now we have an old stencil here that needs conversion
            return ConvertLegacyStencilPorts(location, node);
        }

        //this will set the ports position enum and on save will keep it
        public static GoPort ConvertLegacyStencilPorts(QuickPortLocation location, GoNode node)
        {
            //TODO Test against all shapes

            //entity is broken, choosing top left
            //activity is broken choosing bottom right
            if (node is Shapes.Nodes.Containers.SubgraphNode)
                return (node as Shapes.Nodes.Containers.SubgraphNode).Port;

            string nodeClass = (node as IMetaNode) != null ? (node as IMetaNode).BindingInfo.BindingClass.ToLower() : "";

            bool l = false;
            bool r = false;
            bool b = false;
            bool t = false;

            if (nodeClass == "mutuallyexclusiveindicator") //this shape has different colors colors
            {
                foreach (QuickPort p in node.Ports)
                {
                    //Round equidistant nodes makes this easy
                    if (!l)
                        if (p.Location.X - 5 <= node.Location.X)
                        {
                            l = true;
                            p.PortPosition = QuickPortLocation.Left;
                            //p.FillEllipseGradient(Color.Green);
                            continue;
                        }

                    if (!t)
                        if (p.Location.Y - 5 <= node.Location.Y)
                        {
                            t = true;
                            p.PortPosition = QuickPortLocation.Top;
                            //p.FillEllipseGradient(Color.Red);
                            continue;
                        }

                    if (!r)
                        if (p.Location.X + 15 >= (node.Location.X + node.Width))
                        {
                            r = true;
                            p.PortPosition = QuickPortLocation.Right;
                            //p.FillEllipseGradient(Color.Blue);
                            continue;
                        }

                    if (!b)
                        if (p.Location.Y + 15 >= (node.Location.Y + node.Height))
                        {
                            b = true;
                            p.PortPosition = QuickPortLocation.Bottom;
                            //p.FillEllipseGradient(Color.Yellow);
                        }
                }
            }
            else
            {
                foreach (GoPort gPort in node.Ports)
                {
                    QuickPort p = new QuickPort();
                    if (gPort is QuickPort)
                    {
                        p = gPort as QuickPort;
                    }
                    else
                        continue;
                    //TODO
                    //SolidBrush theBRUSH = p.Brush. as SolidBrush;
                    //if (theBRUSH.Color != Color.FromArgb(-8355712))
                    //    continue;
                    switch (nodeClass)
                    {
                        #region Class Specific

                        case "dataview":
                            if (!l)
                                if (p.Location.X - 10 <= node.Location.X)
                                {
                                    l = true;
                                    p.PortPosition = QuickPortLocation.Left;
                                    //p.FillEllipseGradient(Color.Green);
                                    continue;
                                }

                            if (!t)
                                if (p.Location.Y - 10 <= node.Location.Y)
                                {
                                    t = true;
                                    p.PortPosition = QuickPortLocation.Top;
                                    //p.FillEllipseGradient(Color.Red);
                                    continue;
                                }

                            if (!r)
                                if (p.Location.X + 10 >= (node.Location.X + node.Width))
                                {
                                    r = true;
                                    p.PortPosition = QuickPortLocation.Right;
                                    //p.FillEllipseGradient(Color.Blue);
                                    continue;
                                }

                            if (!b)
                                if (p.Location.Y + 25 >= (node.Location.Y + node.Height))
                                {
                                    b = true;
                                    p.PortPosition = QuickPortLocation.Bottom;
                                    //p.FillEllipseGradient(Color.Yellow);
                                }
                            break;
                        case "dataview_physical":
                        case "dataview_logical":
                            if (!l)
                                if (p.Location.X - 10 <= node.Location.X)
                                {
                                    l = true;
                                    p.PortPosition = QuickPortLocation.Left;
                                    //p.FillEllipseGradient(Color.Green);
                                    continue;
                                }

                            if (!t)
                                if (p.Location.Y - 10 <= node.Location.Y)
                                {
                                    t = true;
                                    p.PortPosition = QuickPortLocation.Top;
                                    //p.FillEllipseGradient(Color.Red);
                                    continue;
                                }

                            if (!r)
                                if (p.Location.X + 10 >= (node.Location.X + node.Width))
                                {
                                    r = true;
                                    p.PortPosition = QuickPortLocation.Right;
                                    //p.FillEllipseGradient(Color.Blue);
                                    continue;
                                }

                            //There is no bottom one
                            //if (!b)
                            //    if (p.Location.Y + 10 >= (node.Location.Y + node.Height))
                            //    {
                            //        b = true;
                            //        p.PortPosition = QuickPortLocation.Bottom;
                            //        //p.FillEllipseGradient(Color.Yellow);
                            //    }
                            break;
                        case "datatable":
                            if (!l)
                                if (p.Location.X <= node.Location.X)
                                {
                                    l = true;
                                    p.PortPosition = QuickPortLocation.Left;
                                    //p.FillEllipseGradient(Color.Green);
                                    continue;
                                }

                            if (!t)
                                if (p.Location.Y <= node.Location.Y + 5)
                                {
                                    t = true;
                                    p.PortPosition = QuickPortLocation.Top;
                                    //p.FillEllipseGradient(Color.Red);
                                    continue;
                                }

                            if (!r)
                                if (p.Location.X + 10 >= (node.Location.X + node.Width))
                                {
                                    r = true;
                                    p.PortPosition = QuickPortLocation.Right;
                                    //p.FillEllipseGradient(Color.Blue);
                                    continue;
                                }

                            if (!b)
                                if (p.Location.Y + 10 >= (node.Location.Y + node.Height))
                                {
                                    b = true;
                                    p.PortPosition = QuickPortLocation.Bottom;
                                    //p.FillEllipseGradient(Color.Yellow);
                                }
                            break;
                        case "dataentity":
                        case "entity": //cuz its round and has attributes
                            if (!l)
                                if (p.Location.X <= node.Location.X)
                                {
                                    l = true;
                                    p.PortPosition = QuickPortLocation.Left;
                                    //p.FillEllipseGradient(Color.Green);
                                    continue;
                                }

                            if (!t)
                                if (p.Location.Y <= node.Location.Y + 5)
                                {
                                    t = true;
                                    p.PortPosition = QuickPortLocation.Top;
                                    //p.FillEllipseGradient(Color.Red);
                                    continue;
                                }

                            if (!r)
                                if (p.Location.X + 10 >= (node.Location.X + node.Width))
                                {
                                    r = true;
                                    p.PortPosition = QuickPortLocation.Right;
                                    //p.FillEllipseGradient(Color.Blue);
                                    continue;
                                }

                            if (!b)
                                if (p.Location.Y + 10 >= (node.Location.Y + node.Height))
                                {
                                    b = true;
                                    p.PortPosition = QuickPortLocation.Bottom;
                                    //p.FillEllipseGradient(Color.Yellow);
                                }
                            break;
                        case "datasubjectarea":
                        case "dataschema": //Bottom
                            if (!l)
                                if (p.Location.X <= node.Location.X)
                                {
                                    l = true;
                                    p.PortPosition = QuickPortLocation.Left;
                                    //p.FillEllipseGradient(Color.Green);
                                    continue;
                                }

                            if (!t)
                                if (p.Location.Y <= node.Location.Y + 5)
                                {
                                    t = true;
                                    p.PortPosition = QuickPortLocation.Top;
                                    //p.FillEllipseGradient(Color.Red);
                                    continue;
                                }

                            if (!r)
                                if (p.Location.X + 10 >= (node.Location.X + node.Width))
                                {
                                    r = true;
                                    p.PortPosition = QuickPortLocation.Right;
                                    //p.FillEllipseGradient(Color.Blue);
                                    continue;
                                }
                            //Bottom
                            if (!b)
                                if (p.Location.Y + 15 >= (node.Location.Y + node.Height))
                                {
                                    b = true;
                                    p.PortPosition = QuickPortLocation.Bottom;
                                    //p.FillEllipseGradient(Color.Yellow);
                                }
                            break;
                        case "location": //Right
                            if (!l)
                                if (p.Location.X <= node.Location.X)
                                {
                                    l = true;
                                    p.PortPosition = QuickPortLocation.Left;
                                    //p.FillEllipseGradient(Color.Green);
                                    continue;
                                }

                            if (!t)
                                if (p.Location.Y <= node.Location.Y + 5)
                                {
                                    t = true;
                                    p.PortPosition = QuickPortLocation.Top;
                                    //p.FillEllipseGradient(Color.Red);
                                    continue;
                                }
                            //Right
                            if (!r)
                                if (p.Location.X + (node.Width / 4) >= (node.Location.X + node.Width))
                                {
                                    r = true;
                                    p.PortPosition = QuickPortLocation.Right;
                                    //p.FillEllipseGradient(Color.Blue);
                                    continue;
                                }

                            if (!b)
                                if (p.Location.Y + 10 >= (node.Location.Y + node.Height))
                                {
                                    b = true;
                                    p.PortPosition = QuickPortLocation.Bottom;
                                    //p.FillEllipseGradient(Color.Yellow);
                                }
                            break;
                        case "object": //Top, Right
                            if (!l)
                                if (p.Location.X <= node.Location.X)
                                {
                                    l = true;
                                    p.PortPosition = QuickPortLocation.Left;
                                    //p.FillEllipseGradient(Color.Green);
                                    continue;
                                }
                            //Right
                            if (!r)
                                if (p.Location.X + (node.Width / 4) >= (node.Location.X + node.Width))
                                {
                                    r = true;
                                    p.PortPosition = QuickPortLocation.Right;
                                    //p.FillEllipseGradient(Color.Blue);
                                    continue;
                                }
                            //Top
                            if (!t)
                                if (p.Location.Y + 10 >= node.Location.Y)
                                {
                                    t = true;
                                    p.PortPosition = QuickPortLocation.Top;
                                    //p.FillEllipseGradient(Color.Red);
                                    continue;
                                }
                            if (!b)
                                if (p.Location.Y + 10 >= (node.Location.Y + node.Height))
                                {
                                    b = true;
                                    p.PortPosition = QuickPortLocation.Bottom;
                                    //p.FillEllipseGradient(Color.Yellow);
                                }
                            break;
                        case "scenario": //Top
                        case "categoryfactor": //Top
                        case "strategictheme": //Top
                        case "categorysuccessfactor": //Top
                            if (!l)
                                if (p.Location.X <= node.Location.X)
                                {
                                    l = true;
                                    p.PortPosition = QuickPortLocation.Left;
                                    //p.FillEllipseGradient(Color.Green);
                                    continue;
                                }
                            //Top Fixed
                            if (!t)
                                if (p.Location.Y <= node.Location.Y + 10)
                                {
                                    t = true;
                                    p.PortPosition = QuickPortLocation.Top;
                                    //p.FillEllipseGradient(Color.Red);
                                    continue;
                                }

                            if (!r)
                                if (p.Location.X + 10 >= (node.Location.X + node.Width))
                                {
                                    r = true;
                                    p.PortPosition = QuickPortLocation.Right;
                                    //p.FillEllipseGradient(Color.Blue);
                                    continue;
                                }

                            if (!b)
                                if (p.Location.Y + 10 >= (node.Location.Y + node.Height))
                                {
                                    b = true;
                                    p.PortPosition = QuickPortLocation.Bottom;
                                    //p.FillEllipseGradient(Color.Yellow);
                                }
                            break;
                        #endregion

                        case null:
                            break;

                        //Working
                        //Function,Process,Conditional,Iteration,Exception
                        //OrganisationUnit,Role,JobPosition,JobName,GovernanceMechanism
                        //Software,SystemComponent,NetworkComponent,StorageComponent,Peripheral
                        //Competency,Employee,Environment,EnvironmentCategory
                        default:
                            if (!l)
                                if (p.Location.X <= node.Location.X)
                                {
                                    l = true;
                                    p.PortPosition = QuickPortLocation.Left;
                                    //p.FillEllipseGradient(Color.Green);
                                    continue;
                                }

                            if (!t)
                                if (p.Location.Y <= node.Location.Y + 5)
                                {
                                    t = true;
                                    p.PortPosition = QuickPortLocation.Top;
                                    //p.FillEllipseGradient(Color.Red);
                                    continue;
                                }

                            if (!r)
                                if (p.Location.X + 10 >= (node.Location.X + node.Width))
                                {
                                    r = true;
                                    p.PortPosition = QuickPortLocation.Right;
                                    //p.FillEllipseGradient(Color.Blue);
                                    continue;
                                }

                            if (!b)
                                if (p.Location.Y + 10 >= (node.Location.Y + node.Height))
                                {
                                    b = true;
                                    p.PortPosition = QuickPortLocation.Bottom;
                                    //p.FillEllipseGradient(Color.Yellow);
                                }
                            break;
                    }
                }
            }

            GoPort fallbackPort = null;
            foreach (GoPort gPort in node.Ports)
            {
                QuickPort p = null;
                if (gPort is QuickPort)
                {
                    fallbackPort = gPort;
                    p = gPort as QuickPort;
                }
                else
                    continue;

                if (p.PortPosition == location)
                    return p;
            }
            QuickPortLocation lastTryLocation = tryMinimizeLocation(location);
            //Change the location to something else and retry
            if (lastTryLocation != location)
                return ReturnPort(lastTryLocation, node);

            //return one of the ports that we found
            if (fallbackPort != null)
                return fallbackPort;
            //This still has an issue with some old stencils
            //If all else fails return the last port, which is usually the bottom right one
            return node.Ports.GetEnumerator().Current as GoPort;

        }
    }
}