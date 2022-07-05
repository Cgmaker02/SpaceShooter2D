using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField]
    private float _shakeTime;
    private float _shakePower;
    [SerializeField]
    private float _duration;

    public IEnumerator ShakeCamera()
    {
        _shakeTime = 0.0f;
        _duration = .3f;
        _shakePower = .2f;

        while(_shakeTime < _duration)
        {
            _shakeTime += Time.deltaTime;
            float x = Random.Range(-1f, 1f) * _shakePower;
            float y = Random.Range(0f, 2f) * _shakePower;
            transform.localPosition = new Vector3(x, y, -10);
            yield return null;
        }

        transform.position = new Vector3(0, 1, -10);
    }
}
