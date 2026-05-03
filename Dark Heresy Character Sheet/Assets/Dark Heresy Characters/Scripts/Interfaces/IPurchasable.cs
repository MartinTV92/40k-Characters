using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RegistrumPersonae
{ 

    public interface IPurchasable
    {
        string Name { get; }
        int XP { get; }
        Prerequisite[] Prerequisites { get; }

        bool CanBuy();
        void Buy();
        void Refund();
    }
}