// FrameRateManager
using UnityEngine;

public class FrameRateManager : MonoBehaviour
{
    private static FrameRateManager _instance;

    private float deltaTime = 0.0f;
    [SerializeField] private float updateRate = 0.5f; // 0.5 seconds interval
    [SerializeField] private float averageFPS = 0.0f;

    public float AverageFPS => averageFPS;

    public static FrameRateManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<FrameRateManager>();

                if (_instance == null)
                {
                    GameObject managerObject = new GameObject("FrameRateManager");
                    _instance = managerObject.AddComponent<FrameRateManager>();
                }
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

        if (Time.timeSinceLevelLoad > updateRate)
        {
            averageFPS = 1.0f / deltaTime;
            updateRate += 0.5f; // Update every 0.5 seconds
        }
    }
}
