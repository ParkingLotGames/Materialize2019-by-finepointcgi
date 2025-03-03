# Materialize
Materialize is a program for converting images to materials.

# Releases
https://github.com/ParkingLotGames/Materialize2019-by-finepointcgi/releases

## Platforms
- Linux and Windows x64 - IL2CPP
- macOS and Windows x86 - Mono

## Contact
For sugestions, doubts or anything related to this tool.
Make an issue and we can talk about it.

# Building
### Prerequisites
- Unity 2019.4.40f1.
- IL2CPP module for Linux or win_x64.
- Remove Mono.Posix and Mono.WebBrowser from Assets/Plugins when building for win_x86 or macOS.

## Changelog:
- Switched build backend to IL2CPP
- Enabled the Incremental Garbage Collector 
- Added the Vulkan API as fallback on Linux
- (In Progress) UnityDynamicPanels based UI - https://github.com/yasirkula/UnityDynamicPanels

## Features added by finepointcgi
### Paste Images from clipboard on Linux
- You can copy a file in your file browser (Tested with nautilus) and then press  the "P" close to the slot you want to paste.
- **Highlight** - You can also press copy image on browser and it will paste also. This make it fast to take a image from internet
### Hide Gui while Rotating / Panning
- The GUI is hidden when panning/rotating the material plane.
### Native File Picker
- Added a new native file/folder picker - Unity Standalone File Browser - https://github.com/gkngkc/UnityStandaloneFileBrowser - Thanks to @gkngkc for the amazing work.

 ### Batch Textures Mode
 - You can export multiple textures using the same settings.
 
 ### FPS Limiter
 - This will limit your fps to 30 60 or 120 for your high refresh rate monitors.
 
 ## Changed from original
 
### Save and Load Project
- When you save your project, every map will be saved in the same place, with there respective types, ex:myTexture_Diffuse.png.
- The extension used will be the one set in the GUI Panel.

#### Suported extensions
##### Save
- jpg
- png
- tga
- exr

##### Load
- jpg
- png
- tga
- exr
- bmp

## (Previous) Future Feature List by finepointcgi
Feel free to contribute to it and I'll merge your work but I don't think I can tackle these, I simply don't have the time, I'm updating and sharing the project because I need it for personal use. I can't make a full time commitment to this project, I'm just contributing what I can.
- QuickSave - Will implement in settings, then you can set the folder to save the texture. This will be a persistent setting, that means you can close and open the program without lose the Quick Save path. *Planned for .41*
- Copy to clipboard.
- New UI*Planned for .50*
- Ability to bake AO into Diffuse Map *Planned for .40*
- Add Texture Presets for Unreal Unity and Cryengine *Planned for .41*
- Create update notification system *Planned for .40*
- Create installer
