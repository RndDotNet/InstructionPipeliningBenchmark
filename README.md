# InstructionPipeliningBenchmark

## Results

### IntegerCalculator benchmark

BenchmarkDotNet=v0.13.2, OS=Windows 10 (10.0.19044.1889/21H2/November2021Update)
Intel Core i7-9750H CPU 2.60GHz, 1 CPU, 12 logical and 6 physical cores
.NET SDK=7.0.100-preview.3.22179.4
  [Host]     : .NET 6.0.8 (6.0.822.36306), X64 RyuJIT AVX2
  Job-ZFPXLS : .NET 6.0.8 (6.0.822.36306), X64 RyuJIT AVX2

IterationCount=100  LaunchCount=1  WarmupCount=10

|                         Method |      Mean |     Error |    StdDev | Ratio | RatioSD |
|------------------------------- |----------:|----------:|----------:|------:|--------:|
|                  EnumerableSum | 463.75 ms | 13.274 ms | 38.720 ms |  1.00 |    0.00 |
|                       SumNaive |  75.31 ms |  1.606 ms |  4.711 ms |  0.16 |    0.02 |
|                 SumNaiveUnsafe |  58.95 ms |  0.756 ms |  2.206 ms |  0.13 |    0.01 |
|                SumTrickyUnsafe |  56.53 ms |  0.585 ms |  1.697 ms |  0.12 |    0.01 |
|        SumTrickyAndSmartUnsafe |  57.30 ms |  0.562 ms |  1.648 ms |  0.12 |    0.01 |
|       SumTrickyAndSmartUnsafe2 |  56.78 ms |  0.522 ms |  1.523 ms |  0.12 |    0.01 |
|       SumTrickyAndSmartUnsafe3 |  57.59 ms |  0.801 ms |  2.297 ms |  0.12 |    0.01 |
|       SumTrickyAndSmartUnsafe8 |  57.62 ms |  0.734 ms |  2.142 ms |  0.13 |    0.01 |
| SumTrickyAndSmartUnsafe2Shift2 |  57.28 ms |  0.691 ms |  1.993 ms |  0.12 |    0.01 |
| SumTrickyAndSmartUnsafe2Shift8 |  57.09 ms |  0.599 ms |  1.739 ms |  0.12 |    0.01 |
|                        SumSimd |  52.12 ms |  0.686 ms |  1.991 ms |  0.11 |    0.01 |

### ConditionalCalculator benchmark

BenchmarkDotNet=v0.13.2, OS=Windows 10 (10.0.19044.1889/21H2/November2021Update)
Intel Core i7-9750H CPU 2.60GHz, 1 CPU, 12 logical and 6 physical cores
.NET SDK=7.0.100-preview.3.22179.4
  [Host]     : .NET 6.0.8 (6.0.822.36306), X64 RyuJIT AVX2
  Job-ORBNRO : .NET 6.0.8 (6.0.822.36306), X64 RyuJIT AVX2

IterationCount=100  LaunchCount=1  WarmupCount=10

|                            Method |        Mean |     Error |    StdDev | Ratio | RatioSD |
|---------------------------------- |------------:|----------:|----------:|------:|--------:|
|                     GetEvensCount | 1,024.90 ms | 21.825 ms | 64.353 ms |  1.00 |    0.00 |
|               GetEvensCountNative |   405.56 ms |  9.402 ms | 27.723 ms |  0.40 |    0.04 |
|        GetEvensCountOrderedNative |    90.14 ms |  1.591 ms |  4.690 ms |  0.09 |    0.01 |
| GetEvensCountPatternOrderedNative |    83.65 ms |  1.754 ms |  5.173 ms |  0.08 |    0.01 |
|           GetEvensCountNativeNoIf |   153.19 ms |  4.001 ms | 11.796 ms |  0.15 |    0.02 |
|    GetEvensCountOrderedNativeNoIf |   145.47 ms |  2.754 ms |  8.076 ms |  0.14 |    0.01 |