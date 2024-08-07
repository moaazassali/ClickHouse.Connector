﻿using System.Runtime.InteropServices;

namespace ClickHouse.Driver.Interop.Columns;

internal static partial class ColumnInt16Interop
{
    [LibraryImport("clickhouse-cpp-c-bridge")]
    public static partial nint chc_column_int16_create();

    [LibraryImport("clickhouse-cpp-c-bridge")]
    public static partial void chc_column_int16_append(nint column, short value);

    [LibraryImport("clickhouse-cpp-c-bridge")]
    public static partial short chc_column_int16_at(nint column, nuint index);
}