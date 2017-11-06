using UnityEngine;
using System.Collections;
using System.IO;
using MSCLoader;
using HutongGames.PlayMaker;

public class Recorder : MonoBehaviour {

    public class SaveData
    {
        public float posX, posY, posZ, rotX, rotY, rotZ;
    }

    public AudioSource audio;
    private KeyCode G = KeyCode.Alpha1;
    private KeyCode A = KeyCode.Alpha2;
    private KeyCode H = KeyCode.Alpha3;
    private KeyCode C = KeyCode.Alpha4;
    private KeyCode D = KeyCode.Alpha5;
    private KeyCode E = KeyCode.Alpha6;
    private KeyCode F = KeyCode.Alpha7;
    private KeyCode G2 = KeyCode.Alpha8;

    private bool IsActive = false;
    public bool IsCarryingRecorder = false; // for shop window collision detection

    private Transform itemPivot;
    public Transform recorder;
    public Transform recorderFPS;
    private Transform hand;

    private void Start()
    {
        Load();

        // Make Object Carriable
        recorder.gameObject.layer = LayerMask.NameToLayer("Parts");
        recorder.gameObject.tag = "ITEM";
        recorder.gameObject.name = "nokkahuilu(itemx)";

        Nokkahuilu.GameHook.InjectStateHook(GameObject.Find("ITEMS"), "Save game", Save);
    }

    // Update is called once per frame
    void Update()
    {
        if (audio == null)
            return;

        if (itemPivot == null)
            itemPivot = PlayMakerGlobals.Instance.Variables.FindFsmGameObject("ItemPivot").Value.transform;
        if (hand == null)
            hand = GameObject.Find("1Hand_Assemble/Hand").transform;

        // Drop recorder (from fps mode)
        if (cInput.GetButtonDown("Use") && recorderFPS.gameObject.activeSelf == true)
        {
            PlayMakerGlobals.Instance.Variables.FindFsmBool("PlayerHandRight").Value = false;
            PlayMakerGlobals.Instance.Variables.FindFsmBool("PlayerHandLeft").Value = false;

            recorder.gameObject.SetActive(true);
            recorder.transform.position = hand.transform.position;

            recorderFPS.gameObject.SetActive(false);
        }

        // Activate if recorder is on player hand and 'Use' is pressed.
        if (recorder.parent != null)
        {
            if (recorder.parent == itemPivot)
            {
                IsCarryingRecorder = true;
                if (cInput.GetButtonDown("Use") && !recorderFPS.gameObject.activeSelf)
                {
                    recorder.gameObject.SetActive(false);
                    recorderFPS.gameObject.SetActive(true);
                }
            }
        } else
        {
            IsCarryingRecorder = false;
        }

        IsActive = recorderFPS.gameObject.activeSelf;

        // Stop code executing here if recorder is not on hands
        if (!IsActive)
        {
            if (audio.isPlaying)
                audio.Stop();
            return;
        }

        PlayMakerGlobals.Instance.Variables.FindFsmBool("PlayerHandRight").Value = true;
        PlayMakerGlobals.Instance.Variables.FindFsmBool("PlayerHandLeft").Value = true;

        // Hold Mouse button to play sound clip.
        if (Input.GetMouseButton(0))
        {
            if (!audio.isPlaying)
                audio.Play();
        }
        else
        {
            if (audio.isPlaying)
                audio.Stop();
        }

        // If no notes are hold, play random FalseSound
        StartCoroutine(FalseSound());

        // Change pitch depending which key is hold.
        if (Input.GetKey(G))
            PlaySound(G);
        else if (Input.GetKey(A))
            PlaySound(A);
        else if (Input.GetKey(H))
            PlaySound(H);
        else if (Input.GetKey(C))
            PlaySound(C);
        else if (Input.GetKey(D))
            PlaySound(D);
        else if (Input.GetKey(E))
            PlaySound(E);
        else if (Input.GetKey(F))
            PlaySound(F);
        else if (Input.GetKey(G2))
            PlaySound(G2);
           
    }

    void PlaySound(KeyCode note)
    {
        if (note == G)    
            audio.pitch = 1;
        else if (note == A) 
            audio.pitch = 1.125f;  
        else if (note == H) 
            audio.pitch = 1.250f;
        else if (note == C)  
            audio.pitch = 1.350f; 
        else if (note == D) 
            audio.pitch = 1.5f;
        else if (note == E) 
            audio.pitch = 1.7f;
        else if (note == F) 
            audio.pitch = 1.9f;      
        else if (note == G2)  
            audio.pitch = 2f;

        // Drunken Flute Simulation
        float drunkLevel = PlayMakerGlobals.Instance.Variables.FindFsmFloat("PlayerDrunk").Value;
        audio.pitch += Mathf.Lerp(0, Random.Range(0 - (drunkLevel*0.075f), 0 + (drunkLevel * 0.075f)), drunkLevel/2);
    }

    IEnumerator FalseSound()
    {
        if (audio == null)
            yield break;

        float drunkLevel = PlayMakerGlobals.Instance.Variables.FindFsmFloat("PlayerDrunk").Value;
        audio.pitch = Mathf.Lerp(Random.Range(1.000f, 1.100f), Random.Range(1.000f, 1.100f),Time.deltaTime*2);
        audio.pitch += Mathf.Lerp(0, Random.Range(0 - (drunkLevel * 0.075f), 0 + (drunkLevel * 0.075f)), drunkLevel / 2);

        yield return new WaitForSeconds(Random.Range(0.02f,0.1f));

    }

    private void Save()
    {
        var data = new SaveData
        {
            posX = recorder.transform.position.x,
            posY = recorder.transform.position.y,
            posZ = recorder.transform.position.z,
            rotX = recorder.transform.rotation.eulerAngles.x,
            rotY = recorder.transform.rotation.eulerAngles.y,
            rotZ = recorder.transform.rotation.eulerAngles.z,
        };

        Nokkahuilu.SaveUtil.SerializeWriteFile(data, SaveFilePath);
    }

    private void Load()
    {
        if (!File.Exists(SaveFilePath))
            return;

        var data = Nokkahuilu.SaveUtil.DeserializeReadFile<SaveData>(SaveFilePath);
        recorder.transform.position = new Vector3(data.posX, data.posY, data.posZ);
        recorder.transform.rotation = Quaternion.Euler(data.rotX, data.rotY, data.rotZ);
    }


    public string SaveFilePath
    {
        get { return Path.Combine(Application.persistentDataPath, "recorderflute.xml"); }
    }
}
