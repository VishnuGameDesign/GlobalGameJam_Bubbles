using System.Collections;
using GameEvents;
using TMPro;
using UnityEngine;

public class JoinAndCountdownUI : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private GameObject _container;
    [SerializeField] private TextMeshProUGUI _pressStartText;
    [SerializeField] private TextMeshProUGUI _countdownText;
    [SerializeField] private BoolEventAsset _allPlayersReadyToRace;
    [SerializeField] private BoolEventAsset _canRaceNow;
    [SerializeField] private string _raceTXT;

    private bool _isReadyToRace;
    
    private void Start()
    {
        _panel.SetActive(true);
        _countdownText.enabled = false;
    }

    private void OnEnable()
    {
        _allPlayersReadyToRace.OnInvoked.AddListener(AllPlayersReadyToRace);
    }

    private void OnDisable()
    {
        _allPlayersReadyToRace.OnInvoked.RemoveListener(AllPlayersReadyToRace);
    }

    private void AllPlayersReadyToRace(bool isReadyToRace)
    {
        _isReadyToRace = true;
        DisplayCountdownText();
    }
    
    private void Update()
    {
        if (_isReadyToRace)
        {
            _pressStartText.enabled = false;
        }
    }
    private void DisplayCountdownText()
    {
        _countdownText.enabled = true;
        StartCoroutine(StartCountdown());
    }

    private IEnumerator StartCountdown()
    {
        for (int i = 3; i > 0; i--)
        {
            _countdownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }
        _countdownText.text = _raceTXT;
        Invoke("DisableCountdownText", 0.5f);
    }

    private void DisableCountdownText()
    {
        _canRaceNow.Invoke(true);
        _countdownText.enabled = false;
        this.gameObject.SetActive(false);
        _container.gameObject.SetActive(false);
    }

}
