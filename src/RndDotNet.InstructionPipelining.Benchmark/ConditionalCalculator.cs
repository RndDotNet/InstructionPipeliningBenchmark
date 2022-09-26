using BenchmarkDotNet.Attributes;

namespace RndDotNet.InstructionPipelining.Benchmark;

[SimpleJob(launchCount: 1, warmupCount: 10, targetCount: 100)]
public class ConditionalCalculator
{
	private const int billsCount = 100_000_000;
	private int[] bills;
	private int[] orderedBills;
	private int[] patternOrderedBills;

	[GlobalSetup]
	public void GlobalSetup()
	{
		var rnd = new Random(61);
		bills = Enumerable.Range(0, billsCount).Select(i => rnd.Next(0, 100000)).ToArray();
		orderedBills = bills.OrderBy(s => s % 2 == 0).ToArray();
		patternOrderedBills = Enumerable.Range(0, billsCount).Select(i => rnd.Next(0, 100000, i % 2 == 0)).ToArray();
	}
	
	[Benchmark(Baseline = true)]
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

	[Benchmark]
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
}
