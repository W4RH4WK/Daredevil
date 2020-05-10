using UnityEngine;
using UnityEngine.Assertions;

public class Bot : MonoBehaviour
{
    void ChaseTarget()
    {
        FlightModel.Destination = -15.0f * CombatModel.Target.transform.forward + CombatModel.Target.transform.position;
        //Debug.Log("Chasing");
    }

    void Idle()
    {
        FlightModel.Destination = 100.0f * transform.forward + transform.position;
        //Debug.Log("Idling");
    }

    //////////////////////////////////////////////////////////////////////////

    BotFlightModel FlightModel;

    BotCombatModel CombatModel;

    void Awake()
    {
        FlightModel = GetComponent<BotFlightModel>();
        Assert.IsNotNull(FlightModel);

        CombatModel = GetComponent<BotCombatModel>();
        Assert.IsNotNull(CombatModel);
    }

    void Update()
    {
        if (CombatModel.Target)
            ChaseTarget();
        //else
        //    Idle();
    }
}
