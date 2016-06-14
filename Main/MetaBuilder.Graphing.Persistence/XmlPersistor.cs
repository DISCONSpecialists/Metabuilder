using System.Collections;
using System.Collections.Generic;
using System.IO;
using MetaBuilder.BusinessLogic;
using MetaBuilder.Core;
using MetaBuilder.Graphing.Containers;
using MetaBuilder.Graphing.Persistence.XMLPersistence;
using MetaBuilder.Graphing.Persistence.XMLPersistence.Groups;
using MetaBuilder.Graphing.Persistence.XMLPersistence.Links;
using MetaBuilder.Graphing.Persistence.XMLPersistence.Nodes;
using MetaBuilder.Graphing.Persistence.XMLPersistence.Nodes.Repeaters;
using MetaBuilder.Graphing.Persistence.XMLPersistence.Other;
using MetaBuilder.Graphing.Persistence.XMLPersistence.Primitives;
using MetaBuilder.Graphing.Shapes;
using MetaBuilder.Graphing.Shapes.Nodes;
using Northwoods.Go;
using Northwoods.Go.Xml;
using System;

namespace MetaBuilder.Graphing.Persistence
{
    public class XmlPersistor
    {
        #region Methods (4)

        // Public Methods (2) 

        public object DepersistCollection(string filename, FileType ftype)
        {
            //Core.Log.WriteLog("Depersist of " + filename + " has started");
            MetaGraphXMLReader reader = new MetaGraphXMLReader();
            IEnumerable<GoXmlTransformer> transformers = GetTransformerList();
            foreach (GoXmlTransformer transformer in transformers)
            {
                reader.AddTransformer(transformer);
            }
            object rootObject = null;
            switch (ftype)
            {
                case FileType.MindMap:
                    rootObject = new GoView();
                    break;
                case FileType.Diagram:
                    rootObject = new NormalDiagram();
                    break;
                case FileType.Stencil:
                    rootObject = new Stencil();
                    break;
                case FileType.Symbol:
                    rootObject = new Symbol();// GraphNode();
                    break;
            }
            reader.RootObject = rootObject;
            Stream file = null;

            try
            {
                file = File.Open(filename, FileMode.Open);
                reader.Consume(file);
            }
            catch (Exception x)
            {
                Core.Log.WriteLog(x.ToString());
                if (file != null)
                    file.Close();
                return null;
            }

            if (file != null)
                file.Close();

            if (ftype == FileType.Diagram)
            {
                NormalDiagram ndiagram = rootObject as NormalDiagram;
                ndiagram.Name = strings.GetFileNameOnly(filename);
            }

            if (ftype == FileType.Stencil)
            {
                Stencil stencil = rootObject as Stencil;
                stencil.Name = strings.GetFileNameOnly(filename);
            }

            if (ftype == FileType.Symbol)
            {
                Symbol s = rootObject as Symbol;
                s.Name = strings.GetFileNameOnly(filename);
                foreach (GoObject o in rootObject as BaseDocument)
                {
                    if (o is GraphNode)
                    {
                        GraphNode n = o as GraphNode;
                        if (n.HasBindingInfo)
                        {
                            s.ShapeBindingInfo = n.BindingInfo;
                        }
                    }
                }
            }
            //Core.Log.WriteLog("Depersist complete");
            return rootObject;
        }

        public void PersistGoCollection(ICollection document, string filename, FileType ftype)
        {
            try
            {
                MetaGraphXMLWriter writer = new MetaGraphXMLWriter();
                //GoXmlWriter writer = new GoXmlWriter();
                writer.NodesGeneratedFirst = true;
                switch (ftype)
                {
                    case FileType.MindMap:
                        writer.RootElementName = "MetaBuilderMindMap";
                        break;
                    case FileType.Diagram:
                        writer.RootElementName = "MetaBuilderDiagram";
                        break;
                    case FileType.Stencil:
                        writer.RootElementName = "MetaBuilderStencil";
                        break;
                    case FileType.Symbol:
                        writer.RootElementName = "MetaBuilderSymbol";
                        break;
                }
                writer.File_Type = ftype;

                //IEnumerable<GoXmlTransformer> transformers = GetTransformerList();
                foreach (GoXmlTransformer transformer in GetTransformerList())
                {
                    writer.AddTransformer(transformer);
                }

                writer.Objects = document;

                FileStream fileStream = File.Open(filename, FileMode.Create);
                writer.Generate(fileStream);

                fileStream.Close();
            }
            catch (System.Exception x)
            {
#if DEBUG
                throw x;
#endif
                Log.WriteLog(x.ToString());
            }
        }

        // Private Methods (2) 

        /// <summary>
        /// Gets the transformer list.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<GoXmlTransformer> GetTransformerList()
        {
            List<GoXmlTransformer> list = new List<GoXmlTransformer>();
            list.Add(new ValueChainSubgraphTransformer());
            list.Add(new GraphNodeTransformer());
            list.Add(new GraphNodeGridTransformer());
            list.Add(new QuickPortTransformer());
            list.Add(new BoundLabelTransformer());
            list.Add(new MetaBaseTransformer());
            list.Add(new RichTextTransformer());
            list.Add(new ImageTransformer());
            list.Add(new GoRoundedRectangleTransformer());
            list.Add(new ArtefactTransformer());
            list.Add(new ArrowTransformer());
            list.Add(new GradientPolygonTransformer());
            list.Add(new BlockArrowTransformer());
            list.Add(new CubeTransformer());
            list.Add(new CylinderTransformer());
            list.Add(new HexagonTransformer());
            list.Add(new HouseShapeTransformer());
            list.Add(new OctagonTransformer());
            list.Add(new ParalellogramTransformer());
            list.Add(new TrapezoidTransformer());
            list.Add(new TriangleTransformer());
            list.Add(new EllipseTransformer());
            list.Add(new RectangleTransformer());
            list.Add(new GoRectangleTransformer());
            list.Add(new DiamondTransformer());
            list.Add(new CollapsibleNodeTransformer());

            //list.Add(new VerticalNodeTransformer());
            list.Add(new ImageNodeTransformer());

            list.Add(new GoImageTransformer());
            list.Add(new RationaleTransformer());
            list.Add(new QLinkTransformer());
            list.Add(new CollapsibleHandleTransformer());
            list.Add(new CollapsibleRecordNodeItemListTransformer());
            list.Add(new RepeaterSectionTransformer());
            list.Add(new GoGroupTransformer());
            list.Add(new GoTextTransformer());
            list.Add(new SubGraphTransformer());
            list.Add(new ExpandableLabelListTransformer());
            list.Add(new ExpandableLabelTransformer());
            list.Add(new ShapeCollapsibleHandleTransformer());
            list.Add(new AllocationHandleTransformer());
            list.Add(new FishlinkTransformer());
            list.Add(new FileAttachmentTransformer());
            list.Add(new StrokeTransformer());
            list.Add(new HyperlinkTransformer());
            list.Add(new IndicatorLabelTransformer());
            list.Add(new HighlightIndicatorTransformer());
            list.Add(new CommentTransformer());
            list.Add(new BalloonCommentTransformer());
            list.Add(new ShapeGroupTransformer());
            list.Add(new SubGraphNodeTransformer());
            list.Add(new MappingCellTransformer());
            //list.Add(new ValueChainShapeTransformer());
            list.Add(new NonPrintQuickPortTransformer());

            //MindMap
            list.Add(new Persistence.XMLPersistence.MindMap.MindMapNodeTransformer());
            list.Add(new Persistence.XMLPersistence.MindMap.GoLinkTransformer());
            //Legend
            list.Add(new LegendNodeTransformer());
            list.Add(new LegendItemTransformer());
            //VisualNodes
            list.Add(new Persistence.XMLPersistence.Nodes.VisualNodeTransformer());
            list.Add(new Persistence.XMLPersistence.Links.VisualLinkTransformer());
            list.Add(new Persistence.XMLPersistence.Links.VisualPortTransformer());

            return list;
        }

        /// <summary>
        /// Sets the diagram defaults after depersistence
        /// </summary>
        /// <param name="fltrans">The frame layer transformer - used to retrieve diagram values.</param>
        /// <param name="ndiagram">The ndiagram.</param>
        private static void SetupDiagram(FrameLayerTransformer fltrans, NormalDiagram ndiagram)
        {
            ndiagram.FrameLayer = ndiagram.Layers.CreateNewLayerBefore(null);
            ndiagram.FrameLayer.AllowCopy = false;
            ndiagram.FrameLayer.AllowSelect = false;
            ndiagram.FrameLayer.AllowDelete = false;
            fltrans.Frame.Selectable = false;
            fltrans.Frame.Remove();
            ndiagram.FrameLayer.Add(fltrans.Frame);
            ndiagram.Layers.MoveBefore(ndiagram.LinksLayer, ndiagram.FrameLayer);
            ndiagram.Layers.MoveBefore(ndiagram.LinksLayer, ndiagram.DefaultLayer);
            ndiagram.Layers.MoveAfter(null, ndiagram.LinksLayer);
            ndiagram.InitVersionManager();
            ndiagram.DocumentFrame = fltrans.Frame;
            ndiagram.DocumentFrame.Reposition();
            ndiagram.FileType = FileTypeList.Diagram;
            ndiagram.VersionManager = new DocumentVersionManager(FileTypeList.Diagram);

            //ndiagram.VersionManager.CurrentVersion.OriginalFileUniqueIdentifier = fltrans.Frame.
            //ndiagram.UndoManager = new GoUndoManager();
        }

        #endregion Methods
    }
}