using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace DeltaDrawing.Model
{
    [Serializable]
    public class DeltaValue : INotifyPropertyChanged
    {
        public DeltaValue()
        {
            this.m_Min = 0;
            this.m_Max = double.MaxValue;
        }

        public DeltaValue(double value, float delta, double valueMinimum, double valueMaximum)
        {
            // Set the validation values first
            this.m_Min = valueMinimum;
            this.m_Max = valueMaximum;

            this.Value = value;
            this.Delta = delta;
        }

        private double m_Value;
        [XmlAttribute("value")]
        public double Value
        {
            get { return validatedValue(m_Value); }
            set { m_Value = value; 
                //FirePropertyChanged("Channel"); 
            }
        }

        private float m_Delta;
        [XmlAttribute("delta")]
        public float Delta
        {
            get { return m_Delta; }
            set { m_Delta = value; 
                //FirePropertyChanged("Delta"); 
            }
        }

        private double m_Min;
        [XmlIgnore]
        public double Min
        {
            get { return m_Min; }
            set { m_Min = value; }
        }

        private double m_Max;
        [XmlIgnore]
        public double Max
        {
            get { return m_Max; }
            set { m_Max = value; }
        }

        public void ApplyDelta()
        {
            this.Value += (double)m_Delta;
        }

        private double validatedValue(double val)
        {
            if (val < m_Min) val = m_Min;
            if (val > m_Max) val = m_Max;
            return val;
        }

        #region INotifyPropertyChanged Members

        internal void FirePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
