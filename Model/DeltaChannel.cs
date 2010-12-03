using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace DeltaDrawing.Model
{
    [Serializable]
    public class DeltaChannel : DeltaValue
    {
        public DeltaChannel()
        {
            this.Min = 0;
            this.Max = 255;
        }
    }
}
