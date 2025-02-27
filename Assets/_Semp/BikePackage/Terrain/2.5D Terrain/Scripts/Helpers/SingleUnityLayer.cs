﻿// Thanks to: https://answers.unity.com/questions/1694073/select-only-one-layer-in-the-inspectorselect-only.html

using UnityEngine;

namespace Kamgam.Terrain25DLib.Helpers
{
	[System.Serializable]
	public class SingleUnityLayer
	{
		[SerializeField]
		private int m_LayerIndex = 0;
		public int LayerIndex
		{
			get { return m_LayerIndex; }
		}

		public void Set(int _layerIndex)
		{
			if (_layerIndex > 0 && _layerIndex < 32)
			{
				m_LayerIndex = _layerIndex;
			}
		}

		public int Mask
		{
			get { return 1 << m_LayerIndex; }
		}
	}
}
