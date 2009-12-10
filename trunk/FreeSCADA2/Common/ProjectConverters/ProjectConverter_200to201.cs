using System;

namespace FreeSCADA.Common.ProjectConverters
{
	class ProjectConverter_200to201 : BaseProjectConverter
    {
        public override int AcceptedVersion
        {
            get { return 200; }
        }
        public override int ResultVersion
        {
            get { return 201; }
        }

        public override bool Convert(Project prj)
        {
            foreach (string schemaName in prj.GetEntities(ProjectEntityType.Schema))
            {
                System.IO.StreamReader reader=new System.IO.StreamReader(prj.GetData("Schemas/" + schemaName + "/xaml"));
                String xml=reader.ReadToEnd();
                xml = xml.Replace("clr-namespace:FreeSCADA.Common.Schema;assembly=Common", "clr-namespace:FreeSCADA.Common.Schema;assembly=CommonGUI");
                xml = xml.Replace("clr-namespace:FreeSCADA.Common.Schema;assembly=Schema", "clr-namespace:FreeSCADA.Common.Schema;assembly=CommonGUI");
                byte[] data=System.Text.ASCIIEncoding.Default.GetBytes(xml);
                System.IO.MemoryStream ms = new System.IO.MemoryStream(data);
                ms.Seek(0,System.IO.SeekOrigin.Begin);

                prj.SetData("Schemas/" + schemaName + "/xaml", ms);
            }
            prj.Version = ResultVersion;
            return true;
        }
        public override bool ConvertBack(Project prj)
        {
            foreach (string schemaName in prj.GetEntities(ProjectEntityType.Schema))
            {

                System.IO.StreamReader reader = new System.IO.StreamReader(prj.GetData("Schemas/" + schemaName + "/xaml"));
                String xml = reader.ReadToEnd();
                xml = xml.Replace("clr-namespace:FreeSCADA.Common.Schema;assembly=CommonGUI", "clr-namespace:FreeSCADA.Common.Schema;assembly=Common");
                byte[] data = System.Text.ASCIIEncoding.Default.GetBytes(xml);
                System.IO.MemoryStream ms = new System.IO.MemoryStream(data);
                ms.Seek(0, System.IO.SeekOrigin.Begin);
                prj.SetData("Schemas/" + schemaName + "/xaml", ms);
                prj.Version = AcceptedVersion;
            }
            return true;
         }


    }
}
