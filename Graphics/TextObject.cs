﻿using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Graphics
{
    public class TextObject : GraphicObject
    {
        // the ShapeObject this TextObject is associated to
        private ShapeObject _parent;
        private string _text;
        private SolidColorBrush _textColor;
        private int _textSize;
        private string _textFont;
        private SolidColorBrush _backgroundColor;

        public TextObject(ShapeObject parent, string text, SolidColorBrush textColor, int textSize, string textFont, SolidColorBrush backgroundColor) : base() 
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

        public override UIElement ConvertToUIElement()
        {
            double parentWidth = Math.Abs(_parent.StartingPoint.X - _parent.EndingPoint.X);
            double parentHeight = Math.Abs(_parent.StartingPoint.Y - _parent.EndingPoint.Y);

            if (_parent.IsPerfectShape)
                parentWidth = parentHeight = Math.Min(parentWidth, parentHeight);

            UIElement textBlock = new TextBlock
            {
                Width = parentWidth,
                Text = _text,
                Foreground = _textColor,
                TextWrapping = TextWrapping.Wrap,
                TextAlignment = TextAlignment.Center,
                FontSize = _textSize,
                FontFamily = new FontFamily("Arial"),
                Background = _backgroundColor,
                Tag = _id
            };

            textBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

            // Align the TextBlock to be in the center of its parent
            textBlock.SetValue(Canvas.LeftProperty, _parent.StartingPoint.X < _parent.EndingPoint.X ?
                _parent.StartingPoint.X + parentWidth / 2 - ((TextBlock)textBlock).Width / 2 :
                _parent.StartingPoint.X - parentWidth / 2 + ((TextBlock)textBlock).Width / 2);
            textBlock.SetValue(Canvas.TopProperty, _parent.StartingPoint.Y < _parent.EndingPoint.Y ? 
                _parent.StartingPoint.Y + parentHeight / 2 - ((TextBlock)textBlock).DesiredSize.Height / 2 :
                _parent.StartingPoint.Y - parentHeight / 2 + ((TextBlock)textBlock).DesiredSize.Height / 2);
            
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

                textBlock.Width = parentWidth;

                textBlock.SetValue(Canvas.LeftProperty, _parent.StartingPoint.X < _parent.EndingPoint.X ?
                    _parent.StartingPoint.X + parentWidth / 2 - textBlock.Width / 2 :
                    _parent.StartingPoint.X - parentWidth / 2 + textBlock.Width / 2);
                textBlock.SetValue(Canvas.TopProperty, _parent.StartingPoint.Y < _parent.EndingPoint.Y ?
                    _parent.StartingPoint.Y + parentHeight / 2 - textBlock.Height / 2 :
                    _parent.StartingPoint.Y - parentHeight / 2 + textBlock.Height / 2);
            }
        }
    }
}