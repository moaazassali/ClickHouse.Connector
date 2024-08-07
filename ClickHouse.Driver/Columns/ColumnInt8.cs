﻿using ClickHouse.Driver.Interop.Columns;

namespace ClickHouse.Driver.Columns;

public class ColumnInt8 : OldColumn, IOldColumn<sbyte>
{
    public ColumnInt8()
    {
        NativeColumn = ColumnInt8Interop.chc_column_int8_create();
    }

    public ColumnInt8(nint nativeColumn)
    {
        NativeColumn = nativeColumn;
    }

    internal override void Add(object value) => Add((sbyte)value);

    public void Add(sbyte value)
    {
        CheckDisposed();
        ColumnInt8Interop.chc_column_int8_append(NativeColumn, value);
    }

    public override object At(int index) => this[index];

    public sbyte this[int index]
    {
        get
        {
            CheckDisposed();
            if ((uint)index >= (uint)Count)
            {
                throw new IndexOutOfRangeException();
            }

            return ColumnInt8Interop.chc_column_int8_at(NativeColumn, (nuint)index);
        }
    }
}