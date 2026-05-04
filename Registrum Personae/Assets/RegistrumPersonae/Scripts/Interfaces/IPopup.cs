using System;

namespace RegistrumPersonae
{   
	public interface IPopup
	{
	    void Open<T>(T target);
	    void Close();
	    Type GetPopupType();
	}
}

