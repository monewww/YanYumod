
using Verse;

namespace YanYu
{
	public class CompProperties_RenameByGender : CompProperties
	{
		public CompProperties_RenameByGender()
		{
			compClass = typeof(CompRenameByGender);
		}
	}

	// 只是个标记，用于在补丁里检测
	public class CompRenameByGender : ThingComp { }
}
