using UnityEngine;

public class TestItemGet : MonoBehaviour
{
    [Header("인벤토리")]
    public Inventory inventory;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
            if(hit.collider != null)
            {
                HitCheckObject(hit);
            }
        }
    }
    void HitCheckObject(RaycastHit2D hit)
    {
        IObjectItem clickInterface = hit.transform.gameObject.GetComponent<IObjectItem>();
        if(clickInterface != null)
        {
            Item item = clickInterface.ClickItem();
            inventory.AddItem(item);
        }
    } 


}
