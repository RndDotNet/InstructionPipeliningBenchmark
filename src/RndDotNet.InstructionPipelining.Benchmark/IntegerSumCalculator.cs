using System.Numerics;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using Microsoft.Diagnostics.Tracing.Parsers.MicrosoftWindowsTCPIP;
using Perfolizer.Mathematics.Common;

namespace RndDotNet.InstructionPipelining.Benchmark;

[MinColumn]
[DisassemblyDiagnoser]
[VeryLongRunJob(RuntimeMoniker.Net60, Jit.RyuJit, Platform.X64)]
public class IntegerSumCalculator
{
	private const int bidsCount = 10_000;
	private const int maxCost = 1000;
	private int[] bids;

	[GlobalSetup]
	public void GlobalSetup()
	{
		var rnd = new Random(61);
		bids = Enumerable.Range(0, bidsCount).Select(i => rnd.Next(0, maxCost)).ToArray();
	}
	
	[Benchmark]
	public int EnumerableSum()
	{
		return bids.Sum();
	}
	
	[Benchmark]
	public int SumNaive()
	{
		int result = 0;
		var bound = bids.Length;
		for (int i = 0; i < bound; i++)
		{
			result += bids[i];
		}
 
		return result;
	}
	
	[Benchmark]
	public unsafe int SumNaiveUnsafe()
	{
		int result = 0;
		var length = bids.Length;
		fixed (int* ptr = bids)
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
	public unsafe int SumTrickyUnsafe()
	{
		int x = 0;
 
		var length = bids.Length;
		fixed (int* ptr = bids)
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
	public unsafe int SumTrickyAndSmartUnsafe()
	{
		int w = 0;
		int x = 0;
		int y = 0;
		int z = 0;
 
		var length = bids.Length;
		fixed (int* ptr = bids)
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
	public unsafe int SumTrickyAndSmartUnsafe2()
	{
		int w = 0;
		int x = 0;

		var length = bids.Length;
		fixed (int* ptr = bids)
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
	public unsafe int SumTrickyAndSmartUnsafe8()
	{
		int w = 0;
		int x = 0;
		int y = 0;
		int z = 0;
		int w2 = 0;
		int x2 = 0;
		int y2 = 0;
		int z2 = 0;
 
		var length = bids.Length;
		fixed (int* ptr = bids)
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
	public unsafe int SumTrickyAndSmartUnsafe2Shift16()
	{
		int w = 0;
		int x = 0;

		var length = bids.Length;
		fixed (int* ptr = bids)
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
	public int SumSimd()
	{
		Vector<int> vectorSum = Vector<int>.Zero;
 
		Span<Vector<int>> vectorsArray = MemoryMarshal.Cast<int, Vector<int>>(bids);
 
		for (var i = 0; i < vectorsArray.Length; i++)
		{
			vectorSum += vectorsArray[i];
		}
 
		return Vector.Dot(vectorSum, Vector<int>.One);
	}
}
