using ClickHouse.Driver.Interop.Columns;

namespace ClickHouse.Driver.Columns;

public class ColumnUInt64 : Column<ulong>, ISupportsNullable
{
    public ColumnUInt64()
    {
        NativeColumn = ColumnUInt64Interop.chc_column_uint64_create();
    }
    
    public ColumnUInt64(nint nativeColumn)
    {
        NativeColumn = nativeColumn;
    }

    public override void Add(ulong value)
    {
        CheckDisposed();
        ColumnUInt64Interop.chc_column_uint64_append(NativeColumn, value);
    }

    public override ulong this[int index]
    {
        get
        {
            CheckDisposed();
            if ((uint)index >= (uint)Count)
            {
                throw new IndexOutOfRangeException();
            }

            return ColumnUInt64Interop.chc_column_uint64_at(NativeColumn, (nuint)index);
        }
    }
}