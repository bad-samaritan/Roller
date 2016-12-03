using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32;

namespace Roller
{
	/// <summary>
	/// Class that can set wallpaper
	/// </summary>
	public class Roller
	{
		const int SPI_SETDESKWALLPAPER = 20;
		const int SPIF_UPDATEINIFILE = 0x01;
		const int SPIF_SENDWININICHANGE = 0x02;

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

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
			var appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string destWallFilePath = Path.Combine(appDataFolder + @"\Microsoft\Windows\Themes", "rollerWallpaper.bmp");
			try
			{
				Image img = Image.FromFile(Path.GetFullPath(wallFilePath));
				img.Save(destWallFilePath, System.Drawing.Imaging.ImageFormat.Bmp);

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

				SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, destWallFilePath,
					SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);

				return true;
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				return false;
			}
		}
	}
}
