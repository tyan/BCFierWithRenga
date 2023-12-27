using System;
using System.Reflection;

namespace Bcfier.RengaPlugin.Entry
{
  public class CmdMain //: IExternalCommand
  {
    internal static CmdMain ThisCmd = null;
    private static bool _isRunning;
    private static ExtAppBcfier _extAppBcfier;

    public void Execute() 
    { }
    //public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    //{
    //  try
    //  {

    //    //Version check

    //    if (!commandData.Application.Application.VersionName.Contains(RevitVersion))
    //    {
    //      using (var td = new TaskDialog("Untested version"))
    //      {
    //        td.TitleAutoPrefix = false;
    //        td.MainInstruction = "Untested Revit Version";
    //        td.MainContent = "This Add-In was built and tested only for Revit " + RevitVersion + ", proceed at your own risk";
    //        td.Show();
    //      }
    //    }

    //    // Form Running?
    //    if (_isRunning && _extAppBcfier != null && _extAppBcfier.RvtWindow.IsLoaded)
    //    {
    //      _extAppBcfier.Focus();
    //      return Result.Succeeded;
    //    }

    //    _isRunning = true;

    //    ThisCmd = this;
    //    _extAppBcfier = new ExtAppBcfier();
    //    _extAppBcfier.ShowForm(commandData.Application);
    //    return Result.Succeeded;

    //  }
    //  catch (Exception e)
    //  {
    //    message = e.Message;
    //    return Result.Failed;
    //  }

    //}

  }
}