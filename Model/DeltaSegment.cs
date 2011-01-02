using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace DeltaDrawing.Model
{
    [Serializable]
    public class DeltaSegment
    {
        public DeltaSegment()
        {
            m_DeltaPoints = new List<DeltaPoint>();
        }

        private bool m_OffsetApplied = false;

        private string m_Name;
        [XmlAttribute("name")]
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        private List<DeltaPoint> m_DeltaPoints;
        public List<DeltaPoint> DeltaPoints
        {
            get { return m_DeltaPoints; }
            set { m_DeltaPoints = value; }
        }

        public void ApplyDelta()
        {
            m_DeltaPoints.ForEach(deltaPoint => deltaPoint.ApplyDelta());
        }

        public void ApplyOffset(double offsetX, double offsetY)
        {
            if (!m_OffsetApplied)
            {
                m_DeltaPoints.ForEach(deltaPoint => deltaPoint.ApplyOffset(offsetX, offsetY));
            }
            m_OffsetApplied = true;
        }

        public override string ToString()
        {
            return Serializer.SerializeObject(this);
        }

        public DeltaSegment FromString(string xml)
        {
            return Serializer.DeserializeObject<DeltaSegment>(xml);
        }
    }
}
