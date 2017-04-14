﻿using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using AdaptiveCards.Rendering;
#if WPF
using System.Windows.Markup;
#elif XAMARIN
using Xamarin.Forms;
#endif


namespace AdaptiveCards.Rendering
{
    public partial class XamlRenderer : AdaptiveRenderer<FrameworkElement, RenderContext>
    {
        protected Action<object, ActionEventArgs> actionCallback;
        protected Action<object, MissingInputEventArgs> missingDataCallback;
        protected HostOptions _defaultCardStyling;

        public XamlRenderer(HostOptions options,
            ResourceDictionary resources,
            Action<object, ActionEventArgs> actionCallback = null,
            Action<object, MissingInputEventArgs> missingDataCallback = null)
            : base(options)
        {
            this.Resources = resources;
            this.actionCallback = actionCallback;
            this.missingDataCallback = missingDataCallback;
            SetObjectTypes();
        }

#if WPF
        public XamlRenderer(HostOptions options,
            string stylePath,
            Action<object, ActionEventArgs> actionCallback = null,
            Action<object, MissingInputEventArgs> missingDataCallback = null)
            : base(options)
        {
            this.StylePath = stylePath;
            this.actionCallback = actionCallback;
            this.missingDataCallback = missingDataCallback;
            SetObjectTypes();
        }
#endif

        private void SetObjectTypes()
        {
            this.SetRenderer<AdaptiveCard>(XamlAdaptiveCard.Render);

            this.SetRenderer<TextBlock>(XamlTextBlock.Render);
            this.SetRenderer<Image>(XamlImage.Render);

            this.SetRenderer<ActionSet>(XamlActionSet.Render);
            this.SetRenderer<Container>(XamlContainer.Render);
            this.SetRenderer<Column>(XamlColumn.Render);
            this.SetRenderer<ColumnSet>(XamlColumnSet.Render);
            this.SetRenderer<ActionSet>(XamlActionSet.Render);
            this.SetRenderer<FactSet>(XamlFactSet.Render);
            this.SetRenderer<ImageSet>(XamlImageSet.Render);

            this.SetRenderer<ChoiceSet>(XamlChoiceSet.Render);
            this.SetRenderer<TextInput>(XamlTextInput.Render);
            this.SetRenderer<NumberInput>(XamlNumberInput.Render);
            this.SetRenderer<DateInput>(XamlDateInput.Render);
            this.SetRenderer<TimeInput>(XamlTimeInput.Render);
            this.SetRenderer<ToggleInput>(XamlToggleInput.Render); ;

            this.SetRenderer<SubmitAction>(XamlSubmitAction.Render);
            this.SetRenderer<HttpAction>(XamlHttpAction.Render); ;
            this.SetRenderer<OpenUrlAction>(XamlOpenUrlAction.Render);
            this.SetRenderer<ShowCardAction>(XamlShowCardAction.Render);
        }

        /// <summary>
        /// Resource dictionary to use when rendering
        /// </summary>
        private ResourceDictionary _resources;
        public ResourceDictionary Resources
        {
            get
            {
                if (_resources != null)
                    return _resources;

#if WPF
                using (var styleStream = File.OpenRead(this.StylePath))
                {
                    _resources = (ResourceDictionary)XamlReader.Load(styleStream);
                }
#elif XAMARIN
                // TODO
#endif
                return _resources;
            }
            set
            {
                this._resources = value;
            }
        }

        /// <summary>
        /// Path to Xaml resource dictionary
        /// </summary>
        private string _stylePath;
        public string StylePath
        {
            get { return _stylePath; }
            set
            {
                this._stylePath = value;
                this._resources = null;
            }
        }


#if WPF
        /// <summary>
        /// AdaptiveCard
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public FrameworkElement RenderAdaptiveCard(AdaptiveCard card, Func<string, MemoryStream> imageResolver = null, HostOptions options = null)
        {
            RenderContext context = new RenderContext(this.actionCallback, this.missingDataCallback, imageResolver)
            {
                Options = options ?? this.DefaultOptions,
                Resources = this.Resources,
                ElementRenderers = this.ElementRenderers
            };
            return context.Render(card);
        }

        public FrameworkElement RenderShowCard(ShowCardAction showCard, Func<string, MemoryStream> imageResolver = null, HostOptions options = null)
        {
            RenderContext context = new RenderContext(this.actionCallback, this.missingDataCallback, imageResolver)
            {
                Options = options ?? this.DefaultOptions,
                Resources = this.Resources,
                ElementRenderers = this.ElementRenderers
            };

            return context.Render(showCard.Card);
        }

#elif XAMARIN
        /// <summary>
        /// AdaptiveCard
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public View RenderAdaptiveCard(AdaptiveCard card, Func<string, MemoryStream> imageResolver = null, HostOptions options = null)
        {
            RenderContext context = new RenderContext(this.actionCallback, this.missingDataCallback, imageResolver)
            {
                Options = options ?? this.DefaultOptions
            };
            return context.Render(card);
        }

        public View RenderShowCard(ShowCardAction showCard, Func<string, MemoryStream> imageResolver = null, HostOptions options = null)
        {
            RenderContext context = new RenderContext(this.actionCallback, this.missingDataCallback, imageResolver)
            {
                Options = options ?? this.DefaultOptions
            };
            return context.Render(showCard);
        }

#endif

    }
}
