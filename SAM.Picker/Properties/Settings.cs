// Decompiled with JetBrains decompiler
// Type: SAM.Picker.Properties.Settings
// Assembly: SAM.Picker, Version=6.3.0.987, Culture=neutral, PublicKeyToken=null
// MVID: EFF56F92-DCC2-4727-8A90-101E83408D77
// Assembly location: F:\Windows\Desktop\SAM\SAM.Picker.exe

using System.CodeDom.Compiler;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace SAM.Picker.Properties
{
  [CompilerGenerated]
  [GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
  internal sealed class Settings : ApplicationSettingsBase
  {
    private static Settings defaultInstance = (Settings) SettingsBase.Synchronized((SettingsBase) new Settings());

    public static Settings Default
    {
      get
      {
        return Settings.defaultInstance;
      }
    }
  }
}
