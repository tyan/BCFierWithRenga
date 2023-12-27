using System.Reflection;
using System;
using System.Windows;

namespace Bcfier.RengaPlugin.Entry
{
  public class ExtAppBcfier //: IExternalApplication
  {
    #region Renga IPlugin Implementation

    //public Result OnStartup(UIControlledApplication application)
    //{
    //  RvtWindow = null;   // no dialog needed yet; the command will bring it  
    //  This = this;  // static access to this application instance  

    //  return Result.Succeeded;
    //}

    //public Result OnShutdown(UIControlledApplication application)
    //{
    //  if (RvtWindow != null && RvtWindow.IsVisible)
    //  {
    //    RvtWindow.Close();
    //  }

    //  return Result.Succeeded;
    //}

    #endregion

    #region public methods
    //public void ShowForm(UIApplication uiapp)
    //{
    //  try
    //  {
    //    // If we do not have a dialog yet, create and show it  
    //    if (RvtWindow != null) return;

    //    // A new handler to handle request posting by the dialog  
    //    var handler = new ExtEvntOpenView();

    //    // External Event for the dialog to use (to post requests)  
    //    var extEvent = ExternalEvent.Create(handler);

    //    // We give the objects to the new dialog;  
    //    // The dialog becomes the owner responsible for disposing them, eventually.
    //    RvtWindow = new RevitWindow(uiapp, extEvent, handler);
    //    RvtWindow.Show();
    //  }
    //  catch (Exception ex)
    //  {
    //    MessageBox.Show(ex.ToString());
    //  }
    //}

    //public void Focus()
    //{
    //  try
    //  {
    //    if (RvtWindow == null) return;
    //    RvtWindow.Activate();
    //    RvtWindow.WindowState = WindowState.Normal;
    //  }
    //  catch (Exception ex)
    //  {
    //    MessageBox.Show(ex.ToString());
    //  }
    //}
    #endregion
  }

}