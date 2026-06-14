using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    [Header("Shatter")]
    [SerializeField] private int columns = 10;
    [SerializeField] private int rows = 10;
    [SerializeField] private float duration = 0.4f;
    [SerializeField] private float maxAngle = 30f;
    [SerializeField] private int blurDownsample = 6;

    private void OnEnable() => PlayerHealth.OnDeath += OnDeath;
    private void OnDisable() => PlayerHealth.OnDeath -= OnDeath;

    private void OnDeath() => StartCoroutine(DeathSequence());

    private IEnumerator DeathSequence()
    {
        yield return new WaitForEndOfFrame();

        RenderTexture capture = new RenderTexture(Screen.width, Screen.height, 0);
        ScreenCapture.CaptureScreenshotIntoRenderTexture(capture);
        RenderTexture screenshot = new RenderTexture(Screen.width, Screen.height, 0);
        Graphics.Blit(capture, screenshot, new Vector2(1f, -1f), new Vector2(0f, 1f));
        capture.Release();

        RenderTexture blurRT = new RenderTexture(Screen.width / blurDownsample, Screen.height / blurDownsample, 0);
        Graphics.Blit(screenshot, blurRT);
        Graphics.Blit(blurRT, screenshot);
        blurRT.Release();

        Time.timeScale = 0f;

        GameObject shatterRoot = new GameObject("ShatterCanvas");
        Canvas canvas = shatterRoot.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 99;

        Piece[] pieces = SpawnPieces(shatterRoot, screenshot);
        yield return AnimatePieces(pieces);

        panel.SetActive(true);
    }

    private struct Piece
    {
        public RectTransform rt;
        public float targetAngle;
    }

    private Piece[] SpawnPieces(GameObject parent, Texture texture)
    {
        float screenW = Screen.width;
        float screenH = Screen.height;
        float pieceW = screenW / columns;
        float pieceH = screenH / rows;
        Piece[] pieces = new Piece[columns * rows];

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                GameObject go = new GameObject($"p{col}_{row}");
                go.transform.SetParent(parent.transform, false);

                RawImage img = go.AddComponent<RawImage>();
                img.texture = texture;
                img.uvRect = new Rect((float)col / columns, (float)row / rows, 1f / columns, 1f / rows);

                RectTransform rt = go.GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(pieceW, pieceH);
                rt.anchorMin = rt.anchorMax = Vector2.zero;
                rt.pivot = Vector2.one * 0.5f;
                rt.anchoredPosition = new Vector2((col + 0.5f) * pieceW, (row + 0.5f) * pieceH);

                pieces[row * columns + col] = new Piece
                {
                    rt = rt,
                    targetAngle = Random.Range(-maxAngle, maxAngle)
                };
            }
        }

        return pieces;
    }

    private IEnumerator AnimatePieces(Piece[] pieces)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float eased = 1f - Mathf.Pow(1f - t, 3f);

            foreach (var p in pieces)
                p.rt.localRotation = Quaternion.Euler(0f, 0f, p.targetAngle * eased);

            yield return null;
        }

        foreach (var p in pieces)
            p.rt.localRotation = Quaternion.Euler(0f, 0f, p.targetAngle);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneController.Instance.ReloadCurrentScene();
    }
}
