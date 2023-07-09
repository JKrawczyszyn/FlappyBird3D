using UnityEngine;

public class PhoneObject : MonoBehaviour
{
    private void Awake()
    {
        if (Application.platform is not RuntimePlatform.Android or RuntimePlatform.IPhonePlayer)
            gameObject.SetActive(false);
    }
}
