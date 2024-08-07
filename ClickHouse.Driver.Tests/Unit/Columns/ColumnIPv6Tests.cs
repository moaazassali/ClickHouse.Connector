using System.Net;
using ClickHouse.Driver.Columns;

namespace ClickHouse.Driver.Tests.Unit.Columns;

public class ColumnIPv6Tests
{
    private readonly Column<ChIPv6> _column = new();

    [Fact]
    public void Add_SingleValue_ReturnsSameValue()
    {
        var value = IPAddress.Parse("2001:0db8:85a3:0000:0000:8a2e:0370:7334");

        _column.Add(value);
        var actual = _column[0];

        Assert.Equal(value, (IPAddress)actual);
    }

    [Fact]
    public void Add_ThrowsException_WhenValueIsNotIPv6()
    {
        var value = IPAddress.Parse("192.168.1.1");

        Assert.Throws<ArgumentException>(() => _column.Add(value));
    }

    [Fact]
    public void Indexer_ThrowsException_WhenIndexIsOutOfRange()
    {
        Assert.Throws<IndexOutOfRangeException>(() => _column[0]);
    }
}