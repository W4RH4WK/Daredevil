using UnityEngine;

public class FlightModelParameters : MonoBehaviour
{
    public float BaseResponseRate = 2.8f;

    public float RotationalResponseRate = 10.0f;

    public float ThrustResponseRate = 0.45f;

    // Determines impact of speed on rotational response rate.
    public float Mobility = 2.8f;

    //////////////////////////////////////////////////////////////////////////

    public float StrafeResponseRate = 0.35f;

    public float StrafeRotationalResponseRateModifier = 1.8f;

    public float HighGTurnPitchRateModifier = 2.0f;

    public float HighGTurnDecelerationModifier = 1.5f;

    //////////////////////////////////////////////////////////////////////////

    public float StallingAttackSpeed = 20.0f;

    public float StallingReleaseSpeed = 25.0f;

    public float StallingMinimumDuration = 1.5f;

    public float StallingRotationRate = 0.7f;

    public float StallingGravity = 15.0f;

    public float StallingThrustCutFactor = 0.5f;

    //////////////////////////////////////////////////////////////////////////

    public float PitchUpRate = 65.0f;

    public float PitchDownRate = 45.0f;

    public float RollRate = 120.0f;

    public float YawRate = 12.0f;

    public float FocusModeRotationRate = 35.0f;

    //////////////////////////////////////////////////////////////////////////

    public float BaseThrust = 32.0f;

    public float MaxSpeed = 80.0f;

    public float Acceleration = 45.0f;

    public float Deceleration = 15.0f;

    public float FlightGravity = 10.0f;

    public float BankingDriftRate = 10.0f;

    public float BankingLiftLossRate = 8.0f;

    public float BankingLiftLossSpeedImpact = 1.0f;
}
