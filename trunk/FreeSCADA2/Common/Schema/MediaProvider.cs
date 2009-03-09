using System;
using System.Windows.Data;

namespace FreeSCADA.Common.Schema
{
    public class MediaProvider:DataSourceProvider
    {
        string mediaFileName;
        public Type ExpectedTarget
        {
            get;
            set;
        }
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
            ExpectedTarget = typeof(Object);
        }
        protected override void BeginQuery()
        {
            try
            {

                
                //System.Windows.Media.Imaging.BitmapImage bi = new System.Windows.Media.Imaging.BitmapImage();
                //System.IO.Stream stream = Env.Current.Project.GetData(ProjectEntityType.Image,mediaFileName);
                //BitmapDecoder bd=BitmapDecoder.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                if(ExpectedTarget == typeof(Object) )
                {
                    AnimatedImage image = new AnimatedImage();
                    image.ImageName = MediaFileName;
                    image.AnimatedControl = true;
                    base.OnQueryFinished(image);
                    System.ComponentModel.TypeDescriptor.AddAttributes(image, new Attribute[] { new System.Windows.Markup.RuntimeNamePropertyAttribute(MediaFileName) });
                }
                
            }
            catch (Exception)
            {
                base.OnQueryFinished(mediaFileName);
            }
        }


    }
}
