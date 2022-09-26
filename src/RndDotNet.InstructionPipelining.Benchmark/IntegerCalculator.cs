using System.Numerics;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;

namespace RndDotNet.InstructionPipelining.Benchmark;

[SimpleJob(launchCount: 1, warmupCount: 10, targetCount: 100)]
public class IntegerCalculator
{
	private const int billsCount = 100_000_008;
	private long[] bills;

	[GlobalSetup]
	public void GlobalSetup()
	{
		var rnd = new Random(61);
		bills = Enumerable.Range(0, billsCount).Select(i => rnd.NextInt64(0, 100000)).ToArray();
	}
	
	[Benchmark(Baseline = true)]
	public long EnumerableSum()
	{
		return bills.Sum();
	}
	
	[Benchmark]
	public long SumNaive()
	{
		long result = 0;
		var bound = bills.Length;
		for (int i = 0; i < bound; i++)
		{
			result += bills[i];
		}
 
		return result;
	}
	
	[Benchmark]
	public unsafe long SumNaiveUnsafe()
	{
		long result = 0;
		var length = bills.Length;
		fixed (long* ptr = bills)
		{
			var pointer = ptr;
			var bound = pointer + length;
			while (pointer != bound)
			{
				result += *pointer;
				pointer += 1;
			}
		}
 
		return result;
	}
	
	[Benchmark]
	public unsafe long SumTrickyUnsafe()
	{
		long x = 0;
 
		var length = bills.Length;
		fixed (long* ptr = bills)
		{
			var pointer = ptr;
			var bound = pointer + length;
			while (pointer != bound)
			{
				x += *pointer;
				x += *(pointer + 1);
				x += *(pointer + 2);
				x += *(pointer + 3);
				pointer += 4;
			}
		}
 
		return x;
	}
	
	[Benchmark]
	public unsafe long SumTrickyAndSmartUnsafe()
	{
		long w = 0;
		long x = 0;
		long y = 0;
		long z = 0;
 
		var length = bills.Length;
		fixed (long* ptr = bills)
		{
			var pointer = ptr;
			var bound = pointer + length;
			while (pointer != bound)
			{
				w += *pointer;
				x += *(pointer + 1);
				y += *(pointer + 2);
				z += *(pointer + 3);
				pointer += 4;
			}
		}
 
		w += x;
		y += z;
		return w + y;
	}
	
	[Benchmark]
	public unsafe long SumTrickyAndSmartUnsafe2()
	{
		long w = 0;
		long x = 0;

		var length = bills.Length;
		fixed (long* ptr = bills)
		{
			var pointer = ptr;
			var bound = pointer + length;
			while (pointer != bound)
			{
				w += *pointer;
				x += *(pointer + 1);
				w += *(pointer + 2);
				x += *(pointer + 3);
				pointer += 4;
			}
		}

		return w + x;
	}
	
	[Benchmark]
	public unsafe long SumTrickyAndSmartUnsafe3()
	{
		long w = 0;
		long x = 0;
		long y = 0;

		var length = bills.Length;
		fixed (long* ptr = bills)
		{
			var pointer = ptr;
			var bound = pointer + length;
			while (pointer != bound)
			{
				w += *pointer;
				x += *(pointer + 1);
				y += *(pointer + 2);
				pointer += 3;
			}
		}

		w += y;
		return w + x;
	}
	
	[Benchmark]
	public unsafe long SumTrickyAndSmartUnsafe8()
	{
		long w = 0;
		long x = 0;
		long y = 0;
		long z = 0;
		long w2 = 0;
		long x2 = 0;
		long y2 = 0;
		long z2 = 0;
 
		var length = bills.Length;
		fixed (long* ptr = bills)
		{
			var pointer = ptr;
			var bound = pointer + length;
			while (pointer != bound)
			{
				w += *pointer;
				x += *(pointer + 1);
				y += *(pointer + 2);
				z += *(pointer + 3);
				w2 += *(pointer + 4);
				x2 += *(pointer + 5);
				y2 += *(pointer + 6);
				z2 += *(pointer + 7);
				pointer += 8;
			}
		}
 
		w += x;
		y += z;
		w2 += x2;
		y2 += z2;
		w += y;
		w2 += y2;
		return w + w2;
	}
	
	[Benchmark]
	public unsafe long SumTrickyAndSmartUnsafe2Shift2()
	{
		long w = 0;
		long x = 0;

		var length = bills.Length;
		fixed (long* ptr = bills)
		{
			var pointer = ptr;
			var bound = pointer + length;
			while (pointer != bound)
			{
				w += *pointer;
				x += *(pointer + 1);
				pointer += 2;
			}
		}

		return w + x;
	}
	
	[Benchmark]
	public unsafe long SumTrickyAndSmartUnsafe2Shift8()
	{
		long w = 0;
		long x = 0;

		var length = bills.Length;
		fixed (long* ptr = bills)
		{
			var pointer = ptr;
			var bound = pointer + length;
			while (pointer != bound)
			{
				w += *pointer;
				x += *(pointer + 1);
				w += *(pointer + 2);
				x += *(pointer + 3);
				w += *(pointer + 4);
				x += *(pointer + 5);
				w += *(pointer + 6);
				x += *(pointer + 7);
				pointer += 8;
			}
		}

		return w + x;
	}

	[Benchmark]
	public long SumSimd()
	{
		Vector<long> vectorSum = Vector<long>.Zero;
 
		Span<Vector<long>> vectorsArray = MemoryMarshal.Cast<long, Vector<long>>(bills);
 
		for (var i = 0; i < vectorsArray.Length; i++)
		{
			vectorSum += vectorsArray[i];
		}
 
		return Vector.Dot(vectorSum, Vector<long>.One);
	}
}
