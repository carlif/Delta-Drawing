using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DeltaDrawing.Model
{
    [Serializable]
    public class TransformContainer
    {
        public TransformContainer() { }

        public CropTransform CropBeforeRendering
        {
            get;
            set;
        }

        public CropTransform CropBeforeRotating
        {
            get;
            set;
        }

        public RotateTransform Rotate
        {
            get;
            set;
        }

        public CropTransform CropAfterRotating
        {
            get;
            set;
        }
    }
}
