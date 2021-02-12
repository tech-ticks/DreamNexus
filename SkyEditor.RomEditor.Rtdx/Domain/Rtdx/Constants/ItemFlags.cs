using System;

namespace SkyEditor.RomEditor.Domain.Rtdx.Constants
{
  [Flags]
	public enum ItemFlags : ushort
	{
		THINK = 1,
		THROW_PARTY = 2,
		THROW_ENEMY = 4,
		IMPLEMENT = 8,
		EXCHANGE = 16,
		DESTROYED = 32,
		MEGUMI_EAT = 64,
		MEGUMI_BURN = 128,
		EAT = 256,
		SECURE = 512,
		DLC = 1024,
		AUTO_LOG = 2048,
		HIGH_CLASS = 4096
	}
}
