using System;
using System.Collections.Generic;

namespace ChikoRokoBot.AntiBotNotify.Extensions
{
	public static class ListExtensions
	{
		public static T GetRandomItem<T>(this IList<T> collection) =>
			collection[Random.Shared.Next(collection.Count)];
	}
}

