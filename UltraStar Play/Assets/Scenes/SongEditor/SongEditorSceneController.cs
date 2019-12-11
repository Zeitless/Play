﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongEditorSceneController : MonoBehaviour
{
    public string defaultSongName;

    [TextArea(3, 8)]
    [Tooltip("Convenience text field to paste and copy song names when debugging.")]
    public string defaultSongNamePasteBin;

    public AudioWaveFormVisualizer audioWaveFormVisualizer;
    public AudioClip audioClip;

    private bool audioWaveFormInitialized;

    public SongMeta SongMeta
    {
        get
        {
            return sceneData.SelectedSongMeta;
        }
    }

    private SongEditorSceneData sceneData;

    void Start()
    {
        sceneData = SceneNavigator.Instance.GetSceneData<SongEditorSceneData>(CreateDefaultSceneData());

        Debug.Log($"Start editing of '{sceneData.SelectedSongMeta.Title}' at {sceneData.PositionInSongMillis} milliseconds.");
    }

    void Update()
    {
        if (!audioWaveFormInitialized && audioClip != null && audioClip.samples > 0)
        {
            audioWaveFormInitialized = true;
            audioWaveFormVisualizer.DrawWaveFormMinAndMaxValues(audioClip);
        }
    }

    public void OnBackButtonClicked()
    {
        ContinueToSingScene();
    }

    private void ContinueToSingScene()
    {
        if (sceneData.PreviousSceneData is SingSceneData)
        {
            (sceneData.PreviousSceneData as SingSceneData).PositionInSongMillis = sceneData.PositionInSongMillis;
        }
        SceneNavigator.Instance.LoadScene(sceneData.PreviousScene, sceneData.PreviousSceneData);
    }

    private SongEditorSceneData CreateDefaultSceneData()
    {
        SongEditorSceneData defaultSceneData = new SongEditorSceneData();
        defaultSceneData.PreviousScene = EScene.SongSelectScene;
        defaultSceneData.PreviousSceneData = null;
        defaultSceneData.PositionInSongMillis = 0;
        defaultSceneData.SelectedSongMeta = SongMetaManager.Instance.FindSongMeta(defaultSongName);
        return defaultSceneData;
    }
}
