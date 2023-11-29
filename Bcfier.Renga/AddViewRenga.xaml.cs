using System.IO;
using System.Windows;
using Bcfier.Bcf.Bcf2;

namespace Bcfier.Renga
{
  /// <summary>
  /// Interaction logic for AddViewRenga.xaml
  /// </summary>
  public partial class AddViewRenga : Window
  {

    public AddViewRenga(Markup issue, string bcfTempFolder/*, Document _doc*/)
    {
      try
      {
        GetRengaSnapshot();
      }
      catch (System.Exception ex1)
      {
        //TaskDialog.Show("Error!", "exception: " + ex1);
      }
    }

    private void GetRengaSnapshot()
    {
      try
      {
        string tempImg = Path.Combine(Path.GetTempPath(), "BCFier", Path.GetTempFileName() + ".png");
        
        //doc.ExportImage(options);

        AddViewControl.AddViewpoint(tempImg);
        File.Delete(tempImg);
      }
      catch (System.Exception ex1)
      {
        //TaskDialog.Show("Error!", "exception: " + ex1);
      }
    }
    
  }
}