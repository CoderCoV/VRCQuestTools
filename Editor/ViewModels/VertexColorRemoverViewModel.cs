﻿// <copyright file="VertexColorRemoverViewModel.cs" company="kurotu">
// Copyright (c) kurotu.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>

using System.Linq;
using KRT.VRCQuestTools.Utils;
using UnityEngine;

namespace KRT.VRCQuestTools.ViewModels
{
    /// <summary>
    /// ViewModel for VertexColorRemover.
    /// </summary>
    internal class VertexColorRemoverViewModel : Object
    {
        /// <summary>
        /// Target game object.
        /// </summary>
        internal GameObject target;

        /// <summary>
        /// Remove vertex color from the target.
        /// </summary>
        internal void RemoveVertexColor()
        {
            Renderer[] skinnedMeshRenderers = target.GetComponentsInChildren<SkinnedMeshRenderer>(true);
            Renderer[] meshRenderers = target.GetComponentsInChildren<MeshRenderer>(true);

            foreach (var renderer in skinnedMeshRenderers.Concat(meshRenderers))
            {
                RendererUtility.RemoveVertexColor(renderer);
            }
        }
    }
}
