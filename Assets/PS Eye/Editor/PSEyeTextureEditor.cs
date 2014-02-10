using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PSEyeTexture))]
public sealed class PSEyeTextureEditor : Editor 
{
	public override void OnInspectorGUI()
	{
		// Draw default public members
		DrawDefaultInspector();
		
		// Get target instance
		var psEye = target as PSEyeTexture;

		if (!psEye.isGaming()) return;

		// Label
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal(); {
			GUILayout.FlexibleSpace();
			EditorGUILayout.LabelField("Camera Sensor");
			GUILayout.FlexibleSpace();
		} EditorGUILayout.EndHorizontal();
		
		// Auto Gain
		bool autoGain = EditorGUILayout.Toggle("Auto Gain", psEye.AutoGain);
		if (autoGain != psEye.AutoGain) psEye.AutoGain = autoGain;
		
		// Gain 
		int gain = EditorGUILayout.IntSlider("Gain", psEye.Gain, 0, 79); 
		if (gain != psEye.Gain) psEye.Gain = gain;
		
		// Auto Exposure
		bool autoExposure = EditorGUILayout.Toggle("Auto Exposure", psEye.AutoExposure);
		if (autoExposure != psEye.AutoExposure) psEye.AutoExposure = autoExposure;
		
		// Exposure 
		int exposure = EditorGUILayout.IntSlider("Exposure", psEye.Exposure, 0, 511); 
		if (exposure != psEye.Exposure) psEye.Exposure = exposure;
		
		// Auto WhiteBalance
		bool autoWhiteBalance = EditorGUILayout.Toggle("Auto White Balance", psEye.AutoWhiteBalance);
		if (autoWhiteBalance != psEye.AutoWhiteBalance) psEye.AutoWhiteBalance = autoWhiteBalance;
		
		// White Balance Red 
		int whiteBaranceRed = EditorGUILayout.IntSlider("White Balance Red", psEye.WhiteBalanceRed, 0, 511); 
		if (whiteBaranceRed != psEye.WhiteBalanceRed) psEye.WhiteBalanceRed = whiteBaranceRed;
		
		// White Balance Green 
		int whiteBaranceGreen = EditorGUILayout.IntSlider("White Balance Green", psEye.WhiteBalanceGreen, 0, 511); 
		if (whiteBaranceGreen != psEye.WhiteBalanceGreen) psEye.WhiteBalanceGreen = whiteBaranceGreen;
		
		// White Balance Blue 
		int whiteBaranceBlue = EditorGUILayout.IntSlider("White Balance Blue", psEye.WhiteBalanceBlue, 0, 511); 
		if (whiteBaranceBlue != psEye.WhiteBalanceBlue) psEye.WhiteBalanceBlue = whiteBaranceBlue;
		
		// Label
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal(); {
			GUILayout.FlexibleSpace();
			EditorGUILayout.LabelField("Camera Linear Transform");
			GUILayout.FlexibleSpace();
		} EditorGUILayout.EndHorizontal();
		
		// Horizontal Flip
		bool horizontalFlip = EditorGUILayout.Toggle("Horizontal Flip", psEye.HorizontalFlip);
		if (horizontalFlip != psEye.HorizontalFlip) psEye.HorizontalFlip = horizontalFlip;
		
		// Vertical Flip
		bool verticalFlip = EditorGUILayout.Toggle("Vertical Flip", psEye.VerticalFlip);
		if (verticalFlip != psEye.VerticalFlip) psEye.VerticalFlip = verticalFlip;
		
		// Horizontal Keystone
		int horizontalKeystone = EditorGUILayout.IntSlider("Horizontal Keystone", psEye.HorizontalKeystone, -500, 500); 
		if (horizontalKeystone != psEye.HorizontalKeystone) psEye.HorizontalKeystone = horizontalKeystone;
		
		// Vertical Keystone
		int verticalKeystone = EditorGUILayout.IntSlider("Vertical Keystone", psEye.VerticalKeystone, -500, 500); 
		if (verticalKeystone != psEye.VerticalKeystone) psEye.VerticalKeystone = verticalKeystone;
		
		// X OFFSET
		int xOffset = EditorGUILayout.IntSlider("X Offset", psEye.XOffset, -500, 500); 
		if (xOffset != psEye.XOffset) psEye.XOffset = xOffset;
		
		// Y OFFSET
		int yOffset = EditorGUILayout.IntSlider("Y Offset", psEye.YOffset, -500, 500); 
		if (yOffset != psEye.YOffset) psEye.YOffset = yOffset;
		
		// Rotation
		int rotation = EditorGUILayout.IntSlider("Rotation", psEye.Rotation, -500, 500); 
		if (rotation != psEye.Rotation) psEye.Rotation = rotation;
		
		// Zoom
		int zoom = EditorGUILayout.IntSlider("Zoom", psEye.Zoom, -500, 500); 
		if (zoom != psEye.Zoom) psEye.Zoom = zoom;
		
		// Label
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal(); {
			GUILayout.FlexibleSpace();
			EditorGUILayout.LabelField("Camera Non-linear Transform");
			GUILayout.FlexibleSpace();
		} EditorGUILayout.EndHorizontal();
		
		// Lens Correction 1
		int lensCorrection1 = EditorGUILayout.IntSlider("Lens Correction 1", psEye.LensCorrection1, -500, 500); 
		if (lensCorrection1 != psEye.LensCorrection1) psEye.LensCorrection1 = lensCorrection1;
		
		// Lens Correction 2
		int lensCorrection2 = EditorGUILayout.IntSlider("Lens Correction 2", psEye.LensCorrection2, -500, 500); 
		if (lensCorrection2 != psEye.LensCorrection2) psEye.LensCorrection2 = lensCorrection2;
		
		// Lens Correction 3
		int lensCorrection3 = EditorGUILayout.IntSlider("Lens Correction 3", psEye.LensCorrection3, -500, 500); 
		if (lensCorrection3 != psEye.LensCorrection3) psEye.LensCorrection3 = lensCorrection3;
		
		// Lens Brightness
		int lensBrightness = EditorGUILayout.IntSlider("Lens Brightness", psEye.LensBrightness, -500, 500); 
		if (lensBrightness != psEye.LensBrightness) psEye.LensBrightness = lensBrightness;
	}
}
