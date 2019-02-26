using UnityEngine;
using AEngine.Audio;

public class Test : MonoBehaviour
{
    private void Start()
    {
        AudioManager.Instance.LoadAudioBlock(AEngine.EAudioBlock.NewBlock);
        AudioManager.Instance.PlayMusic();
    }

}
