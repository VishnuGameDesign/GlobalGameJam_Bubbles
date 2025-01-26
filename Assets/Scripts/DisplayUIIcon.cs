using UnityEngine;

public class DisplayUIIcon : MonoBehaviour
{
    [SerializeField] private GameObject _uiPopUp;
    
    private void Awake()
    {
        _uiPopUp.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController player))
        {
            _uiPopUp.SetActive(true);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        _uiPopUp.SetActive(false);
    }
}
