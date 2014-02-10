using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Threading;

public class PSEyeTexture : MonoBehaviour 
{
	// dll name
    private const string CLEyeMulticam_DllName = "CLEyeMulticam.dll";
    
	#region [ Camera Parameters ]
	// camera color mode
	public enum CLEyeCameraColorMode
	{
	    CLEYE_MONO_PROCESSED,
	    CLEYE_COLOR_PROCESSED,
	    CLEYE_MONO_RAW,
	    CLEYE_COLOR_RAW,
	    CLEYE_BAYER_RAW
	};
	
	// camera resolution
	public enum CLEyeCameraResolution
	{
	    CLEYE_QVGA,
	    CLEYE_VGA
	};
	
	// camera parameters
	public enum CLEyeCameraParameter
	{
	    // camera sensor parameters
	    CLEYE_AUTO_GAIN,			// [false, true]
	    CLEYE_GAIN,					// [0, 79]
	    CLEYE_AUTO_EXPOSURE,		// [false, true]
	    CLEYE_EXPOSURE,				// [0, 511]
	    CLEYE_AUTO_WHITEBALANCE,	// [false, true]
	    CLEYE_WHITEBALANCE_RED,		// [0, 255]
	    CLEYE_WHITEBALANCE_GREEN,	// [0, 255]
	    CLEYE_WHITEBALANCE_BLUE,	// [0, 255]
	    // camera linear transform parameters
	    CLEYE_HFLIP,				// [false, true]
	    CLEYE_VFLIP,				// [false, true]
	    CLEYE_HKEYSTONE,			// [-500, 500]
	    CLEYE_VKEYSTONE,			// [-500, 500]
	    CLEYE_XOFFSET,				// [-500, 500]
	    CLEYE_YOFFSET,				// [-500, 500]
	    CLEYE_ROTATION,				// [-500, 500]
	    CLEYE_ZOOM,					// [-500, 500]
	    // camera non-linear transform parameters
	    CLEYE_LENSCORRECTION1,		// [-500, 500]
	    CLEYE_LENSCORRECTION2,		// [-500, 500]
	    CLEYE_LENSCORRECTION3,		// [-500, 500]
	    CLEYE_LENSBRIGHTNESS		// [-500, 500]
	};
	#endregion
	
	
	#region [ CLEyeMulticam Imports ]
	[DllImport("CLEyeMulticam.dll", CallingConvention = CallingConvention.Cdecl)]
	public static extern int CLEyeGetCameraCount();
	[DllImport("CLEyeMulticam.dll", CallingConvention = CallingConvention.Cdecl)]
	public static extern Guid CLEyeGetCameraUUID(int camId);
	[DllImport("CLEyeMulticam.dll", CallingConvention = CallingConvention.Cdecl)]
	public static extern IntPtr CLEyeCreateCamera(Guid camUUID, CLEyeCameraColorMode mode, CLEyeCameraResolution res, float frameRate);
	[DllImport("CLEyeMulticam.dll", CallingConvention = CallingConvention.Cdecl)]
	public static extern bool CLEyeDestroyCamera(IntPtr camera);
	[DllImport("CLEyeMulticam.dll", CallingConvention = CallingConvention.Cdecl)]
	public static extern bool CLEyeCameraStart(IntPtr camera);
	[DllImport("CLEyeMulticam.dll", CallingConvention = CallingConvention.Cdecl)]
	public static extern bool CLEyeCameraStop(IntPtr camera);
	[DllImport("CLEyeMulticam.dll", CallingConvention = CallingConvention.Cdecl)]
	public static extern bool CLEyeCameraLED(IntPtr camera, bool on);
	[DllImport("CLEyeMulticam.dll", CallingConvention = CallingConvention.Cdecl)]
	public static extern bool CLEyeSetCameraParameter(IntPtr camera, CLEyeCameraParameter param, int value);
	[DllImport("CLEyeMulticam.dll", CallingConvention = CallingConvention.Cdecl)]
	public static extern int CLEyeGetCameraParameter(IntPtr camera, CLEyeCameraParameter param);
	[DllImport("CLEyeMulticam.dll", CallingConvention = CallingConvention.Cdecl)]
	public static extern bool CLEyeCameraGetFrameDimensions(IntPtr camera, ref int width, ref int height);
	[DllImport("CLEyeMulticam.dll", CallingConvention = CallingConvention.Cdecl)]
	public static extern bool CLEyeCameraGetFrame(IntPtr camera, IntPtr pData, int waitTimeout);
	#endregion
	
	
	#region [ Properties ]
	public float Framerate{ get; set; }
	public CLEyeCameraColorMode ColorMode{ get; set; }
	public CLEyeCameraResolution Resolution{ get; set; }
	public bool AutoGain 
	{
		get
		{
			if (camera_ == null) return false;
			return CLEyeGetCameraParameter(camera_, CLEyeCameraParameter.CLEYE_AUTO_GAIN) != 0;
		}
		set
		{
			if (camera_ == null) return;
			CLEyeSetCameraParameter(camera_, CLEyeCameraParameter.CLEYE_AUTO_GAIN, value ? 1 : 0);
		}
	}
	public int Gain
	{
		get
		{
			if (camera_ == null) return 0;
			return CLEyeGetCameraParameter(camera_, CLEyeCameraParameter.CLEYE_GAIN);
		}
		set
		{
			if (camera_ == null) return;
			CLEyeSetCameraParameter(camera_, CLEyeCameraParameter.CLEYE_GAIN, value);
		}
	}
	public bool AutoExposure
	{
		get
		{
			if (camera_ == null) return false;
			return CLEyeGetCameraParameter(camera_, CLEyeCameraParameter.CLEYE_AUTO_EXPOSURE) != 0;
		}
		set
		{
			if (camera_ == null) return;
			CLEyeSetCameraParameter(camera_, CLEyeCameraParameter.CLEYE_AUTO_EXPOSURE, value ? 1 : 0);
		}
	}
	public int Exposure
	{
		get
		{
			if (camera_ == null) return 0;
			return CLEyeGetCameraParameter(camera_, CLEyeCameraParameter.CLEYE_EXPOSURE);
		}
		set
		{
			if (camera_ == null) return;
			CLEyeSetCameraParameter(camera_, CLEyeCameraParameter.CLEYE_EXPOSURE, value);
		}
	}
	public bool AutoWhiteBalance
	{
		get
		{
			if (camera_ == null) return true;
			return CLEyeGetCameraParameter(camera_, CLEyeCameraParameter.CLEYE_AUTO_WHITEBALANCE) != 0;
		}
		set
		{
			if (camera_ == null) return;
			CLEyeSetCameraParameter(camera_, CLEyeCameraParameter.CLEYE_AUTO_WHITEBALANCE, value ? 1 : 0);
		}
	}
	public int WhiteBalanceRed
	{
		get
		{
			if (camera_ == null) return 0;
			return CLEyeGetCameraParameter(camera_, CLEyeCameraParameter.CLEYE_WHITEBALANCE_RED);
		}
		set
		{
			if (camera_ == null) return;
			CLEyeSetCameraParameter(camera_, CLEyeCameraParameter.CLEYE_WHITEBALANCE_RED, value);
		}
	}
	public int WhiteBalanceGreen
	{
		get
		{
			if (camera_ == null) return 0;
			return CLEyeGetCameraParameter(camera_, CLEyeCameraParameter.CLEYE_WHITEBALANCE_GREEN);
		}
		set
		{
			if (camera_ == null) return;
			CLEyeSetCameraParameter(camera_, CLEyeCameraParameter.CLEYE_WHITEBALANCE_GREEN, value);
		}
	}
	public int WhiteBalanceBlue
	{
		get
		{
			if (camera_ == null) return 0;
			return CLEyeGetCameraParameter(camera_, CLEyeCameraParameter.CLEYE_WHITEBALANCE_BLUE);
		}
		set
		{
			if (camera_ == null) return;
			CLEyeSetCameraParameter(camera_, CLEyeCameraParameter.CLEYE_WHITEBALANCE_BLUE, value);
		}
	}
	public bool HorizontalFlip
	{
		get
		{
			if (camera_ == null) return false;
			return CLEyeGetCameraParameter(camera_, CLEyeCameraParameter.CLEYE_HFLIP) != 0;
		}
		set
		{
			if (camera_ == null) return;
			CLEyeSetCameraParameter(camera_, CLEyeCameraParameter.CLEYE_HFLIP, value ? 1 : 0);
		}
	}
	public bool VerticalFlip
	{
		get
		{
			if (camera_ == null) return false;
			return CLEyeGetCameraParameter(camera_, CLEyeCameraParameter.CLEYE_VFLIP) != 0;
		}
		set
		{
			if (camera_ == null) return;
			CLEyeSetCameraParameter(camera_, CLEyeCameraParameter.CLEYE_VFLIP, value ? 1 : 0);
		}
	}
	public int HorizontalKeystone
	{
		get
		{
			if (camera_ == null) return 0;
			return CLEyeGetCameraParameter(camera_, CLEyeCameraParameter.CLEYE_HKEYSTONE);
		}
		set
		{
			if (camera_ == null) return;
			CLEyeSetCameraParameter(camera_, CLEyeCameraParameter.CLEYE_HKEYSTONE, value);
		}
	}
	public int VerticalKeystone
	{
		get
		{
			if (camera_ == null) return 0;
			return CLEyeGetCameraParameter(camera_, CLEyeCameraParameter.CLEYE_VKEYSTONE);
		}
		set
		{
			if (camera_ == null) return;
			CLEyeSetCameraParameter(camera_, CLEyeCameraParameter.CLEYE_VKEYSTONE, value);
		}
	}
	public int XOffset
	{
		get
		{
			if (camera_ == null) return 0;
			return CLEyeGetCameraParameter(camera_, CLEyeCameraParameter.CLEYE_XOFFSET);
		}
		set
		{
			if (camera_ == null) return;
			CLEyeSetCameraParameter(camera_, CLEyeCameraParameter.CLEYE_XOFFSET, value);
		}
	}
	public int YOffset
	{
		get
		{
			if (camera_ == null) return 0;
			return CLEyeGetCameraParameter(camera_, CLEyeCameraParameter.CLEYE_YOFFSET);
		}
		set
		{
			if (camera_ == null) return;
			CLEyeSetCameraParameter(camera_, CLEyeCameraParameter.CLEYE_YOFFSET, value);
		}
	}
	public int Rotation
	{
		get
		{
			if (camera_ == null) return 0;
			return CLEyeGetCameraParameter(camera_, CLEyeCameraParameter.CLEYE_ROTATION);
		}
		set
		{
			if (camera_ == null) return;
			CLEyeSetCameraParameter(camera_, CLEyeCameraParameter.CLEYE_ROTATION, value);
		}
	}
	public int Zoom
	{
		get
		{
			if (camera_ == null) return 0;
			return CLEyeGetCameraParameter(camera_, CLEyeCameraParameter.CLEYE_ZOOM);
		}
		set
		{
			if (camera_ == null) return;
			CLEyeSetCameraParameter(camera_, CLEyeCameraParameter.CLEYE_ZOOM, value);
		}
	}
	public int LensCorrection1
	{
		get
		{
			if (camera_ == null) return 0;
			return CLEyeGetCameraParameter(camera_, CLEyeCameraParameter.CLEYE_LENSCORRECTION1);
		}
		set
		{
			if (camera_ == null) return;
			CLEyeSetCameraParameter(camera_, CLEyeCameraParameter.CLEYE_LENSCORRECTION1, value);
		}
	}
	public int LensCorrection2
	{
		get
		{
			if (camera_ == null) return 0;
			return CLEyeGetCameraParameter(camera_, CLEyeCameraParameter.CLEYE_LENSCORRECTION2);
		}
		set
		{
			if (camera_ == null) return;
			CLEyeSetCameraParameter(camera_, CLEyeCameraParameter.CLEYE_LENSCORRECTION2, value);
		}
	}
	public int LensCorrection3
	{
		get
		{
			if (camera_ == null) return 0;
			return CLEyeGetCameraParameter(camera_, CLEyeCameraParameter.CLEYE_LENSCORRECTION3);
		}
		set
		{
			if (camera_ == null) return;
			CLEyeSetCameraParameter(camera_, CLEyeCameraParameter.CLEYE_LENSCORRECTION3, value);
		}
	}
	public int LensBrightness
	{
		get
		{
			if (camera_ == null) return 0;
			return CLEyeGetCameraParameter(camera_, CLEyeCameraParameter.CLEYE_LENSBRIGHTNESS);
		}
		set
		{
			if (camera_ == null) return;
			CLEyeSetCameraParameter(camera_, CLEyeCameraParameter.CLEYE_LENSBRIGHTNESS, value);
		}
	}
	#endregion
	
    
	#region [ Static members ]
	private static int CameraCount { get { return CLEyeGetCameraCount(); } }
	private static Guid CameraUUID(int idx) { return CLEyeGetCameraUUID(idx); }	
	#endregion
	
	
	#region [ Private members ]
	private IntPtr camera_ = IntPtr.Zero;
	private int width_  = 0;
	private int height_ = 0;
	
	private Texture2D texture_;
	private Color32[] pixels_, pixelsABGR_;
	private GCHandle  pixels_handle_;
	
	private Thread thread_;
	private Mutex  mutex_;
	#endregion
	
	
	#region [ Public members ]
	public int cameraNumber = 0;
	public bool isGaming() {
		return camera_ != IntPtr.Zero;
	}
	#endregion
	
	
	#region [ Member functions ]
	void Awake() {
		// create CLEye Camera
		camera_ = CLEyeCreateCamera(CameraUUID(cameraNumber), CLEyeCameraColorMode.CLEYE_COLOR_RAW, CLEyeCameraResolution.CLEYE_VGA, 30);	
		CLEyeCameraGetFrameDimensions(camera_, ref width_, ref height_);
		
		// set auto mode
		AutoGain         = true;
		AutoWhiteBalance = true;
		AutoExposure     = true;
		
		// initialize texture parameters
		texture_ = new Texture2D(width_, height_, TextureFormat.ARGB32, false);
		pixels_  = texture_.GetPixels32();
		pixels_handle_ = GCHandle.Alloc(pixels_, GCHandleType.Pinned);
		renderer.material.mainTexture = texture_;
		pixelsABGR_ = texture_.GetPixels32();
		
		// make scale-z inverse to reverse texture
		Vector3 scale = transform.localScale;
		transform.localScale = new Vector3(scale.x, scale.y, -scale.z);
		
		// start capturing
		CLEyeCameraStart(camera_);
		
		// initialize thread
		mutex_ = new Mutex(true);
		thread_ = new Thread(ThreadWorker);
		thread_.Start();
	}
	
	void Update() {
		mutex_.ReleaseMutex();
		mutex_.WaitOne();
		
		// set captured pixel data to texture
		texture_.SetPixels32(pixelsABGR_);
		texture_.Apply();
	}
	
	void ThreadWorker() {
		try {
			_ThreadWorker();
		} catch (Exception e) {
			if (!(e is ThreadAbortException)) {
				Debug.LogError("Unexpected Death: " + e.ToString());
			}
		}	
	}
	
	void _ThreadWorker() {
		for (;;) {
			Thread.Sleep(0);
			
			// get pixel data from PS Eye
			CLEyeCameraGetFrame(camera_, pixels_handle_.AddrOfPinnedObject(), 500);
			
			// swap RED and BLUE
			// (synchronize the following procedure with the main thread)
			mutex_.WaitOne();
			for (int i = 0; i < width_ * height_; ++i) {
				pixelsABGR_[i].a = pixels_[i].a; 
				pixelsABGR_[i].r = pixels_[i].b;
				pixelsABGR_[i].g = pixels_[i].g;
				pixelsABGR_[i].b = pixels_[i].r; 
			}
			mutex_.ReleaseMutex();
		}	
	}
	
	void OnApplicationQuit() {
		thread_.Abort();
		pixels_handle_.Free();
		CLEyeCameraStop(camera_);
		CLEyeDestroyCamera(camera_);
	}
	#endregion
}
