using BenchmarkDotNet.Running;
using RndDotNet.InstructionPipelining.Benchmark;

BenchmarkRunner.Run<IntegerSumCalculator>();
BenchmarkRunner.Run<ConditionalCalculator>();
