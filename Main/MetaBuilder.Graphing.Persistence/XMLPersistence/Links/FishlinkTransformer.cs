using System;
using MetaBuilder.Graphing.Persistence.XMLPersistence.Nodes;
using MetaBuilder.Graphing.Shapes;
using Northwoods.Go;
using MetaBuilder.Graphing.Shapes.Nodes;

namespace MetaBuilder.Graphing.Persistence.XMLPersistence.Links
{
    public class FishlinkTransformer : BaseGoObjectTransformer
    {

        #region Fields (1)

        //private bool validLink;

        #endregion Fields

        #region Constructors (1)

        public FishlinkTransformer()
            : base()
        {
            this.ElementName = "fishLink";
            this.TransformerType = typeof(FishLink);
            this.BodyConsumesChildElements = false;
        }

        #endregion Constructors

        #region Methods (6)

        // Public Methods (5) 

        public override object Allocate()
        {
            FishLink slink = new FishLink();
            return base.Allocate();
        }

        public override void ConsumeAttributes(object obj)
        {
            FishLink link = obj as FishLink;
            link.Initializing = true;
            base.ConsumeAttributes(obj);
        }

        public override void ConsumeObjectFinish(object obj)
        {
            //string visibleSave = StringAttr("Visible", "");

            FishLink link = obj as FishLink;
            link.Initializing = true;
            base.ConsumeObjectFinish(obj);
            //18 october 2013 - Must be selectable
            link.Selectable = true;
            FishRealLink x = (FishRealLink)link.RealLink;
            //18 october 2013 - Must be selectable
            x.Selectable = true;
            String fromid = StringAttr("fromArt", null);
            if (fromid != null)
            {
                IMetaNode anode = this.Reader.FindShared(fromid) as IMetaNode;
                if (anode != null)
                {
                    if (anode is ArtefactNode)
                    {
                        x.FromPort = (anode as ArtefactNode).Port;
                    }
                    else
                    {
                        string fromArtPort = StringAttr("fromArtPort", null);
                        if (fromArtPort != null)
                        {
                            //remember which port i used!?
                            IGoPort fromPort = this.Reader.FindShared(fromArtPort) as IGoPort;
                            if (fromPort != null)
                            {
                                x.FromPort = fromPort;
                            }
                        }
                        string fromArtImageNode = StringAttr("fromArtImageNode", null);
                        if (fromArtImageNode != null)
                        {
                            link.FromArtImageNode = fromArtImageNode;
                        }
                    }
                }
                else
                {
                    Reader.AddDelayedRef(obj, "fromArt", fromid);
                }
            }
            String toid = StringAttr("toLink", null);
            if (toid != null)
            {
                QLink slink = this.Reader.FindShared(toid) as QLink;
                if (slink != null)
                {
                    LinkToSmartLink(x, slink);
                }
                else
                {
                    Reader.AddDelayedRef(obj, "toLink", toid);
                }
            }
            link.Initializing = false;
        }

        public override void GenerateAttributes(object obj)
        {
            base.GenerateAttributes(obj, true);
            FishLink link = obj as FishLink;
            WriteAttrVal("fromArt", this.Writer.MakeShared(link.Artefact));
            if (!(link.Artefact is ArtefactNode))
            {
                WriteAttrVal("fromArtPort", this.Writer.MakeShared(link.ArtefactPort));
                if (link.Artefact is ImageNode)
                {
                    WriteAttrVal("fromArtImageNode", link.Artefact.MetaObject.pkid + "|" + (link.Artefact as ImageNode).Location.ToString());
                }
            }
            WriteAttrVal("toLink", this.Writer.MakeShared(link.ToQLink));
        }

        public override void UpdateReference(object obj, string prop, object referred)
        {
            if (prop == "toLink")
            {
                QLink slink = referred as QLink;
                FishLink link = obj as FishLink;
                FishRealLink x = (FishRealLink)link.RealLink;
                LinkToSmartLink(x, slink);
            }
        }

        // Private Methods (1) 

        private void LinkToSmartLink(FishRealLink x, QLink slink)
        {
            try
            {
                GoGroup grp = slink.MidLabel as GoGroup;
                foreach (GoObject o in grp)
                {
                    if (o is FishLinkPort)
                    {
                        FishLinkPort linkPort = o as FishLinkPort;
                        x.ToPort = linkPort;
                    }
                }
            }
            catch
            {
            }
        }

        #endregion Methods

    }
}