using System;


namespace Bcfier.RengaPlugin.Entry
{
  public class CmdMain
  {
    internal static CmdMain ThisCmd = null;
    private static bool _isRunning;
    private static ExtAppBcfier _extAppBcfier;

    public void Execute()
    {
      if (_isRunning && _extAppBcfier != null && _extAppBcfier.Window.IsLoaded)
      {
        _extAppBcfier.Focus();
        return;
      }

      _isRunning = true;
      ThisCmd = this;
      _extAppBcfier = new ExtAppBcfier();
      _extAppBcfier.ShowForm();
    }
  }
}