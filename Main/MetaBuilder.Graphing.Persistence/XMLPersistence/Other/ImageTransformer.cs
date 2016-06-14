using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using MetaBuilder.Graphing.Shapes;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace MetaBuilder.Graphing.Persistence.XMLPersistence.Other
{
    public class ImageTransformer : BaseGoObjectTransformer
    {

        #region Fields (1)

        private readonly Dictionary<string, QuickImage> ProcessedImages;

        #endregion Fields

        #region Constructors (1)

        public ImageTransformer()
            : base()
        {
            ProcessedImages = new Dictionary<string, QuickImage>();
            TransformerType = typeof(QuickImage);
            ElementName = "image";
            IdAttributeUsedForSharedObjects = true;
            BodyConsumesChildElements = false;
        }

        #endregion Constructors

        #region Methods (4)


        // Public Methods (4) 

        public override object Allocate()
        {
            return new QuickImage();
        }

        public override void ConsumeAttributes(object obj)
        {
            base.ConsumeAttributes(obj);

            QuickImage img = obj as QuickImage;
            string filename = StringAttr("Filename", "");
            //if it does not exist then try find it here
            if (!System.IO.File.Exists(filename))
                if (filename.Contains("MetaData\\Images\\"))
                {
                    //get filename only
                    string name = Core.strings.GetFileNameOnly(filename);
                    filename = System.AppDomain.CurrentDomain.BaseDirectory + "MetaData\\Images\\" + name;
                }

            //TODO : if we still cant find it CANCEL IMAGE LOAD
            //if (System.IO.File.Exists(filename))
            img.Filename = filename;
            //else
            //{
            //    img = null;
            //    return;
            //}

            if (ProcessedImages.ContainsKey(img.Filename))
            {
                img.BitmapObject = ProcessedImages[img.Filename].BitmapObject;
            }
            else
            {
                string strBytes = StringAttr("BMPBytes", "");
                if (strBytes.Length > 0)
                {
                    Byte[] bitmapData = new Byte[strBytes.Length];
                    bitmapData = Convert.FromBase64String(FixBase64ForImage(strBytes));
                    System.IO.MemoryStream streamBitmap = new System.IO.MemoryStream(bitmapData);
                    try
                    {
                        Bitmap bitImage = new Bitmap((Bitmap)Image.FromStream(streamBitmap));
                        if (img.Filename.ToLower().Contains("ico"))
                        {
                            try
                            {
                                bitImage.MakeTransparent();
                            }
                            catch
                            {
                            }
                        }
                        img.BitmapObject = bitImage;
                    }
                    catch
                    {
                        try
                        {
                            img.LoadImage();
                        }
                        catch
                        {
                            return;
                        }
                    }

                    ProcessedImages.Add(img.Filename, img);
                }
            }

        }

        public string FixBase64ForImage(string Image)
        {
            System.Text.StringBuilder sbText = new System.Text.StringBuilder(Image, Image.Length);
            sbText.Replace("\r\n", String.Empty);
            sbText.Replace(" ", String.Empty);
            return sbText.ToString();
        }

        public override void GenerateAttributes(object obj)
        {
            base.GenerateAttributes(obj, true);
            QuickImage img = obj as QuickImage;
            WriteAttrVal("Filename", img.Filename);

            try
            {
                if (img.Filename != null)
                    if (!ProcessedImages.ContainsKey(img.Filename))
                    {
                        ProcessedImages.Add(img.Filename, img);
                        StringBuilder sbBytes = new StringBuilder();
                        byte[] bmpBytes = img.BMPBytes;
                        System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
                        string imageString = Convert.ToBase64String(bmpBytes);
                        WriteAttrVal("BMPBytes", imageString);
                    }
            }
            catch (Exception xx)
            {
                LogEntry logEntry = new LogEntry();
                logEntry.Title = "Image Transform Failed";
                logEntry.Message = xx.ToString();
                Logger.Write(logEntry);
            }
        }


        #endregion Methods

    }
}
