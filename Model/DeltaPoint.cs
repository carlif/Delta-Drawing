using System;
using System.Xml.Serialization;

namespace DeltaDrawing.Model
{
    [Serializable]
    public class DeltaPoint
    {
        public DeltaPoint() { }

        public DeltaPoint(double x, double y, double deltaX, double deltaY, double deltaXdelta, double deltaYdelta)
        {
            this.X = x;
            this.Y = y;
            this.DeltaX = deltaX;
            this.DeltaY = deltaY;
            this.DeltaXDelta = deltaXdelta;
            this.DeltaYDelta = deltaYdelta;
        }

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

        private double m_DeltaX;
        [XmlAttribute("deltaX")]
        public double DeltaX
        {
            get { return m_DeltaX; }
            set
            {
                m_DeltaX = value;
            }
        }

        private double m_DeltaY;
        [XmlAttribute("deltaY")]
        public double DeltaY
        {
            get { return m_DeltaY; }
            set
            {
                m_DeltaY = value;
            }
        }

        private double m_DeltaXDelta;
        [XmlAttribute("deltaXdelta")]
        public double DeltaXDelta
        {
            get { return m_DeltaXDelta; }
            set
            {
                m_DeltaXDelta = value;
            }
        }

        private double m_DeltaYDelta;
        [XmlAttribute("deltaYdelta")]
        public double DeltaYDelta
        {
            get { return m_DeltaYDelta; }
            set
            {
                m_DeltaYDelta = value;
            }
        }

        public void ApplyDelta()
        {
            m_X += m_DeltaX;
            m_Y += m_DeltaY;
            m_DeltaX += m_DeltaXDelta;
            m_DeltaY += m_DeltaYDelta;
        }

        public void ApplyOffset(double offsetX, double offsetY)
        {
            m_X += offsetX;
            m_Y += offsetY;
        }

    }
}
