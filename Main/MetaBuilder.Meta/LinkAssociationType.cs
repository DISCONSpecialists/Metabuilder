using System;

namespace MetaBuilder.Meta
{
    [Serializable]
    public enum LinkAssociationType
    {
        Auxiliary = 1,
        Classification = 2,
        Decomposition = 3,
        Mapping = 4,
        LeadsTo = 5,
        LocatedAt = 6,
        Dependency = 7,
        SubSetOf = 8,
        Create = 10,
        Read = 11,
        OneWayCurved = 12,
        Start = 13,
        Stop = 14,
        Concurrent = 15,
        NonConcurrent = 16,
        Update = 18,
        DynamicFlow = 19,
        Maintain = 20,
        Suspend = 21,
        Resume = 22,
        Delete = 23,
        Interrupt = 24,
        Synchronise = 25,
        Zero_To_One = 26,
        ZeroOrMore_To_One = 27,
        One_To_Many = 28,
        Many_To_Many = 29,
        ConnectedTo = 30,
        MutuallyExclusiveLink = 31,
        //IsSupplierTo=32
        CreateOLD = 32,
        FunctionalDependency = 33,
        Provide = 34,
        Use = 35,
        EnabledBy = 36,
        One_To_One = 38,
        DynamicDataFlow = 39

    }
}
