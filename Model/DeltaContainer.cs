using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace DeltaDrawing.Model
{
    [Serializable]
    public class DeltaContainer
    {
        public DeltaContainer()
        {
            // Retain for legacy use
            m_DeltaPaths = new List<DeltaPath>();

            // Now offers more functionality over DeltaPath
            m_DeltaFigures = new List<DeltaFigure>();
            
            XmlDocument doc = new XmlDocument();
            m_Comments = doc.CreateCDataSection(string.Empty);
        }

        private XmlCDataSection m_Comments;
        public XmlCDataSection Comments
        {
            get { return m_Comments; }
            set { m_Comments = value; }
        }

        private List<DeltaPath> m_DeltaPaths;
        public List<DeltaPath> DeltaPaths
        {
            get { return m_DeltaPaths; }
            set { m_DeltaPaths = value; }
        }

        private List<DeltaFigure> m_DeltaFigures;
        public List<DeltaFigure> DeltaFigures
        {
            get { return m_DeltaFigures; }
            set { m_DeltaFigures = value; }
        }

        private string m_Name;
        [XmlAttribute("name")]
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        private double m_Scale;
        [XmlAttribute("scale")]
        public double Scale
        {
            get { return m_Scale; }
            set { m_Scale = value; }
        }

        private double m_DisplayScale;
        [XmlAttribute("displayScale")]
        public double DisplayScale
        {
            get { return m_DisplayScale; }
            set { m_DisplayScale = value; }
        }

        private double m_BlurRadius;
        [XmlAttribute("blurRadius")]
        public double BlurRadius
        {
            get { return m_BlurRadius; }
            set { m_BlurRadius = value; }
        }

        private TransformContainer m_Transforms;
        public TransformContainer Transforms
        {
            get { return m_Transforms; }
            set { m_Transforms = value; }
        }

        public override string ToString()
        {
            return Serializer.SerializeObject(this);
        }

        public DeltaContainer FromString(string xml)
        {
            return Serializer.DeserializeObject<DeltaContainer>(xml);
        }

    }
}
