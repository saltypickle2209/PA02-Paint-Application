﻿using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Graphics
{
    /// <summary>
    /// A TextObject contains every attribute needed to create a TextBlock, which is associated to a ShapeObject
    /// in a parent-child relationship.
    /// </summary>
    public class TextObject : GraphicObject
    {
        // The ShapeObject this TextObject is associated to
        private ShapeObject _parent;

        public ShapeObject Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        private string _text;

        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        private SolidColorBrush _textColor;

        public SolidColorBrush TextColor
        {
            get { return _textColor; }
            set { _textColor = value; }
        }

        private int _textSize;

        public int TextSize
        {
            get { return _textSize; }
            set { _textSize = value; }
        }

        private FontFamily _textFont;

        public FontFamily TextFont
        {
            get { return _textFont; }
            set { _textFont = value; }
        }

        private SolidColorBrush _backgroundColor;

        public SolidColorBrush BackgroundColor
        {
            get { return _backgroundColor; }
            set { _backgroundColor = value; }
        }

        public TextObject(ShapeObject parent, string text, SolidColorBrush textColor, int textSize, FontFamily textFont, SolidColorBrush backgroundColor) : base()
        {
            _parent = parent;
            _text = text;
            _textColor = textColor;
            _textSize = textSize;
            _textFont = textFont;
            _backgroundColor = backgroundColor;
        }

        public override GraphicObject Clone()
        {
            GraphicObject graphicObject = (GraphicObject)this.MemberwiseClone();

            // Assign a new ID
            graphicObject.Id = Guid.NewGuid().ToString();

            return graphicObject;
        }

        public GraphicObject DeepClone(ShapeObject parent)
        {
            GraphicObject graphicObject = (GraphicObject)this.MemberwiseClone();

            ((TextObject)graphicObject).Parent = parent;
            graphicObject.Id = Guid.NewGuid().ToString();
            return graphicObject;
        }

        public override UIElement ConvertToUIElement()
        {
            double parentWidth = Math.Abs(_parent.StartingPoint.X - _parent.EndingPoint.X);
            double parentHeight = Math.Abs(_parent.StartingPoint.Y - _parent.EndingPoint.Y);

            if (_parent.IsPerfectShape)
                parentWidth = parentHeight = Math.Min(parentWidth, parentHeight);

            UIElement textBlock = new TextBlock
            {
                MaxWidth = parentWidth,
                MaxHeight = parentHeight,
                Text = _text,
                Foreground = _textColor,
                TextWrapping = TextWrapping.Wrap,
                TextAlignment = TextAlignment.Center,
                FontSize = _textSize,
                FontFamily = _textFont,
                Background = _backgroundColor,
                Tag = _id
            };

            textBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

            // Align the TextBlock to be in the center of its parent
            textBlock.SetValue(Canvas.LeftProperty, _parent.StartingPoint.X < _parent.EndingPoint.X ?
                _parent.StartingPoint.X + parentWidth / 2 - ((TextBlock)textBlock).DesiredSize.Width / 2 :
                _parent.StartingPoint.X - parentWidth / 2 - ((TextBlock)textBlock).DesiredSize.Width / 2);
            textBlock.SetValue(Canvas.TopProperty, _parent.StartingPoint.Y < _parent.EndingPoint.Y ?
                _parent.StartingPoint.Y + parentHeight / 2 - ((TextBlock)textBlock).DesiredSize.Height / 2 :
                _parent.StartingPoint.Y - parentHeight / 2 - ((TextBlock)textBlock).DesiredSize.Height / 2);

            return textBlock;
        }

        public override void UpdateUIElement(UIElement element)
        {
            if (element is TextBlock textBlock)
            {
                if ((string)textBlock.Tag != _id)
                    return;

                double parentWidth = Math.Abs(_parent.StartingPoint.X - _parent.EndingPoint.X);
                double parentHeight = Math.Abs(_parent.StartingPoint.Y - _parent.EndingPoint.Y);

                if (_parent.IsPerfectShape)
                    parentWidth = parentHeight = Math.Min(parentWidth, parentHeight);

                textBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

                textBlock.SetValue(Canvas.LeftProperty, _parent.StartingPoint.X < _parent.EndingPoint.X ?
                    _parent.StartingPoint.X + parentWidth / 2 - ((TextBlock)textBlock).DesiredSize.Width / 2 :
                _parent.StartingPoint.X - parentWidth / 2 - ((TextBlock)textBlock).DesiredSize.Width / 2);
                textBlock.SetValue(Canvas.TopProperty, _parent.StartingPoint.Y < _parent.EndingPoint.Y ?
                      _parent.StartingPoint.Y + parentHeight / 2 - ((TextBlock)textBlock).DesiredSize.Height / 2 :
                _parent.StartingPoint.Y - parentHeight / 2 - ((TextBlock)textBlock).DesiredSize.Height / 2);

            }
        }
    }
}
