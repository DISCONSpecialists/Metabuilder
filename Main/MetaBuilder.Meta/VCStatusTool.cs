using MetaBuilder.BusinessLogic;

namespace MetaBuilder.Meta
{
    //1	CheckedIn
    //2	CheckedOut
    //5	CheckedOutRead
    //4	Locked
    //8	MarkedForDelete
    //7	None
    //3	Obsolete
    //6	PartiallyCheckedIn
    //9	PCI_Revoked
    public class VCStatusTool
    {

        public static bool IsObsoleteOrMarkedForDelete(ObjectAssociation item)
        {
            if (item != null)
                if (item.VCStatusID == 3 || item.VCStatusID == 8)
                    return true;
            return false;
        }
        public static bool IsObsoleteOrMarkedForDelete(MetaObject item)
        {
            if (item != null)
                if (item.VCStatusID == 3 || item.VCStatusID == 8)
                    return true;
            return false;
        }
        public static bool IsObsoleteOrMarkedForDelete(IRepositoryItem item)
        {
            if (item != null)
                if (item.State == VCStatusList.Obsolete || item.State == VCStatusList.MarkedForDelete)
                    return true;
            return false;
        }
        public static bool IsReadOnly(IRepositoryItem item)
        {
            if (item != null)
                if (item.State == VCStatusList.CheckedOutRead)
                    return true;
            return false;
        }

        public static bool DeletableFromDiagram(IRepositoryItem item)
        {
            if (IsObsoleteOrMarkedForDelete(item) || UserHasControl(item))
                return true;
            return false;
        }

        public static bool UserHasControl(IRepositoryItem item)
        {
            if (item == null)
                return false;

            //7 January 2013 - align with manual || item.State == VCStatusList.Locked
            if (item.State == VCStatusList.CheckedOutRead || item.State == VCStatusList.Obsolete || item.State == VCStatusList.CheckedIn || item.State == VCStatusList.Locked)// || item.State == VCStatusList.MarkedForDelete)
                return false;

            //if it hasn't returned then you have control
            return true;

            //return (item.State == VCStatusList.None || item.State == VCStatusList.CheckedOut || item.State == VCStatusList.PCI_Revoked);
        }
    }
}