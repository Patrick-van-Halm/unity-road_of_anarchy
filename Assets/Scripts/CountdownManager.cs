using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class CountdownManager : NetworkBehaviour
{
    [SyncVar(hook = nameof(CurrentSecondChanged))]
    private int _currentSecond = 4;

    public UnityEvent<int> OnCurrentSecondChanged = new UnityEvent<int>();
    public UnityEvent CountdownEnds = new UnityEvent();

    public static CountdownManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(this);
    }

    [Server]
    public void CheckIfStartCountdown()
    {
        Player[] players = FindObjectsOfType<Player>();
        if (players.Length != NetworkServer.connections.Count) return;
        if (!players.All(p => p.IsReady)) return;
        StartCountdown();
    }

    [Server]
    public void StartCountdown()
    {
        StartCoroutine(CoroCountdown());
    }

    private IEnumerator CoroCountdown()
    {
        while(_currentSecond != 0)
        {
            _currentSecond--;
            yield return new WaitForSeconds(1f);
        }
    }

    private void CurrentSecondChanged(int oldValue, int newValue)
    {
        OnCurrentSecondChanged?.Invoke(_currentSecond);
        if (_currentSecond == 0) CountdownEnds?.Invoke();
    }
}
