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
	/// <summary>
	/// Class that can set wallpaper
	/// </summary>
	public class Roller
	{
		public enum Style
		{
			Fill,
            Fit,
            Span,
            Stretch,
            Tile,
            Center
		}

		public static bool PaintWall(string wallFilePath, Style style)
		{
			var primaryFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string destWallFilePath = Path.Combine(primaryFolder + @"\Microsoft\Windows\Themes", "rollerWallpaper.bmp");

			Image img = null;
			Bitmap imgTemp = null;
			try
			{
				img = Image.FromFile(Path.GetFullPath(wallFilePath));
				imgTemp = new Bitmap(img);
				imgTemp.Save(destWallFilePath, System.Drawing.Imaging.ImageFormat.Bmp);
				Console.WriteLine("Wallpaper saved to primary path: " + destWallFilePath);
			}
			catch (Exception e1)
			{
				Console.WriteLine(e1);
				try
				{
					var secondaryFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
					destWallFilePath = Path.Combine(secondaryFolder, "rollerWallpaper.bmp");
					imgTemp.Save(destWallFilePath, System.Drawing.Imaging.ImageFormat.Bmp);
					Console.WriteLine("Wallpaper saved to secondary path: " + destWallFilePath);
				}
				catch (Exception e2)
				{
					Console.WriteLine(e2);
					return false;
				}
			}
			
			try
			{
				RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);

				if (style == Style.Fill)
				{
					key.SetValue(@"WallpaperStyle", 10.ToString());
					key.SetValue(@"TileWallpaper", 0.ToString());
				}
				if (style == Style.Fit)
				{
					key.SetValue(@"WallpaperStyle", 6.ToString());
					key.SetValue(@"TileWallpaper", 0.ToString());
				}
				if (style == Style.Span) // Windows 8 or newer only!
				{
					key.SetValue(@"WallpaperStyle", 22.ToString());
					key.SetValue(@"TileWallpaper", 0.ToString());
				}
				if (style == Style.Stretch)
				{
					key.SetValue(@"WallpaperStyle", 2.ToString());
					key.SetValue(@"TileWallpaper", 0.ToString());
				}
				if (style == Style.Tile)
				{
					key.SetValue(@"WallpaperStyle", 0.ToString());
					key.SetValue(@"TileWallpaper", 1.ToString());
				}
				if (style == Style.Center)
				{
					key.SetValue(@"WallpaperStyle", 0.ToString());
					key.SetValue(@"TileWallpaper", 0.ToString());
				}

				SysCall.SetSystemWallpaper(destWallFilePath);

				return true;
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				return false;
			}
		}

		public static void PrintScreenInfo()
		{
			foreach (var scr in Screen.AllScreens)
			{
				Console.WriteLine();
				Console.WriteLine("DeviceName: " + scr.DeviceName);
				Console.WriteLine("BitsPerPixel: " + scr.BitsPerPixel);
				Console.WriteLine("Bounds: " + scr.Bounds);
			}

			Graphics graphics = new System.Windows.Forms.Form().CreateGraphics();
			Console.WriteLine();
			Console.WriteLine("DPI X: " + graphics.DpiX + ", DPI Y: " + graphics.DpiY);
		}
	}
}
