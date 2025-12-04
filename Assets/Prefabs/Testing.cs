using UnityEngine;

public class Testing : MonoBehaviour
{
    public DamagePopupSpawner damagePopup;

    void Start()
    {
       damagePopup.Create(Vector3.zero, 300, true);
    }
}
