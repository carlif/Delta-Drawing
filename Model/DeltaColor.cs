using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Xml.Serialization;

namespace DeltaDrawing.Model
{
    [Serializable]
    public class DeltaColor
    {
        public DeltaColor() { }

        public DeltaColor(DeltaChannel alpha, DeltaChannel red, DeltaChannel green, DeltaChannel blue)
        {
            m_Alpha = alpha;
            m_Red = red;
            m_Green = green;
            m_Blue = blue;
        }

        private DeltaChannel m_Alpha;
        public DeltaChannel Alpha
        {
            get { return m_Alpha; }
            set { m_Alpha = value; }
        }

        private DeltaChannel m_Red;
        public DeltaChannel Red
        {
            get { return m_Red; }
            set { m_Red = value; }
        }

        private DeltaChannel m_Green;
        public DeltaChannel Green
        {
            get { return m_Green; }
            set { m_Green = value; }
        }

        private DeltaChannel m_Blue;
        public DeltaChannel Blue
        {
            get { return m_Blue; }
            set { m_Blue = value; }
        }

        private Color m_Color;
        [XmlIgnore]
        public Color Color
        {
            get
            {
                m_Color.A = Convert.ToByte(m_Alpha.Value);
                m_Color.R = Convert.ToByte(m_Red.Value);
                m_Color.G = Convert.ToByte(m_Green.Value);
                m_Color.B = Convert.ToByte(m_Blue.Value);
                return m_Color;
            }
            set
            {
                m_Alpha.Value = Convert.ToDouble(value.A);
                m_Red.Value = Convert.ToDouble(value.R);
                m_Green.Value = Convert.ToDouble(value.G);
                m_Blue.Value = Convert.ToDouble(value.B);
            }
        }

        public override string ToString()
        {
            return Serializer.SerializeObject(this);
        }

        public DeltaColor FromString(string xml)
        {
            return Serializer.DeserializeObject<DeltaColor>(xml);
        }

        public void ApplyDelta()
        {
            m_Alpha.ApplyDelta();
            m_Red.ApplyDelta();
            m_Green.ApplyDelta();
            m_Blue.ApplyDelta();
        }
    }
}
