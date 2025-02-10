using System;

namespace JollyRoger.DarkHeresy
{ 
    public interface IUpdateable
    {
        event Action OnUpdate;
    }
}