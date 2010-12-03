using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using DeltaDrawing.Model;

namespace DeltaDrawing.UI
{
    /// <summary>
    /// Interaction logic for DeltaChannelControl.xaml
    /// </summary>
    public partial class DeltaChannelControl : UserControl
    {
        public DeltaChannelControl()
        {
            InitializeComponent();
            this.Value = new DeltaValue(0, 0, 0, 255);
            this.Value.PropertyChanged += new PropertyChangedEventHandler(Value_PropertyChanged);
        }

        void Value_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateValue();
        }

        private static void LabelChannelTextValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DeltaChannelControl deltaChannelControl = (DeltaChannelControl)d;
            deltaChannelControl.labelChannel.Content = Convert.ToString(e.NewValue);
        }

        public static readonly DependencyProperty LabelChannelTextProperty = DependencyProperty.Register("LabelChannelText", typeof(String), typeof(DeltaChannelControl), new UIPropertyMetadata(DeltaChannelControl.LabelChannelTextValueChanged));
        public String LabelChannelText
        {
            get { return Convert.ToString(GetValue(LabelChannelTextProperty)); }
            set { SetValue(LabelChannelTextProperty, value); }
        }

        private static void LabelDeltaTextValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DeltaChannelControl deltaChannelControl = (DeltaChannelControl)d;
            deltaChannelControl.labelDelta.Content = Convert.ToString(e.NewValue);
        }

        public static readonly DependencyProperty LabelDeltaTextProperty = DependencyProperty.Register("LabelDeltaText", typeof(String), typeof(DeltaChannelControl), new UIPropertyMetadata(DeltaChannelControl.LabelDeltaTextValueChanged));
        public String LabelDeltaText
        {
            get { return Convert.ToString(GetValue(LabelDeltaTextProperty)); }
            set { SetValue(LabelDeltaTextProperty, value); }
        }

        public static DependencyProperty ValueProperty = DependencyProperty.Register("Value",
            typeof(DeltaValue), typeof(DeltaChannelControl),
            new FrameworkPropertyMetadata(null,
            FrameworkPropertyMetadataOptions.AffectsMeasure |
            FrameworkPropertyMetadataOptions.AffectsRender |
            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault |
            FrameworkPropertyMetadataOptions.Inherits,
            new PropertyChangedCallback(OnValueChanged),
            new CoerceValueCallback(CoerceValue)));

        public DeltaValue Value
        {
            get { return (DeltaValue)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }


        //That's pretty clear, right? Next step the callback. I want my control to bubble up value change event 
        //in order to be able to use it within external window (if needed). You are not really have to do it, 
        //if no one will listen to that event. So, when the value will change I'll call OnValueChanged event and update 
        //both TextBoxes with new value from Value property, Then I'll fire my Routed Event for value changing (RaiseEvent method is rules!) J
        static void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            DeltaChannelControl deltaChannel = sender as DeltaChannelControl;

            deltaChannel.UpdateValue();

            RoutedPropertyChangedEventArgs<DeltaValue> e = new RoutedPropertyChangedEventArgs<DeltaValue>(
            (DeltaValue)args.OldValue, (DeltaValue)args.NewValue, ValueChangedEvent);

            deltaChannel.OnValueChanged(e);
        }

        private void UpdateValue()
        {
            this.textChannel.Text = Value.Value.ToString();
            this.textDelta.Text = Value.Delta.ToString();
        }

        private void OnValueChanged(RoutedPropertyChangedEventArgs<DeltaValue> e)
        {
            RaiseEvent(e);
        }

        //Now I have to provide getter and setter for the event and build the event itself. Piece of cake, trust me J. 
        //This one will be RoutedEvent and we'll register it within Event Manager of WPF exactly as we do it with DependencyProperty and Property Manager
        public static readonly RoutedEvent ValueChangedEvent =
            EventManager.RegisterRoutedEvent(
                "ValueChanged",
                RoutingStrategy.Bubble,
                typeof(RoutedPropertyChangedEventHandler<DeltaValue>),
                typeof(DeltaChannelControl));

        public event RoutedPropertyChangedEventHandler<DeltaValue> ValueChanged
        {
            add { AddHandler(ValueChangedEvent, value); }
            remove { RemoveHandler(ValueChangedEvent, value); }
        }

        //Now, we'll handle OnTextChange event fired by TextBox to update our value
        void onTextChanged(object sender, TextChangedEventArgs e)
        {
            //Unregister event
            this.Value.PropertyChanged -= new PropertyChangedEventHandler(Value_PropertyChanged);

            double channel = 0;
            float delta = 0;
            if (double.TryParse(textChannel.Text == String.Empty ? "0" : textChannel.Text, out channel) &
            float.TryParse(textDelta.Text == String.Empty ? "0" : textDelta.Text, out delta))
            {
                Value = new DeltaValue(channel, delta, 0, 255);
            }
            else
            {
                UpdateValue();
            }

            //Re-register event
            this.Value.PropertyChanged += new PropertyChangedEventHandler(Value_PropertyChanged);
        }

        //And build the method for coercing values
        static object CoerceValue(DependencyObject sender, object value)
        {
            DeltaValue val = value as DeltaValue;

            //TODO: Implement validation

            //if (val != null)
            //{
            //    if (val.Latitude < 0)
            //    {
            //        val.Latitude = 0;
            //    }
            //    else if (val.Latitude > 90)
            //    {
            //        val.Latitude = 90;
            //    }

            //    if (val.Longitude < -180)
            //    {
            //        val.Longitude = -180;
            //    }
            //    else if (val.Longitude > 180)
            //    {
            //        val.Longitude = 180;
            //    }
            //}
            return val;
        }
    }
}
