using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

namespace AI_EditorWindow.Utilities
{
    public static class ResourceManager
    {
        private static string resourcePath = "";
        public static void SetDefaultResourcePath(string _defaultPath)
        {
            resourcePath = _defaultPath;
        }

        #region Common Resource Loading

        public static string PreparePath(string _path)
        {
            _path = _path.Replace(Application.dataPath, "Assets");
            if (Application.isPlaying)
            {
                if (_path.Contains("Resources")) _path = _path.Substring(_path.LastIndexOf("Resources") + 10);
                _path = _path.Substring(0, _path.LastIndexOf('.'));
                return _path;
            }
            if (!_path.StartsWith("Assets/")) _path = resourcePath + _path;
            return _path;
        }

        public static T[] LoadResources<T>(string _path) where T : UnityEngine.Object
        {
            _path = PreparePath(_path);
            if (Application.isPlaying) return UnityEngine.Resources.LoadAll<T>(_path);
#if UNITY_EDITOR
            return UnityEditor.AssetDatabase.LoadAllAssetsAtPath(_path).Cast<T>().ToArray();
#else
        return null;
#endif
        }

        public static T LoadResource<T> (string _path) where T : UnityEngine.Object
        {
            _path = PreparePath(_path);
            if (Application.isPlaying) return UnityEngine.Resources.Load<T> (_path);
#if UNITY_EDITOR
            return UnityEditor.AssetDatabase.LoadAssetAtPath<T>(_path);
#else
            return null;
#endif
        }

        #endregion

        #region Texture Management

        private static List<MemoryTexture> loadedTextures = new List<MemoryTexture>();

        public static Texture2D LoadTexture(string _texPath)
        {
            if (String.IsNullOrEmpty(_texPath)) return null;
            int existingInd = loadedTextures.FindIndex((MemoryTexture memTex) => memTex.path == _texPath);
            if(existingInd != -1)
            {
                if (loadedTextures[existingInd].texture == null) loadedTextures.RemoveAt(existingInd);
                else return loadedTextures[existingInd].texture;
            }
            Texture2D tex = LoadResource<Texture2D>(_texPath);
            AddTextureToMemory(_texPath, tex);
            return tex;
        }

        public static Texture2D GetTintedTexture(string _texPath, Color _col)
        {
            string texMod = "Tint" + _col.ToString();
            Texture2D tintTexture = GetTexture(_texPath, texMod);
            if (tintTexture == null)
            {
                tintTexture = LoadTexture(_texPath);
                AddTextureToMemory(_texPath, tintTexture);
                tintTexture = NodeEditorFramework.Utilities.RTEditorGUI.Tint(tintTexture, _col);
                AddTextureToMemory(_texPath, tintTexture, texMod);
            }
            return tintTexture;
        }

        public static void AddTextureToMemory(string _texPath, Texture2D _texture, params string[] _modifications)
        {
            if (_texture == null) return;
            loadedTextures.Add(new MemoryTexture(_texPath, _texture, _modifications));
        }

        public static MemoryTexture FindInMemory(Texture2D _tex)
        {
            int existingInd = loadedTextures.FindIndex((MemoryTexture memTex) => memTex.texture == _tex);
            return existingInd != -1 ? loadedTextures[existingInd] : null;
        }

        public static bool HasInMemory(string _texPath, params string[] _modifications)
        {
            int existingInd = loadedTextures.FindIndex((MemoryTexture memTex) => memTex.path == _texPath);
            return existingInd != -1 && EqualModifications(loadedTextures[existingInd].modifications, _modifications);
        }

        public static MemoryTexture GetMemoryTexture(string _texPath, params string[] _mosifications)
        {
            List<MemoryTexture> textures = loadedTextures.FindAll((MemoryTexture memTex) => memTex.path == _texPath);
            if (textures == null || textures.Count == 0) return null;
            foreach (MemoryTexture memTex in textures)
            {
                if (EqualModifications(memTex.modifications, _mosifications)) return memTex;
            }

            return null;
        }

        public static Texture2D GetTexture(string _texPath, params string[] _modifications)
        {
            MemoryTexture memTex = GetMemoryTexture(_texPath, _modifications);
            return memTex == null ? null : memTex.texture;
        }

        private static bool EqualModifications(string[] _modsA, string[] _modsB)
        {
            return _modsA.Length == _modsB.Length && Array.TrueForAll(_modsA, mod => _modsB.Count(oMod => mod == oMod) == _modsA.Count(oMod => mod == oMod));
        }

        public class MemoryTexture
        {
            public string path;
            public Texture2D texture;
            public string[] modifications;

            public MemoryTexture(string _texPath, Texture2D _texture, params string[] mods)
            {
                path = _texPath;
                texture = _texture;
                modifications = mods;
            }
        }

        #endregion
    }
}


