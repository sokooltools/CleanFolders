using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SokoolTools.CleanFolders
{
	//----------------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// 
	/// </summary>
	//----------------------------------------------------------------------------------------------------------------------------
	public static class StringExtensions
	{
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Returns an indication as to whether the current string 'equals' any of the values in the specified collection.
		/// </summary>
		/// <param name="toFind">This string.</param>
		/// <param name="collection">The collection of items to compare to.</param>
		/// <param name="ignoreCase">if set to <c>true</c> [the default] the comparison is case-insensitive.</param>
		/// <returns></returns>
		//------------------------------------------------------------------------------------------------------------------------
		public static bool EqualsAny(this string toFind, IEnumerable<string> collection, bool ignoreCase = true)
		{
			return collection.Contains(toFind, StringComparer.Create(CultureInfo.InvariantCulture, ignoreCase));
		}

		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Returns an indication as to whether the current string 'ends with' any of the values in the specified collection.
		/// </summary>
		/// <param name="toFind">This string.</param>
		/// <param name="collection">The collection of items to compare to.</param>
		/// <param name="ignoreCase">if set to <c>true</c> [the default] the comparison is case-insensitive.</param>
		/// <returns></returns>
		//------------------------------------------------------------------------------------------------------------------------
		public static bool EndsWithAny(this string toFind, IEnumerable<string> collection, bool ignoreCase = true)
		{
			return collection.Any(c => toFind.EndsWith(c, ignoreCase, CultureInfo.InvariantCulture));
		}
	}

}
