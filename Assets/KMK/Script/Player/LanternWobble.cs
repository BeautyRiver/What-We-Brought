using UnityEngine;

[ExecuteInEditMode]
public class LanternWobble : MonoBehaviour
{
    private Light myLight;

    [Header("불빛 흔들림 설정")]
    public float range = 10f;
    public float minIntensity = 2.0f;
    public float maxIntensity = 4.0f;
    public float flickerSpeed = 5.0f;

    [Header("위치 흔들림 (옵션)")]
    public bool shakePosition = true;
    public float shakeAmount = 0.1f;

    // 이 값을 보존하기 위해 private 대신 시리얼라이즈는 안 되게 설정
    private Vector3 initialPos;

    void OnEnable()
    {
        myLight = GetComponent<Light>();
        // 실행 중이 아닐 때(에디터)는 현재 위치를 기준점으로 계속 갱신하지 않도록 함
        if (Application.isPlaying)
        {
            initialPos = transform.localPosition;
        }
    }

    void Update()
    {
        if (myLight == null) myLight = GetComponent<Light>();

        // 1. 밝기 흔들림
        float noise = Mathf.PerlinNoise(Time.time * flickerSpeed, 0f);
        myLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, noise);
        myLight.range = range;

        // 2. 위치 미세 흔들림
        if (shakePosition)
        {
            // 게임 플레이 중에만 위치를 강제로 고정/흔들기함
            // 에디터 모드에서는 위치 수정을 방해하지 않기 위해 skip하거나 별도 처리
            if (Application.isPlaying)
            {
                float x = (Mathf.PerlinNoise(Time.time * flickerSpeed, 1f) - 0.5f) * shakeAmount;
                float y = (Mathf.PerlinNoise(Time.time * flickerSpeed, 2f) - 0.5f) * shakeAmount;
                transform.localPosition = initialPos + new Vector3(x, y, 0);
            }
        }
    }
}