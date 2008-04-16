-------------------------------------------------------------------------------
Legal 
-------------------------------------------------------------------------------
You may use/modify the source library FREELY for personal
or business use as long as this header kept intact, the 
assembly information (AssemblyInfo.cs) is not altered and the
code is kept as a standalone library module as provided.

I am NOT or may not be held responsible for any damage/loss of
data or malfunction arising from the use of this source/library.

© Aju George 2004 - 2005 ( or System.DateTime.Now.Year ;) ).
-------------------------------------------------------------------------------


-------------------------------------------------------------------------------
Post Scriptum :
-------------------------------------------------------------------------------
If you like to make donations (What is he talking about? huh!)
It might make me write more like this! contact me @ my email ;)
                           *blush*
-------------------------------------------------------------------------------

-------------------------------------------------------------------------------
Notes:
-------------------------------------------------------------------------------
When xml serializing the toolbox, you can use OnSerializeObject and 
OnDeSerializeObject events to handle the  saving/loading of the object 
associated with a tool box item. You can also save/load the control info of a 
tab in these events.
  
Tab/Item Renaming configurable by SelectAllTextWhileRenaming and 
UseItemColorInRename properties.
  
Swapping of Tab/TabItem can be turned on/off by AllowSwappingByDragDrop 
property.
  
Custom sizes of tab items in large/small icon view can be specified using 
LargeItemSize, SmallItemSize property.
  
A window control can be associated with a tab control using it's Control property.
If a control is specified like  this, the tool box items for that tab 
will be destroyed.
  
ToolBoxTabs can be configured to have different look 'n' feel. 
See the ToolBoxTab.cs for a list of new properties.
  
If you specify ItemBorderColor either in toolbox or a toolbox tab, the items 
will not be drawn with 3D raised style when mouse hovers on it.

ItemBackgroundColor of ToolBoxTab or ToolBox can be used to give custom 
background color  for a specific tab or all tabs.

ItemHoverTextColor and TabHoverTextColor can be used to give custom 
text color when mouse hovers on it.
  
Toolbox tabs can be switched by pressing Ctrl+Tab or Ctrl+Shift+Tab.
Tab items can be iterated by using arrow keys and tab keys.
  
You can add custom data formats to the drag data object by mapping
OnBeginDragDrop event in ToolBox
  
ShowOnlyOneItemPerRow property of ToolBox, ToolBoxTab makes only one 
ToolBoxItem to be listed per row in  Large Icon and Small Icon view mode 

-------------------------------------------------------------------------------
