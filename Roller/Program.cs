using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Roller
{
	class Program
	{
		static int Main(string[] args)
		{
			Console.WriteLine("Hello in Roller! I will choose proper wallpaper for you (in terms of your primary screen resolution)");

			// Check param
			string wallPath;
			if (args.Length == 1)
			{
				wallPath = args[0];
			}
			else
			{
				Console.WriteLine("You must supply path to wallpapers as first param! Exiting...");
				return 1;
			}
			
			// Get wallpaper files
			var wallpaperCandidates = Hunter.GetWallTuples(wallPath);
			if (wallpaperCandidates == null)
			{
				Console.WriteLine("Incorrect / inaccessible path supplied! Exiting...");
				return 2;
			}
			if (wallpaperCandidates.Count == 0)
			{
				Console.WriteLine("No files matching '<something>_<ResX>x<ResY>.jpg' found! Exiting...");
				return 3;
			}

			// Get screen resolution
			var mainScreenResX = Screen.PrimaryScreen.Bounds.Width;
			var mainScreenResY = Screen.PrimaryScreen.Bounds.Height;
			Console.WriteLine("Primary screen resolution is: " + mainScreenResX + "x" + mainScreenResY);

			// Try to find proper resolution
			string thePathOfTheChosenOne;
			var theChosenOne = wallpaperCandidates.FirstOrDefault(x => (x.xRes == mainScreenResX && x.yRes == mainScreenResY));
			if (!string.IsNullOrEmpty(theChosenOne.fullPath))
			{
				// It's Neo!
				thePathOfTheChosenOne = theChosenOne.fullPath;
				Console.WriteLine("Found perfect match - " + thePathOfTheChosenOne);
			}
			else
			{
				// So maybe there is 1024x768 Neo?
				theChosenOne = wallpaperCandidates.FirstOrDefault(x => (x.xRes == 1024 && x.yRes == 768));
				if (!string.IsNullOrEmpty(theChosenOne.fullPath))
				{
					// We are saved!
					thePathOfTheChosenOne = theChosenOne.fullPath;
					Console.WriteLine("Perfect match not found but default resolution found - " + thePathOfTheChosenOne);
				}
				else
				{
					// Ehh... so a first one will be Neo!
					theChosenOne = wallpaperCandidates.FirstOrDefault();
					thePathOfTheChosenOne = theChosenOne.fullPath;
					Console.WriteLine("Perfect res nor default res not found so choosing first - " + thePathOfTheChosenOne);
				}
			}

			// Now the ceremony
			if (Roller.PaintWall(thePathOfTheChosenOne, Roller.Style.Center))
			{
				Console.WriteLine("Successfully changed wallpaper to " + thePathOfTheChosenOne + "!");
				return 0;
			}
			else
			{
				Console.WriteLine("Wallpaper change failed :< (" + thePathOfTheChosenOne + ")");
				return 4;
			}
		}
	}
}
