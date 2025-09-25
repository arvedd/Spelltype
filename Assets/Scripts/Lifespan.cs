using System.Security.Cryptography;
using UnityEditor.UIElements;
using UnityEngine;

public class Lifespan : MonoBehaviour
{
    public float lifespan;

    // Update is called once per frame
    void Update()
    {
        lifespan -= Time.deltaTime;
       

        if (lifespan <= 0)
        {
            Destroy(gameObject);
        }
    }
}
