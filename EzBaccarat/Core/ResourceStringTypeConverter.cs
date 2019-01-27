// --------------------------------------------------------------------------------
// <copyright file="ResourceStringTypeConverter.cs" company="Oce Display Graphics Systems, Inc.">
//   Copyright (c) Oce Display Graphics Systems, Inc. All rights reserved.
// </copyright>
// <summary>
//   The resource string type converter.
// </summary>
// --------------------------------------------------------------------------------

namespace Shadows.Core
{
    using System;
    using System.ComponentModel;
    using System.Globalization;

    /// <summary>
    /// The resource string type converter.
    /// </summary>
    public class ResourceStringTypeConverter : TypeConverter
    {
        #region Public Methods and Operators

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (typeof(string).IsAssignableFrom(sourceType))
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType.IsAssignableFrom(typeof(string)))
            {
                return true;
            }

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var s = value as string;
            if (s != null)
            {
                return new ResourceString { Value = s };
            }

            return null;
        }

        #endregion
    }
}
