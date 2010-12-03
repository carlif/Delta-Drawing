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
            m_DeltaPaths = new List<DeltaPath>();
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

        //private double m_CropAtX;
        //[XmlAttribute("cropAtX")]
        //public double CropAtX
        //{
        //    get { return m_CropAtX; }
        //    set { m_CropAtX = value; }
        //}

        //private double m_CropAtY;
        //[XmlAttribute("cropAtY")]
        //public double CropAtY
        //{
        //    get { return m_CropAtY; }
        //    set { m_CropAtY = value; }
        //}

        //private double m_CropHeight;
        //[XmlAttribute("cropHeight")]
        //public double CropHeight
        //{
        //    get { return m_CropHeight; }
        //    set { m_CropHeight = value; }
        //}

        //private double m_CropWidth;
        //[XmlAttribute("cropWidth")]
        //public double CropWidth
        //{
        //    get { return m_CropWidth; }
        //    set { m_CropWidth = value; }
        //}

        //private double m_RotateDegrees;
        //[XmlAttribute("rotateDegrees")]
        //public double RotateDegrees
        //{
        //    get { return m_RotateDegrees; }
        //    set { m_RotateDegrees = value; }
        //}

        //private double m_RotatePosX;
        //[XmlAttribute("rotatePosX")]
        //public double RotatePosX
        //{
        //    get { return m_RotatePosX; }
        //    set { m_RotatePosX = value; }
        //}

        //private double m_RotatePosY;
        //[XmlAttribute("rotatePosY")]
        //public double RotatePosY
        //{
        //    get { return m_RotatePosY; }
        //    set { m_RotatePosY = value; }
        //}

        //private bool m_CropAfterRotating;
        //[XmlAttribute("cropAfterRotating")]
        //public bool CropAfterRotating
        //{
        //    get { return m_CropAfterRotating; }
        //    set { m_CropAfterRotating = value; }
        //}

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
