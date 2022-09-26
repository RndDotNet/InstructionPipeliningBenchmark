using BenchmarkDotNet.Running;
using RndDotNet.InstructionPipelining.Benchmark;

BenchmarkRunner.Run<IntegerCalculator>();
BenchmarkRunner.Run<ConditionalCalculator>();
