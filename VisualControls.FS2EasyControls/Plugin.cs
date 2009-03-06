using System.Collections.Generic;
using FreeSCADA.Interfaces;
using FreeSCADA.Interfaces.Plugins;

namespace FreeSCADA.VisualControls.FS2EasyControls
{
    public class Plugin: IVisualControlsPlug
    {
        private IEnvironment environment;
        List<IVisualControlDescriptor> controls = new List<IVisualControlDescriptor>();

        ~Plugin()
        {
        }

        #region IVisualControlsPlug Members

        public string Name
        {
            get { return StringConstants.PluginName; }
        }

        public IVisualControlDescriptor[] Controls
        {
            get { return controls.ToArray(); }
        }

        public string PluginId
        {
            get { return StringConstants.PluginId; }
        }

        public void Initialize(IEnvironment environment)
        {
            this.environment = environment;
            environment.Project.ProjectLoaded += new System.EventHandler(OnProjectLoad);

            AnalogTextValueDescriptor.Plugin = this;
            this.controls.Add(new AnalogTextValueDescriptor());

            LoadSettings();

            if (environment.Mode == EnvironmentMode.Designer)
            {
                ICommandContext context = environment.Commands.GetPredefinedContext(PredefinedContexts.VisualControls);
                environment.Commands.AddCommand(context, new PropertyCommand(this));
            }
        }
        #endregion

        public IEnvironment Environment
        {
            get { return environment; }
            set { Initialize(value); }
        }

        public void SaveSettings()
        {
            // Save global setting for the library to the project file, if any persisting global settings are necessary
        }

        void LoadSettings()
        {
            // Load global setting for the library from project file, if any persisting global settings are necessary
        }

        void OnProjectLoad(object sender, System.EventArgs e)
        {
            LoadSettings();
        }
    }
}
