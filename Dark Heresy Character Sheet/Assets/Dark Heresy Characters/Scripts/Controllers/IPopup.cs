using System;

namespace SunJack.DarkHeresy
{   
    public interface IPopup
    {
        void Open<T>(T target);
        void Close();
        Type GetPopupType();
    }
}