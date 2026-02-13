using UnityEngine;

[ExecuteInEditMode] // 에디터에서 실행만 해도 바로 보이게 함 (플레이 안 해도 됨)
public class SceneAtmosphere : MonoBehaviour
{
    [Header("안개 설정")]
    public Color atmosphereColor = Color.gray; // 이 색 하나로 안개+배경 통일!
    public float fogStart = 5f;
    public float fogEnd = 30f;

    [Header("옵션")]
    public bool applyToCamera = true; // 카메라 배경색도 바꿀 건지

    void Update() // 에디터에서 값 바꿀 때 실시간 반영을 위해 Update 사용
    {
        // 1. 안개 설정 적용
        RenderSettings.fog = true;
        RenderSettings.fogMode = FogMode.Linear;
        RenderSettings.fogColor = atmosphereColor;
        RenderSettings.fogStartDistance = fogStart;
        RenderSettings.fogEndDistance = fogEnd;

        // 2. 메인 카메라 배경색도 똑같이 맞춤 (경계선 없애기)
        if (applyToCamera && Camera.main != null)
        {
            Camera.main.clearFlags = CameraClearFlags.SolidColor;
            Camera.main.backgroundColor = atmosphereColor;
        }
    }
}