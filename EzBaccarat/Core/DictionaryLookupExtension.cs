// --------------------------------------------------------------------------------
// <copyright file="DictionaryLookupExtension.cs" company="Oce Display Graphics Systems, Inc.">
//   Copyright (c) Oce Display Graphics Systems, Inc. All rights reserved.
// </copyright>
// <summary>
//   Defines the DictionaryLookupExtension type.
// </summary>
// --------------------------------------------------------------------------------
namespace Shadows.Core
{
    using System;
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Markup;

    /// <summary>
    /// Looks up a value in dictionary.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Define messages enumeration or use other way to identify messages.
    /// </para>
    /// <example>
    /// Public enum MessageIds
    /// {
    ///     Message1,
    ///     Message2,
    ///     Message3
    /// }
    /// </example>
    /// <para>
    /// Then define a dictionary that can be used to define messages in XAML.
    /// Any base class that implements <see cref="IDictionary"/> works.
    /// </para>
    /// <example>
    /// Public class MessagesDictionary : Dictionary&lt;MessageIds, string&gt;
    /// {
    /// }
    /// </example>
    /// <para>
    /// Define localizable strings in XAML using MessageIds values as keys.
    /// </para>
    /// <example>
    /// &lt;UserControl.Resources&gt;
    ///     &lt;MessagesDictionary x:Key="Messages"&gt;
    ///         &lt;String x:Key="{x:Static MessageIds.Message1}"&gt;Message 1&lt;/String&gt;
    ///         &lt;String x:Key="{x:Static MessageIds.Message2}"&gt;Message 2 with parameter {0}&lt;/String&gt;
    ///         &lt;String x:Key="{x:Static MessageIds.Message3}"&gt;Message 3 with parameter {0}&lt;/String&gt;
    ///     &lt;/MessagesDictionary&gt;
    /// &lt;/UserControl.Resources&gt;
    /// </example>
    /// <para>
    /// As an alternative <see cref="Hashtable"/> can be used instead of defining MessagesDictionary. Keep in mind however
    /// that if enumeration is used as a key, then <see cref="Hashtable"/> will not work correctly due to boxing. The internal key comparison
    /// will attempt to compare boxed keys by reference instead of by value.
    /// </para>
    /// <para>
    /// <list type="bullet">
    /// <item>
    /// Specify Binding that resolves to a value used to lookup the message.
    /// </item>
    /// <item>
    /// Specify either Source as a static resource that returns <see cref="IDictionary"/>
    /// or ResourceKey as a resource key that will be resolved dynamically.
    /// </item>
    /// <item>
    /// Specify optional Parameter property. The Parameter is used to format the resulting string if both the resulting
    /// string and resolved parameter value are not null.
    /// </item>
    /// </list>
    /// </para>
    /// <example>
    /// &lt;TextBlock Text="{DictionaryLookup Binding={Binding MessageId}, Source={StaticResource Messages}, Parameter={Binding MessageParameter}}"/&gt;
    /// </example>
    /// </remarks>
    [MarkupExtensionReturnType(typeof(object))]
    [ContentProperty("Parameters")]
    public class DictionaryLookupExtension : MarkupExtension
    {
        #region Fields

        private readonly Collection<BindingBase> bindings = new Collection<BindingBase>();

        #endregion

        #region Constructors and Destructors

        public DictionaryLookupExtension()
        {
            this.FormatString = true;
        }

        public DictionaryLookupExtension(BindingBase binding)
        {
            this.FormatString = true;
            this.Binding = binding;
        }

        #endregion

        #region Public Properties

        public BindingBase Binding { get; set; }

        public bool FormatString { get; set; }

        public BindingBase Parameter { get; set; }

        public Collection<BindingBase> Parameters
        {
            get
            {
                return this.bindings;
            }
        }

        public string ResourceKey { get; set; }

        public IDictionary Source { get; set; }

        public object UndefinedValue { get; set; }

        #endregion

        #region Public Methods and Operators

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var provideValueTarget = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            if (provideValueTarget == null)
            {
                return this;
            }

            var frameworkElement = provideValueTarget.TargetObject as FrameworkElement;
            if (frameworkElement == null)
            {
                return this;
            }

            if (this.Binding == null)
            {
                if (!DesignerProperties.GetIsInDesignMode(frameworkElement))
                {
                    Debug.Fail("A value binding is expected.");
                }

                return this;
            }

            if (this.Source != null && !string.IsNullOrEmpty(this.ResourceKey))
            {
                if (!DesignerProperties.GetIsInDesignMode(frameworkElement))
                {
                    Debug.Fail("Source and ResourceKey cannot both specified.");
                }

                return this;
            }

            var mb = new MultiBinding();

            if (this.Parameter != null && this.Parameters.Count > 0)
            {
                throw new ArgumentException("Parameter and Parameters properties cannot be set at the same time.");
            }

            if (this.Parameter != null)
            {
                mb.Bindings.Add(this.Parameter);
            }
            else
            {
                foreach (var parameter in this.Parameters)
                {
                    mb.Bindings.Add(parameter);
                }
            }

            mb.Bindings.Add(this.Binding);

            mb.Converter = new DictionaryLookupConverter(
                frameworkElement,
                this.Source,
                this.ResourceKey,
                this.UndefinedValue,
                this.FormatString);

            return mb.ProvideValue(serviceProvider);
        }

        #endregion
    }
}
