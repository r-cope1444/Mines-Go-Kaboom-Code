
using UnityEngine;

//Randomly plays seagull noises.
public class GullAudioController : MonoBehaviour
{
    private AudioSource[] seaGulls;
    private AudioSource activeGull;
    private bool audioPlaying;

    void Start()
    {
        seaGulls = GetComponentsInChildren<AudioSource>();
        audioPlaying = false;
    }

    void Update()
    {
        if (!audioPlaying)
        {
            float fps = 1.0f / Time.deltaTime;
            int max = (int)(fps * 20);
            int x = Random.Range(0, max);
            if (x == 2)
            {
                int y = Random.Range(0, seaGulls.Length);
                activeGull = seaGulls[y];
                audioPlaying = true;
                activeGull.Play();
            }
        }
        else
        {
            audioPlaying = activeGull.isPlaying;
        }
    }
}
