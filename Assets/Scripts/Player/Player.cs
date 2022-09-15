using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerAttributeComponent))]
[RequireComponent(typeof(PlayerHUDComponent))]
public class Player : MonoBehaviour
{

#region Player Components
    private PlayerAttributeComponent PlayerAttributes { get; set; }
    private PlayerHUDComponent HudComponent {get; set; }
#endregion

    private void Awake()
    {
        PlayerAttributes = GetComponent<PlayerAttributeComponent>();
        HudComponent = GetComponent<PlayerHUDComponent>();

        if (PlayerAttributes is null)
            PlayerAttributes = gameObject.AddComponent<PlayerAttributeComponent>();

        if (HudComponent is null)
            HudComponent = gameObject.AddComponent<PlayerHUDComponent>();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            PlayerAttributes.ApplyDamage(10f);
        }
    }
}
