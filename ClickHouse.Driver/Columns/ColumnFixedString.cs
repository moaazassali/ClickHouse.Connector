using ClickHouse.Driver.Interop.Columns;

namespace ClickHouse.Driver.Columns;

public class ColumnFixedString : Column<string>, ISupportsNullable
{
    public ColumnFixedString(int size)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(size);
        NativeColumn = ColumnFixedStringInterop.chc_column_fixed_string_create((nuint)size);
    }

    public ColumnFixedString(nint nativeColumn)
    {
        NativeColumn = nativeColumn;
    }

    public override void Add(string value)
    {
        CheckDisposed();
        // we could throw here if string has more bytes than size, but that would require UTF-8 encoding
        // which will again be done when passing value to native method with marshalling
        // in the future, we could do the UTF-8 encoding here and pass nint to native method instead of string
        var nativeResultStatus =
            ColumnFixedStringInterop.chc_column_fixed_string_append(NativeColumn, value);

        if (nativeResultStatus.Code != 0)
        {
            throw new ClickHouseException(nativeResultStatus);
        }
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

            var x = ColumnFixedStringInterop.chc_column_fixed_string_at(NativeColumn, (nuint)index);
            return x.ToString();
        }
    }
}