using UnityEngine;
using System;

public interface IAnimationEventHandler
{
    /// <summary>
    /// アニメーションイベントを登録する
    /// </summary>
    void RegisterEvent(string animationName, float normalizedTime, Action callback);

    /// <summary>
    /// アニメーションイベントを解除する
    /// </summary>
    void UnregisterEvent(string animationName, float normalizedTime);

    /// <summary>
    /// アニメーション完了時のハンドラーを登録する
    /// </summary>
    void RegisterCompletionHandler(string animationName, Action<string> handler);

    /// <summary>
    /// アニメーション完了時のハンドラーを解除する
    /// </summary>
    void UnregisterCompletionHandler(string animationName);

    /// <summary>
    /// アニメーションの状態を更新する
    /// </summary>
    void UpdateAnimation(string currentAnimation, float normalizedTime);
} 