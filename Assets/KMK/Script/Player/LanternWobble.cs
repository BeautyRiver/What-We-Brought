using UnityEngine;

[ExecuteInEditMode]
public class LanternWobble : MonoBehaviour
{
    private Light myLight;

    [Header("불빛 흔들림 설정")]
    public float range = 10f;
    public float minIntensity = 2.0f; // 최소 밝기
    public float maxIntensity = 4.0f; // 최대 밝기
    public float flickerSpeed = 5.0f; // 깜빡이는 속도

    [Header("위치 흔들림 (옵션)")]
    public bool shakePosition = true;
    public float shakeAmount = 0.1f;
    private Vector3 initialPos;

    void Awake()
    {
        myLight = GetComponent<Light>();
        initialPos = transform.localPosition;
    }

    void Update()
    {
        // 1. 밝기 흔들림 (펄린 노이즈를 써서 부드럽게 불타는 느낌)
        float noise = Mathf.PerlinNoise(Time.time * flickerSpeed, 0f);
        myLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, noise);
        myLight.range = range;

        // 2. 위치 미세 흔들림 (불꽃이 일렁이는 느낌)
        if (shakePosition)
        {
            float x = (Mathf.PerlinNoise(Time.time * flickerSpeed, 1f) - 0.5f) * shakeAmount;
            float y = (Mathf.PerlinNoise(Time.time * flickerSpeed, 2f) - 0.5f) * shakeAmount;
            transform.localPosition = initialPos + new Vector3(x, y, 0);
        }
    }
}