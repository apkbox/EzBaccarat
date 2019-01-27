// --------------------------------------------------------------------------------
// <copyright file="DynamicResourceReference.cs" company="Oce Display Graphics Systems, Inc.">
//   Copyright (c) Oce Display Graphics Systems, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the DynamicResourceReference type.
// </summary>
// --------------------------------------------------------------------------------
namespace Shadows.Core
{
    using System;

    /// <summary>
    /// Provides dynamic resource reference that <see cref="DictionaryLookupExtension"/>
    /// uses to resolve individual items in dictionary.
    /// </summary>
    public class DynamicResourceReference
    {
        #region Public Properties

        public string ResourceKey { get; set; }

        public Type TargetType { get; set; }

        #endregion
    }
}