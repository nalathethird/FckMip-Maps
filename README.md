# FckMipMaps

A [ResoniteModLoader](https://github.com/resonite-modding-group/ResoniteModLoader) mod for [Resonite](https://resonite.com/) that disables automatic mipmap generation during image import.

## License

This project is licensed under the MIT License. You are free to use, modify, and redistribute this mod, provided you give credit to Zeia Nala (the creator of this mod) and Toxic-Cookie (for the mipmap patch method, as used in DefaultToAnisotropic).

See LICENSE file for full terms.

## Features

- Disables mipmap generation when importing image files (PNG, JPG, WEBP)
- Configurable through mod settings (should keep off unless your batch importing images that dont need Mip-Maps)
- Preserves the ability to manually enable mipmaps after import (Open an Inspector on the Image -> Static2DTexture -> MipMaps= True)

## Installation

1. Install [ResoniteModLoader](https://github.com/resonite-modding-group/ResoniteModLoader).
2. Place [FckMipMaps.dll](https://github.com/tgrafk12/FckMip-Maps/releases/latest/download/FckMipMaps.dll) into your `rml_mods` folder. This folder should be at `C:\Program Files (x86)\Steam\steamapps\common\Resonite\rml_mods` for a default install. You can create it if it's missing, or if you launch the game once with ResoniteModLoader installed it will create this folder for you.
3. Start the game. If you want to verify that the mod is working you can check your Resonite logs.

## Configuration

The mod can be configured through the Resonite mod settings menu:

- `Disable MipMaps OnImport`: Toggle to enable/disable automatic mipmap generation during image import - If On, MipMaps do not generate (default: true)

## How it Works

The mod hooks into Resonite's texture import process and disables mipmap generation for image files. It does this by:

1. Detecting image file types through their file signatures
2. Setting the `generateMipMaps` parameter to false during the import process
3. Preserving all other import functionality - Creating a Steam Screenshot/Pixel Art/Sprite/360 Photo/Stereo Photo/LUT/Raw etc.

This allows you to:
- Save memory by not generating mipmaps for images that are Temporary Images that are not meant for saving in World/Avatars/Spawnables
- Still allows users to enable mipmaps manually after import in case you forgot to turn off the mod - In the Static2D Component

## Credits

Created by Zeia Nala

## Other Credits

Based on the approach from [DefaultToAnisotropic](https://github.com/Toxic-Cookie/DefaultToAnisotropic) by Toxic-Cookie.

Special thanks to them for the code pattern to patch MipMaps on Import!
