using Graphics;

namespace LayerManager
{
	/// <summary>
	/// A Layer is where you store or remove GraphicObjects
	/// </summary>
    public class Layer
    {
		private List<GraphicObject> _graphicObjectList = new List<GraphicObject>();

		public List<GraphicObject> GraphicObjectList
		{
			get { return _graphicObjectList; }
			set { _graphicObjectList = value; }
		}

		private string _name = "";

		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}


		/// <summary>
		/// Add an item to the layer
		/// </summary>
		/// <param name="graphicObject">The GraphicObject to be added</param>
		public void AddItem(GraphicObject graphicObject)
		{
			GraphicObjectList.Add(graphicObject);
		}

		/// <summary>
		/// Add a list of item to the layer
		/// </summary>
		/// <param name="graphicObjects">The list of GraphicObject to be added</param>
		public void AddItems(List<GraphicObject> graphicObjects)
		{
			GraphicObjectList.AddRange(graphicObjects);
		}

		/// <summary>
		/// Remove an item which matches the given id from the layer
		/// </summary>
		/// <param name="id">The id of the item to be removed</param>
		public void RemoveItem(string id)
		{
			var foundObject = GraphicObjectList.FirstOrDefault(x => x.Id == id);
			
			if(foundObject != null)
			{
				GraphicObjectList.Remove(foundObject);
			}
		}

		/// <summary>
		/// Remove multiple items from the layer if they exist
		/// </summary>
		/// <param name="graphicObjects">The list of GraphicObject to be removed</param>
		public void RemoveItems(List<GraphicObject> graphicObjects)
		{
			graphicObjects.ForEach(graphicObject =>
			{
				if(GraphicObjectList.Contains(graphicObject))
					GraphicObjectList.Remove(graphicObject);
			});
		}
	}
}
