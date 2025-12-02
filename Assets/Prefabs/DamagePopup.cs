using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    private TextMeshPro text;
    float moveYSpeed = 2f;
    float dissapearSpeed = 3f;
    private float disappearTime;
    private const float DISAPPEAR_TIMER_MAX = 1f;
    private Color textColor;
    private Vector3 moveVector;

    void Awake()
    {
        text = transform.GetComponent<TextMeshPro>();    
    }

    public void Setup(int damageAmount, bool isWeak)
    {
        text.SetText(damageAmount.ToString());

        if (!isWeak)
        {
            //Normal Hit
            text.fontSize = 7;
            textColor = Color.white;
        }
        else
        {
            //Critical Hit
            text.fontSize = 10;
            textColor = Color.red;
        }

        text.color = textColor;
        disappearTime = DISAPPEAR_TIMER_MAX;
        moveVector = new Vector3(1, 1) * 2f;
    }

    void Update()
    {
        transform.position += moveVector * Time.deltaTime;
        moveVector -= moveVector * 2f * Time.deltaTime;

        if (disappearTime < DISAPPEAR_TIMER_MAX * 0.5f)
        {
            float increaseLocalScale = 1f;
            transform.localScale += Vector3.one * increaseLocalScale * Time.deltaTime; 
        } 
        else
        {
            float decreaseLocalScale = 1f;
            transform.localScale -= Vector3.one * decreaseLocalScale * Time.deltaTime; 
        }

        disappearTime -= Time.deltaTime;
        if (disappearTime < 0)
        {
            textColor.a -= dissapearSpeed * Time.deltaTime;
            text.color = textColor;
        }

        if (textColor.a < 0)
        {
            Destroy(gameObject);
        }
    }
}
