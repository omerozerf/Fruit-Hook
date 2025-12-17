#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;

namespace _Game._Scripts.Editor
{
    public static class AutoSpriteAtlasCreator
    {
        [MenuItem("Tools/Loop Games/Create Sprite Atlas From Folder")]
        private static void CreateSpriteAtlasFromFolder()
        {
            var selection = Selection.activeObject;
            if (!selection)
            {
                Debug.LogError("First select a folder in the Project view.");
                return;
            }

            var selectedPath = AssetDatabase.GetAssetPath(selection);
            if (string.IsNullOrEmpty(selectedPath) || !AssetDatabase.IsValidFolder(selectedPath))
            {
                Debug.LogError("Selected object is not a folder.");
                return;
            }

            var folderName = Path.GetFileName(selectedPath);
            var atlasName = $"atlas_{folderName}";
            var atlasPath = Path.Combine(selectedPath, atlasName + ".spriteatlas").Replace("\\", "/");

            var existingAtlas = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(atlasPath);

            // If atlas exists, ask user whether to overwrite its contents.
            if (existingAtlas)
            {
                var overwrite = EditorUtility.DisplayDialog(
                    "Sprite Atlas",
                    $"A Sprite Atlas named '{atlasName}' already exists.\n\nOverwrite its contents?",
                    "Yes, overwrite",
                    "No, cancel");

                if (!overwrite)
                    return;
            }

            // Create new or reuse existing
            var atlas = existingAtlas ? existingAtlas : new SpriteAtlas();

            // 1) Apply safe packing/texture settings (playable-friendly defaults)
            ApplyAtlasSettings(atlas);

            // 2) Collect sprites in folder
            var sprites = LoadAllSpritesInFolder(selectedPath);

            // 3) Overwrite contents properly (clear packables, then add new)
            OverwritePackables(atlas, sprites);

            // 4) Create asset if needed
            if (!existingAtlas) AssetDatabase.CreateAsset(atlas, atlasPath);

            // 5) Ensure importer flags (Include in Build etc.) are set
            EnsureImporterSettings(atlasPath);

            // 6) Save & force pack for current build target (helps catch issues early)
            EditorUtility.SetDirty(atlas);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            ForcePack(atlas);

            Debug.Log($"Sprite Atlas updated: '{atlasName}' at '{atlasPath}' | Sprites: {sprites.Length}");
            EditorGUIUtility.PingObject(atlas);
        }

        private static void ApplyAtlasSettings(SpriteAtlas atlas)
        {
            var packingSettings = new SpriteAtlasPackingSettings
            {
                enableTightPacking = false,
                enableRotation = false, // safer default (especially for UI)
                padding = 4
            };
            atlas.SetPackingSettings(packingSettings);

            var textureSettings = new SpriteAtlasTextureSettings
            {
                readable = false,
                generateMipMaps = false,
                sRGB = true,
                filterMode = FilterMode.Bilinear
            };
            atlas.SetTextureSettings(textureSettings);

            // Default platform settings (safe baseline)
            var defaultPlatform = new TextureImporterPlatformSettings
            {
                name = "DefaultTexturePlatform",
                overridden = true,
                maxTextureSize = 2048,
                format = TextureImporterFormat.AutomaticCompressed,
                compressionQuality = 50
            };
            atlas.SetPlatformSettings(defaultPlatform);

            // WebGL specific (playable builds)
            var webglPlatform = new TextureImporterPlatformSettings
            {
                name = "WebGL",
                overridden = true,
                maxTextureSize = 2048,
                format = TextureImporterFormat.AutomaticCompressed,
                compressionQuality = 50
            };
            atlas.SetPlatformSettings(webglPlatform);
        }

        private static Sprite[] LoadAllSpritesInFolder(string folderPath)
        {
            var guids = AssetDatabase.FindAssets("t:Sprite", new[] { folderPath });
            if (guids == null || guids.Length == 0)
                return new Sprite[0];

            var sprites = new Sprite[guids.Length];
            for (var i = 0; i < guids.Length; i++)
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[i]);
                sprites[i] = AssetDatabase.LoadAssetAtPath<Sprite>(path);
            }

            return sprites;
        }

        private static void OverwritePackables(SpriteAtlas atlas, Sprite[] sprites)
        {
            // Remove existing packables (true overwrite)
            var currentPackables = atlas.GetPackables();
            if (currentPackables is { Length: > 0 }) atlas.Remove(currentPackables);

            if (sprites == null || sprites.Length == 0)
            {
                Debug.LogWarning("The selected folder doesn't contain any Sprites.");
                return;
            }

            // Add all sprites in the folder
            // SpriteAtlas.Add accepts UnityEngine.Object[]; Sprite is an Object.
            var objects = new Object[sprites.Length];
            for (var i = 0; i < sprites.Length; i++)
                objects[i] = sprites[i];

            atlas.Add(objects);
        }

        private static void EnsureImporterSettings(string atlasAssetPath)
        {
            var importer = AssetImporter.GetAtPath(atlasAssetPath) as SpriteAtlasImporter;
            if (!importer)
            {
                // In some Unity setups, importer can be null until asset is imported.
                AssetDatabase.ImportAsset(atlasAssetPath, ImportAssetOptions.ForceUpdate);
                importer = AssetImporter.GetAtPath(atlasAssetPath) as SpriteAtlasImporter;
            }

            if (!importer)
            {
                Debug.LogWarning(
                    $"SpriteAtlasImporter not found for '{atlasAssetPath}'. Include-in-build may not be enforced here.");
                return;
            }

            importer.includeInBuild = true;
            importer.SaveAndReimport();
        }

        private static void ForcePack(SpriteAtlas atlas)
        {
            // Packs for current active build target (good sanity check)
            SpriteAtlasUtility.PackAtlases(new[] { atlas }, EditorUserBuildSettings.activeBuildTarget);
        }
    }
}
#endif