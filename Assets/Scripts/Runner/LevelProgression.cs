using UnityEngine;

public class LevelProgression : MonoBehaviour
{
    [SerializeField] private TrackManager trackManager;
    [SerializeField] private float gracePeriod = 3f;

    [Header("Obstacle Chance over Speed")]
    [SerializeField] private AnimationCurve obstacleChanceCurve = new AnimationCurve(
        new Keyframe(6f, 0.1f), new Keyframe(12f, 0.35f), new Keyframe(20f, 0.6f));

    [Header("Coin Chance over Speed")]
    [SerializeField] private AnimationCurve coinChanceCurve = new AnimationCurve(
        new Keyframe(6f, 0.25f), new Keyframe(12f, 0.5f), new Keyframe(20f, 0.7f));

    [Header("Special Collectible Chance over Speed")]
    [SerializeField] private AnimationCurve specialChanceCurve = new AnimationCurve(
        new Keyframe(6f, 0.05f), new Keyframe(12f, 0.12f), new Keyframe(20f, 0.2f));

    [Header("Spawn Interval over Speed (higher = sparser)")]
    [SerializeField] private AnimationCurve spawnIntervalCurve = new AnimationCurve(
        new Keyframe(6f, 14f), new Keyframe(12f, 8f), new Keyframe(20f, 5f));

    private float elapsedTime;

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime < gracePeriod)
        {
            trackManager.SpawningEnabled = false;
            return;
        }

        float speed = trackManager.WorldSpeed;
        trackManager.SpawningEnabled = true;
        trackManager.ObstacleChance = obstacleChanceCurve.Evaluate(speed);
        trackManager.CoinChance = coinChanceCurve.Evaluate(speed);
        trackManager.SpecialChance = specialChanceCurve.Evaluate(speed);
        trackManager.SpawnInterval = spawnIntervalCurve.Evaluate(speed);
    }
}
