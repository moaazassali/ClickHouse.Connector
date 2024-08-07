using ClickHouse.Driver.Interop.Columns;

namespace ClickHouse.Driver.Columns;

public class ColumnInt64 : OldColumn, IOldColumn<long>
{
    public ColumnInt64()
    {
        NativeColumn = ColumnInt64Interop.chc_column_int64_create();
    }

    public ColumnInt64(nint nativeColumn)
    {
        NativeColumn = nativeColumn;
    }

    internal override void Add(object value) => Add((long)value);

    public void Add(long value)
    {
        CheckDisposed();
        ColumnInt64Interop.chc_column_int64_append(NativeColumn, value);
    }

    public override object At(int index) => this[index];

    public long this[int index]
    {
        get
        {
            CheckDisposed();
            if ((uint)index >= (uint)Count)
            {
                throw new IndexOutOfRangeException();
            }

            return ColumnInt64Interop.chc_column_int64_at(NativeColumn, (nuint)index);
        }
    }
}