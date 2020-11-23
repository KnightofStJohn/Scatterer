using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace scatterer
{
	public class ScaledScatteringContainer
	{
		GameObject scaledScatteringGO;
		MeshRenderer scaledScatteringMR;
		
		public GameObject GameObject { get { return scaledScatteringGO; } }
		public MeshRenderer MeshRenderer { get { return scaledScatteringMR; } }
		
		Transform parentLocalTransform, parentScaledTransform;
		
		public ScaledScatteringContainer(Mesh planetMesh, Material material, Transform inParentLocalTransform, Transform inParentScaledTransform)
		{
			parentScaledTransform = inParentScaledTransform;
			parentLocalTransform = inParentLocalTransform;

			scaledScatteringGO = new GameObject ();

			scaledScatteringGO.transform.SetParent (parentScaledTransform, false);

			MeshFilter skySphereMF = scaledScatteringGO.AddComponent<MeshFilter>();
			skySphereMF.mesh = (Mesh) Mesh.Instantiate (planetMesh);
			
			scaledScatteringMR = scaledScatteringGO.AddComponent<MeshRenderer>();
			scaledScatteringMR.sharedMaterial = material;
			Utils.EnableOrDisableShaderKeywords (scaledScatteringMR.sharedMaterial, "LOCAL_MODE_ON", "LOCAL_MODE_OFF", false);
			
			scaledScatteringMR.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
			scaledScatteringMR.receiveShadows = false;
			scaledScatteringMR.enabled = true;
			
			if (HighLogic.LoadedScene == GameScenes.MAINMENU)
				scaledScatteringGO.layer = 15;
			else
				scaledScatteringGO.layer = 10;
		}
		
		public void SwitchLocalMode()
		{
			scaledScatteringGO.layer = 15;

			scaledScatteringGO.transform.localScale = parentScaledTransform.localScale * ScaledSpace.ScaleFactor;

			scaledScatteringGO.transform.parent = parentLocalTransform;
			
			Utils.EnableOrDisableShaderKeywords (scaledScatteringMR.sharedMaterial, "LOCAL_MODE_ON", "LOCAL_MODE_OFF", true);
		}
		
		public void SwitchScaledMode()
		{
			scaledScatteringGO.layer = 10;

			scaledScatteringGO.transform.localScale = Vector3.one;
			scaledScatteringGO.transform.localPosition = Vector3.zero;
			scaledScatteringGO.transform.localRotation = Quaternion.identity;

			scaledScatteringGO.transform.SetParent(parentScaledTransform, false);
			
			Utils.EnableOrDisableShaderKeywords (scaledScatteringMR.sharedMaterial, "LOCAL_MODE_ON", "LOCAL_MODE_OFF", false);
		}
		
		public void Cleanup()
		{
			if (!ReferenceEquals (scaledScatteringMR, null))
			{
				scaledScatteringMR.enabled = false;
				UnityEngine.Component.Destroy (scaledScatteringMR);
			}

			if (!ReferenceEquals (scaledScatteringGO, null))
				UnityEngine.Object.Destroy(scaledScatteringGO);
		}
	}
}