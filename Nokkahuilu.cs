using System;
using System.IO;
using MSCLoader;
using UnityEngine;
using HutongGames.PlayMaker;

namespace Nokkahuilu
{
	public class Nokkahuilu : Mod
	{
		public override string ID => "RecorderFlute"; 
		public override string Name => "Recorder Flute"; 
		public override string Author => "ajanhallinta"; 
		public override string Version => "1.0";
        public override bool UseAssetsFolder => true;

        private bool HasLoaded = false;

		public override void OnLoad()
		{
            ModConsole.Print("Recorder Flute loaded.");
		}

        public override void Update()
        {
            if (HasLoaded)
                return;
            else
            {
                if(GameObject.Find("FPSCamera"))
                    LoadRecorder();
            }
        }

		public void LoadRecorder()
		{
            // Create GameObject, load mesh and set Collider and Rigidbody for recorder.
            GameObject _nokkahuilu = LoadAssets.LoadOBJ(this, Path.Combine(ModLoader.GetModAssetsFolder(this), "nokkahuilu.obj"), true,true);
            _nokkahuilu.name = "Nokkahuilu";
            _nokkahuilu.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.None;
            _nokkahuilu.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;

            // Set flute to table.
            _nokkahuilu.transform.position = new Vector3(-10.03136f, 0.23f, 14.44618f); 
            _nokkahuilu.transform.localRotation = Quaternion.Euler(358.67f, 89.36f, 321.8837f);

            // Create Material
            Material mat = new Material(Shader.Find("Specular"));
            mat.name = "RecorderMaterial";
            mat.color = new Color(0.640f, 0.494f, 0.181f, 1);
            _nokkahuilu.GetComponent<Renderer>().material = mat;

            // Make pivot object for recorder and add the actual recorder logic to it (Recorder.cs)
            GameObject RecorderPivot = new GameObject();
            RecorderPivot.name = "RecorderPivot";
            RecorderPivot.transform.parent = GameObject.Find("FPSCamera").transform;
            RecorderPivot.transform.localPosition = Vector3.zero;
            RecorderPivot.transform.localRotation = Quaternion.Euler(0, 0, 0);
            _nokkahuilu.AddComponent<RecorderCollision>().recorder = RecorderPivot.AddComponent<Recorder>();

            // Instantiate copy of recorder to be used in FPS mode
            GameObject _nokkahuiluFPS = GameObject.Instantiate(_nokkahuilu);
            GameObject.Destroy(_nokkahuiluFPS.GetComponent<MeshCollider>());
            GameObject.Destroy(_nokkahuiluFPS.GetComponent<Rigidbody>());
            _nokkahuiluFPS.transform.parent = GameObject.Find("FPSCamera").transform;
            _nokkahuiluFPS.transform.localPosition = new Vector3(0.0291f, -0.0919f, 0.301f);
            _nokkahuiluFPS.transform.localRotation = Quaternion.Euler(0, 180, 0);

            // Add and setup AudioSource component for recorder.
            RecorderPivot.gameObject.AddComponent<AudioSource>();
            RecorderPivot.GetComponent<AudioSource>().loop = true;
            RecorderPivot.GetComponent<AudioSource>().spatialBlend = 0;
            RecorderPivot.GetComponent<AudioSource>().volume = 1;

            // Set variables to RecorderPivot
            RecorderPivot.GetComponent<Recorder>().audio = RecorderPivot.GetComponent<AudioSource>();
            RecorderPivot.GetComponent<Recorder>().recorder = _nokkahuilu.transform;
            RecorderPivot.GetComponent<Recorder>().recorderFPS = _nokkahuiluFPS.transform;
            _nokkahuiluFPS.transform.parent = RecorderPivot.transform;
            _nokkahuiluFPS.gameObject.SetActive(false);

            // Load .wav file using WWW -class.
            WWW RecorderSound = new WWW("file:///" + @Path.Combine(ModLoader.GetModAssetsFolder(this), "g5.wav"));
            while (!RecorderSound.isDone) { }; // Wait sound to be loaded.

            // Add clip to recorder flute.
            RecorderPivot.GetComponent<AudioSource>().clip = RecorderSound.GetAudioClip(false);

            // Stop the loading loop.
            HasLoaded = true;
		}
	}
}
