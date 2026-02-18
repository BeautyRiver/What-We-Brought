using System.Collections.Generic;
using UnityEngine;

public class SceneSpawnManager : MonoBehaviour
{
    public static SceneSpawnManager instance;

    [Header("이 씬에 있는 스폰 포인트들")]
    public Transform[] spawnPoints;

    private void Awake()
    {
        instance = this;
    }

    // ID(이름)로 스폰 포인트 위치를 찾아주는 함수
    public Transform GetSpawnPoint(string id)
    {
        foreach (Transform point in spawnPoints)
        {
            if (point.name == id)
            {
                return point;
            }
        }

        Debug.LogWarning($"[SceneSpawnManager] '{id}'라는 이름의 스폰 포인트를 리스트에서 못 찾았어요!");
        return null;
    }
}