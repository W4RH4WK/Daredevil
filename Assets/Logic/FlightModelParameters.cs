using UnityEngine;

public class FlightModelParameters : MonoBehaviour
{
    [Header("Inertia")]

    public Vector3 PitchYawRollRate = new Vector3(30.0f, 8.0f, 100.0f);

    public Vector3 PitchYawRollResponseRate = new Vector3(10.0f, 10.0f, 10.0f);

    public Vector3 PitchYawRollResponseMaxSpeed = new Vector3(120.0f, 20.0f, 160.0f);

    public float PitchUpRateModifier = 1.25f;

    [Header("Modes")]

    public float FocusPitchYawRollRate = 25.0f;

    public float FocusPitchYawRollResponseMaxSpeed = 100.0f;

    public float StrafeVelocityVectorResponseRate = 0.2f;

    public float StrafePitchYawRollRateModifier = 1.8f;

    public float HighGTurnPitchRateModifier = 2.0f;

    public float HighGTurnDecelerationModifier = 1.5f;

    [Header("Speed")]

    public float VelocityVectorResponseRate = 5.0f;

    public float BaseThrust = 32.0f;

    public float ThrustResponseRate = 0.45f;

    public float Mobility = 1.5f;

    public float MaxSpeed = 80.0f;

    public float Acceleration = 45.0f;

    public float Deceleration = 15.0f;

    public float FlightGravity = 10.0f;

    [Header("Stalling")]

    public float StallingAttackSpeed = 25.0f;

    public float StallingReleaseSpeed = 30.0f;

    public float StallingMinimumDuration = 1.5f;

    public float StallingRotationRate = 0.7f;

    public float StallingGravity = 15.0f;

    public float StallingThrustCutFactor = 0.5f;

    [Header("Banking")]

    public float BankingDriftRate = 2.0f;

    public float BankingLiftLossRate = 2.0f;
}
