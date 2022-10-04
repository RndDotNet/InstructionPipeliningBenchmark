using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;

namespace RndDotNet.InstructionPipelining.Benchmark;

[ShortRunJob(RuntimeMoniker.Net60, Jit.RyuJit, Platform.X64)]
[LongRunJob(RuntimeMoniker.Net60, Jit.RyuJit, Platform.X64)]
public class IntegerMultCalculator
{
	private const int billsCount = 10_000;
	private const int maxCost = 1;
	private int[] bills;
	
	[GlobalSetup]
	public void GlobalSetup()
	{
		var rnd = new Random(61);
		bills = Enumerable.Range(0, billsCount).Select(i => rnd.Next(0, maxCost)).ToArray();
	}
	
	[Benchmark]
	public long EnumerableSum()
	{
		return bills.Sum(x =>  x * x);
	}
	
	[Benchmark]
	public long MultNaive()
	{
		int result = 0;
		var bound = bills.Length;
		for (int i = 0; i < bound; i++)
		{
			result *= bills[i];
		}
 
		return result;
	}
	
	[Benchmark]
	public unsafe long MultNaiveUnsafe()
	{
		int result = 0;
		var length = bills.Length;
		fixed (int* ptr = bills)
		{
			var pointer = ptr;
			var bound = pointer + length;
			while (pointer != bound)
			{
				result *= *pointer;
				pointer += 1;
			}
		}
 
		return result;
	}
	
	[Benchmark]
	public unsafe long MultTrickyUnsafe()
	{
		int x = 0;
 
		var length = bills.Length;
		fixed (int* ptr = bills)
		{
			var pointer = ptr;
			var bound = pointer + length;
			while (pointer != bound)
			{
				x *= *pointer;
				x *= *(pointer + 1);
				x *= *(pointer + 2);
				x *= *(pointer + 3);
				pointer += 4;
			}
		}
 
		return x;
	}
	
	[Benchmark(Baseline = true)]
	public unsafe long MultTrickyAndSmartUnsafe()
	{
		long w = 0;
		long x = 0;
		long y = 0;
		long z = 0;
 
		var length = bills.Length;
		fixed (int* ptr = bills)
		{
			var pointer = ptr;
			var bound = pointer + length;
			while (pointer != bound)
			{
				w *= *pointer;
				x *= *(pointer + 1);
				y *= *(pointer + 2);
				z *= *(pointer + 3);
				pointer += 4;
			}
		}
 
		w *= x;
		y *= z;
		return w * y;
	}
}
