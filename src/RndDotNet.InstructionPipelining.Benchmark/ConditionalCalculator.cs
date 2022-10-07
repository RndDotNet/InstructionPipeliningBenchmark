using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;

namespace RndDotNet.InstructionPipelining.Benchmark;

[MinColumn]
[HardwareCounters(
	HardwareCounter.BranchInstructionRetired, 
	HardwareCounter.BranchMispredictions, 
	HardwareCounter.BranchMispredictsRetired)]
[VeryLongRunJob(RuntimeMoniker.Net60, Jit.RyuJit, Platform.X64)]
public class ConditionalCalculator
{
	private const int billsCount = 10_000;
	private const int maxCost = 1000;
	private int[] bills;
	private int[] orderedBills;
	private int[] patternOrderedBills;

	[GlobalSetup]
	public void GlobalSetup()
	{
		var rnd = new Random(61);
		patternOrderedBills = Enumerable.Range(0, billsCount).Select(i => rnd.Next(0, maxCost, i % 2 == 0)).ToArray();
		bills = patternOrderedBills.OrderBy(x => Guid.NewGuid()).ToArray();
		orderedBills = bills.OrderBy(s => s % 2 == 0).ToArray();
		
	}
	
	[Benchmark]
	public int GetEvensCount()
	{
		return bills.Count(t => t % 2 == 0);
	}
	
	[Benchmark]
	public int GetEvensCountNative()
	{
		var evens = 0;
		for (var i = 0; i < bills.Length; i++)
			if (bills[i] % 2 == 0)
				evens++;
 
		return evens;
	}
	
	[Benchmark]
	public int GetEvensCountOrderedNative()
	{
		var evens = 0;
		for (var i = 0; i < orderedBills.Length; i++)
			if (orderedBills[i] % 2 == 0)
				evens++;
 
		return evens;
	}
	
	[Benchmark]
	public int GetEvensCountPatternOrderedNative()
	{
		var evens = 0;
		for (var i = 0; i < patternOrderedBills.Length; i++)
			if (patternOrderedBills[i] % 2 == 0)
				evens++;
 
		return evens;
	}

	[Benchmark(Baseline = true)]
	public int GetEvensCountNativeNoIf()
	{
		var evens = 0;
		for (var i = 0; i < bills.Length; i++)
			evens += 1 ^ (bills[i] % 2);

		return evens;
	}
	
	[Benchmark]
	public int GetEvensCountOrderedNativeNoIf()
	{
		var evens = 0;
		for (var i = 0; i < bills.Length; i++)
			evens += 1 ^ (bills[i] % 2);

		return evens;
	}
	
	[Benchmark]
	public unsafe int GetEvensCountNativeNoIfTrickyAndSmart()
	{
		var evens = 0;
		var events2 = 0;
		var events3 = 0;
		var events4 = 0;
		
		var length = bills.Length;
		fixed (int* ptr = bills)
		{
			var pointer = ptr;
			var bound = pointer + length;
			while (pointer != bound)
			{
				evens += 1 ^ (*pointer % 2);
				events2 += 1 ^ (*(pointer + 1) % 2);
				events3 += 1 ^ (*(pointer + 2) % 2);
				events4 += 1 ^ (*(pointer + 3) % 2);
				pointer += 4;
			}
		}
 
		evens += events2;
		events3 += events4;
		return evens + events3;
	}
}
