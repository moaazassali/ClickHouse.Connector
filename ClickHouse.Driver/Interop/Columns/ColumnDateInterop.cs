using System.Runtime.InteropServices;

namespace ClickHouse.Driver.Interop.Columns;

internal static partial class ColumnDateInterop
{
    [LibraryImport("clickhouse-cpp-c-bridge")]
    public static partial nint chc_column_date_create();

    [LibraryImport("clickhouse-cpp-c-bridge")]
    public static partial void chc_column_date_append(nint column, ushort value);

    [LibraryImport("clickhouse-cpp-c-bridge")]
    public static partial ushort chc_column_date_at(nint column, nuint index);
}