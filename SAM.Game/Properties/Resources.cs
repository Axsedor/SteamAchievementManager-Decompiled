// Decompiled with JetBrains decompiler
// Type: SAM.Game.Properties.Resources
// Assembly: SAM.Game, Version=6.3.0.987, Culture=neutral, PublicKeyToken=null
// MVID: C778E0E6-4989-4E7A-95E7-530B61DDD3BB
// Assembly location: F:\Windows\Desktop\SAM\SAM.Game.exe

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace SAM.Game.Properties
{
  [DebuggerNonUserCode]
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
  [CompilerGenerated]
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
        if (object.ReferenceEquals((object) SAM.Game.Properties.Resources.resourceMan, (object) null))
          SAM.Game.Properties.Resources.resourceMan = new ResourceManager("SAM.Game.Properties.Resources", typeof (SAM.Game.Properties.Resources).Assembly);
        return SAM.Game.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get
      {
        return SAM.Game.Properties.Resources.resourceCulture;
      }
      set
      {
        SAM.Game.Properties.Resources.resourceCulture = value;
      }
    }

    internal static Bitmap Download
    {
      get
      {
        return (Bitmap) SAM.Game.Properties.Resources.ResourceManager.GetObject(nameof (Download), SAM.Game.Properties.Resources.resourceCulture);
      }
    }

    internal static Bitmap Invert
    {
      get
      {
        return (Bitmap) SAM.Game.Properties.Resources.ResourceManager.GetObject(nameof (Invert), SAM.Game.Properties.Resources.resourceCulture);
      }
    }

    internal static Bitmap Lock
    {
      get
      {
        return (Bitmap) SAM.Game.Properties.Resources.ResourceManager.GetObject(nameof (Lock), SAM.Game.Properties.Resources.resourceCulture);
      }
    }

    internal static Bitmap Refresh
    {
      get
      {
        return (Bitmap) SAM.Game.Properties.Resources.ResourceManager.GetObject(nameof (Refresh), SAM.Game.Properties.Resources.resourceCulture);
      }
    }

    internal static Bitmap Reset
    {
      get
      {
        return (Bitmap) SAM.Game.Properties.Resources.ResourceManager.GetObject(nameof (Reset), SAM.Game.Properties.Resources.resourceCulture);
      }
    }

    internal static Bitmap Sad
    {
      get
      {
        return (Bitmap) SAM.Game.Properties.Resources.ResourceManager.GetObject(nameof (Sad), SAM.Game.Properties.Resources.resourceCulture);
      }
    }

    internal static Bitmap Save
    {
      get
      {
        return (Bitmap) SAM.Game.Properties.Resources.ResourceManager.GetObject(nameof (Save), SAM.Game.Properties.Resources.resourceCulture);
      }
    }

    internal static Bitmap Unlock
    {
      get
      {
        return (Bitmap) SAM.Game.Properties.Resources.ResourceManager.GetObject(nameof (Unlock), SAM.Game.Properties.Resources.resourceCulture);
      }
    }
  }
}
