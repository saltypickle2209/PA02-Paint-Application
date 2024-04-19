using System.Collections.ObjectModel;

namespace LayerManager
{
	/// <summary>
	/// A LayerList contains a list of Layer objects, together with functions to add/delete layers.
	/// </summary>
    public class LayerList
    {
		private ObservableCollection<Layer> _layers;

		public ObservableCollection<Layer> Layers
		{
			get { return _layers; }
			set { _layers = value; }
		}

		private int _currentLayerIndex;

		public int CurrentLayerIndex
		{
			get { return _currentLayerIndex; }
			set { _currentLayerIndex = value; }
		}


		public LayerList()
		{
			_layers = new ObservableCollection<Layer> ();
			_currentLayerIndex = -1;
		}

        /// <summary>
        /// Get the layer at the current layer index
        /// </summary>
        /// <returns>The Layer object if there exists a Layer in the list, null otherwise</returns>
        public Layer? GetCurrentLayer()
		{
			if(_currentLayerIndex != -1)
			{
				return _layers[_currentLayerIndex];
			}
			return null;
		}

		/// <summary>
		/// Get the layer at a specific index
		/// </summary>
		/// <param name="index">The index of the layer to be retrieved</param>
		/// <returns>The Layer object if there exists a Layer at the given index, null otherwise</returns>
		public Layer? GetLayerAtIndex(int index)
		{
			if(index >= 0 && index < _layers.Count)
			{
				return _layers[index];
			}
			return null;
		}

		/// <summary>
		/// Add a new layer to the list
		/// </summary>
		/// <param name="layer">The layer to be added</param>
		public void AddLayer(Layer layer)
		{
			_layers.Add(layer);

			// After adding a new layer, immediately move to this layer
			_currentLayerIndex = _layers.Count - 1;
            layer.Name = $"Layer {_currentLayerIndex + 1}";
        }

		/// <summary>
		/// Add a new layer at a specific index
		/// </summary>
		/// <param name="layer">The layer to be added</param>
		/// <param name="index">The index in the list where that layer should be placed</param>
		public void AddLayerAtIndex(Layer layer, int index)
		{
			_layers.Insert(index, layer);

			if (_currentLayerIndex == -1)
				_currentLayerIndex = 0;
			else if (_currentLayerIndex >= index)
				_currentLayerIndex++;
		}

		/// <summary>
		/// Remove a layer at a specific index from the list
		/// </summary>
		/// <param name="index">The index of the layer to be deleted</param>
		public void RemoveLayer(int index)
		{
			if(index >= 0 && index < _layers.Count)
			{
				_layers.RemoveAt(index);

				if (_layers.Count == 0)
					_currentLayerIndex = -1;
				else if (_currentLayerIndex >= index)
					_currentLayerIndex--;
			}
		}
	}
}
