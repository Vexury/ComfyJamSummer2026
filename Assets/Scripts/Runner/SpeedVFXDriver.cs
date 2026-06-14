using UnityEngine;

public class SpeedVFXDriver : MonoBehaviour
{
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private TrackManager trackManager;
    [SerializeField] private float rateMultiplier = 4.0f;

    private void Update()
    {
        var emission = particles.emission;
        emission.rateOverTime = trackManager.WorldSpeed * rateMultiplier;
    }
}
