using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public bool clearRatTutorial;
    
    public GameObject glassBead;
    
    public void InstantiateGlassBead()
    {
        glassBead.SetActive(true);
    }
}
