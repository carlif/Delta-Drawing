using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DeltaDrawing.Model
{
    [Serializable]
    public class RotateTransform : Transform
    {
        private double m_Angle;
        [XmlAttribute("angle")]
        public double Angle
        {
            get { return m_Angle; }
            set { m_Angle = value; }
        }
    }
}
