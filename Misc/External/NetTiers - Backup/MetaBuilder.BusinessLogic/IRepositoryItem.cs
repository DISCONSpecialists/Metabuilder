using System.ComponentModel;

namespace MetaBuilder.BusinessLogic
{
    public interface IRepositoryItem
    {
        [Browsable(false)]
        VCStatusList State { get;set;}
        int pkid { get;}
        string VCUser { get;set;}
        string MachineName { get;}
    }
}
