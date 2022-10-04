# InstructionPipeliningBenchmark

## Results

### IntegerSumCalculator benchmark

BenchmarkDotNet=v0.13.2, OS=Windows 10 (10.0.19044.1889/21H2/November2021Update)
Intel Core i7-9750H CPU 2.60GHz, 1 CPU, 12 logical and 6 physical cores
.NET SDK=7.0.100-preview.3.22179.4
  [Host]                      : .NET 6.0.8 (6.0.822.36306), X64 RyuJIT AVX2
  LongRun-.NET 6.0-RyuJit-X64 : .NET 6.0.8 (6.0.822.36306), X64 RyuJIT AVX2

Job=LongRun-.NET 6.0-RyuJit-X64  Jit=RyuJit  Platform=X64
Runtime=.NET 6.0  IterationCount=100  LaunchCount=3
WarmupCount=15

|                          Method |        Mean |     Error |      StdDev |      Median | Ratio | RatioSD |
|-------------------------------- |------------:|----------:|------------:|------------:|------:|--------:|
|                   EnumerableSum | 45,113.6 ns | 972.06 ns | 5,031.55 ns | 45,410.9 ns | 18.26 |    2.24 |
|                        SumNaive |  7,374.0 ns |  97.64 ns |   508.03 ns |  7,336.7 ns |  3.02 |    0.28 |
|                  SumNaiveUnsafe |  5,423.4 ns | 142.43 ns |   741.04 ns |  5,588.2 ns |  2.23 |    0.28 |
|                 SumTrickyUnsafe |  2,750.8 ns |  35.16 ns |   179.13 ns |  2,653.4 ns |  1.13 |    0.11 |
|         SumTrickyAndSmartUnsafe |  2,446.7 ns |  31.16 ns |   157.07 ns |  2,413.5 ns |  1.00 |    0.00 |
|        SumTrickyAndSmartUnsafe2 |  3,119.9 ns |  60.54 ns |   304.04 ns |  3,038.1 ns |  1.28 |    0.15 |
|        SumTrickyAndSmartUnsafe8 |  2,211.5 ns |  27.59 ns |   143.80 ns |  2,136.1 ns |  0.91 |    0.07 |
| SumTrickyAndSmartUnsafe2Shift16 |  2,345.9 ns |  32.38 ns |   168.75 ns |  2,366.6 ns |  0.96 |    0.09 |
|                         SumSimd |    834.2 ns |  18.22 ns |    94.66 ns |    831.1 ns |  0.34 |    0.04 |

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