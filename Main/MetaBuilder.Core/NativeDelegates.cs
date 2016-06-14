namespace MetaBuilder.Core
{
    internal sealed class NativeDelegates
    {
        internal delegate void OVERLAPPED_COMPLETION_ROUTINE(
            uint dwErrorCode,
            uint dwNumberOfBytesTransferred,
            ref NativeStructs.OVERLAPPED lpOverlapped
            );

        private NativeDelegates()
        {
        }
    }
}
