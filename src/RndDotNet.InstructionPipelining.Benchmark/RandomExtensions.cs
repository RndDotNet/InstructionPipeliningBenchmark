namespace RndDotNet.InstructionPipelining.Benchmark;

public static class RandomExtensions
{
	public static int Next(this Random rnd, int minValue, int maxValue, bool isOdd)
	{
		while (true)
		{
			var randomNumber = rnd.Next(minValue, maxValue);
			if ((isOdd && randomNumber % 2 == 1) || (!isOdd && randomNumber % 2 == 0))
				return randomNumber;
		}
	}
}
