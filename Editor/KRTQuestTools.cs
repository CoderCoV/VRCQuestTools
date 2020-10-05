﻿using UnityEditor;

namespace KRTQuestTools
{
    public static class KRTQuestTools
    {
        public const string Version = "0.0.0";
    }

    internal static class MenuPaths
    {
        private const string RootMenu = "KRTQuestTools/";
        internal const string ConvertAvatarForQuest = RootMenu + "Convert Avatar For Quest";
        internal const string BlendShapesCopy = RootMenu + "BlendShapes Copy";
        internal const string AutoRemoveVertexColors = RootMenu + "Auto Remove Vertex Colors";
        internal const string UnitySettings = RootMenu + "Unity Settings";

        internal const string GameObjectRemoveAllVertexColors = "GameObject/Remove All Vertex Colors";
        internal const string GameObjectConvertAvatarForQuest = "GameObject/Convert Avatar For Quest";
        internal const string ContextBlendShapesCopy = "CONTEXT/SkinnedMeshRenderer/Copy BlendShape Weights";
    }

    internal enum MenuPriorities : int
    {
        ConvertAvatarForQuest = 0,
        BlendShapesCopy,
        AutoRemoveVertexColors = 100,
        UnitySettings = 200
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

        private static void SetBooleanConfigValue(string name, bool value)
        {
            var v = value ? TRUE : FALSE;
            EditorUserSettings.SetConfigValue(name, v);
        }

        private static bool GetBooleanConfigValue(string name, bool defaultValue)
        {
            var d = defaultValue ? TRUE : FALSE;
            return (EditorUserSettings.GetConfigValue(name) ?? d) == TRUE;
        }

        public static string LastVersion
        {
            get { return EditorUserSettings.GetConfigValue(Keys.LAST_VERSION) ?? ""; }
            set { EditorUserSettings.SetConfigValue(Keys.LAST_VERSION, value); }
        }

        public static bool IsDontShowSettingsWindowOnLoadEnabled
        {
            get { return GetBooleanConfigValue(Keys.DONT_SHOW_ON_LOAD, false); }
            set { SetBooleanConfigValue(Keys.DONT_SHOW_ON_LOAD, value); }
        }

        public static bool IsAutoRemoveVertexColorsEnabled
        {
            get { return GetBooleanConfigValue(Keys.AUTO_REMOVE_VERTEX_COLORS, true); }
            set { SetBooleanConfigValue(Keys.AUTO_REMOVE_VERTEX_COLORS, value); }
        }
    }
}
