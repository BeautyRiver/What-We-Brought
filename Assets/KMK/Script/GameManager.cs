using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        SoundManager.instance.PlayBGM(0);       
    }
}
