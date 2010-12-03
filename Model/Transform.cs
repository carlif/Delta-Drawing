using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DeltaDrawing.Model
{
    [Serializable]
    public class Transform
    {
        private double m_X;
        [XmlAttribute("x")]
        public double X
        {
            get { return m_X; }
            set
            {
                m_X = value;
            }
        }

        private double m_Y;
        [XmlAttribute("y")]
        public double Y
        {
            get { return m_Y; }
            set
            {
                m_Y = value;
            }
        }

        [XmlAttribute("apply")]
        public bool Apply
        {
            get;
            set;
        }
    }
}
