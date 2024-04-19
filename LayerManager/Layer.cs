using Graphics;
using System.Windows;

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

		/// <summary>
		/// Find a GraphicObject whose bounds contain a given point
		/// </summary>
		/// <param name="point">The 2D point used to find an item</param>
		/// <returns>A single GraphicObject</returns>
		public GraphicObject? FindItem(Point point)
		{
			GraphicObject? foundObject = null;
			double lowestXYDiff = double.PositiveInfinity;

			foreach(GraphicObject graphicObject in _graphicObjectList)
			{
				if(graphicObject is ShapeObject shapeObject)
				{
					double minX = Math.Min(shapeObject.StartingPoint.X, shapeObject.EndingPoint.X);
                    double minY = Math.Min(shapeObject.StartingPoint.Y, shapeObject.EndingPoint.Y);
                    double maxX = Math.Max(shapeObject.StartingPoint.X, shapeObject.EndingPoint.X);
                    double maxY = Math.Max(shapeObject.StartingPoint.Y, shapeObject.EndingPoint.Y);

					if (point.X >= minX && point.X <= maxX && point.Y >= minY && point.Y <= maxY)
					{
						double xYDiff = Math.Abs(Math.Abs((maxX - point.X) - (point.X - minX)) - Math.Abs((maxY - point.Y) - (point.Y - minY)));

                        if (xYDiff < lowestXYDiff)
						{
							foundObject = graphicObject;
							lowestXYDiff = xYDiff;
						}
					}
                }
			}
			return foundObject;
		}

        /// <summary>
        /// Find a list of GraphicObjects that are in the rectangle bound drawn by 2 given point
        /// </summary>
        /// <param name="startingPoint">The starting point of the bound</param>
        /// <param name="endingPoint">The ending point of the bound</param>
        /// <returns>A list of GraphicObjects</returns>
        public List<GraphicObject> FindItemInRange(Point startingPoint, Point endingPoint)
		{
			double selectionMinX = Math.Min(startingPoint.X, endingPoint.X);
            double selectionMinY = Math.Min(startingPoint.Y, endingPoint.Y);
            double selectionMaxX = Math.Max(startingPoint.X, endingPoint.X);
            double selectionMaxY = Math.Max(startingPoint.Y, endingPoint.Y);

            List<GraphicObject> graphicObjects = new List<GraphicObject>();

			foreach (GraphicObject graphicObject in _graphicObjectList)
			{
				if (graphicObject is ShapeObject shapeObject)
                {
                    double minX = Math.Min(shapeObject.StartingPoint.X, shapeObject.EndingPoint.X);
                    double minY = Math.Min(shapeObject.StartingPoint.Y, shapeObject.EndingPoint.Y);
                    double maxX = Math.Max(shapeObject.StartingPoint.X, shapeObject.EndingPoint.X);
                    double maxY = Math.Max(shapeObject.StartingPoint.Y, shapeObject.EndingPoint.Y);

					bool xOverlap = selectionMinX < maxX && selectionMaxX > minX;
					bool yOverlap = selectionMinY < maxY && selectionMaxY > minY;

					if(xOverlap && yOverlap)
					{
						graphicObjects.Add(graphicObject);
					}
                }
				// Supposing ShapeObject must be drawn first before a TextObject associated to that ShapeObject
				// is created
				else if (graphicObject is TextObject textObject)
				{
					if (graphicObjects.Contains(textObject.Parent))
					{
						graphicObjects.Add(graphicObject);
					}
				}
            }

			return graphicObjects;
		}
	}
}
