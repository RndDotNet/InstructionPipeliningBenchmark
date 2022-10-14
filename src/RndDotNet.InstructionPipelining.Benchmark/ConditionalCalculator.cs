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
	private const int bidsCount = 10_000;
	private const int maxCost = 1000;
	private int[] bids;
	private int[] orderedBids;
	private int[] patternOrderedBids;

	[GlobalSetup]
	public void GlobalSetup()
	{
		var rnd = new Random(61);
		patternOrderedBids = Enumerable.Range(0, bidsCount).Select(i => rnd.Next(0, maxCost, i % 2 == 0)).ToArray();
		bids = patternOrderedBids.OrderBy(x => Guid.NewGuid()).ToArray();
		orderedBids = bids.OrderBy(s => s % 2 == 0).ToArray();
		
	}
	
	[Benchmark]
	public int GetEvensCount() 
		=> bids.Count(t => t % 2 == 0);

	[Benchmark]
	public int GetEvensCountNative() 
		=> GetEvensCountNativeInternal(bids);

	[Benchmark]
	public int GetEvensCountOrderedNative()
		=> GetEvensCountNativeInternal(orderedBids);
	
	[Benchmark]
	public int GetEvensCountPatternOrderedNative()
		=> GetEvensCountNativeInternal(patternOrderedBids);

	[Benchmark(Baseline = true)]
	public int GetEvensCountNativeNoIf() 
		=> GetEvensCountNativeNoIfInternal(bids);

	[Benchmark]
	public int GetEvensCountOrderedNativeNoIf()
		=> GetEvensCountNativeNoIfInternal(orderedBids);
	
	[Benchmark]
	public unsafe int GetEvensCountNativeNoIfTrickyAndSmart()
	{
		var evens = 0;
		var events2 = 0;
		var events3 = 0;
		var events4 = 0;
		
		var length = bids.Length;
		fixed (int* ptr = bids)
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
	
	private int GetEvensCountNativeInternal(int[] source)
	{

		int evens = 0;
		for (var i = 0; i < source.Length; i++)
			if (source[i] % 2 == 0)
				evens++;

		return evens;
	}

	private int GetEvensCountNativeNoIfInternal(int[] source)
	{

		var evens = 0;
		for (var i = 0; i < source.Length; i++)
			evens += 1 ^ (source[i] % 2);

		return evens;
	}
}
