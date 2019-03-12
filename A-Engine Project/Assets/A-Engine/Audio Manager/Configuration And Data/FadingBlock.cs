// A-Engine, Code version: 1

#if UNITY_EDITOR
using UnityEditor;
#endif

using System.Xml;
using System;
using UnityEngine;
using AEngine.Parser;

namespace AEngine.Audio
{
    [Serializable]
    public class FadingBlock
    {
        public FadeMods FadeMode { get; set; }
        public float FadeOutDuration { get; set; }
        public float FadeInDuration { get; set; }
        public bool ClampDuration { get; set; }
        public float ClampedOutTrack { get; set; }
        public float ClampedInTrack { get; set; }
        public bool IsEvenly { get; set; }

        public FadingBlock()
        {
            this.FadeMode = FadeMods.NotFading;
            this.FadeOutDuration = 0.4f;
            this.FadeInDuration = 0.4f;
            this.ClampDuration = false;
            this.ClampedOutTrack = 0.05f;
            this.ClampedInTrack = 0.05f;
            this.IsEvenly = true;
        }

#if UNITY_EDITOR
        const float CAPTION_WIDTH = 210f;
        const float SMALL_CAPTION_WIDTH = 120f;
        const float BASE_FIELD_WIDTH = 50f;
        const float FADE_POPUP_WIDTH = 150f;        
        const float SLIDER_WIDHT = 200f;
        

        public void Draw(string caption, float offset)
        {
            //_index = EditorGUILayout.Popup(_index, _fadeInOptions, GUILayout.Width(COMPRESSED_SLIDER_WIDHT));

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(offset);
            EditorGUILayout.LabelField(caption, GUILayout.Width(CAPTION_WIDTH));
            this.FadeMode = (FadeMods)EditorGUILayout.EnumPopup(this.FadeMode, GUILayout.Width(FADE_POPUP_WIDTH));
            EditorGUILayout.EndHorizontal();

            switch (this.FadeMode)
            {
                case FadeMods.NotFading:
                    break;

                case FadeMods.FadeOut:
                    DrawFadeOut(offset);
                    break;

                case FadeMods.FadeIn:
                    DrawFadeIn(offset);
                    break;

                case FadeMods.FadeInOut:
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(offset);
                    this.IsEvenly = Toogle(this.IsEvenly, "Distribute evenly", SMALL_CAPTION_WIDTH, BASE_FIELD_WIDTH);
                    EditorGUILayout.EndHorizontal();

                    GUILayout.Space(5f);

                    if (this.IsEvenly)
                    {
                        float duration = (this.FadeOutDuration + this.FadeInDuration) / 2f;

                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space(offset);
                        duration = FloatField(duration, "Fade Duration", SMALL_CAPTION_WIDTH, BASE_FIELD_WIDTH);
                        EditorGUILayout.EndHorizontal();

                        this.FadeOutDuration = duration;
                        this.FadeInDuration = duration;

                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space(offset);
                        this.ClampDuration = Toogle(this.ClampDuration, "Clamp duration", SMALL_CAPTION_WIDTH, BASE_FIELD_WIDTH);

                        if (this.ClampDuration)
                        {
                            float clampValue = (this.ClampedOutTrack + this.ClampedInTrack) / 2f;
                            GUILayout.Space(CAPTION_WIDTH - (SMALL_CAPTION_WIDTH + BASE_FIELD_WIDTH + 7f));
                            clampValue = Slider(clampValue, "Clamped value by track length", CAPTION_WIDTH, SLIDER_WIDHT);

                            this.ClampedOutTrack = clampValue;
                            this.ClampedInTrack = clampValue;
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                    else
                    {
                        DrawFadeOut(offset);
                        GUILayout.Space(5f);
                        DrawFadeIn(offset);
                    }
                    break;

                case FadeMods.FullFadeCrossMusic:

                    break;
            }

        }
        
        private void DrawFadeOut(float offset)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(offset);
            this.FadeOutDuration = FloatField(this.FadeOutDuration, "Fade Out Duration", SMALL_CAPTION_WIDTH, BASE_FIELD_WIDTH);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(offset);
            this.ClampDuration = Toogle(this.ClampDuration, "Clamp duration", SMALL_CAPTION_WIDTH, BASE_FIELD_WIDTH);

            if (this.ClampDuration)
            {
                GUILayout.Space(CAPTION_WIDTH - (SMALL_CAPTION_WIDTH + BASE_FIELD_WIDTH + 7f));
                this.ClampedOutTrack = Slider(this.ClampedOutTrack, "Clamped value by out track length", CAPTION_WIDTH, SLIDER_WIDHT);
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawFadeIn(float offset)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(offset);
            this.FadeInDuration = FloatField(this.FadeInDuration, "Fade In Duration", SMALL_CAPTION_WIDTH, BASE_FIELD_WIDTH);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(offset);
            this.ClampDuration = Toogle(this.ClampDuration, "Clamp duration", SMALL_CAPTION_WIDTH, BASE_FIELD_WIDTH);

            if (this.ClampDuration)
            {
                GUILayout.Space(CAPTION_WIDTH - (SMALL_CAPTION_WIDTH + BASE_FIELD_WIDTH + 7f));
                this.ClampedInTrack = Slider(this.ClampedInTrack, "Clamped value by in track length", CAPTION_WIDTH, SLIDER_WIDHT);
            }
            EditorGUILayout.EndHorizontal();
        }

        private bool Toogle(bool targetValue, string caption, float captionWidth, float toogleWidth)
        {
            EditorGUILayout.LabelField(caption, GUILayout.Width(captionWidth));
            targetValue = EditorGUILayout.Toggle(targetValue, GUILayout.Width(toogleWidth));

            return targetValue;
        }

        private float Slider(float targetValue, string caption, float captionWidth, float sliderWidth)
        {
            EditorGUILayout.LabelField(caption, GUILayout.Width(captionWidth));
            targetValue = EditorGUILayout.Slider(targetValue, 0f, 1f, GUILayout.Width(sliderWidth));

            return targetValue;
        }

        private float FloatField(float targetField, string caption, float captionWidth, float fieldWwidth)
        {
            EditorGUILayout.LabelField(caption, GUILayout.Width(captionWidth));
            targetField = EditorGUILayout.FloatField(targetField, GUILayout.Width(fieldWwidth));

            return targetField;
        }
#endif
    }
}