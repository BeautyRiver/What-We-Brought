using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject Player;
    public GameObject Rat;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 시작 시 플레이어 활성화, 쥐는 비활성화
        Player.SetActive(true);
        Rat.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // E가 입력됐을 때
        {
            if (Player.activeSelf == true) // 플레이어와 쥐 무엇이 활성화 되어있는지 체크
            {
                //플레이어 비활성화, 쥐 활성화
                Player.SetActive(false);
                Rat.SetActive(true);
            }
            else if (Rat.activeSelf == true)
            {
                //플레이어 활성화, 쥐 비활성화
                Player.SetActive(true);
                Rat.SetActive(false);
            }
            else print("유효한 플레이어와 쥐 오브젝트를 배정하십시오.");
        }
    }
}