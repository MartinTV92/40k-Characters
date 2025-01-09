using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TS.PageSlider;
using JollyRoger.UI.Utilities;

namespace JollyRoger.DarkHeresy.Utilities
{ 

    public class PageSliderSetup : MonoBehaviour
    {
		#region----- VARIABLES -----

		public PageSlider slider;
		public PageScroller scroller;
		public ScrollRect[] scrollRects;
		public PageContainer[] containers;
		public DragEventPropagator dragEventPropegator;

		#endregion


		#region----- MONOBEHAVIOUR ----

		private void Awake()
		{
			FindReferences();
			SetupPages();
		}

		void OnEnable()
        {
			//Subscribe();
        }

		private void OnDisable()
		{
			//Unsubscribe();
		}

		private void OnValidate()
		{
			FindReferences();
			SetupPages();
		}

		#endregion


		#region----- CUSTOM BEHAVIORUS -----


		void FindReferences()
		{
			slider = GetComponent<PageSlider>();
			scroller = GetComponentInChildren<PageScroller>();
			scrollRects = scroller.GetComponentsInChildren<ScrollRect>();
			containers = scroller.GetComponentsInChildren<PageContainer>();
			dragEventPropegator = GetComponentInChildren<DragEventPropagator>();
		}

		void SetupPages()
		{
			if(slider == null)
				return;

			slider._pages = new ();
			for(int i = 0; i < containers.Length; i++)
				slider._pages.Add(containers[i]);
		}

		void Subscribe()
		{
			dragEventPropegator.Subscribe(scroller);
			foreach(var scroll in scrollRects)
				dragEventPropegator.Subscribe(scroll);
		}

		void Unsubscribe()
		{
			dragEventPropegator.Unsubscribe(scroller);
			foreach (var scroll in scrollRects)
				dragEventPropegator.Unsubscribe(scroll);
		}

		#endregion

	}
}