using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerAttributeComponent))]
public class Player : MonoBehaviour
{

#region Player Components
    private PlayerAttributeComponent PlayerAttributes { get; set; }
    public PlayerHUDComponent HudComponent {get; set; }
#endregion

    private void Awake()
    {
        PlayerAttributes = GetComponent<PlayerAttributeComponent>();

        if (PlayerAttributes is null)
            PlayerAttributes = gameObject.AddComponent<PlayerAttributeComponent>();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            PlayerAttributes.CmdApplyDamage(10f);
        }
    }
}
