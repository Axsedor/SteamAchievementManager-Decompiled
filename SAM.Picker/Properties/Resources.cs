// Decompiled with JetBrains decompiler
// Type: SAM.Picker.Properties.Resources
// Assembly: SAM.Picker, Version=6.3.0.987, Culture=neutral, PublicKeyToken=null
// MVID: EFF56F92-DCC2-4727-8A90-101E83408D77
// Assembly location: F:\Windows\Desktop\SAM\SAM.Picker.exe

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace SAM.Picker.Properties
{
  [CompilerGenerated]
  [DebuggerNonUserCode]
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
  internal class Resources
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal Resources()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (object.ReferenceEquals((object) SAM.Picker.Properties.Resources.resourceMan, (object) null))
          SAM.Picker.Properties.Resources.resourceMan = new ResourceManager("SAM.Picker.Properties.Resources", typeof (SAM.Picker.Properties.Resources).Assembly);
        return SAM.Picker.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get
      {
        return SAM.Picker.Properties.Resources.resourceCulture;
      }
      set
      {
        SAM.Picker.Properties.Resources.resourceCulture = value;
      }
    }

    internal static Bitmap Download
    {
      get
      {
        return (Bitmap) SAM.Picker.Properties.Resources.ResourceManager.GetObject(nameof (Download), SAM.Picker.Properties.Resources.resourceCulture);
      }
    }

    internal static Bitmap Filter
    {
      get
      {
        return (Bitmap) SAM.Picker.Properties.Resources.ResourceManager.GetObject(nameof (Filter), SAM.Picker.Properties.Resources.resourceCulture);
      }
    }

    internal static Bitmap Refresh
    {
      get
      {
        return (Bitmap) SAM.Picker.Properties.Resources.ResourceManager.GetObject(nameof (Refresh), SAM.Picker.Properties.Resources.resourceCulture);
      }
    }

    internal static Bitmap Search
    {
      get
      {
        return (Bitmap) SAM.Picker.Properties.Resources.ResourceManager.GetObject(nameof (Search), SAM.Picker.Properties.Resources.resourceCulture);
      }
    }
  }
}
