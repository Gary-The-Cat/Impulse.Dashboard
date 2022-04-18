// <copyright file="ObamaFactory.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;

namespace Impulse.Dashboard.Debug.DemoScreens.TemplatePractice;

public class ObamaFactory
{
    private const int ObamaCount = 7;

    private static readonly string[] ObamaAdjectives =
    {
        "Professional",
        "Competent",
        "Skillful",
        "Experienced",
        "Qualified",
        "Adept",
        "Expert",
    };

    private static readonly Random Random = new Random();

    public static Obama CreateObama()
    {
        return new Obama
        {
            Name = $"{ObamaAdjectives[Random.Next(ObamaCount)]} Obama",
        };
    }
}
