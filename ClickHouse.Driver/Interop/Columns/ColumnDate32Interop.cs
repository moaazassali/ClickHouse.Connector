using System.Runtime.InteropServices;

namespace ClickHouse.Driver.Interop.Columns;

internal static partial class ColumnDate32Interop
{
    [LibraryImport("clickhouse-cpp-c-bridge")]
    public static partial nint chc_column_date32_create();

    [LibraryImport("clickhouse-cpp-c-bridge")]
    public static partial void chc_column_date32_append(nint column, int value);

    [LibraryImport("clickhouse-cpp-c-bridge")]
    public static partial int chc_column_date32_at(nint column, nuint index);
}