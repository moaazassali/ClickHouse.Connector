using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnostics.Windows.Configs;
using ClickHouse.Driver.Columns;
using ChDriver = ClickHouse.Driver;
using ChAdo = ClickHouse.Ado;
using ChClient = ClickHouse.Client;
using ChOcto = Octonica.ClickHouseClient;

namespace ClickHouse.Driver.Benchmarks;

[MemoryDiagnoser]
// [NativeMemoryProfiler]
[WarmupCount(2)]
[IterationCount(10)]
public class InsertBenchmark
{
    private ChDriver.ClickHouseConnection ChDriverConnection;
    private ChAdo.ClickHouseConnection ChAdoConnection;
    private ChClient.ADO.ClickHouseConnection ChClientConnection;
    private ChOcto.ClickHouseConnection ChOctoConnection;

    private const int Count = 100;


    [GlobalSetup]
    public void Setup()
    {
        var options = new ChDriver.ClickHouseClientOptions
            { Host = "localhost", CompressionMethod = CompressionMethod.None };
        ChDriverConnection = new ChDriver.ClickHouseConnection(options);

        ChAdoConnection = new ChAdo.ClickHouseConnection("Host=localhost;Port=9000;User=default;Password=");
        ChAdoConnection.Open();

        ChClientConnection = new ChClient.ADO.ClickHouseConnection("Compression=false");
        ChClientConnection.Open();

        ChOctoConnection = new ChOcto.ClickHouseConnection("Host=localhost");
        ChOctoConnection.Open();
    }

    [IterationSetup]
    public void IterationSetup()
    {
        ChDriverConnection.Execute("CREATE DATABASE IF NOT EXISTS test");
        ChDriverConnection.Execute("DROP TABLE IF EXISTS test.test");
        ChDriverConnection.Execute(
            "CREATE TABLE test.test (ts DateTime64(3), id UInt32, pressure Float64) ENGINE = Memory");
    }

    [Benchmark(Description = "ClickHouse.Driver: Insert", Baseline = true)]
    public void ChDriverInsert()
    {
        using var ts = new Column<ChDateTime64>();
        using var id = new Column<ChUInt32>();
        using var pressure = new Column<ChFloat64>();

        for (var i = 0; i < Count; i++)
        {
            ts.Add(DateTime.Now.Ticks);
            id.Add((uint)i);
            pressure.Add(1000.0 + i);
        }

        using var block = new ClickHouseBlock();
        block.AppendColumn("ts", ts);
        block.AppendColumn("id", id);
        block.AppendColumn("pressure", pressure);
        ChDriverConnection.Insert("test.test", block);
    }

    [Benchmark(Description = "ClickHouse.Ado: Insert")]
    public void ChAdoInsert()
    {
        var values = Enumerable.Range(0, Count)
            .Select(i => new object[] { DateTime.Now, (uint)i, 1000.0 + i });

        var cmd = ChAdoConnection.CreateCommand();
        cmd.CommandText = "INSERT INTO test.test (ts, id, pressure) VALUES @bulk";
        cmd.Parameters.Add(new ChAdo.ClickHouseParameter
        {
            ParameterName = "bulk",
            Value = values
        });
        cmd.ExecuteNonQuery();
    }

    [Benchmark(Description = "ClickHouse.Client: Insert")]
    public void ChClientInsert()
    {
        var bulkCopy = new ChClient.Copy.ClickHouseBulkCopy(ChClientConnection)
        {
            DestinationTableName = "test.test",
            ColumnNames = new[] { "ts", "id", "pressure" },
            BatchSize = Count,
        };

        Task.Run(() => bulkCopy.InitAsync()).GetAwaiter().GetResult();
        var values = Enumerable.Range(0, Count)
            .Select(i => new object[] { DateTime.Now, (uint)i, 1000.0 + i });
        Task.Run(() => bulkCopy.WriteToServerAsync(values)).GetAwaiter().GetResult();
    }

    [Benchmark(Description = "Octonica.ClickHouseClient: Insert")]
    public void ChOctoInsert()
    {
        var ts = new List<DateTime>();
        var id = new List<uint>();
        var pressure = new List<double>();

        for (var i = 0; i < Count; i++)
        {
            ts.Add(DateTime.Now);
            id.Add((uint)i);
            pressure.Add(1000.0 + i);
        }

        using var writer = ChOctoConnection.CreateColumnWriter("insert into test.test(ts, id, pressure) values");
        writer.WriteTable(new object[] { ts, id, pressure }, id.Count, default);
    }

    // private ClickHouseBlock Block;
    //
    // [IterationSetup(Target = nameof(ChDriverInsert100MArray))]
    // public void IterationSetupChDriverInsert100MArray()
    // {
    //     ChDriverConnection.Execute("CREATE DATABASE IF NOT EXISTS test");
    //     ChDriverConnection.Execute("DROP TABLE IF EXISTS test.test");
    //     ChDriverConnection.Execute(
    //         "CREATE TABLE test.test (ts DateTime64(3), pressure Array(Array(Array(Float64)))) ENGINE = Memory");
    //
    //     var ts = new Column<ChDateTime64>();
    //     var pressure = new Column<ChArray<ChArray<ChArray<ChFloat64>>>>();
    //
    //     for (var i = 0; i < 100_000; i++)
    //     {
    //         ts.Add(DateTime.Now.Ticks);
    //         pressure.Add(new List<ChArray<ChArray<ChFloat64>>>
    //         {
    //             new List<ChArray<ChFloat64>>
    //             {
    //                 new List<ChFloat64> { 1000.0 + i }
    //             }
    //         });
    //     }
    //
    //     var block = new ClickHouseBlock();
    //     block.AppendColumn("ts", ts);
    //     block.AppendColumn("pressure", pressure);
    //     Block = block;
    // }
    //
    // [Benchmark(Description = "ClickHouse.Driver: Insert 100M", Baseline = true)]
    // public void ChDriverInsert100MArray()
    // {
    //     ChDriverConnection.Insert("test.test", Block);
    // }
    //
    //
    // private ChClient.Copy.ClickHouseBulkCopy BulkCopy;
    // private IEnumerable<object[]>? Values;
    // [IterationSetup(Target = nameof(ChClientInsert100MArray))]
    // public void IterationSetupChClientInsert100MArray()
    // {
    //     ChDriverConnection.Execute("CREATE DATABASE IF NOT EXISTS test");
    //     ChDriverConnection.Execute("DROP TABLE IF EXISTS test.test");
    //     ChDriverConnection.Execute(
    //         "CREATE TABLE test.test (ts DateTime64(3), pressure Array(Array(Array(Float64)))) ENGINE = Memory");
    //
    //     BulkCopy = new ChClient.Copy.ClickHouseBulkCopy(ChClientConnection)
    //     {
    //         DestinationTableName = "test.test",
    //         ColumnNames = new[] { "ts", "pressure" },
    //         BatchSize = 100_000,
    //         // MaxDegreeOfParallelism = 5,
    //     };
    //
    //     Task.Run(() => BulkCopy.InitAsync()).GetAwaiter().GetResult();
    //     Values = Enumerable.Range(0, 100_000)
    //         .Select(i => new object[] { DateTime.Now, new[] { new[] { new[] { 1000.0 + i } } } });
    // }
    //
    // [Benchmark(Description = "ClickHouse.Client: Insert 100M")]
    // public void ChClientInsert100MArray()
    // {
    //     Task.Run(() => BulkCopy.WriteToServerAsync(Values)).GetAwaiter().GetResult();
    // }
}