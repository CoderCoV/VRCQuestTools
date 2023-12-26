// <copyright file="ModularAvatarUtility.cs" company="kurotu">
// Copyright (c) kurotu.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>

using System;
using System.Linq;
#if VQT_HAS_MODULAR_AVATAR
using nadena.dev.modular_avatar.core;
#endif
using UnityEngine;

namespace KRT.VRCQuestTools.Utils
{
    /// <summary>
    /// Modular Avatar utilities.
    /// </summary>
    internal class ModularAvatarUtility
    {
        /// <summary>
        /// Type object of ModularAvatarMergeAnimator.
        /// </summary>
        internal static Type MergeAnimatorType = SystemUtility.GetTypeByName("nadena.dev.modular_avatar.core.ModularAvatarMergeAnimator");

        /// <summary>
        /// Gets a value indicating whether Modular Avatar is imported.
        /// </summary>
        internal static bool IsModularAvatarImported => MergeAnimatorType != null;

        /// <summary>
        /// Gets unsupported MA components for Android.
        /// </summary>
        /// <param name="gameObject">GameObject to inspect.</param>
        /// <param name="includeInactive">Whether to include inactive objects.</param>
        /// <returns>Unsupported components.</returns>
        internal static Component[] GetUnsupportedComponentsInChildren(GameObject gameObject, bool includeInactive)
        {
#if VQT_HAS_MODULAR_AVATAR
            var types = new Type[] { typeof(ModularAvatarVisibleHeadAccessory), typeof(ModularAvatarWorldFixedObject) };
            var components = types.SelectMany(t => gameObject.GetComponentsInChildren(t, includeInactive)).ToArray();
            return components;
#else
            return new Component[] { };
#endif
        }

        /// <summary>
        /// Remove unsupported MA components for Android.
        /// </summary>
        /// <param name="gameObject">GameObject to inspect.</param>
        /// <param name="includeInactive">Whether to include inactive objects.</param>
        internal static void RemoveUnsupportedComponents(GameObject gameObject, bool includeInactive)
        {
            var components = GetUnsupportedComponentsInChildren(gameObject, includeInactive);
            foreach (var component in components)
            {
                var obj = component.gameObject;
                var message = $"[{VRCQuestTools.Name}] Removed {component.GetType().Name} from {obj.name}";
                UnityEngine.Object.DestroyImmediate(component);
                Debug.Log(message, obj);
            }
        }
    }
}
