using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class VoiceNavigation : MonoBehaviour
{
    private NavMeshPath path;
    private int currentWaypoint = 0;
    [SerializeField] private Transform player;

    private AndroidJavaObject tts; // Android Text-to-Speech

    private void Start()
    {
        path = new NavMeshPath();
        // Initialize Android TTS
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        tts = new AndroidJavaObject("android.speech.tts.TextToSpeech", currentActivity, null);

        StartCoroutine(CheckPosition());
    }

    private IEnumerator CheckPosition()
    {
        while (true)
        {
            if (path.corners.Length > 1 && currentWaypoint < path.corners.Length)
            {
                float distance = Vector3.Distance(player.position, path.corners[currentWaypoint]);

                if (distance < 1.5f) // Player reaches waypoint
                {
                    GiveDirection(currentWaypoint);
                    currentWaypoint++;
                }
            }
            yield return new WaitForSeconds(2f); // Check every 2 seconds
        }
    }

    private void GiveDirection(int index)
    {
        if (index < path.corners.Length - 1)
        {
            Vector3 direction = path.corners[index + 1] - path.corners[index];
            float angle = Vector3.SignedAngle(player.forward, direction, Vector3.up);
            string message;

            if (angle > 30)
                message = "Turn right";
            else if (angle < -30)
                message = "Turn left";
            else
                message = "Go straight";

            Speak(message);
        }
        else
        {
            Speak("You have reached your destination.");
        }
    }

    private void Speak(string text)
    {
        tts.Call("speak", text, 0, null, null);
    }
}
