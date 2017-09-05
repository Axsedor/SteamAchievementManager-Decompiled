// Decompiled with JetBrains decompiler
// Type: SAM.Game.Program
// Assembly: SAM.Game, Version=6.3.0.987, Culture=neutral, PublicKeyToken=null
// MVID: C778E0E6-4989-4E7A-95E7-530B61DDD3BB
// Assembly location: F:\Windows\Desktop\SAM\SAM.Game.exe

using SAM.API;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace SAM.Game
{
  internal static class Program
  {
    private static void WarningMessage(int pipe, string message)
    {
      int num = (int) MessageBox.Show(message);
    }

    [STAThread]
    public static void Main(string[] args)
    {
      if (args.Length == 0)
      {
        Process.Start("SAM.Picker.exe");
      }
      else
      {
        long result;
        if (!long.TryParse(args[0], out result))
        {
          int num1 = (int) MessageBox.Show("Could not parse application ID from commandline argument.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else if (Steam.GetInstallPath() == Application.StartupPath)
        {
          int num2 = (int) MessageBox.Show("This tool declines to being run from the Steam directory.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          Client client;
          try
          {
            client = new Client();
            if (!client.Initialize(result))
            {
              int num3 = (int) MessageBox.Show("Steam is not running. Please start Steam then run this tool again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              return;
            }
          }
          catch (DllNotFoundException ex)
          {
            int num3 = (int) MessageBox.Show("You've caused an exceptional error!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return;
          }
          Application.EnableVisualStyles();
          Application.SetCompatibleTextRenderingDefault(false);
          Application.Run((Form) new Manager(result, client));
        }
      }
    }
  }
}
