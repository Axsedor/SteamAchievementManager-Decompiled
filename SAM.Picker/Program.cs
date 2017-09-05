// Decompiled with JetBrains decompiler
// Type: SAM.Picker.Program
// Assembly: SAM.Picker, Version=6.3.0.987, Culture=neutral, PublicKeyToken=null
// MVID: EFF56F92-DCC2-4727-8A90-101E83408D77
// Assembly location: F:\Windows\Desktop\SAM\SAM.Picker.exe

using SAM.API;
using System;
using System.Windows.Forms;

namespace SAM.Picker
{
  internal static class Program
  {
    [STAThread]
    private static void Main()
    {
      if (Steam.GetInstallPath() == Application.StartupPath)
      {
        int num1 = (int) MessageBox.Show("This tool declines to being run from the Steam directory.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        Client client;
        try
        {
          client = new Client();
          if (!client.Initialize(0L))
          {
            int num2 = (int) MessageBox.Show("Steam is not running. Please start Steam then run this tool again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return;
          }
        }
        catch (DllNotFoundException ex)
        {
          int num2 = (int) MessageBox.Show("You've caused an exceptional error!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return;
        }
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run((Form) new GamePicker(client));
      }
    }
  }
}
