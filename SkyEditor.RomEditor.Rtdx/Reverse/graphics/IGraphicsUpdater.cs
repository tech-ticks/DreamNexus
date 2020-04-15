using System;
using System.Collections.Generic;
using System.Text;

namespace SkyEditor.RomEditor.Rtdx.Reverse.graphics
{
	public interface IGraphicsUpdater
	{
		void UpdateFrameBuffer();
		void UpdateGraphics();
	}
}
