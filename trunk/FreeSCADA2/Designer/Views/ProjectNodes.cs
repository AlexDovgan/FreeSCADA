using System.Collections.Generic;
using System.Windows.Forms;
using FreeSCADA.Archiver;
using FreeSCADA.Common;

namespace FreeSCADA.Designer.Views.ProjectNodes
{

	static class Resources
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

		static Resources()
		{
			TreeIcons.Images.Add(global::FreeSCADA.Designer.Properties.Resources.tree_project);
			TreeIcons.Images.Add(global::FreeSCADA.Designer.Properties.Resources.tree_schemas);
			TreeIcons.Images.Add(global::FreeSCADA.Designer.Properties.Resources.tree_schema);
			TreeIcons.Images.Add(global::FreeSCADA.Designer.Properties.Resources.tree_channels);
			TreeIcons.Images.Add(global::FreeSCADA.Designer.Properties.Resources.tree_plugin);
			TreeIcons.Images.Add(global::FreeSCADA.Designer.Properties.Resources.tree_variable);
			TreeIcons.Images.Add(global::FreeSCADA.Designer.Properties.Resources.tree_archiver);
			TreeIcons.Images.Add(global::FreeSCADA.Designer.Properties.Resources.tree_rule);
			TreeIcons.Images.Add(global::FreeSCADA.Designer.Properties.Resources.tree_scripts);
			TreeIcons.Images.Add(global::FreeSCADA.Designer.Properties.Resources.tree_script);
		}
	}

	abstract class BaseNode
	{
		public abstract string Name
		{
			get;
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

			return node;
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
		string entityName;
		ProjectEntityType entityType;

		public BaseEntityNode(ProjectEntityType entityType, string entityName)
		{
			this.entityType = entityType;
			this.entityName = entityName;
		}

		public override string Name
		{
			get { return entityName; }
		}

		public virtual ProjectEntityType EntityType
		{
			get { return entityType; }
		}

		public virtual void Rename(string newName)
		{
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
		}

		public override int ImageIndex
		{
			get { return (int)Resources.IconIndexes.Project; }
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

			return node;
		}
	}

	class SchemasNode : BaseNode
	{
		public override string Name
		{
			get { return StringResources.SchemasItemName; }
		}

		public override int ImageIndex
		{
			get { return (int)Resources.IconIndexes.Schemas; }
		}

		public override TreeNode CreateTreeNode(TreeNodeCollection nodes, Dictionary<string, TreeNode> existingNodesMap)
		{
			TreeNode root = base.CreateTreeNode(nodes, existingNodesMap);

			List<BaseNode> newNodes = new List<BaseNode>();
			foreach (string entity in Env.Current.Project.GetEntities(ProjectEntityType.Schema))
				newNodes.Add(new SchemaNode(entity));
			UpdateNodes(newNodes.ToArray(), root);

			return root;
		}
	}

	class SchemaNode : BaseEntityNode
	{
		public SchemaNode(string entityName)
			:base(ProjectEntityType.Schema, entityName)
		{
		}

		public override int ImageIndex
		{
			get { return (int)Resources.IconIndexes.Schema; }
		}

		public override TreeNode CreateTreeNode(TreeNodeCollection nodes, Dictionary<string, TreeNode> existingNodesMap)
		{
			TreeNode root = base.CreateTreeNode(nodes, existingNodesMap);

			List<BaseNode> newNodes = new List<BaseNode>();
			if (Env.Current.Project.ContainsEntity(ProjectEntityType.Script, Name))
				newNodes.Add(new ScriptNode(Name));
			UpdateNodes(newNodes.ToArray(), root);

			return root;
		}
	}

	class ChannelsNode : BaseNode
	{
		public override string Name
		{
			get { return StringResources.ChannelsItemName; }
		}

		public override int ImageIndex
		{
			get { return (int)Resources.IconIndexes.Channels; }
		}

		public override TreeNode CreateTreeNode(TreeNodeCollection nodes, Dictionary<string, TreeNode> existingNodesMap)
		{
			TreeNode root = base.CreateTreeNode(nodes, existingNodesMap);

			List<BaseNode> newNodes = new List<BaseNode>();
			foreach (string plugId in Env.Current.CommunicationPlugins.PluginIds)
				newNodes.Add(new ChannelPluginNode(plugId));
			UpdateNodes(newNodes.ToArray(), root);

			return root;
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
		}

		public override int ImageIndex
		{
			get { return (int)Resources.IconIndexes.Plugin; }
		}

		public override TreeNode CreateTreeNode(TreeNodeCollection nodes, Dictionary<string, TreeNode> existingNodesMap)
		{
			TreeNode root = base.CreateTreeNode(nodes, existingNodesMap);

			List<BaseNode> newNodes = new List<BaseNode>();
			foreach (FreeSCADA.Interfaces.IChannel ch in Env.Current.CommunicationPlugins[pluginId].Channels)
				newNodes.Add(new ChannelNode(pluginId, ch.Name));
			UpdateNodes(newNodes.ToArray(), root);

			return root;
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
		}

		public override int ImageIndex
		{
			get { return (int)Resources.IconIndexes.Channel; }
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
		}

		public override int ImageIndex
		{
			get { return (int)Resources.IconIndexes.Archiver; }
		}

		public override TreeNode CreateTreeNode(TreeNodeCollection nodes, Dictionary<string, TreeNode> existingNodesMap)
		{
			TreeNode root = base.CreateTreeNode(nodes, existingNodesMap);

			List<BaseNode> newNodes = new List<BaseNode>();
			foreach (Rule rule in ArchiverMain.Current.ChannelsSettings.Rules)
				newNodes.Add(new ArchiverRuleNode(rule));
			UpdateNodes(newNodes.ToArray(), root);

			return root;
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
		}

		public override int ImageIndex
		{
			get { return (int)Resources.IconIndexes.ArchiverRule; }
		}
	}

	class ScriptsNode : BaseNode
	{
		public override string Name
		{
			get { return StringResources.ScriptsItemName; }
		}

		public override int ImageIndex
		{
			get { return (int)Resources.IconIndexes.Scripts; }
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

			return root;
		}
	}

	class ScriptNode : BaseEntityNode
	{
		public ScriptNode(string entityName)
			:base(ProjectEntityType.Script, entityName)
		{
		}

		public override int ImageIndex
		{
			get { return (int)Resources.IconIndexes.Script; }
		}
	}
}
