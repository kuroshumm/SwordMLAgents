using UnityEngine;
using System;

public enum EventTiming
{
    Start,      // 状態開始時
    Progress,   // 進行中の特定タイミング
    Complete,   // 完了時
    Cancel      // キャンセル時
}

public class TimelineEventArgs : EventArgs
{
    public string StateName { get; }
    public float Progress { get; }
    public EventTiming Timing { get; }
    public object Context { get; }

    public TimelineEventArgs(string stateName, float progress, EventTiming timing, object context = null)
    {
        StateName = stateName;
        Progress = progress;
        Timing = timing;
        Context = context;
    }
}

public delegate void TimelineEventHandler(object sender, TimelineEventArgs e);

public interface ITimelineEventHandler
{
    /// <summary>
    /// イベントを登録する
    /// </summary>
    void RegisterEvent(string stateName, EventTiming timing, TimelineEventHandler handler, float progress = 0f, object context = null);

    /// <summary>
    /// イベントを解除する
    /// </summary>
    void UnregisterEvent(string stateName, TimelineEventHandler handler);

    /// <summary>
    /// 状態を更新する
    /// </summary>
    void UpdateState(string currentState, float progress);

    /// <summary>
    /// 現在の状態情報を取得する
    /// </summary>
    string GetCurrentState();
} 