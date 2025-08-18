using UnityEngine;

public static class UnityLayers
{
	public static readonly int LayerDefault = LayerMask.NameToLayer("Default");
	public static readonly int LayerTransparentfx = LayerMask.NameToLayer("TransparentFX");
	public static readonly int LayerIgnoreRaycast = LayerMask.NameToLayer("Ignore Raycast");
	public static readonly int LayerWater = LayerMask.NameToLayer("Water");
	public static readonly int LayerUI = LayerMask.NameToLayer("UI");

	public static readonly int LayerMaskDefault = 1 << LayerDefault;
	public static readonly int LayerMaskTransparentfx = 1 << LayerTransparentfx;
	public static readonly int LayerMaskIgnoreRaycast = 1 << LayerIgnoreRaycast;
	public static readonly int LayerMaskWater = 1 << LayerWater;
	public static readonly int LayerMaskUI = 1 << LayerUI;
}
