using System;
using System.Collections.Generic;
using MetaBuilder.Graphing.Shapes;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Tools
{
    public class AutoRelinkTool
    {
        #region Methods (3)

        //rewrite
        public static void CalculateAndRelink(GoNode n)
        {
            List<GoPort> fromPorts = new List<GoPort>();
            List<GoPort> toPorts = new List<GoPort>();

            //get all nodes ports
            foreach (GoPort prt in n.Ports)
            {
                GoPort PORT = prt;
                if (PORT.CanLinkFrom() && PORT.Parent == n)
                {
                    fromPorts.Add(PORT);
                }
            }

            //get all to ports of nodes this object is linked to
            foreach (GoPort fromPort in fromPorts)
            {
                GoPort fPORT = fromPort;
                foreach (GoObject o in fPORT.DestinationLinks)
                {
                    if (o is QLink)
                    {
                        QLink lnk = o as QLink;
                        if (lnk.ToNode != null)
                        {
                            foreach (GoPort prtPossibleDestination in lnk.ToNode.Ports)
                            {
                                GoPort tPORT = prtPossibleDestination;
                                if (prtPossibleDestination.CanLinkTo() && prtPossibleDestination.Parent == lnk.ToNode)
                                    toPorts.Add(tPORT);
                            }
                            // Do relink here
                            Relink(lnk, fromPorts, toPorts);
                        }
                    }
                }
            }
        }

        public void RelinkCollection(IGoCollection col)
        {
            foreach (GoObject o in col)
            {
                if (o is GoNode)
                {
                    // Calculate and relink 
                    CalculateAndRelink(o as GoNode);
                }

                if (o is IGoCollection)
                {
                    RelinkCollection(o as IGoCollection);
                }
            }
        }

        private static void Relink(QLink lnk, List<GoPort> fromPorts, List<GoPort> toPorts)
        {
            //bool relink = false;
            //GoPort tNEw = null;
            //GoPort fNEw = null;

            // Calculate the shortest possible route
            foreach (GoPort from in fromPorts)
            {
                GoPort f = from;
                foreach (GoPort to in toPorts)
                {
                    GoPort t = to;

                    GoPort originPort = lnk.FromPort as GoPort;
                    GoPort currentDestinationPort = lnk.ToPort as GoPort;

                    double fExistingLength = Math.Sqrt(Math.Pow(Convert.ToDouble(originPort.Position.X - currentDestinationPort.Position.X), 2) + Math.Pow(Convert.ToDouble(originPort.Position.Y - currentDestinationPort.Position.Y), 2));

                    double fPossibleLinkLength = Math.Sqrt(Math.Pow(Convert.ToDouble(f.Position.X - t.Position.X), 2) + Math.Pow(Convert.ToDouble(f.Position.Y - t.Position.Y), 2));

                    if (fPossibleLinkLength < fExistingLength)
                    {
                        //prevent updates of length to ports that are not 'mid' ports unless they are not quickports
                        //if (t is QuickPort && f is QuickPort)
                        //{
                        //    QuickPort qF = f as QuickPort;
                        //    QuickPort qT = t as QuickPort;

                        //    if (qT.PortPosition != MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation.Circumferential)
                        //    {
                        //        lnk.ToPort = qT;
                        //    }
                        //    if (qF.PortPosition != MetaBuilder.Graphing.Shapes.General.QuickPortHelper.QuickPortLocation.Circumferential)
                        //    {
                        //        lnk.FromPort = qF;
                        //    }
                        //}
                        //else
                        //{
                        lnk.ToPort = t;
                        lnk.FromPort = f;
                        //}
                    }
                }
            }
        }

        // Private Methods (1) 
        /*
                private static bool ShouldRelink(GoPort originPort, GoPort currentDestinationPort,
                                                 GoPort possibleDestinationPort, bool TestDestinationForMainPort)
                {
                    SolidBrush sbrush = possibleDestinationPort.Brush as SolidBrush;
                    if (sbrush != null)
                    {
                        if (sbrush.Color.ToString() != "Color [A=255, R=128, G=128, B=128]")
                            return false;
                    }
           
                }*/

        #endregion Methods
    }
}