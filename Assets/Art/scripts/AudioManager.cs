using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("BGM Sources (children)")]
    [SerializeField] private AudioSource[] bgmSources;   // BGM
    [Header("SFX Sources (children)")]
    [SerializeField] private AudioSource[] sfxSources;   // SFX

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // index 从 0 开始
    public void PlayBGM(int index, bool loop = true)
    {
        if (!IsValid(index, bgmSources)) return;
        StopAllBGM();
        bgmSources[index].loop = loop;
        bgmSources[index].Play();
    }

    private void Start()
    {
        // 如果有至少一首 BGM，就在游戏开始时循环播放第 0 首
        if (bgmSources.Length > 0)
        {
            PlayBGM(0);   // 第二个参数缺省 = true，自动循环
        }
    }

    public void PlaySFX(int index)
    {
        if (!IsValid(index, sfxSources))
        {
            Debug.LogWarning($"[AudioManager] PlaySFX: 索引 {index} 无效或未赋值");
            return;
        }
        var src = sfxSources[index];
        if (src.clip == null)
        {
            Debug.LogWarning($"[AudioManager] SFX[{index}] 的 AudioSource.clip 为空");
            return;
        }
        src.PlayOneShot(src.clip);
    }


    public void StopAllBGM()
    {
        foreach (var src in bgmSources) src.Stop();
    }

    private bool IsValid(int idx, AudioSource[] arr)
    {
        return idx >= 0 && idx < arr.Length && arr[idx] != null;
    }
}
