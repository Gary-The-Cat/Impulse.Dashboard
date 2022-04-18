// <copyright file="EasingWorker.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;

namespace Impulse.Shared.Maths;

public class EasingWorker
{
    private readonly Func<double, double> getValue;

    private readonly Action<double> setValue;

    private readonly float duration;

    private readonly float minValue;

    private readonly float difference;

    private float timeAlive;

    public EasingWorker(
        Func<double, double> getValue,
        Action<double> setValue,
        float durationSeconds,
        float minValue = 0,
        float maxValue = 1)
    {
        this.getValue = getValue;
        this.setValue = setValue;
        this.duration = durationSeconds;
        this.minValue = minValue;
        this.difference = maxValue - minValue;
        timeAlive = 0;
    }

    public bool IsAlive => timeAlive < duration;

    public void OnUpdate(float deltaT)
    {
        if (!IsAlive)
        {
            return;
        }

        timeAlive += deltaT;

        var proportion = timeAlive / duration;

        var scale = getValue(proportion);

        var newValue = minValue + (scale * difference);

        setValue(newValue);
    }
}
