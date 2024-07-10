using ClickHouse.Driver.Interop.Columns;

namespace ClickHouse.Driver.Columns;

public class ColumnString : Column<string>, ISupportsNullable
{
    public ColumnString()
    {
        NativeColumn = ColumnStringInterop.chc_column_string_create();
    }

    public ColumnString(nint nativeColumn)
    {
        NativeColumn = nativeColumn;
    }

    public override void Add(string value)
    {
        CheckDisposed();
        ColumnStringInterop.chc_column_string_append(NativeColumn, value);
    }

    public override string this[int index]
    {
        get
        {
            CheckDisposed();
            if ((uint)index >= (uint)Count)
            {
                throw new IndexOutOfRangeException();
            }

            var x = ColumnStringInterop.chc_column_string_at(NativeColumn, (nuint)index);
            return x.ToString();
        }
    }
}