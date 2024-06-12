using UnityEngine;
using UnityEngine.VFX;

public class VisualEffectSetPosition : MonoBehaviour
{
    public VisualEffect _visualEffect;
    
    public Transform _transform;

    void Start()
    {
        if (_visualEffect == null)
        {
            _visualEffect = GetComponent<VisualEffect>();
        }
    }

    void Update()
    {
        if (_visualEffect != null)
        {
            _visualEffect.SetVector3("SpawnPosition", _transform.position);
        }
    }
}
