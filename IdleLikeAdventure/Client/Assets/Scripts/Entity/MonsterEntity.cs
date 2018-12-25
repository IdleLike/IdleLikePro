using Entity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEntity : BaseEntity
{
    private uint offerEXP;

    /// <summary>
    /// 杀死所提供经验
    /// </summary>
    public uint OfferEXP
    {
        get
        {
            return offerEXP;
        }

        set
        {
            offerEXP = value;
        }
    }
}
