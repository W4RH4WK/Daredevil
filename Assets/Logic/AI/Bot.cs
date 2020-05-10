using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    IEnumerator Straight(float time) { yield return Pitch(Vector3.zero, time); }

    IEnumerator PitchUp(float time) { yield return Pitch(Vector3.up, time); }
    IEnumerator PitchDown(float time) { yield return Pitch(Vector3.down, time); }
    IEnumerator PitchLeft(float time) { yield return Pitch(Vector3.left, time); }
    IEnumerator PitchRight(float time) { yield return Pitch(Vector3.right, time); }

    IEnumerator Pitch(Vector3 relativeDirection, float time)
    {
        for (; time > 0.0f; time -= Time.deltaTime)
        {
            FlightModel.Destination = transform.rotation * (10.0f * (Vector3.forward + relativeDirection)) + transform.position;
            yield return null;
        }
    }

    IEnumerator PullUp(float time) { yield return Pull(Vector3.up, time); }
    IEnumerator PullDown(float time) { yield return Pull(Vector3.down, time); }

    IEnumerator Pull(Vector3 absoluteDirection, float time)
    {
        for (; time > 0.0f; time -= Time.deltaTime)
        {
            FlightModel.Destination = (10.0f * (transform.forward + absoluteDirection)) + transform.position;
            yield return null;
        }
    }

    IEnumerator LevelPitch()
    {
        while (true)
        {
            FlightModel.Destination = 10.0f * transform.forward + transform.position;
            FlightModel.Destination.y = transform.position.y;

            var toDestination = FlightModel.Destination - transform.position;

            if (Vector3.Angle(transform.forward, toDestination) < 1.0f)
                break;

            yield return null;
        }
    }

    //////////////////////////////////////////////////////////////////////////

    delegate IEnumerator Maneuver(float time);

    IEnumerator BarrelRight(float time) { yield return Pitch(Vector3.up + Vector3.right, time); }

    //////////////////////////////////////////////////////////////////////////

    IEnumerator ChaseTarget(float time)
    {
        for (; time > 0.0f; time -= Time.deltaTime)
        {
            if (!CombatModel.Target)
                break;

            FlightModel.Destination = -15.0f * CombatModel.Target.transform.forward + CombatModel.Target.transform.position;

            yield return null;
        }
    }

    //////////////////////////////////////////////////////////////////////////

    IEnumerator Behaviour()
    {
        var combatManeuvers = new[] {
            Tuple.Create<float, Maneuver>(1.0f, PullUp),
            Tuple.Create<float, Maneuver>(3.0f, BarrelRight),
        };

        var targetTimer = 0.0f;

        while (true)
        {
            if (CombatModel.Target)
            {
                targetTimer += Time.deltaTime;
                if (targetTimer > 10.0f)
                {
                    CombatModel.FindTarget();
                    targetTimer = 0.0f;
                }

                FlightModel.AutoLevelRoll = false;

                yield return ChaseTarget(RandomRange(5.0f, 15.0f));
                yield return RandomElement(combatManeuvers)(RandomRange(2.0f, 12.0f));
            }
            else
            {
                FlightModel.AutoLevelRoll = true;

                yield return LevelPitch();
                yield return Straight(1.0f);
            }
        }
    }

    //////////////////////////////////////////////////////////////////////////

    BotFlightModel FlightModel;

    BotCombatModel CombatModel;

    Coroutine Routine;

    void Awake()
    {
        FlightModel = GetComponent<BotFlightModel>();
        Assert.IsNotNull(FlightModel);

        CombatModel = GetComponent<BotCombatModel>();
        Assert.IsNotNull(CombatModel);
    }

    void Start()
    {
        Routine = StartCoroutine(Behaviour());
    }

    static Func<float, float, float> RandomRange => UnityEngine.Random.Range;

    static T RandomElement<T>(IEnumerable<Tuple<float, T>> elementsWithWeight)
    {
        var weightSum = 0.0f;
        foreach (var elementWithWeight in elementsWithWeight)
            weightSum += elementWithWeight.Item1;

        var weightAccumulator = 0.0f;
        var selection = UnityEngine.Random.Range(0.0f, weightSum);

        foreach (var elementWithWeight in elementsWithWeight)
        {
            weightAccumulator += elementWithWeight.Item1;
            if (selection < weightAccumulator)
                return elementWithWeight.Item2;
        }

        return default(T);
    }
}
