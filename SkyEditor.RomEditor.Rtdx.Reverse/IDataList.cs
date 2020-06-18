using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SkyEditor.RomEditor.Rtdx.Reverse
{
	public interface IDataList<T> : IEnumerable<T>, IEnumerable
	{
		T GetFromListId(int id);
		int GetNum();
		int GetMax();
		bool Add(T data);
		void Clear();
		List<T> GetCoreList();
	}
}
