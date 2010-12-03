using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace DeltaDrawing.Model
{
    [Serializable]
    public class DeltaPath
    {
        public DeltaPath()
        {
            m_SkipIterationsList = new List<int>();
            this.SetSkipIterationList();
        }
        
        public DeltaPath(double thickness, float thicknessDelta, int startIteration, int endIteration, string skipIterationsCSV)
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

        private DeltaPoint m_Point0;

        public DeltaPoint Point0
        {
            get { return m_Point0; }
            set { m_Point0 = value; }
        }

        private DeltaPoint m_Point1;

        public DeltaPoint Point1
        {
            get { return m_Point1; }
            set { m_Point1 = value; }
        }

        private DeltaPoint m_Point2;

        public DeltaPoint Point2
        {
            get { return m_Point2; }
            set { m_Point2 = value; }
        }

        private DeltaPoint m_Point3;

        public DeltaPoint Point3
        {
            get { return m_Point3; }
            set { m_Point3 = value; }
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
            m_Point0.ApplyDelta();
            m_Point1.ApplyDelta();
            m_Point2.ApplyDelta();
            m_Point3.ApplyDelta();
            m_DeltaColor.ApplyDelta();
            m_Thickness.ApplyDelta();
        }

        public void ApplyOffset()
        {
            if (!m_OffsetApplied)
            {
                m_Point0.ApplyOffset(m_OffsetX, m_OffsetY);
                m_Point1.ApplyOffset(m_OffsetX, m_OffsetY);
                m_Point2.ApplyOffset(m_OffsetX, m_OffsetY);
                m_Point3.ApplyOffset(m_OffsetX, m_OffsetY);
            }
            m_OffsetApplied = true;
        }

        public override string ToString()
        {
            return Serializer.SerializeObject(this);
        }

        public DeltaPath FromString(string xml)
        {
            return Serializer.DeserializeObject<DeltaPath>(xml);
        }
    }
}
