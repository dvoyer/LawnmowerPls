using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LawnmowerPls
{
    public class CanvasPanel
    {
        private GameObject canvas;
        private Vector2 position;
        private Vector2 size;
        private Dictionary<string, CanvasPanel> panels = new Dictionary<string, CanvasPanel>();
        private Dictionary<string, CanvasText> texts = new Dictionary<string, CanvasText>();

        public bool active;

        public CanvasPanel(GameObject parent, Vector2 pos, Vector2 sz)
        {
            if (parent == null) return;

            position = pos;
            size = sz;
            canvas = parent;
            active = true;
        }

        public void AddPanel(string name, Vector2 pos, Vector2 sz)
        {
            CanvasPanel panel = new CanvasPanel(canvas, position + pos, sz);

            panels.Add(name, panel);
        }

        public void AddText(string name, string text, Vector2 pos, Vector2 sz, Font font, int fontSize = 13, FontStyle style = FontStyle.Normal, TextAnchor alignment = TextAnchor.UpperLeft)
        {
            CanvasText t = new CanvasText(canvas, position + pos, sz, font, text, fontSize, style, alignment);

            texts.Add(name, t);
        }

        public CanvasPanel GetPanel(string panelName)
        {
            if (panels.ContainsKey(panelName))
            {
                return panels[panelName];
            }

            return null;
        }

        public CanvasText GetText(string textName, string panelName = null)
        {
            if (panelName != null && panels.ContainsKey(panelName))
            {
                return panels[panelName].GetText(textName);
            }

            if (texts.ContainsKey(textName))
            {
                return texts[textName];
            }

            return null;
        }



        public void SetPosition(Vector2 pos)
        {

            Vector2 deltaPos = position - pos;
            position = pos;

            foreach (CanvasText text in texts.Values)
            {
                text.SetPosition(text.GetPosition() - deltaPos);
            }

            foreach (CanvasPanel panel in panels.Values)
            {
                panel.SetPosition(panel.GetPosition() - deltaPos);
            }
        }

        public void TogglePanel(string name)
        {
            if (active && panels.ContainsKey(name))
            {
                panels[name].ToggleActive();
            }
        }

        public void ToggleActive()
        {
            active = !active;
            SetActive(active, false);
        }

        public void SetActive(bool b, bool panel)
        {
            foreach (CanvasText t in texts.Values)
            {
                t.SetActive(b);
            }

            if (panel)
            {
                foreach (CanvasPanel p in panels.Values)
                {
                    p.SetActive(b, false);
                }
            }

            active = b;
        }

        public Vector2 GetPosition()
        {
            return position;
        }

        public void FixRenderOrder()
        {
            foreach (CanvasText t in texts.Values)
            {
                t.MoveToTop();
            }
            foreach (CanvasPanel panel in panels.Values)
            {
                panel.FixRenderOrder();
            }
        }

        public void Destroy()
        {
            foreach (CanvasText t in texts.Values)
            {
                t.Destroy();
            }

            foreach (CanvasPanel p in panels.Values)
            {
                p.Destroy();
            }
        }
    }
}
