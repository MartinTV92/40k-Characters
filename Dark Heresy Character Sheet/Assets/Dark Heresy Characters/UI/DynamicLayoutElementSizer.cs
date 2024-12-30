using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicLayoutElementSizer : Selectable
{
	public RectTransform rect2Match;
    public RectTransform elementHolder;
    public LayoutElement[] elements;

	protected override void OnValidate()
	{
		//Debug.Log("Resize");
		
		//Resize();
	}

	protected override void OnRectTransformDimensionsChange()
	{
		Resize();
	}


	private void Resize()
	{
		if(!rect2Match || !elementHolder)
			return;

		elements = GetComponentsInChildren<LayoutElement>();
		for (int i = 0; i < elements.Length; i++)
		{
			elements[i].preferredWidth = rect2Match.rect.width;
			elements[i].preferredHeight = rect2Match.rect.height;
		}
	}
}
