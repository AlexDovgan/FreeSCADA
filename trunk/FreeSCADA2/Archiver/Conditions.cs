using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace FreeSCADA.Archiver
{
	[Serializable]
	[XmlInclude(typeof(TimeIntervalCondition))]
	public abstract class BaseCondition
	{
		bool isValid = false;

		[Browsable(false)]
		[XmlIgnore]
		virtual public bool IsValid
		{
			get { return isValid; }
			internal set { isValid = value;	}
		}

		[Browsable(false)]
		virtual public string Name
		{
			get { return this.GetType().ToString(); }
		}

		[Browsable(false)]
		virtual public string Description
		{
			get { return string.Format("Description of {0}", Name); }
		}

		virtual public void Process()
		{
		}
	}

	[Serializable]
	public class TimeIntervalCondition:BaseCondition
	{
		int interval; //Interval in ms
		DateTime lastCheck = new DateTime();

		public TimeIntervalCondition()
		{
			interval = 1000;
		}

		public TimeIntervalCondition(int interval)
		{
			this.interval = interval;
		}

		public int Interval
		{
			get { return interval; }
			set { interval = value; }
		}

		override public string Name
		{
			get { return StringConstants.TimeIntervalConditionName; }
		}

		override public string Description
		{
			get { return StringConstants.TimeIntervalConditionDescription; }
		}

		override public void Process()
		{
			DateTime currentTime = DateTime.Now;
			TimeSpan delta = new TimeSpan(0, 0, 0, 0, interval);
			if (lastCheck + delta <= currentTime)
			{
				lastCheck = currentTime;
				IsValid = true;
			}
			else
				IsValid = false;
		}
	}
}
