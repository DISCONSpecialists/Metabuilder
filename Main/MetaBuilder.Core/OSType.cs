using System;

namespace MetaBuilder.Core
{
    [Flags]
    public enum OSType
    {
        Unknown = 0,
        Workstation = (int)NativeConstants.VER_NT_WORKSTATION,
        DomainController = (int)NativeConstants.VER_NT_DOMAIN_CONTROLLER,
        Server = (int)NativeConstants.VER_NT_SERVER,
    }
}
