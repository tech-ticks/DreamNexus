using System;
using System.Collections.Generic;
using System.Text;

namespace SkyEditor.RomEditor.Rtdx.Reverse.graphics.camera
{
    public class CameraParameter
    {
        public string symbol;
        public float positionX;
        public float positionY;
        public float positionZ;
        public float atX;
        public float atY;
        public float atZ;
        public float fov;
        public float near;
        public float far;
        public float shadowDistance;

        public void ReadStream(IBinaryDataByteStream stream)
        {
            throw new NotImplementedException();
        }

        public void WriteStream(IBinaryDataByteStream stream)
        {
            throw new NotImplementedException();
        }

        public CameraParameter()
        {
            throw new NotImplementedException();
        }
    }
}
