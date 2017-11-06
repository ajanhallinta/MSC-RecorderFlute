using UnityEngine;
using MSCLoader;
using HutongGames.PlayMaker;

public class RecorderCollision : MonoBehaviour
{

    public Recorder recorder;

    private void OnCollisionEnter(Collision collision)
    {
        // If recorder is thrown at Teimo's window, send PlayMaker event to break it.
        if (collision.gameObject.name == "BreakableWindow" && !recorder.IsCarryingRecorder)
            collision.gameObject.GetComponent<PlayMakerFSM>().SendEvent("FINISHED");
    }

}
