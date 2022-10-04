﻿using System.Numerics;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;

namespace RndDotNet.InstructionPipelining.Benchmark;

[ShortRunJob(RuntimeMoniker.Net60, Jit.RyuJit, Platform.X64)]
[LongRunJob(RuntimeMoniker.Net60, Jit.RyuJit, Platform.X64)]
public class IntegerSumCalculator
{
	private const int billsCount = 10_000;
	private const int maxCost = 1000;
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
		fixed (int* ptr = bills)
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
		fixed (int* ptr = bills)
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
	
	[Benchmark(Baseline = true)]
	public unsafe long SumTrickyAndSmartUnsafe()
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
		fixed (int* ptr = bills)
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
		fixed (int* ptr = bills)
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
	public unsafe long SumTrickyAndSmartUnsafe2Shift16()
	{
		long w = 0;
		long x = 0;

		var length = bills.Length;
		fixed (int* ptr = bills)
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
				w += *(pointer + 8);
				x += *(pointer + 9);
				w += *(pointer + 10);
				x += *(pointer + 11);
				w += *(pointer + 12);
				x += *(pointer + 13);
				w += *(pointer + 14);
				x += *(pointer + 15);
				pointer += 16;
			}
		}

		return w + x;
	}

	[Benchmark]
	public long SumSimd()
	{
		Vector<int> vectorSum = Vector<int>.Zero;
 
		Span<Vector<int>> vectorsArray = MemoryMarshal.Cast<int, Vector<int>>(bills);
 
		for (var i = 0; i < vectorsArray.Length; i++)
		{
			vectorSum += vectorsArray[i];
		}
 
		return Vector.Dot(vectorSum, Vector<int>.One);
	}
}