using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SokoolTools.CleanFolders
{
	//--------------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Provides extension methods for performing set-based and collection-based comparisons on strings, such as determining 
	/// whether a string equals or ends with any value in a specified collection.
	/// </summary>
	/// <remarks>
	/// These methods are intended to simplify common string comparison scenarios involving collections, supporting both 
	/// case-sensitive and case-insensitive operations using the invariant culture. All methods are static and can be called as 
	/// extension methods on string instances.
	/// </remarks>
	//--------------------------------------------------------------------------------------------------------------------------
	public static class StringExtensions
	{
		//----------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Returns an indication whether the current string 'equals' any of the values in the specified collection.
		/// </summary>
		/// <param name="toFind">This string.</param>
		/// <param name="collection">The collection of items to compare to.</param>
		/// <param name="ignoreCase">if set to <c>true</c> [the default] the comparison is case-insensitive.</param>
		/// <returns></returns>
		//----------------------------------------------------------------------------------------------------------------------
		public static bool EqualsAny(this string toFind, IEnumerable<string> collection, bool ignoreCase = true)
		{
			return collection.Contains(toFind, StringComparer.Create(CultureInfo.InvariantCulture, ignoreCase));
		}

		//----------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Returns an indication whether the current string 'ends with' any of the values in the specified collection.
		/// </summary>
		/// <param name="toFind">This string.</param>
		/// <param name="collection">The collection of items to compare to.</param>
		/// <param name="ignoreCase">if set to <c>true</c> [the default] the comparison is case-insensitive.</param>
		/// <returns></returns>
		//----------------------------------------------------------------------------------------------------------------------
		public static bool EndsWithAny(this string toFind, IEnumerable<string> collection, bool ignoreCase = true)
		{
			return collection.Any(c => toFind.EndsWith(c, ignoreCase, CultureInfo.InvariantCulture));
		}
	}

}
