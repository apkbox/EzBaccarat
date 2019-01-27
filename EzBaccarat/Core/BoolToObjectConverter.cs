// --------------------------------------------------------------------------------
// <copyright file="BoolToObjectConverter.cs" company="Oce Display Graphics Systems, Inc.">
//   Copyright (c) Oce Display Graphics Systems, Inc. All rights reserved.
// </copyright>
// <summary>
//   The bool to visibility converter.
// </summary>
// --------------------------------------------------------------------------------
namespace Shadows.Core.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    /// <summary>
    /// The bool to visibility converter.
    /// </summary>
    [ValueConversion(typeof(bool), typeof(object))]
    public class BoolToObjectConverter : IValueConverter
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BoolToObjectConverter"/> class. 
        /// </summary>
        public BoolToObjectConverter()
        {
            // set defaults
            this.TrueValue = "True";
            this.FalseValue = "False";
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the false value.
        /// </summary>
        public object FalseValue { get; set; }

        /// <summary>
        /// Gets or sets the true value.
        /// </summary>
        public object TrueValue { get; set; }

        #endregion

        #region Public Methods and Operators

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool))
            {
                return null;
            }

            return (bool)value ? this.TrueValue : this.FalseValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (object.Equals(value, this.TrueValue))
            {
                return true;
            }

            if (object.Equals(value, this.FalseValue))
            {
                return false;
            }

            return null;
        }

        #endregion
    }
}