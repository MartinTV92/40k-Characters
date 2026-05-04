using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RegistrumPersonae
{
    public static class Bootstrap
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
			UIShell.Init();
        }
    }
}
