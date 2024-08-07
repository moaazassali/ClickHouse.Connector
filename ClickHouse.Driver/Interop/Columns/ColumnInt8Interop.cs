﻿using System.Runtime.InteropServices;

namespace ClickHouse.Driver.Interop.Columns;

internal static partial class ColumnInt8Interop
{
    [LibraryImport("clickhouse-cpp-c-bridge")]
    public static partial nint chc_column_int8_create();

    [LibraryImport("clickhouse-cpp-c-bridge")]
    public static partial void chc_column_int8_append(nint column, sbyte value);

    [LibraryImport("clickhouse-cpp-c-bridge")]
    public static partial sbyte chc_column_int8_at(nint column, nuint index);
}