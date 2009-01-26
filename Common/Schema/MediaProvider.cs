using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Data;
using System.Windows.Media.Imaging;
namespace FreeSCADA.Common.Schema
{
    public class MediaProvider:DataSourceProvider
    {
        string mediaFileName;
        public string MediaFileName
        {
            get { return mediaFileName; }
            set{
                if(value==mediaFileName)
                    return;
                mediaFileName=value;
                base.OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("MediaFileName"));
            }
        }
        public MediaProvider()
        {
        }
        protected override void BeginQuery()
        {
            try
            {

                System.Windows.Controls.Image image = new System.Windows.Controls.Image();
                //System.Windows.Media.Imaging.BitmapImage bi = new System.Windows.Media.Imaging.BitmapImage();
                System.IO.Stream stream = Env.Current.Project.GetData(ProjectEntityType.Image,mediaFileName);
                image.Source = BitmapDecoder.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad).Frames[0];
                System.ComponentModel.TypeDescriptor.AddAttributes(image, new Attribute[] { new System.Windows.Markup.RuntimeNamePropertyAttribute(MediaFileName) });
                base.OnQueryFinished(image);
            }
            catch (Exception)
            {
                base.OnQueryFinished(mediaFileName);
            }
        }


    }
}
