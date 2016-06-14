using System;

namespace MetaBuilder.BusinessLogic
{
    /// <summary>
    /// 
    /// </summary>
    /// <remark>this enumeration contains the items contained in the table AssociationType</remark>
    [Serializable]
    public enum AssociationTypeList
    {
        /// <summary> 
        /// 
        /// </summary>
        [EnumTextValue("")]
        Auxiliary = 1,

        /// <summary> 
        /// 
        /// </summary>
        [EnumTextValue("")]
        Classification = 2,

        /// <summary> 
        /// 
        /// </summary>
        [EnumTextValue("")]
        Decomposition = 3,

        /// <summary> 
        /// 
        /// </summary>
        [EnumTextValue("")]
        Mapping = 4,

        /// <summary> 
        /// 
        /// </summary>
        [EnumTextValue("")]
        LeadsTo = 5,

        /// <summary> 
        /// 
        /// </summary>
        [EnumTextValue("")]
        LocatedAt = 6,

        /// <summary> 
        /// 
        /// </summary>
        [EnumTextValue("")]
        Dependencies = 7,

        /// <summary> 
        /// 
        /// </summary>
        [EnumTextValue("")]
        SubSetOf = 8,

        /// <summary> 
        /// 
        /// </summary>
        [EnumTextValue("")]
        Create = 10,

        /// <summary> 
        /// 
        /// </summary>
        [EnumTextValue("")]
        Read = 11,

        /// <summary> 
        /// 
        /// </summary>
        [EnumTextValue("")]
        OneWayCurved = 12,

        /// <summary> 
        /// 
        /// </summary>
        [EnumTextValue("")]
        Start = 13,

        /// <summary> 
        /// 
        /// </summary>
        [EnumTextValue("")]
        Stop = 14,

        /// <summary> 
        /// 
        /// </summary>
        [EnumTextValue("")]
        Concurrent = 15,

        /// <summary> 
        /// 
        /// </summary>
        [EnumTextValue("")]
        NonConcurrent = 16,

        /// <summary> 
        /// 
        /// </summary>
        [EnumTextValue("")]
        Update = 18,

        /// <summary> 
        /// 
        /// </summary>
        [EnumTextValue("")]
        DynamicFlow = 19,

        /// <summary> 
        /// 
        /// </summary>
        [EnumTextValue("")]
        Maintain = 20,

        /// <summary> 
        /// 
        /// </summary>
        [EnumTextValue("")]
        Suspend = 21,

        /// <summary> 
        /// 
        /// </summary>
        [EnumTextValue("")]
        Resume = 22,

        /// <summary> 
        /// 
        /// </summary>
        [EnumTextValue("")]
        Delete = 23,

        /// <summary> 
        /// 
        /// </summary>
        [EnumTextValue("")]
        Interrupt = 24,

        /// <summary> 
        /// 
        /// </summary>
        [EnumTextValue("")]
        Synchronise = 25,

        /// <summary> 
        /// 
        /// </summary>
        [EnumTextValue("")]
        Zero_To_One = 26,

        /// <summary> 
        /// 
        /// </summary>
        [EnumTextValue("")]
        ZeroOrMore_To_One = 27,

        /// <summary> 
        /// 
        /// </summary>
        [EnumTextValue("")]
        One_To_Many = 28,

        /// <summary> 
        /// 
        /// </summary>
        [EnumTextValue("")]
        Many_To_Many = 29,

        /// <summary> 
        /// 
        /// </summary>
        [EnumTextValue("")]
        ConnectedTo = 30,

        /// <summary> 
        /// 
        /// </summary>
        [EnumTextValue("")]
        MutuallyExclusiveLink = 31,

        /// <summary> 
        /// 
        /// </summary>
        [EnumTextValue("")]
        CreateOld = 32,

        /// <summary> 
        /// 
        /// </summary>
        [EnumTextValue("")]
        FunctionalDependency = 33,

        /// <summary> 
        /// 
        /// </summary>
        [EnumTextValue("")]
        Use = 34,

        /// <summary> 
        /// 
        /// </summary>
        [EnumTextValue("")]
        Provide = 35,

        /// <summary> 
        /// 
        /// </summary>
        [EnumTextValue("")]
        EnabledBy = 36

    }
}