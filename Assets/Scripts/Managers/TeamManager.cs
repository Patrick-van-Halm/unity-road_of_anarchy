using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static LobbyPlayer;

public class TeamManager : NetworkBehaviour
{
    public static TeamManager Instance { get; private set; }

    public IReadOnlyList<Team> Teams => _teams;
    private List<Team> _teams = new List<Team>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    public void GenerateTeams(IEnumerable<LobbyPlayer> lobbyPlayers)
    {
        if (lobbyPlayers == null) return;
        if (lobbyPlayers.Count() == 0) return;
        if (lobbyPlayers.Count() % 2 != 0) return;

        List<LobbyPlayer> driverOnly = lobbyPlayers.Where(l => l.RolePreference == RolesPreference.OnlyDriver).ToList();
        List<LobbyPlayer> gunnerOnly = lobbyPlayers.Where(l => l.RolePreference == RolesPreference.OnlyGunner).ToList();
        List<LobbyPlayer> both = lobbyPlayers.Where(l => l.RolePreference == RolesPreference.Both).ToList();

        for(int i = 0; i < lobbyPlayers.Count() / 2; i++)
        {
            Team team = new Team();
            _teams.Add(team);

            List<LobbyPlayer> list;
            if (driverOnly.Count > 0) list = driverOnly;
            else if (both.Count > 0) list = both;
            else list = gunnerOnly;
            LobbyPlayer player = list.Random();
            team.DriverClientId = player.connectionToClient.connectionId;
            list.Remove(player);

            if (gunnerOnly.Count > 0) list = gunnerOnly;
            else if (both.Count > 0) list = both;
            else list = driverOnly;
            player = list.Random();
            team.GunnerClientId = player.connectionToClient.connectionId;
            list.Remove(player);
        }
    }
}
