using UnityEngine;
using UnityEngine.UI;

public class GhostItemUI : MonoBehaviour
{
    public Image ghostImage; // 따라다닐 이미지 컴포넌트

    private void Awake()
    {
        // 시작할 땐 숨김
        Hide();
        ghostImage.raycastTarget = false;
    }

    private void Update()
    {
        // 이미지가 켜져 있을 때만 마우스 위치 따라가기
        if (ghostImage.gameObject.activeSelf)
        {
            transform.position = Input.mousePosition;
        }
    }

    public void SetItemSprite(Item item)
    {
        if (item == null) return;
        ghostImage.sprite = item.itemImage;
    }

    // 외부에서 켜고 끌 수 있게 함수 열어주기
    public void Show() => ghostImage.gameObject.SetActive(true);
    public void Hide() => ghostImage.gameObject.SetActive(false);
}