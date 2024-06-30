using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseAnimeSystem : SystemBase, IOnUpdate
{
    public override void SetUp()
    {
        gameStat.turnManger.enemyStartSetPhase += aa;
    }
    public void OnUpdate()
    {
        /*
        if(gameStat.isMySetPhase)
        {
            StartSetBlockPhase();
        }

        if(gameStat.isMyMovePhase)
        {
            StartMovePhase();
        }
        */
    }

    private void StartSetBlockPhase()
    {

    }
    private void StartMovePhase()
    {

    }
    private void PhaseEnd()
    {

    }
    private void aa()
    {
        Debug.Log("TurnManagerから呼ばれた");
        gameStat.isMySetPhase = true;
    }
}
