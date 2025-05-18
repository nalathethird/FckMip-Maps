using FrooxEngine;

using HarmonyLib;

using ResoniteModLoader;

using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FckMipMaps {
	// FckMipMaps - Disables mipmap generation on image import in Resonite
	//
	// Based on an approach from DefaultToAnisotropic by Toxic-Cookie
	// https://github.com/Toxic-Cookie/DefaultToAnisotropic
	//
	// Special thanks to Toxic-Cookie for code patterns to MipMaps method.
	// More info on creating mods: https://github.com/resonite-modding-group/ResoniteModLoader/wiki/Creating-Mods
	public class FckMipMaps : ResoniteMod {
		internal const string VERSION_CONSTANT = "1.0.0";
		public override string Name => "FckMipMaps";
		public override string Author => "Zeia Nala";
		public override string Version => VERSION_CONSTANT;
		public override string Link => "https://github.com/nalathethird/FckMip-Maps";

		internal static FckMipMaps Instance = null!;
		internal static ModConfiguration config = null!;

		[AutoRegisterConfigKey]
		internal static ModConfigurationKey<bool> DISABLE_MIPMAPS = new ModConfigurationKey<bool>(
			"disable_mipmaps",
			"Disable mipmap generation during image import",
			() => true
		);

		[AutoRegisterConfigKey]
		internal static readonly ModConfigurationKey<bool?> MipMaps = new(
			"MipMaps",
			"Enable mipmaps",
			() => null
		);

		public override void OnEngineInit() {
			Instance = this;
			Msg($"[FckMipMaps] Initializing... v{VERSION_CONSTANT}");
			config = GetConfiguration();
			config.Save(true);

			try {
				Harmony harmony = new Harmony("com.ZeiaNala.FckMipMaps");

				// Patch StaticTexture2D constructor
				var staticTexture2DCtor = typeof(StaticTexture2D).GetConstructor(Type.EmptyTypes);
				if (staticTexture2DCtor != null) {
					harmony.Patch(
						staticTexture2DCtor,
						postfix: new HarmonyMethod(typeof(StaticTexture2D_Constructor_Patch), nameof(StaticTexture2D_Constructor_Patch.Postfix))
					);
					Msg("[FckMipMaps] Patched StaticTexture2D constructor");
				}

				// Patch OnImport method if it exists
				var onImportMethod = typeof(StaticTexture2D).GetMethod("OnImport", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
				if (onImportMethod != null) {
					harmony.Patch(
						onImportMethod,
						postfix: new HarmonyMethod(typeof(StaticTexture2D_OnImport_Patch), nameof(StaticTexture2D_OnImport_Patch.Postfix))
					);
					Msg("[FckMipMaps] Patched StaticTexture2D OnImport method");
				}

				// Patch OnAwake method if it exists
				var onAwakeMethod = typeof(StaticTexture2D).GetMethod("OnAwake", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
				if (onAwakeMethod != null) {
					harmony.Patch(
						onAwakeMethod,
						postfix: new HarmonyMethod(typeof(StaticTexture2D_OnAwake_Patch), nameof(StaticTexture2D_OnAwake_Patch.Postfix))
					);
					Msg("[FckMipMaps] Patched StaticTexture2D OnAwake method");
				}

				Msg("[FckMipMaps] All patches applied successfully!");
			} catch (Exception ex) {
				Error("[FckMipMaps] Failed to apply patches: " + ex);
			}
		}
	}

	// Patch for StaticTexture2D constructor
	public static class StaticTexture2D_Constructor_Patch {
		public static void Postfix(StaticTexture2D __instance) {
			try {
				if (FckMipMaps.config.GetValue(FckMipMaps.DISABLE_MIPMAPS)) {
					__instance.MipMaps.Value = false;
					System.Console.WriteLine($"[FckMipMaps] Disabled mipmaps in constructor for texture: {__instance}");
				}
			} catch (Exception ex) {
				System.Console.WriteLine($"[FckMipMaps] Error in constructor patch: {ex}");
			}
		}
	}

	// Patch for OnImport method
	public static class StaticTexture2D_OnImport_Patch {
		public static void Postfix(StaticTexture2D __instance) {
			try {
				if (FckMipMaps.config.GetValue(FckMipMaps.DISABLE_MIPMAPS)) {
					__instance.MipMaps.Value = false;
					System.Console.WriteLine($"[FckMipMaps] Disabled mipmaps during import for texture: {__instance}");
				}
			} catch (Exception ex) {
				System.Console.WriteLine($"[FckMipMaps] Error in OnImport patch: {ex}");
			}
		}
	}

	// Patch for OnAwake method
	public static class StaticTexture2D_OnAwake_Patch {
		public static void Postfix(StaticTexture2D __instance) {
			try {
				if (FckMipMaps.config.GetValue(FckMipMaps.DISABLE_MIPMAPS)) {
					__instance.MipMaps.Value = false;
					System.Console.WriteLine($"[FckMipMaps] Disabled mipmaps during awake for texture: {__instance}");
				}
			} catch (Exception ex) {
				System.Console.WriteLine($"[FckMipMaps] Error in OnAwake patch: {ex}");
			}
		}
	}
}
