// Decompiled with JetBrains decompiler
// Type: SAM.API.NativeWrapper`1
// Assembly: SAM.API, Version=6.3.0.804, Culture=neutral, PublicKeyToken=null
// MVID: 7DF108F6-41E2-4750-9029-3DA2C808D0A1
// Assembly location: F:\Windows\Desktop\SAM\SAM.API.dll

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SAM.API
{
  public abstract class NativeWrapper<TNativeFunctions> : INativeWrapper
  {
    private Dictionary<IntPtr, Delegate> FunctionCache = new Dictionary<IntPtr, Delegate>();
    protected IntPtr ObjectAddress;
    protected TNativeFunctions Functions;

    public override string ToString()
    {
      return string.Format("Steam Interface<{0}> #{1:X8}", (object) typeof (TNativeFunctions), (object) this.ObjectAddress.ToInt32());
    }

    public void SetupFunctions(IntPtr objectAddress)
    {
      this.ObjectAddress = objectAddress;
      this.Functions = (TNativeFunctions) Marshal.PtrToStructure(((NativeClass) Marshal.PtrToStructure(this.ObjectAddress, typeof (NativeClass))).VirtualTable, typeof (TNativeFunctions));
    }

    protected Delegate GetDelegate<TDelegate>(IntPtr pointer)
    {
      Delegate forFunctionPointer;
      if (!this.FunctionCache.ContainsKey(pointer))
      {
        forFunctionPointer = Marshal.GetDelegateForFunctionPointer(pointer, typeof (TDelegate));
        this.FunctionCache[pointer] = forFunctionPointer;
      }
      else
        forFunctionPointer = this.FunctionCache[pointer];
      return forFunctionPointer;
    }

    protected TDelegate GetFunction<TDelegate>(IntPtr pointer) where TDelegate : class
    {
      return (TDelegate) this.GetDelegate<TDelegate>(pointer);
    }

    protected void Call<TDelegate>(IntPtr pointer, params object[] args)
    {
      this.GetDelegate<TDelegate>(pointer).DynamicInvoke(args);
    }

    protected TReturn Call<TReturn, TDelegate>(IntPtr pointer, params object[] args)
    {
      return (TReturn) this.GetDelegate<TDelegate>(pointer).DynamicInvoke(args);
    }
  }
}
