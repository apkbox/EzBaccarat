// --------------------------------------------------------------------------------
// <copyright file="DictionaryLookupConverter.cs" company="Oce Display Graphics Systems, Inc.">
//   Copyright (c) Oce Display Graphics Systems, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the DictionaryLookupConverter type.
// </summary>
// --------------------------------------------------------------------------------
namespace Shadows.Core
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    internal class DictionaryLookupConverter : IMultiValueConverter
    {
        #region Fields

        private readonly FrameworkElement frameworkElement;

        private readonly string resourceKey;

        private readonly IDictionary sourceDictionary;

        private readonly object undefinedValue;

        private readonly bool formatString;

        #endregion

        #region Constructors and Destructors

        public DictionaryLookupConverter(
            FrameworkElement frameworkElement,
            IDictionary sourceDictionary,
            string resourceKey,
            object undefinedValue,
            bool formatString)
        {
            this.frameworkElement = frameworkElement;
            this.sourceDictionary = sourceDictionary;
            this.resourceKey = resourceKey;
            this.undefinedValue = undefinedValue;
            this.formatString = formatString;
        }

        #endregion

        #region Public Methods and Operators

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null)
            {
                return null;
            }

            if (this.frameworkElement == null)
            {
                return null;
            }

            // At least one value, which is a binding result, is exptected.
            if (values.Length < 1)
            {
                if (!DesignerProperties.GetIsInDesignMode(this.frameworkElement))
                {
                    Debug.Fail("At least one value is expected.");
                }

                return null;
            }

            var dictionary = this.sourceDictionary;
            var key = values[values.Length - 1];

            if (dictionary == null && !string.IsNullOrEmpty(this.resourceKey))
            {
                // Try to resolve dynamic resource.
                var resource = this.frameworkElement.TryFindResource(this.resourceKey);

                // ReSharper disable once CanBeReplacedWithTryCastAndCheckForNull
                if (resource is IDictionary)
                {
                    dictionary = (IDictionary)resource;
                }
            }

            if (dictionary == null)
            {
                return null;
            }

            object value = null;
            if (key != null)
            {
                value = dictionary[key];
            }

            if (value == null)
            {
                value = this.undefinedValue ?? DependencyProperty.UnsetValue;
            }

            if (value is string && this.formatString)
            {
                return string.Format((string)value, values);
            }

            var resourceString = value as ResourceString;
            if (resourceString != null)
            {
                if (this.formatString)
                {
                    return string.Format(resourceString.Value, values);
                }

                return resourceString.Value;
            }

            if (value is DynamicResourceReference)
            {
                var resourceReference = value as DynamicResourceReference;
                value = this.frameworkElement.TryFindResource(resourceReference.ResourceKey);

                // Make sure the referenced resource, if found, is of an expected type.
                if (resourceReference.TargetType != null && value != null)
                {
                    if (!resourceReference.TargetType.IsInstanceOfType(value))
                    {
                        value = null;
                    }
                }
            }

            return value;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
