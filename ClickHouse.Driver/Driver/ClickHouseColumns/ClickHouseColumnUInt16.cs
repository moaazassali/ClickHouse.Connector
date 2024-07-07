namespace ClickHouse.Driver.Driver.ClickHouseColumns;

public class ClickHouseColumnUInt16 : ClickHouseColumn<ushort>
{
    public ClickHouseColumnUInt16()
    {
        NativeColumn = Native.Columns.NativeColumnUInt16.chc_column_uint16_create();
    }
    
    public ClickHouseColumnUInt16(nint nativeColumn)
    {
        NativeColumn = nativeColumn;
    }

    public override void Append(ushort value)
    {
        CheckDisposed();
        Native.Columns.NativeColumnUInt16.chc_column_uint16_append(NativeColumn, value);
    }

    public ushort this[int index]
    {
        get
        {
            CheckDisposed();
            return Native.Columns.NativeColumnUInt16.chc_column_uint16_at(NativeColumn, (nuint)index);
        }
    }
}