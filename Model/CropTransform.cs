using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DeltaDrawing.Model
{
    [Serializable]
    public class CropTransform : Transform
    {
        private double m_Width;
        [XmlAttribute("width")]
        public double Width
        {
            get { return m_Width; }
            set { m_Width = value; }
        }

        private double m_Height;
        [XmlAttribute("height")]
        public double Height
        {
            get { return m_Height; }
            set { m_Height = value; }
        }
    }
}
