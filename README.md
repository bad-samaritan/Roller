# Roller
Roller is a simple application for changing Windows wallpaper (desktop background).
It's meant to be used with deploymnet packages with the same wallpaper in many screen resolutions.

# Usage
You just give it a parameter with path to your wallpapers and voila!
(Roller.exe "C:\Pictures\wallpaper pack\")
 Roller chooses proper file and set it as a centered background on every screen available.

# How it works
1. Roller reads given folder contents and filter jpg|jpeg|bmp files
  with resolution in it's name like something_[ResX]x[ResY].ext
  For example: myFavWall_1920x1080.jpg, wallpaper_pro_1024x768.bmp
  
2. Roller searches for file with name matching primary screen resolution.
When it doesn't find the perfect one, it searches for 1024x768 wallpaper.
If it also fails, the first file in folder is being choosen.

3. Roller saves chosen file to %appdata%\Microsoft\Windows\Themes as rollerWallpaper.bmp
and sets is as centered background picture for all screens

# Future?
I've spent a few hours only on this project because I needed it ASAP so there is a lot of work ahead.
I will add features like cmd args:
- set wallpaper from choosen file
- choose different wallpaper style (centered, tiled, etc.)
- action when wallpaper matching exact resolution is not found
- log events

# Licence and terms
This software comes with absolutely no warranty or guaranty. Use it at your own risk.
Fill free to use it (commecrial / non-commercial).
