using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] Image image;
    [SerializeField] Outline outline;   // Slot Item에 있는 Outline

    public void SetHighlight(bool on)
    {
        if (outline != null)
            outline.enabled = on;
    }

    private Item _item;
    public Item item
    {
        get { return _item; }
        set
        {
            _item = value;
            if (_item != null)
            {
                image.sprite = _item.itemImage;
                image.color = Color.white;
            }
            else
            {
                image.sprite = null;
                image.color = new Color(1, 1, 1, 0);
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_item == null)
            return;

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            var eq = EquipmentManager.Instance;
            if (eq == null)
            {
                Debug.LogWarning("EquipmentManager가 씬에 없습니다.");
                return;
            }

            // 이미 이 슬롯 아이템이 장착 중이면 해제
            if (eq.equippedItem == _item)
            {
                eq.Unequip();
                eq.SetEquippedSlot(null);      // 여기까지 OK
            }
            else
            {
                eq.Equip(_item);               // 장착 아이템 교체
                eq.SetEquippedSlot(this);      // ★ 새 슬롯을 넘겨줘야 함
            }
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
