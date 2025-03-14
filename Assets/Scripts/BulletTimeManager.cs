using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class BulletTimeManager : MonoBehaviour
{
    public bool isSlowTime;
    public float slowTimeScale;
    public float slowTimeMaxCost;
    public float slowTimeCost;
    public UnityEngine.UI.Image timeBar;
    private SpriteRenderer _slowBackground;


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
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (slowTimeCost < 1f) return;
            StopAllCoroutines();
            StartCoroutine(SlowTimeCoroutine());
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            StopAllCoroutines();
            StartCoroutine(OriginTimeCoroutine());
        }
    }


    IEnumerator SlowTimeCoroutine()
    {
        Time.timeScale = slowTimeScale;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        isSlowTime = true;
        while (_slowBackground.color.a < 0.9f)
        {
            _slowBackground.color = new Color(_slowBackground.color.r, _slowBackground.color.g, _slowBackground.color.b, _slowBackground.color.a + 0.1f);
            yield return new WaitForSecondsRealtime(0.01f);
        }
    }

    IEnumerator OriginTimeCoroutine()
    {
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02f;
        isSlowTime = false;
        while (_slowBackground.color.a > 0)
        {
            _slowBackground.color = new Color(_slowBackground.color.r, _slowBackground.color.g, _slowBackground.color.b, _slowBackground.color.a - 0.1f);
            yield return new WaitForSecondsRealtime(0.01f);
        }

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
        timeBar.fillAmount = slowTimeCost / slowTimeMaxCost;
    }

}