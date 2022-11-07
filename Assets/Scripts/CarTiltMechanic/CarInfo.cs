using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarInfo : MonoBehaviour
{
    [SerializeField] private Rigidbody _playerRB;
    [SerializeField] private NewKartScript _kartScript;
    [SerializeField] private LayerMask _environmentLayer;

    public bool WholeCarGrounded { get; private set; }
    public bool LastWholeCarGrounded { get; private set; }
    public bool CarWithoutWheelsGrounded { get; private set; }
    public bool AllWheelsGrounded { get; private set; }
    public bool LastAllWheelsGrounded { get; private set; }
    public bool SomeWheelsGrounded { get; private set; }
    public bool NoWheelsGrounded { get; private set; }

    /// <summary>
    /// Tracks if the car (wheels excluded) of player is colliding with the ground (environment) and when the player does, <br/>
    /// the boolean <b>_carWithoutWheelsGrounded</b> will be true
    /// </summary>
    /// <param name="collision">Any collision that the player is colliding with.</param>
    private void OnCollisionStay(Collision collision)
    {
        if (_environmentLayer == (1 << collision.gameObject.layer)) CarWithoutWheelsGrounded = true;
    }

    /// <summary>
    /// Tracks if the car (wheels excluded) of player stops colliding with the ground (environment) and when the player does, <br/>
    /// the boolean <b>_carWithoutWheelsGrounded</b> will be false
    /// </summary>
    /// <param name="collision">Any collision that the player is colliding with.</param>
    private void OnCollisionExit(Collision collision)
    {
        if (_environmentLayer == (1 << collision.gameObject.layer)) CarWithoutWheelsGrounded = false;
    }

    private void Update()
    {
        UpdateWheelVariables();
        CheckVerticlePosition();
    }

    /// <summary>
    /// Updates: <br/>
    /// - LastAllWheelsGrounded <br/>
    /// - AllWheelsGrounded <br/>
    /// - SomeWheelsGrounded <br/>
    /// - NoWheelsGrounded
    /// </summary>
    private void UpdateWheelVariables()
    {
        LastAllWheelsGrounded = AllWheelsGrounded;
        AllWheelsGrounded = WheelsGrounded(true, true);
        SomeWheelsGrounded = WheelsGrounded(false, true);
        NoWheelsGrounded = WheelsGrounded(false, false);
    }

    /// <summary>
    /// Checks every wheel if it is grounded or not.
    /// </summary>
    /// <param name="allWheelsGrounded">
    /// Set to <b>true</b> -> if <b>all</b> wheels need to be on the ground.<br/> 
    /// Set to <b>false</b> -> if <b>not all</b> wheels need to be on the ground. </param>
    /// <param name="someWheelsGrounded">
    /// <b>Warning</b>: Set <i>allWheelsGrounded</i> to false else this value does not matter. <br/> 
    /// Set to <b>true</b> -> if <b>some</b> wheels need to be on the ground. <br/>
    /// Set to <b>false</b> -> if <b>no</b> wheels need to be on the ground. </param>
    /// <returns>True or false depending on the parameters.</returns>
    private bool WheelsGrounded(bool allWheelsGrounded, bool someWheelsGrounded)
    {
        float _wheelsGroundedPercent = _kartScript.GroundPercent;

        // configure the return value for _noWheelsGrounded
        if (!allWheelsGrounded && !someWheelsGrounded && _wheelsGroundedPercent == 0) return true;
        if (!allWheelsGrounded && !someWheelsGrounded && _wheelsGroundedPercent > 0) return false;

        // configre the return value for _someWheelsGrounded
        if (!allWheelsGrounded && someWheelsGrounded && _wheelsGroundedPercent < 1 && _wheelsGroundedPercent > 0) return true;
        if (!allWheelsGrounded && someWheelsGrounded && _wheelsGroundedPercent == 1) return false;

        return _wheelsGroundedPercent == 1;
    }

    /// <summary>
    /// Checks wether the car is in the air or is grounded.
    /// </summary>
    private void CheckVerticlePosition()
    {
        LastWholeCarGrounded = WholeCarGrounded;
        if (!CarWithoutWheelsGrounded && NoWheelsGrounded && WholeCarGrounded)
        {
            WholeCarGrounded = false;
        }

        if ((CarWithoutWheelsGrounded || SomeWheelsGrounded || AllWheelsGrounded) && !WholeCarGrounded)
        {
            WholeCarGrounded = true;
        }
    }

    /// <summary>
    /// Checks wether the car is able to tilt in a cetain direction.
    /// </summary>
    /// <param name="direction">The direction the car tries to tilt.</param>
    /// <returns>If able <b>true</b> else <b>false</b></returns>
    public bool AbleToGroundTiltInDirection(float direction)
    {
        float _zEulerAngles = _playerRB.transform.localEulerAngles.z;
        if (0 <= _zEulerAngles && _zEulerAngles <= 180.5f && direction == -1) return true;
        if (179.5f <= _zEulerAngles && _zEulerAngles <= (360 - float.MinValue) && direction == 1) return true;
        return false;
    }
}
