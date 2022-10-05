# InstructionPipeliningBenchmark

## Results

### IntegerSumCalculator benchmark

BenchmarkDotNet=v0.13.2, OS=Windows 10 (10.0.19044.1889/21H2/November2021Update)
Intel Core i7-9750H CPU 2.60GHz, 1 CPU, 12 logical and 6 physical cores
.NET SDK=7.0.100-preview.3.22179.4
  [Host]                          : .NET 6.0.8 (6.0.822.36306), X64 RyuJIT AVX2
  VeryLongRun-.NET 6.0-RyuJit-X64 : .NET 6.0.8 (6.0.822.36306), X64 RyuJIT AVX2

Job=VeryLongRun-.NET 6.0-RyuJit-X64  Jit=RyuJit  Platform=X64
Runtime=.NET 6.0  IterationCount=500  LaunchCount=4
WarmupCount=30

|                          Method |        Mean |     Error |      StdDev |      Median |         Min | Ratio | RatioSD | Code Size |
|-------------------------------- |------------:|----------:|------------:|------------:|------------:|------:|--------:|----------:|
|                   EnumerableSum | 41,229.6 ns | 251.46 ns | 3,412.57 ns | 39,601.8 ns | 35,439.4 ns | 25.06 |    2.42 |     205 B |
|                        SumNaive |  5,412.8 ns |  27.97 ns |   379.18 ns |  5,235.7 ns |  4,821.1 ns |  3.29 |    0.32 |      68 B |
|                  SumNaiveUnsafe |  3,358.3 ns |  18.48 ns |   249.82 ns |  3,456.5 ns |  2,912.4 ns |  2.04 |    0.20 |     106 B |
|                 SumTrickyUnsafe |  2,632.1 ns |  11.49 ns |   155.77 ns |  2,692.2 ns |  2,260.6 ns |  1.60 |    0.14 |     118 B |
|         SumTrickyAndSmartUnsafe |  1,652.1 ns |   8.26 ns |   111.72 ns |  1,713.1 ns |  1,444.4 ns |  1.00 |    0.00 |     144 B |
|        SumTrickyAndSmartUnsafe2 |  2,121.9 ns |   9.25 ns |   125.19 ns |  2,175.1 ns |  1,864.1 ns |  1.29 |    0.12 |     114 B |
|        SumTrickyAndSmartUnsafe8 |  1,561.7 ns |   7.28 ns |    98.67 ns |  1,613.7 ns |  1,379.2 ns |  0.95 |    0.09 |     174 B |
| SumTrickyAndSmartUnsafe2Shift16 |  1,982.7 ns |   5.95 ns |    80.29 ns |  2,009.1 ns |  1,757.1 ns |  1.20 |    0.09 |     156 B |
|                         SumSimd |    669.8 ns |   4.25 ns |    57.72 ns |    633.1 ns |    578.2 ns |  0.41 |    0.05 |     130 B |

### ConditionalCalculator benchmark

BenchmarkDotNet=v0.13.2, OS=Windows 10 (10.0.19044.1889/21H2/November2021Update)
Intel Core i7-9750H CPU 2.60GHz, 1 CPU, 12 logical and 6 physical cores
.NET SDK=7.0.100-preview.3.22179.4
  [Host]                      : .NET 6.0.8 (6.0.822.36306), X64 RyuJIT AVX2
  LongRun-.NET 6.0-RyuJit-X64 : .NET 6.0.8 (6.0.822.36306), X64 RyuJIT AVX2

Job=LongRun-.NET 6.0-RyuJit-X64  Jit=RyuJit  Platform=X64
Runtime=.NET 6.0  IterationCount=100  LaunchCount=3
WarmupCount=15

|                            Method |      Mean |     Error |   StdDev |     Median | Ratio | RatioSD |
|---------------------------------- |----------:|----------:|---------:|-----------:|------:|--------:|
|                     GetEvensCount | 99.595 us | 1.4385 us | 7.472 us | 101.784 us |  1.00 |    0.00 |
|               GetEvensCountNative | 46.372 us | 0.7948 us | 4.128 us |  47.241 us |  0.47 |    0.05 |
|        GetEvensCountOrderedNative |  9.528 us | 0.2603 us | 1.343 us |   9.031 us |  0.10 |    0.02 |
| GetEvensCountPatternOrderedNative |  8.498 us | 0.2105 us | 1.097 us |   8.334 us |  0.09 |    0.01 |
|           GetEvensCountNativeNoIf | 14.783 us | 0.3467 us | 1.807 us |  14.330 us |  0.15 |    0.02 |
|    GetEvensCountOrderedNativeNoIf | 14.604 us | 0.3606 us | 1.808 us |  14.025 us |  0.15 |    0.02 |