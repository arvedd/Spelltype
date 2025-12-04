using UnityEngine;

public class DamagePopupSpawner : MonoBehaviour
{
    [SerializeField] private Transform pfDamagePopup;
    public void Create(Vector3 position, int damageAmount, bool isWeak)
    {
        Transform damagePopupTransform = Instantiate(pfDamagePopup, position, Quaternion.identity);
        DamagePopup damagePopup = damagePopupTransform.GetComponent<DamagePopup>();
        damagePopup.Setup(damageAmount, isWeak);
    }
}
