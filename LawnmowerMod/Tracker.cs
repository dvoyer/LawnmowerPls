using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Modding;

namespace LawnmowerPls
{
    public static class Tracker
    {
        private static CanvasPanel panel;
        private static Font fontType;
        private static GameObject canvas;

        public static void makeCanvas()
        {
            //Modding.Logger.Log("in makecanvas");
            canvas = new GameObject();
            canvas.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            CanvasScaler scaler = canvas.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            canvas.AddComponent<GraphicRaycaster>();
            canvas.SetActive(false);
            Object.DontDestroyOnLoad(canvas);
            //Modding.Logger.Log("made the canvas");
        }

        public static void showCanvas(bool state)
        {
            canvas.SetActive(state);
            //Modding.Logger.Log("Setting canvas visibility to " + state);
        }

        public static void makeMenu()
        {
            //Modding.Logger.Log("in makemenu");
            panel = new CanvasPanel(canvas, new Vector2(225, 108), new Vector2(1000, 500));
            panel.AddText("tracker", "", new Vector2(50f, 25f), new Vector2(1000f, 500f), fontType, 30);
            panel.SetActive(true, true);
            //Modding.Logger.Log("made a panel");
        }

        public static void updateText(string newText)
        {
            panel.GetText("tracker").UpdateText(newText);
        }

        public static void loadResources()
        {
            foreach (Font f in Resources.FindObjectsOfTypeAll<Font>())
            {
                if (f != null && f.name == "Perpetua")
                {
                    fontType = f;
                    //Modding.Logger.Log("found the font");
                }
            }
        }
    }
}