﻿using GameEngine.Event;
using GameEngine.Resource;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.GUI
{
    public abstract class Widget : Transformable, Drawable
    {
        public enum WidgetState
        {
            notActive = 0,
            active,
            selected,
            warning
        };                                                                     // final
        public enum WidgetSize
        {
            Narrow = 0,
            Small,
            Wide
        };                                                                      // final

        public Widget(WidgetSize size)
        {
            #region Subscribing

            Program.window.MouseMoved += Window_MouseMoved;
            Program.window.MouseButtonPressed += Window_MouseButtonPressed;
            Program.window.MouseButtonReleased += Window_MouseButtonReleased;
            Program.window.KeyPressed += Window_KeyPressed;
            Program.window.TextEntered += Window_TextEntered;

            #endregion



            state = WidgetState.active;
            gloabalMousePos = new Vector2f(0, 0);
            localMousePos = new Vector2f(0, 0);
            rect = new RectangleShape();
            text = new Text();
            isActive = true;
            isClicked = false;
            isEntered = false; ;
            outlineThickness = -2;

            rect.Size = new Vector2f(dimensions[(int)size], 40);
            rect.FillColor = new Color(52, 152, 219);
            rect.OutlineColor = Color.Black;
            rect.OutlineThickness = outlineThickness;

            text.CharacterSize = 25;
            text.Color = Color.White;
            text.Font = ResourceHolder.Fonts.Get("minecraft");
        }

        public string Text
        {
            get => text.DisplayedString;
            set
            {
                text.DisplayedString = value;
                UpdateText();
            }
        }
        public virtual Texture Texture
        {
            get => rect.Texture;
            set
            {
                Reset();
                rect.Texture = value;
                UpdateState();
            }
        }
        //public abstract int Value { get; set; }
        public bool IsActive
        {
            get => isActive;
            set
            {
                isActive = value;
                state = isActive ? WidgetState.active : WidgetState.notActive;
                UpdateState();
            }
        }
        public Vector2f Size
        {
            get         => rect.Size; 
            private set => rect.Size = value;
        }

        public void Update()
        {
            UpdateState();
        }

        public FloatRect GetLocalBounds()
        {
            return rect.GetLocalBounds();
        }                                                           // final
        public FloatRect GetGlobalBounds()
        {
            return Transform.TransformRect(GetLocalBounds());
        }                                                          // final

        public void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform *= Transform;
            localMousePos = states.Transform.GetInverse().TransformPoint(gloabalMousePos);

            target.Draw(rect, states);
            DrawResource(target, states);
            target.Draw(text, states);
        }

        protected void DrawResource(RenderTarget target, RenderStates states)
        {
        }

        protected virtual void Window_MouseMoved(object sender, MouseMoveEventArgs e)
        {
            gloabalMousePos = new Vector2f(e.X, e.Y);
            isEntered = rect.GetGlobalBounds().Contains(localMousePos.X, localMousePos.Y);
        }
        protected virtual void Window_MouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
        }
        protected virtual void Window_MouseButtonReleased(object sender, MouseButtonEventArgs e)
        {
        }
        protected virtual void Window_KeyPressed(object sender, KeyEventArgs e)
        {
        }
        protected virtual void Window_TextEntered(object sender, TextEventArgs e)
        {
        }

        protected virtual void Reset()
        {
            rect.FillColor = Color.White;
            //rect.OutlineColor = Color.Transparent;
            //rect.OutlineThickness = 0;
        }
        protected virtual void UpdateState()
        {
            switch (state)
            {
                case WidgetState.notActive:     NotActiveState();   break;
                case WidgetState.active:        ActiveState();      break;
                case WidgetState.selected:      SelectedState();    break;
                case WidgetState.warning:       WarningState();     break;
            }
        }
        protected abstract void UpdateText();

        protected virtual void NotActiveState()
        {
            throw new NotImplementedException();
        }
        protected virtual void ActiveState()
        {
            throw new NotImplementedException();
        }
        protected virtual void SelectedState()
        {
            throw new NotImplementedException();
        }
        protected virtual void WarningState()
        {
            throw new NotImplementedException();
        }


        public abstract event EventHandler<WidgetEventArgs>  WidgetEvent;

        protected Vector2f          localMousePos;                                                     // final
        protected Vector2f          gloabalMousePos;                                                   // final
        protected RectangleShape    rect;                                                              // final
        protected Text              text;                                                              // final

        protected WidgetState       state;
        //public event EventHandler                   MouseEntered;
        //public event EventHandler                   MouseLeft;

        protected bool isActive;
        protected bool isClicked;
        protected bool isEntered;

        readonly float[] dimensions = new float[]
        {
            88f,
            192f,
            400f
        };

        protected float outlineThickness;

    }
}
