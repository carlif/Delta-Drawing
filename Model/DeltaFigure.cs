using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace DeltaDrawing.Model
{
    /// <summary>
    /// DeltaFigure has a collection of DeltaSegments. It is a line that changes position and shape.
    /// </summary>
    [Serializable]
    public class DeltaFigure
    {
        public DeltaFigure()
        {
            m_DeltaSegments = new List<DeltaSegment>();
            m_SkipIterationsList = new List<int>();
            this.SetSkipIterationList();
        }

        private List<DeltaSegment> m_DeltaSegments;
        public List<DeltaSegment> DeltaSegments
        {
            get { return m_DeltaSegments; }
            set { m_DeltaSegments = value; }
        }

        private DeltaPoint m_StartPoint;
        public DeltaPoint StartPoint
        {
            get { return m_StartPoint; }
            set { m_StartPoint = value; }
        }
        
        public DeltaFigure(double thickness, float thicknessDelta, int startIteration, int endIteration, string skipIterationsCSV)
        {
            m_SkipIterationsList = new List<int>();
            m_Thickness = new DeltaValue(thickness, thicknessDelta, 0, double.MaxValue);
            m_StartIteration = startIteration;
            m_EndIteration = endIteration;
            m_SkipIterationsCSV = skipIterationsCSV;
            this.SetSkipIterationList();
        }

        private bool m_OffsetApplied = false;

        private string m_Name;
        [XmlAttribute("name")]
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        private DeltaColor m_DeltaColor;

        public DeltaColor DeltaColor
        {
            get { return m_DeltaColor; }
            set { m_DeltaColor = value; }
        }

        private DeltaValue m_Thickness;

        public DeltaValue Thickness
        {
            get { return m_Thickness; }
            set { m_Thickness = value; }
        }

        private int m_StartIteration;
        [XmlAttribute("startIteration")]
        public int StartIteration
        {
            get { return m_StartIteration; }
            set { m_StartIteration = value; }
        }

        private int m_EndIteration;
        [XmlAttribute("endIteration")]
        public int EndIteration
        {
            get { return m_EndIteration; }
            set { m_EndIteration = value; }
        }

        private double m_OffsetX;
        [XmlAttribute("offsetX")]
        public double OffsetX
        {
            get { return m_OffsetX; }
            set { m_OffsetX = value; }
        }

        private double m_OffsetY;
        [XmlAttribute("offsetY")]
        public double OffsetY
        {
            get { return m_OffsetY; }
            set { m_OffsetY = value; }
        }

        private string m_SkipIterationsCSV;
        [XmlAttribute("skipIterationsCSV")]
        public string SkipIterationsCSV
        {
            get { return m_SkipIterationsCSV; }
            set
            {
                m_SkipIterationsCSV = value;
                SetSkipIterationList();
            }
        }

        private List<int> m_SkipIterationsList;

        [XmlIgnore]
        public List<int> SkipIterationsList
        {
            get { return m_SkipIterationsList; }
        }

        public void SetSkipIterationList()
        {
            m_SkipIterationsList.Clear();
            if (!String.IsNullOrEmpty(m_SkipIterationsCSV))
            {
                foreach (string iteration in m_SkipIterationsCSV.Split(','))
                {
                    m_SkipIterationsList.Add(int.Parse(iteration));
                }
            }
        }

        public void ApplyDelta()
        {
            m_Thickness.ApplyDelta();
            m_DeltaColor.ApplyDelta();
            m_StartPoint.ApplyDelta();
            m_DeltaSegments.ForEach(deltaSegment => deltaSegment.ApplyDelta());
        }

        public void ApplyOffset()
        {
            if (!m_OffsetApplied)
            {
                m_StartPoint.ApplyOffset(m_OffsetX, m_OffsetY);
                m_DeltaSegments.ForEach(deltaSegment => deltaSegment.ApplyOffset(m_OffsetX, m_OffsetY));
            }
            m_OffsetApplied = true;
        }

        public override string ToString()
        {
            return Serializer.SerializeObject(this);
        }

        public DeltaFigure FromString(string xml)
        {
            return Serializer.DeserializeObject<DeltaFigure>(xml);
        }
    }
}
