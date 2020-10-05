﻿using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace KRTQuestTools
{
    enum CacheServerMode
    {
        Local = 0,
        Remote,
        Disable
    }

    static class UnitySettings
    {
        private static class EditorPrefsKeys
        {
            public const string CacheServerMode = "CacheServerMode";
            public const string CompressTexturesOnImport = "kCompressTexturesOnImport";
        }

        public static bool ValidateAll()
        {
            return ValidateCacheServerMode() && ValidateAndroidTextureCompression();
        }

        public static CacheServerMode GetCacheServerMode()
        {
            var mode = EditorPrefs.GetInt(EditorPrefsKeys.CacheServerMode, (int)CacheServerMode.Disable);
            return (CacheServerMode)System.Enum.ToObject(typeof(CacheServerMode), mode);
        }

        public static bool ValidateCacheServerMode()
        {
            return GetCacheServerMode() != CacheServerMode.Disable;
        }

        public static void EnableLocalCacheServer()
        {
            EditorPrefs.SetInt(EditorPrefsKeys.CacheServerMode, (int)CacheServerMode.Local);
        }

        public static MobileTextureSubtarget GetAndroidTextureCompression()
        {
            return EditorUserBuildSettings.androidBuildSubtarget;
        }

        public static bool ValidateAndroidTextureCompression()
        {
            return GetAndroidTextureCompression() == MobileTextureSubtarget.ASTC;
        }

        public static void EnableAndroidASTC()
        {
            EditorUserBuildSettings.androidBuildSubtarget = MobileTextureSubtarget.ASTC;
        }
    }

    static class KRTQuestToolsSettings
    {
        private static class Keys
        {
            private const string PREFIX = "dev.kurotu.KRTQuestTools.";
            public const string LAST_VERSION = PREFIX + "LastQuestToolsVersion";
            public const string DONT_SHOW_ON_LOAD = PREFIX + "DontShowOnLoad";
            public const string AUTO_REMOVE_VERTEX_COLORS = PREFIX + "AutoRemoveVertexColors";
        }

        private const string FALSE = "FALSE";
        private const string TRUE = "TRUE";
        private static string GetStringValue(bool boolean)
        {
            return boolean ? TRUE : FALSE;
        }

        public static string GetLastVersion()
        {
            return EditorUserSettings.GetConfigValue(Keys.LAST_VERSION) ?? "";
        }

        public static void SetLastVersion(string version)
        {
            EditorUserSettings.SetConfigValue(Keys.LAST_VERSION, version);
        }

        public static bool IsDontShowOnLoadEnabled()
        {
            return (EditorUserSettings.GetConfigValue(Keys.DONT_SHOW_ON_LOAD) ?? FALSE) == TRUE;
        }

        public static void SetDontShowOnLoad(bool enabled)
        {
            var value = GetStringValue(enabled);
            EditorUserSettings.SetConfigValue(Keys.DONT_SHOW_ON_LOAD, value);
        }

        public static bool IsAutoRemoveVertexColorsEnabled()
        {
            var defaultValue = TRUE;
            return (EditorUserSettings.GetConfigValue(Keys.AUTO_REMOVE_VERTEX_COLORS) ?? defaultValue) == TRUE;
        }

        public static void SetAutoRemoveVertexColors(bool enabled)
        {
            var value = GetStringValue(enabled);
            EditorUserSettings.SetConfigValue(Keys.AUTO_REMOVE_VERTEX_COLORS, value);
        }
    }

    public class UnitySettingsWindow : EditorWindow
    {
        private delegate void Action();
        private readonly UnitySettingsI18nBase i18n = UnitySettingsI18n.Create();

        [InitializeOnLoadMethod]
        static void InitOnLoad()
        {
            var lastVersion = KRTQuestToolsSettings.GetLastVersion();
            var hasUpdated = !lastVersion.Equals(KRTQuestTools.Version);
            if (hasUpdated)
            {
                KRTQuestToolsSettings.SetLastVersion(KRTQuestTools.Version);
            }
            var shouldShowWindow = !UnitySettings.ValidateAll();

            if (shouldShowWindow && (!KRTQuestToolsSettings.IsDontShowOnLoadEnabled() || hasUpdated))
            {
                Init();
            }
        }

        [MenuItem(MenuPaths.UnitySettings, false, (int)MenuPriorities.UnitySettings)]
        static void Init()
        {
            var window = GetWindow(typeof(UnitySettingsWindow));
            window.Show();
        }

        private void OnGUI()
        {
            titleContent.text = "KRT Unity Settings";
            EditorGUILayout.LabelField("Unity Preferences", EditorStyles.boldLabel);
            var allActions = new List<Action>();

            EditorGUILayout.LabelField($"{i18n.CacheServerModeLabel}: {UnitySettings.GetCacheServerMode()}");

            if (!UnitySettings.ValidateCacheServerMode())
            {
                EditorGUILayout.HelpBox(i18n.CacheServerHelp, MessageType.Warning);
                allActions.Add(UnitySettings.EnableLocalCacheServer);
                if (GUILayout.Button(i18n.CacheServerButtonLabel))
                {
                    UnitySettings.EnableLocalCacheServer();
                }
            }

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Build Settings", EditorStyles.boldLabel);

            EditorGUILayout.LabelField($"{i18n.TextureCompressionLabel}: {UnitySettings.GetAndroidTextureCompression()}");
            if (!UnitySettings.ValidateAndroidTextureCompression())
            {
                EditorGUILayout.HelpBox(i18n.TextureCompressionHelp, MessageType.Warning);
                allActions.Add(UnitySettings.EnableAndroidASTC);
                if (GUILayout.Button(i18n.TextureCompressionButtonLabel))
                {
                    UnitySettings.EnableAndroidASTC();
                }
            }

            EditorGUILayout.Space();

            if (allActions.Count >= 2)
            {
                if (GUILayout.Button(i18n.ApplyAllButtonLabel))
                {
                    foreach (var action in allActions)
                    {
                        action();
                    }
                }
            }
            else if (allActions.Count == 0)
            {
                EditorGUILayout.HelpBox(i18n.AllAppliedHelp, MessageType.Info);
            }

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            var donotshow = KRTQuestToolsSettings.IsDontShowOnLoadEnabled();
            donotshow = EditorGUILayout.Toggle(i18n.DontShowOnStartupLabel, donotshow);
            KRTQuestToolsSettings.SetDontShowOnLoad(donotshow);
        }
    }

    static class UnitySettingsI18n
    {
        public static UnitySettingsI18nBase Create()
        {
            if (System.Globalization.CultureInfo.CurrentCulture.Name == "ja-JP")
            {
                return new UnitySettingsI18nJapanese();
            }
            else
            {
                return new UnitySettingsI18nEnglish();
            }
        }
    }

    abstract class UnitySettingsI18nBase
    {
        public abstract string CacheServerModeLabel { get; }
        public abstract string CacheServerHelp { get; }
        public abstract string CacheServerButtonLabel { get; }
        public abstract string TextureCompressionLabel { get; }
        public abstract string TextureCompressionHelp { get; }
        public abstract string TextureCompressionButtonLabel { get; }
        public abstract string ApplyAllButtonLabel { get; }
        public abstract string DontShowOnStartupLabel { get; }
        public abstract string AllAppliedHelp { get; }
    }

    class UnitySettingsI18nEnglish : UnitySettingsI18nBase
    {
        public override string CacheServerModeLabel => "Cache Server Mode";
        public override string CacheServerHelp => "By enabling the local cache server, you can save time for texture compression (such as \"Switch Platform\") from the next. In default preferences, the server takes 10 GB from C drive at maximum.";
        public override string CacheServerButtonLabel => "Enable Local Cache Server";
        public override string TextureCompressionLabel => "Android Texture Compression";
        public override string TextureCompressionHelp => "ASTC improves texture quality in exchange for long compression time";
        public override string TextureCompressionButtonLabel => "Set texture compression to ASTC";
        public override string ApplyAllButtonLabel => "Apply All Settings";
        public override string DontShowOnStartupLabel => "Don't show on startup";
        public override string AllAppliedHelp => "OK, all recommended settings are applied.";
    }

    class UnitySettingsI18nJapanese : UnitySettingsI18nBase
    {
        public override string CacheServerModeLabel => "キャッシュサーバー";
        public override string CacheServerHelp => "ローカルキャッシュサーバーを使用すると、次回以降のSwitch Platformによるテクスチャ圧縮にかかる時間を短縮できることがあります。デフォルト設定ではCドライブを最大10GB使用します。";
        public override string CacheServerButtonLabel => "ローカルキャッシュサーバーを有効化する";
        public override string TextureCompressionLabel => "Android テクスチャ圧縮";
        public override string TextureCompressionHelp => "ASTCを使用すると圧縮に時間がかかる代わりにテクスチャの画質が向上します";
        public override string TextureCompressionButtonLabel => "ASTCでテクスチャを圧縮";
        public override string ApplyAllButtonLabel => "すべての設定を適用";
        public override string DontShowOnStartupLabel => "起動時に表示しない";
        public override string AllAppliedHelp => "すべての推奨設定が適用されています";
    }
}
