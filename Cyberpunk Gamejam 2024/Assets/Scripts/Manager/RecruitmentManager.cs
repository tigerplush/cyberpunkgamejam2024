using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class facilitates recruiting with the party manager.
/// </summary>
public class RecruitmentManager : MonoBehaviour
{
    [SerializeField]
    private Manager _partyManager;

    [SerializeField]
    private Currency _currency;

    public void RecruitOffensive(Recruit recruit)
    {
        if(!_partyManager.CanAddOffensive)
        {
            return;
        }
        if(_currency.Credits < recruit.Cost)
        {
            return;
        }
        _currency.Credits -= recruit.Cost;
        _partyManager.AddOffensiveCharacter(recruit.Element);
        recruit.Consume();
    }

    public void RecruitDefensive(Recruit recruit)
    {
        if (!_partyManager.CanAddDefensive)
        {
            return;
        }
        if (_currency.Credits < recruit.Cost)
        {
            return;
        }
        _currency.Credits -= recruit.Cost;
        _partyManager.AddDefensiveCharacter(recruit.Element);
        recruit.Consume();
    }

    public void RecruitBench(Recruit recruit)
    {
        if (!_partyManager.CanAddBench)
        {
            return;
        }
        if (_currency.Credits < recruit.Cost)
        {
            return;
        }
        _currency.Credits -= recruit.Cost;
        _partyManager.AddBenchedCharacter(recruit.Element);
        recruit.Consume();
    }
}
