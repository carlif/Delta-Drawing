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
    /// Interaction logic for DeltaColorControl.xaml
    /// </summary>
    public partial class DeltaColorControl : UserControl
    {
        public DeltaColorControl()
        {
            InitializeComponent();
            labelBrush = new SolidColorBrush(this.Color);
            labelSwatch.Background = labelBrush;
            AlphaChannel.ValueChanged += new RoutedPropertyChangedEventHandler<DeltaValue>(Channel_ValueChanged);
            RedChannel.ValueChanged += new RoutedPropertyChangedEventHandler<DeltaValue>(Channel_ValueChanged);
            GreenChannel.ValueChanged += new RoutedPropertyChangedEventHandler<DeltaValue>(Channel_ValueChanged);
            BlueChannel.ValueChanged += new RoutedPropertyChangedEventHandler<DeltaValue>(Channel_ValueChanged);
        }

        void Channel_ValueChanged(object sender, RoutedPropertyChangedEventArgs<DeltaValue> e)
        {
            //labelBrush.Color = this.Color;
        }

        private SolidColorBrush labelBrush;

        public Color Color
        {
            get { return (Color)ColorConverter.ConvertFromString(ColorString()); }
            set { setColor(value); }
        }

        private Color originalColor;

        public Color OriginalColor
        {
            get { return originalColor; }
            set { originalColor = value; }
        }

        public String ColorString()
        {
            return String.Format("#{0}{1}{2}{3}",
                hexFromInt(Convert.ToInt32(this.AlphaChannel.Value.Value)),
                hexFromInt(Convert.ToInt32(this.RedChannel.Value.Value)),
                hexFromInt(Convert.ToInt32(this.GreenChannel.Value.Value)),
                hexFromInt(Convert.ToInt32(this.BlueChannel.Value.Value))
                );
        }

        private String hexFromInt(int i)
        {
            return String.Format("{0:X2}", validatedColorInt(i));
        }

        private void setColor(Color c)
        {
            this.AlphaChannel.Value.Value = Convert.ToInt32(c.A);
            this.RedChannel.Value.Value = Convert.ToInt32(c.R);
            this.GreenChannel.Value.Value = Convert.ToInt32(c.G);
            this.BlueChannel.Value.Value = Convert.ToInt32(c.B);
        }

        private int intFromHex(string hex)
        {
            return (System.Int32.Parse(hex, System.Globalization.NumberStyles.AllowHexSpecifier));
        }

        private int validatedColorInt(int i)
        {
            if (i < 0) return 0;
            if (i > 255) return 255;
            return i;
        }

    }
}
