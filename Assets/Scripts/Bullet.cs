using System.Collections;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int bulletSpeed = 10;

    private EventInstance bulletInstance;
    [SerializeField] private EventReference bulletFlySound;

    IEnumerator Start() 
    {
        if (!bulletFlySound.IsNull)
        {
            bulletInstance = RuntimeManager.CreateInstance(bulletFlySound);
            RuntimeManager.AttachInstanceToGameObject(bulletInstance, gameObject.transform);
            bulletInstance.start();
        }

        yield return new WaitForSeconds(3);

        // Stop the bullet flying sound before destroying it
        bulletInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

        Destroy(gameObject);
    }

    void Update()
    {
        // Add movement to bullet
        transform.position += transform.forward * bulletSpeed * Time.deltaTime;
    }

    private void OnDisable()
    {
        bulletInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }
}
