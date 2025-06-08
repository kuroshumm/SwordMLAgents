using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using Define.Client;

public class AnimationEventManager : ComponentBase
{
    private class EventRegistration
    {
        public EventTiming Timing { get; }
        public float Progress { get; }
        public TimelineEventHandler Handler { get; }
        public bool IsExecuted { get; set; }
        public object Context { get; }

        public EventRegistration(EventTiming timing, float progress, TimelineEventHandler handler, object context = null)
        {
            Timing = timing;
            Progress = progress;
            Handler = handler;
            Context = context;
            IsExecuted = false;
        }
    }

    private readonly Dictionary<string, List<EventRegistration>> _eventRegistrations = new();
    private string _currentState = string.Empty;
    private float _prevProgress = 0f;
    private bool _isDebugMode = false;

    private Animator _targetAnimator;
    private string _currentStateName;

    public event Action<string> OnDebugLog;

    public override void Enter(IUnityActor owner)
    {
        base.Enter(owner);
        _targetAnimator = owner.Transform.GetComponentInChildren<Animator>();
    }

    public override void Execute(float deltaTime)
    {
        if (_targetAnimator == null || string.IsNullOrEmpty(_currentStateName)) return;

        AnimatorStateInfo currentAnimStateInfo = _targetAnimator.GetCurrentAnimatorStateInfo(0);
        if (!currentAnimStateInfo.IsName(_currentStateName)) return;

        float progress = currentAnimStateInfo.normalizedTime;

        // アニメーションが終了した場合の処理
        if (progress >= 1f && _currentState == _currentStateName)
        {
            ExecuteEvents(_currentStateName, 1f, EventTiming.Complete);
            _currentState = string.Empty; // 状態をリセット
        }

        UpdateState(_currentStateName.ToLower(), progress);
    }

    public override void Exit()
    {
        ClearAllEvent();
    }

    // イベントグループを登録する新しいメソッド
    public void RegisterEventGroup(AnimationEventGroup group)
    {
        if (group == null || string.IsNullOrEmpty(group.StateName))
        {
            LogDebug("Invalid event group");
            return;
        }

        // 既存のイベントを一旦クリア
        if (_eventRegistrations.ContainsKey(group.StateName))
        {
            _eventRegistrations[group.StateName].Clear();
        }

        foreach (var (timing, progress, handler, context) in group.Events)
        {
            RegisterEvent(group.StateName, timing, handler, progress, context);
        }

        LogDebug($"Registered event group for state {group.StateName} with {group.Events.Count} events");
    }

    // 複数のイベントグループを一括登録するメソッド
    public void RegisterEventGroups(IEnumerable<AnimationEventGroup> groups)
    {
        foreach (var group in groups)
        {
            RegisterEventGroup(group);
        }
    }

    // イベントグループを解除するメソッド
    public void UnregisterEventGroup(string stateName)
    {
        if (_eventRegistrations.ContainsKey(stateName))
        {
            _eventRegistrations.Remove(stateName);
            LogDebug($"Unregistered event group for state {stateName}");
        }
    }

    public void RegisterEvent(string stateName, EventTiming timing, TimelineEventHandler handler, float progress = 0f, object context = null)
    {
        if (!_eventRegistrations.ContainsKey(stateName))
        {
            _eventRegistrations[stateName] = new List<EventRegistration>();
        }

        var registration = new EventRegistration(timing, progress, handler, context);
        _eventRegistrations[stateName].Add(registration);
        _eventRegistrations[stateName].Sort((a, b) => a.Progress.CompareTo(b.Progress));
        
        LogDebug($"Registered event for state {stateName} at progress {progress:F2} with timing {timing}");
    }

    public void ClearAllEvent()
    {
        _eventRegistrations.Clear();
    }

    public void UnregisterEvent(string stateName, TimelineEventHandler handler)
    {
        if (_eventRegistrations.TryGetValue(stateName, out var registrations))
        {
            registrations.RemoveAll(r => r.Handler == handler);
            LogDebug($"Unregistered event for state {stateName}");
        }
    }

    public void SetCurrentAnimation(string stateName)
    {
        if (_currentState != stateName)
        {
            if (!string.IsNullOrEmpty(_currentState))
            {
                // 前の状態の終了イベントを発火
                ExecuteEvents(_currentState, 1f, EventTiming.Complete);
            }

            _currentState = stateName;
            _currentStateName = stateName;
            _prevProgress = 0f;
            
            // 新しい状態のイベントフラグをリセット
            if (_eventRegistrations.TryGetValue(stateName, out var newRegistrations))
            {
                foreach (var registration in newRegistrations)
                {
                    registration.IsExecuted = false;
                }
            }
            
            // 新しい状態の開始イベントを発火
            ExecuteEvents(stateName, 0f, EventTiming.Start);
            LogDebug($"State changed to {stateName}");
        }
    }

    private void UpdateState(string newState, float progress)
    {
        // 進行中のイベントを処理
        if (_currentState == newState && progress > _prevProgress)
        {
            ExecuteEvents(newState, progress, EventTiming.Progress);
        }

        _prevProgress = progress;
    }

    private void ExecuteEvents(string stateName, float progress, EventTiming timing)
    {
        if (!_eventRegistrations.TryGetValue(stateName, out var registrations))
            return;

        var eligibleEvents = registrations
            .Where(r => r.Timing == timing && !r.IsExecuted && r.Progress <= progress)
            .OrderBy(r => r.Progress)
            .ToList();

        foreach (var evt in eligibleEvents)
        {
            evt.Handler?.Invoke(this, new TimelineEventArgs(stateName, progress, timing, evt.Context));
            evt.IsExecuted = true;
            LogDebug($"Executed event for state {stateName} at progress {progress:F2} with timing {timing}");
        }
    }

    public string GetCurrentState()
    {
        return _currentState;
    }

    private void LogDebug(string message)
    {
        if (!_isDebugMode) return;
        OnDebugLog?.Invoke(message);
        Debug.Log($"[AnimationEventManager] {message}");
    }

    public string DumpState()
    {
        var dump = new System.Text.StringBuilder();
        dump.AppendLine($"Current State: {_currentState}");
        dump.AppendLine($"Registered Events:");
        
        foreach (var (state, events) in _eventRegistrations)
        {
            dump.AppendLine($"  State: {state}");
            foreach (var evt in events)
            {
                dump.AppendLine($"    - {evt.Timing} at {evt.Progress:F2} (Executed: {evt.IsExecuted})");
            }
        }

        return dump.ToString();
    }
}