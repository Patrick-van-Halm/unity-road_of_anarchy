using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public static class UnityWebRequestExtensions
{
    public static IEnumerator ProcessRequest(this UnityWebRequest request, UnityAction<UnityWebRequest> callback = null)
    {
        yield return request.SendWebRequest();
        if(callback != null) callback(request);
    }
}
