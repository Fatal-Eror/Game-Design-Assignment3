using System.Collections;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LaserScript : MonoBehaviour
{
    [SerializeField] private GameObject laserSpawnPosition;
    private LineRenderer _lineRender;
    private Coroutine _fadeCoroutine;

    // When laser is fired, it should be opaque at first
    private Color _startColor = Color.red;
    // After a few second, it would be transparent
    private Color _endColor = new Color(1, 0, 0, 0);
    [Tooltip("Duration time from opaque to transparent")]
    [SerializeField] private float duration = 1f;


    private void Awake()
    {
        _lineRender = GetComponent<LineRenderer>();
        _lineRender.enabled = false;
        _lineRender.startColor = _startColor;
        _lineRender.endColor = _startColor;

        LevelState_Level1.OnGameStart += GameStart;
        LevelState_Level1.OnGameEnd += GameEnd;
    }

    private void GameStart()
    {
        _lineRender.enabled = true;

        PlayerCamera.OnRayCastFire += ShootLaser;
    }

    private void GameEnd()
    {
        _lineRender.enabled = false;

        PlayerCamera.OnRayCastFire -= ShootLaser;

        LevelState_Level1.OnGameStart -= GameStart;
        LevelState_Level1.OnGameEnd -= GameEnd;
    }
    private void ShootLaser(Vector3 hitPosition)
    {
        _lineRender.SetPositions(new[] { laserSpawnPosition.transform.position, hitPosition });

        // Initialize color
        _lineRender.startColor = _startColor;
        _lineRender.endColor = _startColor;

        // If there is any coroutine, we should stop it at first
        if (_fadeCoroutine != null)
        {
            StopCoroutine(_fadeCoroutine);
        }

        // Start new coroutine
        _fadeCoroutine = StartCoroutine(FadeLine());
    }

    private IEnumerator FadeLine()
    {
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            float normalizedTime = time / duration;

            // Use lerp to change color
            Color color = Color.Lerp(_startColor, _endColor, normalizedTime);
            _lineRender.startColor = color;
            _lineRender.endColor = color;

            yield return null;
        }

        // Ensure the line to be totally transparent
        _lineRender.startColor = _endColor;
        _lineRender.endColor = _endColor;
    }
}
