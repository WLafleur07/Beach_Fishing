using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BF_ShakeSadFace : MonoBehaviour
{

    public static BF_ShakeSadFace Instance { get; set; }

    private void Awake()
    {

        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this.gameObject);

        _startPos = transform.position;
    }

    [Header("Info")]
    private Vector2 _startPos;
    private float _timer;
    private Vector2 _randomPos;

    [Header("Settings")]
    [Range(0f, 4f)]
    public float _time = 0.2f;
    [Range(0f, 2f)]
    public float _distance = 0.1f;
    [Range(0f, 0.1f)]
    public float _delayBetweenShakes = 0f;

    private float _startMusicVolume = 0;

    private void OnValidate()
    {
        if (_delayBetweenShakes > _time)
            _delayBetweenShakes = _time;
    }

    public void Begin()
    {
        _startMusicVolume = BF_AudioManager.Instance.GetMusicVolume();
        StopAllCoroutines();
        StartCoroutine(Shake());
        BF_AudioManager.Instance.SetMusicVolume(0);
        BF_AudioManager.Instance.PlaySFX((int)BF_AudioManager.SFX.FISH_LOST_1);
        BF_UIManager.Instance.cancelFishingButton.gameObject.SetActive(false);
        BF_UIManager.Instance.castButton.gameObject.SetActive(false);
    }

    private IEnumerator Shake()
    {
        _timer = 0f;

        while (_timer < _time)
        {
            _timer += Time.deltaTime;

            _randomPos = _startPos + (Random.insideUnitCircle * _distance);

            transform.position = _randomPos;

            if (_delayBetweenShakes > 0f)
            {
                yield return new WaitForSeconds(_delayBetweenShakes);
            }
            else
            {
                yield return null;
            }
        }

        transform.position = _startPos;
        gameObject.SetActive(false);
        BF_AudioManager.Instance.SetMusicVolume(_startMusicVolume);
        BF_UIManager.Instance.cancelFishingButton.gameObject.SetActive(false);
        BF_UIManager.Instance.OpenScreen((int)BF_UIManager.BF_UI_Screens.SETTINGS_PANEL);
        BF_UIManager.Instance.OpenScreen((int)BF_UIManager.BF_UI_Screens.CHALLENGE_PANEL);
        BF_UIManager.Instance.CastButton();
    }
}

