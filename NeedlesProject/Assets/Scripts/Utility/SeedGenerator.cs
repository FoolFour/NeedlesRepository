using System;

public sealed class SeedGenerator
{
	public static int Generate()
	{
		// Tickはナノ秒で返してくる為
		// 1/10秒まで表示
		return (int)(DateTime.Now.Ticks/1000000L);
	}
}
