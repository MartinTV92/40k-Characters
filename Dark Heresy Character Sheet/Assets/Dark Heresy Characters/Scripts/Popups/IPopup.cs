using System;

namespace JollyRoger.DarkHeresy
{   
    public interface IPopup
    {
        void Open<T>(T target);
        void Close();
        Type GetPopupType();
    }
}