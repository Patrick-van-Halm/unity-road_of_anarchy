using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Mirror;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestShooting
{
    // A Test behaves as an ordinary method
    [Test]
    public void TestShootingInstantiatingBullet()
    {
        GameObject obj = new GameObject("car", typeof(Shooting), typeof(Camera));
        Shooting shooting = obj.GetComponent<Shooting>();
        Camera camera = obj.GetComponent<Camera>();

        // Set required variables
        shooting.weaponEndPointPosition = obj.transform;
        shooting.weapon = obj.transform;
        shooting.bulletSpeed = 100;
        shooting.bullet = new GameObject("testBullet");
        shooting.gunnerCamera = camera;

        // Execute instantiate bullet method
        shooting.GetType().GetMethod("InstantiateBullet", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(shooting, new object[] { });

        // Check if bullet is instantiated
        Assert.IsNotNull(GameObject.Find("testBullet(Clone)"));
    }

    [UnityTest]
    public IEnumerator TestBulletDestroy()
    {
        GameObject obj = new GameObject("car", typeof(Shooting), typeof(Camera));
        Shooting shooting = obj.GetComponent<Shooting>();
        Camera camera = obj.GetComponent<Camera>();

        // Set required variables
        shooting.weaponEndPointPosition = obj.transform;
        shooting.weapon = obj.transform;
        shooting.bulletSpeed = 100;
        shooting.bullet = new GameObject("testBullet");
        shooting.gunnerCamera = camera; 

        // Execute instantiate bullet method
        shooting.GetType().GetMethod("InstantiateBullet", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(shooting, new object[] { });

        // Wait six seconds to ensure that the bullet is destroyed
        yield return new WaitForSeconds(6);

        // Check if bullet is destroyed
        Assert.IsNull(GameObject.Find("testBullet(Clone)"));
    }
}
