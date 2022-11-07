using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostFXScript : MonoBehaviour
{
    [SerializeField] private PostProcessVolume _volume;
    [SerializeField] private float _speedLensModifier = -1.2f;
    [SerializeField] private float _speedChromAbModifier = 50f;

    private LensDistortion _lensDis;
    private ChromaticAberration _chromAb;

    void Start()
    {
        _volume.profile.TryGetSettings(out _lensDis);
        _volume.profile.TryGetSettings(out _chromAb);
    }

    public void SetLensDis(float speed) 
    { 
        _lensDis.intensity.value = speed * _speedLensModifier;
        _chromAb.intensity.value = speed / _speedChromAbModifier;
    }
}