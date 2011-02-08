using System.Collections.Generic;
using System.Windows.Forms;
using FreeSCADA.Archiver;
using FreeSCADA.Common;
using FreeSCADA.CommonUI.Interfaces;
using FreeSCADA.Designer.Dialogs;

namespace FreeSCADA.Designer.Views.ProjectNodes
{

	static class TreeResources
	{
		static public ImageList TreeIcons = new ImageList();
		public enum IconIndexes
		{
			Project = 0,
			Schemas,
			Schema,
			Channels,
			Plugin,
			Channel,
			Archiver,
			ArchiverRule,
			Scripts,
			Script
		}

		static TreeResources()
		{
			TreeIcons.Images.Add(global::FreeSCADA.Designer.Resources.tree_project);
			TreeIcons.Images.Add(global::FreeSCADA.Designer.Resources.tree_schemas);
			TreeIcons.Images.Add(global::FreeSCADA.Designer.Resources.tree_schema);
			TreeIcons.Images.Add(global::FreeSCADA.Designer.Resources.tree_channels);
			TreeIcons.Images.Add(global::FreeSCADA.Designer.Resources.tree_plugin);
			TreeIcons.Images.Add(global::FreeSCADA.Designer.Resources.tree_variable);
			TreeIcons.Images.Add(global::FreeSCADA.Designer.Resources.tree_archiver);
			TreeIcons.Images.Add(global::FreeSCADA.Designer.Resources.tree_rule);
			TreeIcons.Images.Add(global::FreeSCADA.Designer.Resources.tree_scripts);
			TreeIcons.Images.Add(global::FreeSCADA.Designer.Resources.tree_script);
		}
	}
    
	abstract class BaseNode
	{
        public abstract string Name
        {
            get;
            protected set;
        }
		public abstract int ImageIndex
		{
			get;
		}
		public virtual string Tooltip
		{
			get
			{
				return "";
			}
		}
        public TreeNode TreeNode
        {
            get;
            protected set;
        }
        /// <summary>
        /// return list of actions avilable for a node
        /// </summary>
        /// <returns></returns>
        public virtual List<BaseCommand> GetActions()
        {
            return new List<BaseCommand>();
        }

        public virtual IDocumentView GetView()
        {
            return null;
        }

		/// <summary>
		/// Creates or updates current node in tree view control.
		/// </summary>
		/// <param name="nodes">Collection of nodes in TreeView control</param>
		/// <param name="existingNodesMap">Map of node name and node objects existing in the treeView. If passed, then increases performance. Can be null, if not needed</param>
		/// <returns>Existing or new tree node</returns>
		public virtual TreeNode CreateTreeNode(TreeNodeCollection nodes, Dictionary<string, TreeNode> existingNodesMap)
		{
			//Don't create new node if it exists
			if (existingNodesMap == null)
			{
				TreeNode[] existingNodes = nodes.Find(Name, false);
				if (existingNodes.Length > 0)
					return existingNodes[0];
			}
			else
			{
				if (existingNodesMap.ContainsKey(Name))
					return existingNodesMap[Name];
			}

			TreeNode node = nodes.Add(Name);
			UpdateTreeNode(node);

			return TreeNode=node;
		}

		protected void UpdateTreeNode(TreeNode node)
		{
			node.Text = Name;
			node.Name = Name;
			node.ToolTipText = Tooltip;
			node.Tag = this;

			node.ImageIndex = ImageIndex;
			node.SelectedImageIndex = ImageIndex;
		}

		protected void UpdateNodes(BaseNode[] nodes, TreeNode root)
		{
			if (nodes.Length > 100)
			{
				//System.Console.WriteLine("Number of nodes to update is too big. Consider to use SetNodes function");
			}

			Dictionary<string, TreeNode> existingNodesMap = new Dictionary<string, TreeNode>();
			foreach (TreeNode node in root.Nodes)
				existingNodesMap[node.Name] = node;

			foreach (BaseNode node in nodes)
			{
				node.CreateTreeNode(root.Nodes, existingNodesMap);

				if (existingNodesMap.ContainsKey(node.Name))
					existingNodesMap.Remove(node.Name);
			}

			foreach (TreeNode node in existingNodesMap.Values)
				root.Nodes.Remove(node);
		}
	}

	abstract class BaseEntityNode : BaseNode
	{
		protected string entityName;
		ProjectEntityType entityType;

		public BaseEntityNode(ProjectEntityType entityType, string entityName)
		{
			this.entityType = entityType;
			this.entityName = entityName;
		}

		public override string Name
		{
			get { return entityName; }
            protected set { entityName = value; }
		}

		public virtual ProjectEntityType EntityType
		{
			get { return entityType; }
		}
	}

	class ProjectNode : BaseNode
	{
		public override string Name
		{
			get
			{
				if (string.IsNullOrEmpty(Env.Current.Project.FileName))
					return StringResources.UnsavedProjectName;
				else
				{
					string name = System.IO.Path.GetFileNameWithoutExtension(Env.Current.Project.FileName);
					return "{" + name + "}";
				}
			}
            protected set { }
		}

		public override int ImageIndex
		{
			get { return (int)TreeResources.IconIndexes.Project; }
		}

		public override TreeNode CreateTreeNode(TreeNodeCollection nodes, Dictionary<string, TreeNode> existingNodesMap)
		{
			TreeNode node;
			if (nodes.Count > 0)
			{
				node = nodes[0];
				base.UpdateTreeNode(node);
			}
			else
				node = base.CreateTreeNode(nodes, existingNodesMap);

			BaseNode[] newNodes =
			{
				new SchemasNode(),
				new ChannelsNode(),
				new ArchiverNode(),
				new ScriptsNode()
			};
			UpdateNodes(newNodes, node);

            return TreeNode = node;
		}
	}

	class SchemasNode : BaseNode
	{
		public override string Name
		{
			get { return StringResources.SchemasItemName; }
            protected set { }
		}

		public override int ImageIndex
		{
			get { return (int)TreeResources.IconIndexes.Schemas; }
		}

        public override IDocumentView GetView()
        {
            return new Views.SchemaView(new Common.Documents.SchemaDocument());
        }
		public override TreeNode CreateTreeNode(TreeNodeCollection nodes, Dictionary<string, TreeNode> existingNodesMap)
		{
			TreeNode root = base.CreateTreeNode(nodes, existingNodesMap);

			List<BaseNode> newNodes = new List<BaseNode>();
			foreach (string entity in Env.Current.Project.GetEntities(ProjectEntityType.Schema))
				newNodes.Add(new SchemaNode(entity));
			UpdateNodes(newNodes.ToArray(), root);

            return TreeNode = root;
		}
	}

	class SchemaNode : BaseEntityNode
	{
        class RemoveSchemaAction : BaseCommand
        {
            SchemaNode _node;
            public override string Name
            {
                get { return StringResources.ProjectContextMenuRemove; }
            }

            public RemoveSchemaAction(SchemaNode node)                
            {
                _node = node;
            }
            public override void Execute()
            {
                Env.Current.Project.RemoveEntity(ProjectEntityType.Schema,_node.Name );
                _node.TreeNode.Parent.Nodes.Remove(_node.TreeNode);
            }
        }
        class RenameSchemaAction : BaseCommand
        {
            SchemaNode _node;
            
            public override string Name
            {
                get { return StringResources.ProjectContextMenuRename; }
            }

            public RenameSchemaAction(SchemaNode node)
            {
                _node = node;
            }
            public override void Execute()
            {
                RenameEntityForm dlg = new RenameEntityForm(_node.Name);
                if (dlg.ShowDialog() == DialogResult.OK)
                {


                    Env.Current.Project.RenameEntity(ProjectEntityType.Schema, _node.Name, dlg.EntityName);
                    _node.Name = dlg.EntityName;

                    _node.UpdateTreeNode(_node.TreeNode);

                    List<BaseNode> newNodes = new List<BaseNode>();
                    if (Env.Current.Project.ContainsEntity(ProjectEntityType.Script, _node.Name))
                        newNodes.Add(new ScriptNode(_node.Name));
                    _node.UpdateNodes(newNodes.ToArray(), _node.TreeNode);
                }
            }
        }
/*        class OpenSchemaAction: BaseNodeAction
        {
            public OpenSchemaAction(SchemaNode node)
            {
                
            }
            public override bool IsDoubleClicked()
            { 
                return true; 
            }
            public override void Execute()
            {
                
            }
            
        }*/

        public override List<BaseCommand> GetActions()
        {
            List<BaseCommand> commands = new List<BaseCommand>();
            commands.Add(new RemoveSchemaAction(this));
            commands.Add(new RenameSchemaAction(this));
            return commands;
        }
		public SchemaNode(string entityName)
			:base(ProjectEntityType.Schema, entityName)
		{
		}

        public override IDocumentView GetView()
        {
            return new Views.SchemaView(new Common.Documents.SchemaDocument(entityName));
        }
		public override int ImageIndex
		{
			get { return (int)TreeResources.IconIndexes.Schema; }
		}

		public override TreeNode CreateTreeNode(TreeNodeCollection nodes, Dictionary<string, TreeNode> existingNodesMap)
		{
			TreeNode root = base.CreateTreeNode(nodes, existingNodesMap);

			List<BaseNode> newNodes = new List<BaseNode>();
			if (Env.Current.Project.ContainsEntity(ProjectEntityType.Script, Name))
				newNodes.Add(new ScriptNode(Name));
			UpdateNodes(newNodes.ToArray(), root);

            return TreeNode = root;
		}
	}

	class ChannelsNode : BaseNode
	{
		public override string Name
		{
			get { return StringResources.ChannelsItemName; }
            protected set { }
		}

		public override int ImageIndex
		{
			get { return (int)TreeResources.IconIndexes.Channels; }
		}

		public override TreeNode CreateTreeNode(TreeNodeCollection nodes, Dictionary<string, TreeNode> existingNodesMap)
		{
			TreeNode root = base.CreateTreeNode(nodes, existingNodesMap);

			List<BaseNode> newNodes = new List<BaseNode>();
			foreach (string plugId in Env.Current.CommunicationPlugins.PluginIds)
				newNodes.Add(new ChannelPluginNode(plugId));
			UpdateNodes(newNodes.ToArray(), root);

            return TreeNode = root;
		}
	}

	class ChannelPluginNode : BaseNode
	{
		string pluginId;

		public ChannelPluginNode(string pluginId)
		{
			this.pluginId = pluginId;
		}

		public override string Name
		{
			get { return Env.Current.CommunicationPlugins[pluginId].Name; }
            protected set { }
		}

		public override int ImageIndex
		{
			get { return (int)TreeResources.IconIndexes.Plugin; }
		}

		public override TreeNode CreateTreeNode(TreeNodeCollection nodes, Dictionary<string, TreeNode> existingNodesMap)
		{
			TreeNode root = base.CreateTreeNode(nodes, existingNodesMap);

			List<BaseNode> newNodes = new List<BaseNode>();
			foreach (FreeSCADA.Interfaces.IChannel ch in Env.Current.CommunicationPlugins[pluginId].Channels)
				newNodes.Add(new ChannelNode(pluginId, ch.Name));
			UpdateNodes(newNodes.ToArray(), root);

            return TreeNode = root;
		}
	}

	class ChannelNode : BaseNode
	{
		string pluginId;
		string channelName;

		public ChannelNode(string pluginId, string channelName)
		{
			this.pluginId = pluginId;
			this.channelName = channelName;
		}

		public override string Name
		{
			get { return channelName; }
            protected set { }
		}

		public override int ImageIndex
		{
			get { return (int)TreeResources.IconIndexes.Channel; }
		}

		public string FullId
		{
			get {return pluginId + "." + channelName; }
		}

		public FreeSCADA.Interfaces.IChannel Channel
		{
			get
			{
				foreach (FreeSCADA.Interfaces.IChannel ch in Env.Current.CommunicationPlugins[pluginId].Channels)
				{
					if(ch.Name == channelName)
						return ch;
				}
				return null;
			}
		}
	}

	class ArchiverNode : BaseNode
	{
		public override string Name
		{
			get { return StringResources.ArchiverItemName; }
            protected set { }
		}

		public override int ImageIndex
		{
			get { return (int)TreeResources.IconIndexes.Archiver; }
		}

		public override TreeNode CreateTreeNode(TreeNodeCollection nodes, Dictionary<string, TreeNode> existingNodesMap)
		{
			TreeNode root = base.CreateTreeNode(nodes, existingNodesMap);

			List<BaseNode> newNodes = new List<BaseNode>();
			foreach (Rule rule in ArchiverMain.Current.ChannelsSettings.Rules)
				newNodes.Add(new ArchiverRuleNode(rule));
			UpdateNodes(newNodes.ToArray(), root);

            return TreeNode = root;
		}
	}

	class ArchiverRuleNode : BaseNode
	{
		Rule rule;

		public ArchiverRuleNode(Rule rule)
		{
			this.rule = rule;
		}

		public override string Name
		{
			get { return rule.Name; }
            protected set { }
		}

		public override int ImageIndex
		{
			get { return (int)TreeResources.IconIndexes.ArchiverRule; }
		}
	}

	class ScriptsNode : BaseNode
	{
		public override string Name
		{
			get { return StringResources.ScriptsItemName; }
            protected set { }
		}

		public override int ImageIndex
		{
			get { return (int)TreeResources.IconIndexes.Scripts; }
		}

		public override TreeNode CreateTreeNode(TreeNodeCollection nodes, Dictionary<string, TreeNode> existingNodesMap)
		{
			TreeNode root = base.CreateTreeNode(nodes, existingNodesMap);

			List<BaseNode> newNodes = new List<BaseNode>();
			foreach (string script in Env.Current.Project.GetEntities(ProjectEntityType.Script))
			{
				if (Env.Current.Project.ContainsEntity(ProjectEntityType.Schema, script))
					continue;

				newNodes.Add(new ScriptNode(script));
			}
			UpdateNodes(newNodes.ToArray(), root);

            return TreeNode = root;
		}
	}

	class ScriptNode : BaseEntityNode
	{
        class RemoveScriptAction : BaseCommand
        {
            ScriptNode _node;
            public override string Name
            {
                get { return StringResources.ProjectContextMenuRemove; }
            }

            public RemoveScriptAction(ScriptNode node)
            {
                _node = node;
            }
            public override void Execute()
            {
                Env.Current.Project.RemoveEntity(ProjectEntityType.Script, _node.Name);
                _node.TreeNode.Parent.Nodes.Remove(_node.TreeNode);
            }
        }
        class RenameScriptAction : BaseCommand  {
            ScriptNode  _node;

            public override string Name
            {
                get { return StringResources.ProjectContextMenuRename; }
            }

            public RenameScriptAction(ScriptNode node)
            {
                _node = node;
            }
            public override void Execute()
            {
                RenameEntityForm dlg = new RenameEntityForm(_node.Name);
                if (dlg.ShowDialog() == DialogResult.OK)
                {

                    Env.Current.Project.RenameEntity(ProjectEntityType.Script, _node.Name, dlg.EntityName);
                    _node.Name = dlg.EntityName;
                    _node.UpdateTreeNode(_node.TreeNode);
                    List<BaseNode> newNodes = new List<BaseNode>();
                    _node.UpdateNodes(newNodes.ToArray(), _node.TreeNode);
                }
            }
        }

		public ScriptNode(string entityName)
			:base(ProjectEntityType.Script, entityName)
		{
		}

		public override int ImageIndex
		{
			get { return (int)TreeResources.IconIndexes.Script; }
		}
        public override List<BaseCommand> GetActions()
        {
            List<BaseCommand> commands = new List<BaseCommand>();
            commands.Add(new RemoveScriptAction(this));
            commands.Add(new RenameScriptAction(this));
            return commands;
        }
	}
}
