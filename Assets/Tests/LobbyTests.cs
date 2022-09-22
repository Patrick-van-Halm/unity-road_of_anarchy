using System.Collections;
using System.Collections.Generic;
using Mirror;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Reflection;
using UnityEditor;

public class LobbyTests
{
    private GameObject lobbyPlayerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/LobbyPlayer.prefab");

    [TearDown]
    public void TearDown()
    {
        // Cleanup all objects in scene
        foreach(GameObject gameObject in Object.FindObjectsOfType<GameObject>())
        {
            Object.DestroyImmediate(gameObject);
        }
    }

    [Test]
    public void TestLobbyDoesNotStartIfLessThanFourPlayers()
    {
        // Create instances
        GameObject obj = new GameObject("Lobby", typeof(NetworkIdentity), typeof(Lobby));

        // Set required variables
        Lobby lobby = obj.GetComponent<Lobby>();
        lobby.LobbyPlayerPrefab = lobbyPlayerPrefab;

        // Create lobby player,
        GameObject instance = lobby.GetType().GetMethod("CreateLobbyPlayer", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(lobby) as GameObject;

        // Mark ready
        LobbyPlayer lobbyPlayer = instance.GetComponent<LobbyPlayer>();
        lobbyPlayer.IsReady = true;

        // Run lobby start
        bool isStarted = lobby.StartLobby();

        // check if result is true
        Assert.IsFalse(isStarted);
    }

    [Test]
    public void TestLobbyStartIfFourPlayersAndEveryoneIsReady()
    {
        // Create instances
        GameObject obj = new GameObject("Lobby", typeof(NetworkIdentity), typeof(Lobby));

        // Set required variables
        Lobby lobby = obj.GetComponent<Lobby>();
        lobby.LobbyPlayerPrefab = lobbyPlayerPrefab;
        lobby.GameScenes.Add("TestScene");

        // Create 4 lobby players that are ready
        for (int i = 0; i <= 4; i++)
        {
            // Create lobby player,
            GameObject instance = lobby.GetType().GetMethod("CreateLobbyPlayer", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(lobby) as GameObject;

            // Mark ready
            LobbyPlayer lobbyPlayer = instance.GetComponent<LobbyPlayer>();
            lobbyPlayer.IsReady = true;
        }
        
        // Run lobby start
        bool isStarted = lobby.StartLobby();

        // check if result is true
        Assert.IsTrue(isStarted);
    }

    [Test]
    public void TestLobbyDoesNotStartIfFourPlayersAndNotEveryoneIsReady()
    {
        // Create instances
        GameObject obj = new GameObject("Lobby", typeof(NetworkIdentity), typeof(Lobby));

        // Set required variables
        Lobby lobby = obj.GetComponent<Lobby>();
        lobby.LobbyPlayerPrefab = lobbyPlayerPrefab;
        lobby.GameScenes.Add("TestScene");

        // Create 4 lobby players that are not ready
        for (int i = 0; i <= 4; i++)
        {
            // Create lobby player,
            lobby.GetType().GetMethod("CreateLobbyPlayer", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(lobby);
        }

        // Run lobby start
        bool isStarted = lobby.StartLobby();

        // check if result is false
        Assert.IsFalse(isStarted);
    }

    [Test]
    public void TestLobbyDoesNotStartIfNoGameScenesAreProvided()
    {
        // Create instances
        GameObject obj = new GameObject("Lobby", typeof(NetworkIdentity), typeof(Lobby));

        // Set required variables
        Lobby lobby = obj.GetComponent<Lobby>();
        lobby.LobbyPlayerPrefab = lobbyPlayerPrefab;

        // Create 4 lobby players that are ready
        for (int i = 0; i < 4; i++)
        {
            // Create lobby player,
            GameObject instance = lobby.GetType().GetMethod("CreateLobbyPlayer", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(lobby) as GameObject;

            // Mark ready
            LobbyPlayer lobbyPlayer = instance.GetComponent<LobbyPlayer>();
            lobbyPlayer.IsReady = true;
        }

        // Run lobby start
        bool isStarted = lobby.StartLobby();

        // check if result is false
        Assert.IsFalse(isStarted);
    }

    [Test]
    public void TestLobbyPlayerWillBeSpawned()
    {
        // Create lobby
        GameObject obj = new GameObject("Lobby", typeof(NetworkIdentity), typeof(Lobby));

        Lobby lobby = obj.GetComponent<Lobby>();
        lobby.LobbyPlayerPrefab = lobbyPlayerPrefab;

        // Create lobby player,
        GameObject instance = lobby.GetType().GetMethod("CreateLobbyPlayer", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(lobby) as GameObject;

        // Check if created
        Assert.NotNull(instance);
        Assert.NotNull(GameObject.Find("LobbyPlayer(Clone)"));
    }
}
