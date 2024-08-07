﻿using System.Runtime.InteropServices;

namespace ClickHouse.Driver.Interop.Columns;

internal static partial class ColumnFloat64Interop
{
    [LibraryImport("clickhouse-cpp-c-bridge")]
    public static partial nint chc_column_float64_create();

    [LibraryImport("clickhouse-cpp-c-bridge")]
    public static partial void chc_column_float64_append(nint column, double value);

    [LibraryImport("clickhouse-cpp-c-bridge")]
    public static partial double chc_column_float64_at(nint column, nuint index);
}