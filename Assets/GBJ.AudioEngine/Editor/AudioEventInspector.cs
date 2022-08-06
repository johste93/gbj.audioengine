using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using GBJ.AudioEngine.Settings;

namespace GBJ.AudioEngine.Editor
{
    [CustomEditor(typeof(AudioEvent))]
    public class AudioEventInspector : UnityEditor.Editor
    {
        AudioEvent audioEvent { get { return (AudioEvent) target; } }

        private SerializedProperty _assetReferances;
        private ReorderableList reorderableList;

        private AudioPlayer currentPlayer;

        private void OnEnable()
        {
            Initalise();
            UpdateName();

            _assetReferances = serializedObject.FindProperty("AssetReferances");
            reorderableList = new ReorderableList(serializedObject, _assetReferances, true, true, true, true);
            reorderableList.drawHeaderCallback = DrawHeader;
            reorderableList.drawElementCallback = DrawListItems;
        }

        private void OnDisable() => UpdateName();

        private void Initalise()
        {
            if(audioEvent.AudioSourceSettings == null)
            {
                audioEvent.AudioSourceSettings = new AudioSourceSettings();
                AudioSourceSettingsInspector.Reset(audioEvent.AudioSourceSettings, false);
            }
            
            if(audioEvent.AudioChorusFilterSettings == null)
            {
                audioEvent.AudioChorusFilterSettings = new AudioChorusFilterSettings();
                AudioChorusFilterSettingsInspector.Reset(audioEvent.AudioChorusFilterSettings, false);
            }

            if(audioEvent.AudioDistortionFilterSettings == null)
            {
                audioEvent.AudioDistortionFilterSettings = new AudioDistortionFilterSettings();
                AudioDistortionFilterSettingsInspector.Reset(audioEvent.AudioDistortionFilterSettings, false);
            }

            if(audioEvent.AudioEchoFilterSettings == null)
            {
                audioEvent.AudioEchoFilterSettings = new AudioEchoFilterSettings();
                AudioEchoFilterSettingsInspector.Reset(audioEvent.AudioEchoFilterSettings, false);
            }
            
            if(audioEvent.AudioHighPassFilterSettings == null)
            {
                audioEvent.AudioHighPassFilterSettings = new AudioHighPassFilterSettings();
                AudioHighPassFilterSettingsInspector.Reset(audioEvent.AudioHighPassFilterSettings, false);
            }

            if(audioEvent.AudioLowPassFilterSettings == null)
            {
                audioEvent.AudioLowPassFilterSettings = new AudioLowPassFilterSettings();
                AudioLowPassFilterSettingsInspector.Reset(audioEvent.AudioLowPassFilterSettings, false);
            }

            if(audioEvent.AudioReverbFilterSettings == null)
            {
                audioEvent.AudioReverbFilterSettings = new AudioReverbFilterSettings();
            }
        }

        private void UpdateName()
        {
            if(audioEvent == null) //Fix Missing Referance Exeption when deleting an event.
                return;

            if(target.name != audioEvent.Name)
            {
                audioEvent.Name = target.name;
                EditorUtility.SetDirty(target);
            }
        }

        public override void OnInspectorGUI()
        {
            DrawEventName();

            DrawPlayButtons();

            serializedObject.Update();
            EditorGUI.BeginChangeCheck();

            DropArea();
            reorderableList.DoLayoutList();

            audioEvent.PlayOrder = (PlayOrder)EditorGUILayout.EnumPopup("Play Order", audioEvent.PlayOrder);
            audioEvent.SurviveSceneChanges = EditorGUILayout.Toggle("Survive Scene Changes", audioEvent.SurviveSceneChanges);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Tags"));

            EditorGUILayout.Space();
            
            AudioSourceSettingsInspector.Draw(audioEvent.AudioSourceSettings);
            AudioChorusFilterSettingsInspector.Draw(audioEvent.AudioChorusFilterSettings);
            AudioDistortionFilterSettingsInspector.Draw(audioEvent.AudioDistortionFilterSettings);
            AudioEchoFilterSettingsInspector.Draw(audioEvent.AudioEchoFilterSettings);
            AudioHighPassFilterSettingsInspector.Draw(audioEvent.AudioHighPassFilterSettings);
            AudioLowPassFilterSettingsInspector.Draw(audioEvent.AudioLowPassFilterSettings);
            AudioReverbFilterSettingsInspector.Draw(audioEvent.AudioReverbFilterSettings);

            if(EditorGUI.EndChangeCheck())
            {
                if(Application.isPlaying)
                    HotReload();
                
                EditorUtility.SetDirty(target);
                serializedObject.ApplyModifiedProperties();
            }
        }

        private void DropArea()
        {
            EditorGUILayout.Space();
            // Targets Drop Area
            Event evt = Event.current;
            Rect drop_area = GUILayoutUtility.GetRect(0.0f, 50.0f, GUILayout.ExpandWidth(true));
            var style = new GUIStyle("box");
            if (EditorGUIUtility.isProSkin)
                style.normal.textColor = Color.white;
            GUI.Box(drop_area, "\nDROP AUDIOCLIPS HERE", style);
            
            Undo.RecordObject(audioEvent, "Added audioclips");
            switch (evt.type)
            {
                case EventType.DragUpdated:
                case EventType.DragPerform:
                    if (!drop_area.Contains(evt.mousePosition))
                        return;

                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                    if (evt.type == EventType.DragPerform)
                    {
                        DragAndDrop.AcceptDrag();

                        foreach (Object dragged_object in DragAndDrop.objectReferences)
                        {
                            _assetReferances.arraySize++;
                            serializedObject.ApplyModifiedProperties();
                        }

                        int index = _assetReferances.arraySize - DragAndDrop.objectReferences.Length;
                        foreach (var dragged_object in DragAndDrop.objectReferences)
                        {
                            if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(dragged_object, out string guid,
                                out long localId))
                            {
                                audioEvent.AssetReferances[index] = new AssetReferenceAudioClip(guid);
                                index++;
                            }
                        }
                        
                        EditorUtility.SetDirty(audioEvent);
                    }
                    break;
            }
            EditorGUILayout.Space();
        }

        private void DrawEventName()
        {
            EditorGUILayout.BeginHorizontal();
            GUI.enabled = false;
            EditorGUILayout.LabelField("Name:", audioEvent.Name);
            GUI.enabled = true;
            if(GUILayout.Button("Copy Code", GUILayout.Width(EditorGUIUtility.fieldWidth*2)))
            {
                EditorGUIUtility.systemCopyBuffer = $"Audio.Play(\"{audioEvent.Name}\");";
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawPlayButtons()
        {
            EditorGUILayout.BeginHorizontal();
            GUI.color = Color.green;
            if(GUILayout.Button("Play"))
                Play();
            GUI.color = Color.white;
            if(GUILayout.Button("Stop"))
                Stop();
            EditorGUILayout.EndHorizontal();
        }

        private void Play()
        {
            if(currentPlayer != null)
                DestroyImmediate(currentPlayer.gameObject);

            currentPlayer = Audio.Play(audioEvent);

            System.Reflection.Assembly assembly = typeof(UnityEditor.EditorWindow).Assembly;
            System.Type type = assembly.GetType( "UnityEditor.GameView" );
            EditorWindow gameview = EditorWindow.GetWindow(type);
            gameview.Repaint();
        }

        private void Stop()
        {
            if(currentPlayer != null)
                DestroyImmediate(currentPlayer.gameObject);
        }

        private void HotReload()
        {
            audioEvent.OnEventChanged?.Invoke(audioEvent);
        }

        // Draws the elements on the list
        private void DrawListItems(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty element = reorderableList.serializedProperty.GetArrayElementAtIndex(index); // The element in the list
            
            EditorGUIUtility.labelWidth = 0.1f;
            GUIContent emptyContent = new GUIContent(GUIContent.none);
            
            GUILayout.FlexibleSpace();
            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y, Screen.width-50, EditorGUIUtility.singleLineHeight), 
                element,
                emptyContent,
                true
            ); 
            
            EditorGUIUtility.labelWidth = 0f;
        }

        //Draws the header
        private void DrawHeader(Rect rect)
        {
            string name = "Audio Clips";
            EditorGUI.LabelField(rect, name);
        }
    }
}