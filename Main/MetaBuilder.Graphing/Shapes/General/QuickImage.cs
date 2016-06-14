using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Xml.Serialization;
using MetaBuilder.Graphing.Containers;
using MetaBuilder.Graphing.Helpers;
using Northwoods.Go;

namespace MetaBuilder.Graphing.Shapes
{
    [Serializable]
    public class QuickImage : GoImage
    {
        #region Fields (4)

        private Bitmap bitmapObject;
        private string filename;
        private Icon iconObject;
        [NonSerialized]
        private ImageFormat imageFormat;

        #endregion Fields

        #region Constructors (2)

        public QuickImage(Bitmap bmpToSerialize, string FileName)
        {
            bitmapObject = bmpToSerialize;
            Width = bmpToSerialize.Width;
            Height = bmpToSerialize.Height;
            filename = FileName;
        }

        //XmlSerializer can only serialize classes that have default constructor
        public QuickImage()
        {
            bitmapObject = null;
            iconObject = null;
        }

        #endregion Constructors

        #region Properties (6)

        [XmlIgnore]
        public Bitmap Bitmap
        {
            get { return bitmapObject; }
        }

        public Bitmap BitmapObject
        {
            get { return bitmapObject; }
            set { bitmapObject = value; }
        }

        public byte[] BMPBytes
        {
            get
            {
                if (iconObject != null || bitmapObject != null)
                {
                    StoreImageFormat();
                    MemoryStream memStream = new MemoryStream();

                    if (bitmapObject == null)
                    {
                        ImageFormat = ImageFormat.Bmp;
                        bitmapObject = Image as Bitmap;
                    }
                    ImageFormat = ImageFormat.Bmp;
                    bitmapObject.Save(memStream, ImageFormat);
                    int ImageSize = Convert.ToInt32(memStream.Length);
                    byte[] ImageBytes = new byte[ImageSize];
                    memStream.Position = 0;
                    memStream.Read(ImageBytes, 0, ImageSize);
                    memStream.Close();
                    return ImageBytes;
                }
                return new byte[] { };
            }
            set
            {
                if (Document is BaseDocument)
                {
                    BaseDocument icontainer = Document as BaseDocument;
                    bitmapObject = icontainer.EmbeddedImages[filename].Image as Bitmap;
                    return;
                }
                bitmapObject = new Bitmap(new MemoryStream(value));

                StoreImageFormat();
            }
        }

        public string Filename
        {
            get { return filename; }
            set
            {
                filename = value;
                ExtractImage();
            }
        }

        [XmlIgnore]
        public new Image Image
        {
            get
            {
                if (iconObject != null)
                    return iconObject.ToBitmap();
                ;
                if (bitmapObject != null)
                    return bitmapObject;
                return null;
            }
        }

        public ImageFormat ImageFormat
        {
            get { return imageFormat; }
            set { imageFormat = value; }
        }

        #endregion Properties

        #region Methods (5)

        // Public Methods (1) 

        public override Image LoadImage()
        {
            try
            {
                ExtractImage();
                return Image;
            }
            catch
            {
                try
                {
                    return TryLoadingFromPhysicalFileOnDisk();
                }
                catch
                {
                    return null;
                }
            }
            return base.LoadImage();
        }


        // Private Methods (4) 

        private void ExtractImage()
        {
            if (Document is BaseDocument)
            {
                BaseDocument icontainer = Document as BaseDocument;
                if (icontainer.EmbeddedImages == null)
                {
                    icontainer.EmbeddedImages = new SerializableDictionary<string, QuickImage>();
                }
                if (filename != null)
                    if (icontainer.EmbeddedImages.ContainsKey(filename))
                    {
                        GetImageObject();
                    }
                    else
                    {
                        LoadFromFile();
                    }
            }
        }

        private void GetImageObject()
        {
            BaseDocument icontainer = Document as BaseDocument;
            if (icontainer.EmbeddedImages[filename].bitmapObject != null)
            {
                bitmapObject = icontainer.EmbeddedImages[filename].bitmapObject;
            }
            else
            {
                iconObject = icontainer.EmbeddedImages[filename].iconObject;
            }
        }

        private void LoadFromFile()
        {
            BaseDocument icontainer = Document as BaseDocument;
            if (filename.ToLower().EndsWith(".ico"))
            {
                filename = filename.Replace("E:\\", "C:\\");
                string filename2 = filename.Replace("C:\\", "E:\\");
                if (File.Exists(filename))
                {
                    iconObject = new Icon(filename);
                }
                else if (File.Exists(filename2))
                {
                    iconObject = new Icon(filename2);
                }
            }
            else
            {
                filename = filename.Replace("E:\\", "C:\\");
                if (File.Exists(filename))
                    bitmapObject = new Bitmap(filename);
                else
                {
                    filename = filename.Replace("C:\\", "E:\\");
                    if (File.Exists(filename))
                        bitmapObject = new Bitmap(filename);
                }
            }
            if (icontainer.EmbeddedImages.ContainsKey(filename))
            {
                icontainer.EmbeddedImages[filename] = this;
            }
            else
            {
                icontainer.EmbeddedImages.Add(filename, this);
            }
            if (Width == 10)
            {
                Width = Image.Width;
                Height = Image.Height;
            }
        }

        private Image TryLoadingFromPhysicalFileOnDisk()
        {
            Image img;
            if (filename != null)
            {
                if (File.Exists(filename))
                {
                    img = Image.FromFile(filename);
                    ExtractImage();
                    return img;
                }

                filename = filename.Replace("E:", "C:");
                if (File.Exists(filename))
                {
                    img = Image.FromFile(filename);
                    return img;
                }

                filename = filename.Replace("E:", "C:");
                if (File.Exists(filename))
                {
                    img = Image.FromFile(filename.Replace("E:", "C:"));
                    return img;
                }
                return null;
            }
            return null;
        }

        #endregion Methods

        #region Helpers

        private void StoreImageFormat()
        {
            string fnameLowerCase = filename.ToLower();
            if (fnameLowerCase.EndsWith(".bmp"))
            {
                ImageFormat = ImageFormat.Bmp;
                return;
            }
            if (fnameLowerCase.EndsWith(".jpg") || fnameLowerCase.EndsWith(".jpeg"))
            {
                ImageFormat = ImageFormat.Jpeg;
                return;
            }
            if (fnameLowerCase.EndsWith(".png"))
            {
                ImageFormat = ImageFormat.Png;
                return;
            }
            if (fnameLowerCase.EndsWith(".ico"))
            {
                ImageFormat = ImageFormat.Icon;
                return;
            }
            if (fnameLowerCase.EndsWith(".wmf"))
            {
                ImageFormat = ImageFormat.Wmf;
                return;
            }
            if (fnameLowerCase.EndsWith(".gif"))
            {
                ImageFormat = ImageFormat.Gif;
                return;
            }
        }

        #endregion
    }

    /*[Serializable]
    public class QuickImage:GoImage
    {

		#region Fields (1) 

        private string filename;

		#endregion Fields 

		#region Properties (1) 

        public string Filename
        {
            get { return filename; }
            set { filename = value; }
        }

		#endregion Properties 

		#region Methods (1) 


		// Public Methods (1) 

        public override System.Drawing.Image LoadImage()
        {
            Image img;
            if (filename != null)
            {
                try
                {
                    if (System.IO.File.Exists(filename))
                    {
                        img = System.Drawing.Image.FromFile(filename);
                        
                        return img;
                    }
                    else
                    {
                        img = System.Drawing.Image.FromFile(filename.Replace("E:", "C:"));
                        return img;
                    }
                }
                catch
                {
                    img = System.Drawing.Image.FromFile(filename.Replace("E:", "C:"));
                    return img;
                }
            }
            else
            {
                return null;
            }
        }


		#endregion Methods 

    }*/
}