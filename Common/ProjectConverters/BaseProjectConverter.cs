
namespace FreeSCADA.Common.ProjectConverters
{
	abstract class BaseProjectConverter
	{

		public virtual int AcceptedVersion
		{
			get { return -1; }
		}
		public virtual int ResultVersion
		{
			get { return -1; }
		}

		public virtual bool Convert(Project prj)
		{
			if (prj.Version == AcceptedVersion)
				return true;
			return false;
		}
		public virtual bool ConvertBack(Project prj)
		{
			if (prj.Version == ResultVersion)
				return true;
			return false;
		}

	}
}
