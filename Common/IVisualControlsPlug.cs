using System;

namespace FreeSCADA.Interfaces
{
	namespace Plugins
	{
        public interface IVisualControlsPlug
		{
			String Name
			{
				get;
			}

            IVisualControlDescriptor[] Controls
			{
				get;
			}

			string PluginId
			{
				get;
			}

			void Initialize(IEnvironment environment);

        }
	}
}
