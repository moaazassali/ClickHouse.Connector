using ClickHouse.Driver.Interop.Columns;

namespace ClickHouse.Driver.Columns;

public class ColumnDateTime : Column, IColumn<uint>
{
    public ColumnDateTime()
    {
        NativeColumn = ColumnDateTimeInterop.chc_column_datetime_create();
    }

    public ColumnDateTime(nint nativeColumn)
    {
        NativeColumn = nativeColumn;
    }

    public void Add(uint value)
    {
        CheckDisposed();
        ColumnDateTimeInterop.chc_column_datetime_append(NativeColumn, value);
    }

    public uint this[int index]
    {
        get
        {
            CheckDisposed();
            if ((uint)index >= (uint)Count)
            {
                throw new IndexOutOfRangeException();
            }

            return (uint)ColumnDateTimeInterop.chc_column_datetime_at(NativeColumn, (nuint)index);
        }
    }
}