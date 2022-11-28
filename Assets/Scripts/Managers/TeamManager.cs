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

        List<LobbyPlayer> assignedPlayers = new();

        foreach(LobbyPlayer player in lobbyPlayers.Where(p => p.TeamColor.HasValue))
        {
            // Get player and team
            if (assignedPlayers.Contains(player)) continue;
            Color color = player.TeamColor.Value;

            // Get team member if no team member skip member so player will be included in random team assignment
            LobbyPlayer player2 = lobbyPlayers.FirstOrDefault(p => p != player && p.TeamColor.HasValue && p.TeamColor.Value == color);
            if(player2 == null) continue;

            Team team = new Team();
            _teams.Add(team);

            // Select roles
            LobbyPlayer driver;
            LobbyPlayer gunner;
            if (player.RolePreference == RolesPreference.OnlyDriver && player2.RolePreference != RolesPreference.OnlyDriver)
            {
                // Only player 1 has selected to become a driver so gunner automatically is player 2
                driver = player;
                gunner = player2;
            }
            else if (player2.RolePreference == RolesPreference.OnlyDriver && player.RolePreference != RolesPreference.OnlyDriver)
            {
                // Only player 2 has selected to become a driver so gunner automatically is player 1
                driver = player2;
                gunner = player;
            }
            else if (Random.Range(0, 2) == 0) // Both player 1 and player 2 have selected to become a driver so a random selects either player 1 or player 2 becomes a driver
            {
                driver = player;
                gunner = player2;
            }
            else
            {
                driver = player2;
                gunner = player;
            }

            // Assign roles
            team.DriverClientId = driver.connectionToClient.connectionId;
            team.GunnerClientId = gunner.connectionToClient.connectionId;

            // Mark assigned
            assignedPlayers.Add(driver);
            assignedPlayers.Add(gunner);
        }

        List<LobbyPlayer> driverOnly = lobbyPlayers.Where(l => !assignedPlayers.Contains(l) && l.RolePreference == RolesPreference.OnlyDriver).ToList();
        List<LobbyPlayer> gunnerOnly = lobbyPlayers.Where(l => !assignedPlayers.Contains(l) && l.RolePreference == RolesPreference.OnlyGunner).ToList();
        List<LobbyPlayer> both = lobbyPlayers.Where(l => !assignedPlayers.Contains(l) && l.RolePreference == RolesPreference.Both).ToList();

        // Go through all left over lobby players where teams are randomized
        while(assignedPlayers.Count != lobbyPlayers.Count()) 
        {
            Team team = new Team();
            _teams.Add(team);

            List<LobbyPlayer> list;
            // Select driver
            if (driverOnly.Count > 0) list = driverOnly;
            else if (both.Count > 0) list = both;
            else list = gunnerOnly;
            LobbyPlayer player = list.Random();
            team.DriverClientId = player.connectionToClient.connectionId;
            list.Remove(player);
            assignedPlayers.Add(player);

            // Select gunner
            if (gunnerOnly.Count > 0) list = gunnerOnly;
            else if (both.Count > 0) list = both;
            else list = driverOnly;
            player = list.Random();
            team.GunnerClientId = player.connectionToClient.connectionId;
            list.Remove(player);
            assignedPlayers.Add(player);
        }
    }
}
