using System.Drawing;

namespace MetaBuilder.ResourceManagement
{
    public class Reader
    {
        public static string ApplicationName
        {
            get
            {
                return AppResources.ApplicationName;
            }
        }

        public static Bitmap Logo
        {
            get
            {
                return AppResources.metabuilderSplash;
            }
        }

        public static Bitmap PoweredByDisconSpecialistsImage
        {
            get
            {
                return AppResources.poweredByDisconSpecialists;
            }
        }

        public static Bitmap LoginImage
        {
            get
            {
                return AppResources.LoginImage;
            }
        }

        public static Bitmap PadLockImage
        {
            get
            {
                return AppResources.PadLockImage;
            }
        }


    }
}
