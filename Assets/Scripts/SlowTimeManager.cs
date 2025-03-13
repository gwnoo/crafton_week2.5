using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class SlowTimeManager : MonoBehaviour
{
    public bool isSlowTime;
    public float slowTimeScale;
    public float slowTimeMaxCost;
    public float slowTimeCost;
    public Slider timeSlider;
    private SpriteRenderer _slowBackground;
    public CinemachineCamera cinemachineCamera;


    private void Start()
    {
        _slowBackground = transform.GetChild(0).GetComponent<SpriteRenderer>();
        slowTimeCost = slowTimeMaxCost;
    }

    private void Update()
    {
        /*if (GameManager.instance.isPlaying == false)
        {
            if (isSlowTime)
            {
                StopAllCoroutines();
                StartCoroutine(OriginTimeCoroutine());
            }
            return;
        }*/
        SliderSet();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (slowTimeCost < 1f) return;
            StopAllCoroutines();
            StartCoroutine(SlowTimeCoroutine());
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            StopAllCoroutines();
            StartCoroutine(OriginTimeCoroutine());
        }
    }


    IEnumerator SlowTimeCoroutine()
    {
        cinemachineCamera.Lens.OrthographicSize = 6;
        Time.timeScale = slowTimeScale;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        isSlowTime = true;
        while (_slowBackground.color.a < 0.9f)
        {
            cinemachineCamera.Lens.OrthographicSize = cinemachineCamera.Lens.OrthographicSize - 0.11f;
            _slowBackground.color = new Color(_slowBackground.color.r, _slowBackground.color.g, _slowBackground.color.b, _slowBackground.color.a + 0.1f);
            yield return new WaitForSecondsRealtime(0.01f);
        }
        cinemachineCamera.Lens.OrthographicSize = 5;
    }

    IEnumerator OriginTimeCoroutine()
    {
        cinemachineCamera.Lens.OrthographicSize = 5;
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02f;
        isSlowTime = false;
        while (_slowBackground.color.a > 0)
        {
            cinemachineCamera.Lens.OrthographicSize = cinemachineCamera.Lens.OrthographicSize + 0.1f;
            _slowBackground.color = new Color(_slowBackground.color.r, _slowBackground.color.g, _slowBackground.color.b, _slowBackground.color.a - 0.1f);
            yield return new WaitForSecondsRealtime(0.01f);
        }
        cinemachineCamera.Lens.OrthographicSize = 6;

    }

    private void SliderSet()
    {
        if (isSlowTime)
        {
            slowTimeCost -= Time.deltaTime * 10;
            if (slowTimeCost <= 0)
            {
                slowTimeCost = 0;
                StopAllCoroutines();
                StartCoroutine(OriginTimeCoroutine());
            }
        }
        else
        {
            slowTimeCost += Time.deltaTime * 3;
            if (slowTimeCost >= slowTimeMaxCost)
            {
                slowTimeCost = slowTimeMaxCost;
            }
        }
        timeSlider.value = slowTimeCost / slowTimeMaxCost;
    }

}