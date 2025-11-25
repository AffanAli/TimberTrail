using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlinkText : MonoBehaviour
{
    public TMP_Text txt;
    public float from;
    public float to;
    public bool loop;
    public float speed;

    float timer;
    Color color;
    float a, b;
    int mul = 1;

    private void Update()
    {
        color = txt.color;

        color.a = Mathf.Lerp(from, to, timer);
        txt.color = color;

        timer += Time.deltaTime * speed * mul;

        if (timer >= 1 || timer <= 0)
        {
            mul *= -1;
        }
    }
}
