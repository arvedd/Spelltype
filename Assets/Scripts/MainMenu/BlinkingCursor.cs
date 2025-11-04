using UnityEngine;
using UnityEngine.UI;

public class BlinkingCursor : MonoBehaviour
{
    public Graphic cursor;
    public float blinkSpeed = 1.2f;

    void Update()
    {
        float alpha = Mathf.PingPong(Time.time * blinkSpeed, 1f);
        var color = cursor.color;
        color.a = alpha;
        cursor.color = color;
    }
}
