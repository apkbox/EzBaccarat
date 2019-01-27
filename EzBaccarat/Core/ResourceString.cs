// --------------------------------------------------------------------------------
// <copyright file="ResourceString.cs" company="Oce Display Graphics Systems, Inc.">
//   Copyright (c) Oce Display Graphics Systems, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the ResourceString type.
// </summary>
// --------------------------------------------------------------------------------
namespace Shadows.Core
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Markup;

    /// <summary>
    /// Use of this class instead of <see cref="System.String"/> when
    /// defining WPF string resource that requires <see cref="Localization"/>
    /// attached properties.
    /// </summary>
    /// <remarks>
    /// Use of defined string in StringFormat property of a Binding causes
    /// exception, because StringFormat explicitly requires <see cref="System.String"/>
    /// type and does not invoke <see cref="TypeConverter"/>.
    /// To workaround the limitation use <see cref="StringFormatConverter"/>
    /// converter and provide the string via <c>ConverterParameter</c>.
    /// </remarks>
    [ContentProperty("Value")]
    [TypeConverter(typeof(ResourceStringTypeConverter))]
    public class ResourceString
    {
        #region Constructors and Destructors

        public ResourceString()
        {
            this.Value = string.Empty;
        }

        #endregion

        #region Public Properties

        public string Value { get; set; }

        #endregion

        #region Public Methods and Operators

        public override string ToString()
        {
            return this.Value;
        }

        #endregion
    }
}