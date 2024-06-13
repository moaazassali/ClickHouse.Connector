using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace ClickHouse.Connector.Native.Structs;

[StructLayout(LayoutKind.Sequential)]
internal struct NativeClientOptions
{
    internal nint Host;
    internal ushort Port;
    internal nint Endpoints;
    internal nuint EndpointsCount;
    internal nint DefaultDatabase;
    internal nint User;
    internal nint Password;
    internal byte RethrowExceptions;
    internal byte PingBeforeQuery;
    internal uint SendRetries;
    internal long RetryTimeout;
    internal int CompressionMethod;
    internal byte TcpKeepalive;
    internal long TcpKeepaliveIdle;
    internal long TcpKeepaliveIntvl;
    internal uint TcpKeepaliveCnt;
    internal byte TcpNodelay;
    internal long ConnectionConnectTimeout;
    internal long ConnectionRecvTimeout;
    internal long ConnectionSendTimeout;
    internal byte BackwardCompatibilityLowcardinalityAsWrappedColumn;
    internal uint MaxCompressionChunkSize;

    // Used to free the memory allocated by this project
    internal void Free(NativeEndpoint[] nativeEndpoints)
    {
        Marshal.FreeHGlobal(Host);

        foreach (var nativeEndpoint in nativeEndpoints)
        {
            nativeEndpoint.Free();
        }

        Marshal.FreeHGlobal(Endpoints);
        Marshal.FreeHGlobal(DefaultDatabase);
        Marshal.FreeHGlobal(User);
        Marshal.FreeHGlobal(Password);
    }
}