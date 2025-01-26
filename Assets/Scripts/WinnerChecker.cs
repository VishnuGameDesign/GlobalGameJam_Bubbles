using System;
using TMPro;
using UnityEngine;

public class WinnerChecker : MonoBehaviour
{
    [SerializeField] private GameObject _winCanvas;
    [SerializeField] private TextMeshProUGUI _winnerText;

    private void Awake()
    {
        _winCanvas.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        _winCanvas.SetActive(true);
        _winnerText.text = other.gameObject.name + "Wins";
    }
}
