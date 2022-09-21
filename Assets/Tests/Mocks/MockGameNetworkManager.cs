using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MockGameNetworkManager : GameNetworkManager
{
    public new bool ServerChangeScene(string scene)
    {
        return true;
    }
}
