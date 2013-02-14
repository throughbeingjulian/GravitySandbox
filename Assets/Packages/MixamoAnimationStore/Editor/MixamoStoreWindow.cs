using System;
using UnityEngine;
using UnityEditor;

public class MixamoStoreWindow : EditorWindow
{
	System.DateTime lastOnGUITime = System.DateTime.Now;
	
	private const string MXE = "pro";
	
	private Mixamo.MixamoStore _store = null;
	private Mixamo.MixamoStore Store {
		get {
			if( _store == null ) {
				_store = new Mixamo.MixamoStore(this , MXE);
			}
			return _store;
		}
	}
	
	[MenuItem ("Window/Mixamo Store #%1")]
	static void Init() {
		MixamoStoreWindow window = (MixamoStoreWindow)EditorWindow.GetWindow(typeof (MixamoStoreWindow) ,false, "Mixamo Store");
		window.wantsMouseMove = false;
		window.Show();

		Mixamo.MixamoStore.VersionCompatibility = new Unity4VersionCompatibility();

	}
	
	void OnGUI() {
		Store.OnGUI();
	}
	
	void Update() {
		if( this == EditorWindow.mouseOverWindow && ( (System.DateTime.Now - lastOnGUITime).TotalSeconds > (1f/20f)) ) {
			lastOnGUITime = System.DateTime.Now;
			this.Repaint();
		}
		
		Store.Update();
	}
	
	void OnDestroy() {
		Store.OnDestroy();
	}

	public class Unity4VersionCompatibility : Mixamo.IVersionCompatibility
	{
		public void U4_ImportWorkaround(ModelImporter mi)
		{
			#if UNITY_4_0
				mi.animationType = UnityEditor.ModelImporterAnimationType.Legacy;
			#endif
		}

		public int U4_AnimationTypeWorkaround(ModelImporter mi) {
			#if UNITY_4_0
			switch (mi.animationType) {
				case UnityEditor.ModelImporterAnimationType.None:
					return Mixamo.MixamoStore.IMPORT_TYPE_NONE;
				case UnityEditor.ModelImporterAnimationType.Legacy:
					return Mixamo.MixamoStore.IMPORT_TYPE_LEGACY;
				case UnityEditor.ModelImporterAnimationType.Generic:
					return Mixamo.MixamoStore.IMPORT_TYPE_GENERIC;
				case UnityEditor.ModelImporterAnimationType.Human:
					return Mixamo.MixamoStore.IMPORT_TYPE_HUMANOID;
			}
			#endif

			return Mixamo.MixamoStore.IMPORT_TYPE_LEGACY;
		}

	}

}

