using FMODUnity;
using UnityEngine;

public class Gamemanager : MonoBehaviour
{
    [SerializeField] private EventReference _bgm;


    private void Start()
    {
        PlayBGM();
    }

    private void PlayBGM()
    {
        RuntimeManager.PlayOneShot(_bgm, transform.position);
    }
}
