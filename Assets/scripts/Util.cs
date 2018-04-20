using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util {

	/// Takes a hex color with opacity as a string and returns ist as a Color
	public static Color TryParseHtmlString(string hexColor) {
		Color newCol;
		ColorUtility.TryParseHtmlString (hexColor, out newCol);
		return newCol;
	}
}
