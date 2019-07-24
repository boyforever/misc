using System;
using System.Diagnostics;
					
public class Program
{
	public static void Main()
	{
		DoublePowerTest();
		DoubleDivisionTest();
		DecimalDivisionTest();
		DoubleAdditionTest();
		DoubleSpeedTest();
		DecimalSpeedTest();
	}
	private static void DoublePowerTest()
	{
		Console.WriteLine("------DoublePowerTest--------");
		
		Double x = 4;
		Double y = 0.5;
		
		Console.WriteLine("{0}^{1} = {2:R}", x, y, Math.Pow(x, y));
		
		x = (Double)2;
		y = (Double)0.25;
		Double z = Math.Pow(x, y);
		
		Console.WriteLine("{0}^{1} = {2:R}", x, y, z);
		
		Console.WriteLine("{0}^(1/{1}) = {2:R}", x, y, Math.Pow(z, 1/y));
	}
	private static void DoubleDivisionTest()
	{
		Console.WriteLine("------DoubleDivisionTest--------");
		Double x = 8;
		Double y = 2;
		
		Console.WriteLine("{0:R}", x/y);
		
		x = (Double)8;
		y = (Double)3;
		
		Console.WriteLine("{0:R}", x/y);
	}
	private static void DecimalDivisionTest()
	{
		Console.WriteLine("------DecimalDivisionTest--------");
		Decimal x = 8;
		Decimal y = 2;
		
		Console.WriteLine(x/y);
		
		x = 8m;
		y = 3m;
		
		Console.WriteLine(x/y);
	}
	
	private static void DoubleAdditionTest()
	{
		Console.WriteLine("------DoubleAdditionTest--------");
		Double x = .1;
		Double result = 10 * x;
		Double result2 = x + x + x + x + x + x + x + x + x + x;

		Console.WriteLine("{0} - {1}", result, result2);
		Console.WriteLine("{0:R} - {1:R}", result, result2);
	}
	private static int iterations = 100000000;

	private static void DoubleSpeedTest()
	{
		Console.WriteLine("------DoubleSpeedTest--------");
		Stopwatch watch = new Stopwatch();
		watch.Start();
		Double z = 0;
		for (int i = 0; i < iterations; i++)
		{
			Double x = i;
			Double y = x * i;
			z += y;
		}
		watch.Stop();
		Console.WriteLine("Double: " + watch.ElapsedTicks);
		Console.WriteLine("{0:R}", z);
	}

	private static void DecimalSpeedTest()
	{
		Console.WriteLine("------DecimalSpeedTest--------");
		Stopwatch watch = new Stopwatch();
		watch.Start();
		Decimal z = 0;
		for (int i = 0; i < iterations; i++)
		{
			Decimal x = i;
			Decimal y = x * i;
			z += y;
		}
		watch.Stop();
		Console.WriteLine("Decimal: " + watch.ElapsedTicks);
		Console.WriteLine(z);
	}
}
