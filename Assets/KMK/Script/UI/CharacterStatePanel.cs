using UnityEngine;
using UnityEngine.UI;

public class CharacterStatePanel : MonoBehaviour
{
    [SerializeField] private Sprite humanSprite;
    [SerializeField] private Sprite ratSprite;
    [SerializeField] private Image currentStateImg;

    private void Start()
    {
        ChangSprite("Human");
    }

    public void ChangSprite(string name)
    {
        if (name == "Human")
        {
            currentStateImg.sprite = humanSprite;
        }
        else if (name == "Rat")
        {
            currentStateImg.sprite = ratSprite;
        }
        else
            return;
    }
}
