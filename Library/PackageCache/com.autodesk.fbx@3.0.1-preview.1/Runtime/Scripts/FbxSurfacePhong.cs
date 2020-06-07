//------------------------------------------------------------------------------
// <auto-generated />
//
// This file was automatically generated by SWIG (http://www.swig.org).
// Version 3.0.12
//
// Do not make changes to this file unless you know what you are doing--modify
// the SWIG interface file instead.
//------------------------------------------------------------------------------

namespace Autodesk.Fbx {

public class FbxSurfacePhong : FbxSurfaceLambert {
  internal FbxSurfacePhong(global::System.IntPtr cPtr, bool ignored) : base(cPtr, ignored) { }

  // override void Dispose() {base.Dispose();}

  public new static FbxSurfacePhong Create(FbxManager pManager, string pName) {
    global::System.IntPtr cPtr = NativeMethods.FbxSurfacePhong_Create__SWIG_0(FbxManager.getCPtr(pManager), pName);
    FbxSurfacePhong ret = (cPtr == global::System.IntPtr.Zero) ? null : new FbxSurfacePhong(cPtr, false);
    if (NativeMethods.SWIGPendingException.Pending) throw NativeMethods.SWIGPendingException.Retrieve();
    return ret;
  }

  public new static FbxSurfacePhong Create(FbxObject pContainer, string pName) {
    global::System.IntPtr cPtr = NativeMethods.FbxSurfacePhong_Create__SWIG_1(FbxObject.getCPtr(pContainer), pName);
    FbxSurfacePhong ret = (cPtr == global::System.IntPtr.Zero) ? null : new FbxSurfacePhong(cPtr, false);
    if (NativeMethods.SWIGPendingException.Pending) throw NativeMethods.SWIGPendingException.Retrieve();
    return ret;
  }

  public FbxPropertyDouble3 Specular {
    get {
      FbxPropertyDouble3 ret = new FbxPropertyDouble3(NativeMethods.FbxSurfacePhong_Specular_get(swigCPtr), false);
      if (NativeMethods.SWIGPendingException.Pending) throw NativeMethods.SWIGPendingException.Retrieve();
      return ret;
    } 
  }

  public FbxPropertyDouble SpecularFactor {
    get {
      FbxPropertyDouble ret = new FbxPropertyDouble(NativeMethods.FbxSurfacePhong_SpecularFactor_get(swigCPtr), false);
      if (NativeMethods.SWIGPendingException.Pending) throw NativeMethods.SWIGPendingException.Retrieve();
      return ret;
    } 
  }

  public FbxPropertyDouble Shininess {
    get {
      FbxPropertyDouble ret = new FbxPropertyDouble(NativeMethods.FbxSurfacePhong_Shininess_get(swigCPtr), false);
      if (NativeMethods.SWIGPendingException.Pending) throw NativeMethods.SWIGPendingException.Retrieve();
      return ret;
    } 
  }

  public FbxPropertyDouble3 Reflection {
    get {
      FbxPropertyDouble3 ret = new FbxPropertyDouble3(NativeMethods.FbxSurfacePhong_Reflection_get(swigCPtr), false);
      if (NativeMethods.SWIGPendingException.Pending) throw NativeMethods.SWIGPendingException.Retrieve();
      return ret;
    } 
  }

  public FbxPropertyDouble ReflectionFactor {
    get {
      FbxPropertyDouble ret = new FbxPropertyDouble(NativeMethods.FbxSurfacePhong_ReflectionFactor_get(swigCPtr), false);
      if (NativeMethods.SWIGPendingException.Pending) throw NativeMethods.SWIGPendingException.Retrieve();
      return ret;
    } 
  }

  public override int GetHashCode(){
      return swigCPtr.Handle.GetHashCode();
  }

  public bool Equals(FbxSurfacePhong other) {
    if (object.ReferenceEquals(other, null)) { return false; }
    return this.swigCPtr.Handle.Equals (other.swigCPtr.Handle);
  }

  public override bool Equals(object obj){
    if (object.ReferenceEquals(obj, null)) { return false; }
    /* is obj a subclass of this type; if so use our Equals */
    var typed = obj as FbxSurfacePhong;
    if (!object.ReferenceEquals(typed, null)) {
      return this.Equals(typed);
    }
    /* are we a subclass of the other type; if so use their Equals */
    if (typeof(FbxSurfacePhong).IsSubclassOf(obj.GetType())) {
      return obj.Equals(this);
    }
    /* types are unrelated; can't be a match */
    return false;
  }

  public static bool operator == (FbxSurfacePhong a, FbxSurfacePhong b) {
    if (object.ReferenceEquals(a, b)) { return true; }
    if (object.ReferenceEquals(a, null) || object.ReferenceEquals(b, null)) { return false; }
    return a.Equals(b);
  }

  public static bool operator != (FbxSurfacePhong a, FbxSurfacePhong b) {
    return !(a == b);
  }

}

}
