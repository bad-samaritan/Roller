using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Roller
{
	/// <summary>
	/// Class that can extract wallpaper candidates
	/// </summary>
	class Hunter
	{
		public static List<WallTuple> GetWallTuples(string path)
		{
			if (string.IsNullOrEmpty(path) || !Directory.Exists(path))
				return null;

			// 'something_1920x1080.jpg' will match
			var wallNameRegex = new Regex(@"[_\-\.][0-9]{3,4}x[0-9]{3,4}\.(bmp|jpg|jpeg|png)$");
			
			// '1920x1080' will match
			var resolutionRegex = new Regex(@"[0-9]{3,4}x[0-9]{3,4}");

			var wallFileList = Directory.GetFiles(path).Where(x => wallNameRegex.IsMatch(x)).ToList();
			var wallTupleList = 
				from wallFilePath in wallFileList
				let resExp = wallNameRegex.Match(wallFilePath).Groups[0].Value
				let xExp = Int32.Parse(resolutionRegex.Match(resExp).Groups[0].Value.Split('x')[0])
				let yExp = Int32.Parse(resolutionRegex.Match(resExp).Groups[0].Value.Split('x')[1])
				select new WallTuple
				{
					fullPath = wallFilePath,
					xRes = xExp,
					yRes = yExp
				};

			return wallTupleList.ToList();
		}
	}

	struct WallTuple
	{
		public string fullPath;
		public int xRes;
		public int yRes;
	}
}
