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

            // 슬롯 기준으로 장착/해제
            if (eq.equippedSlot == this)
            {
                eq.Unequip();
                eq.SetEquippedSlot(null);
            }
            else
            {
                eq.Equip(_item);
                eq.SetEquippedSlot(this);
            }
        }
    }
}
