// <copyright file="DateWeight.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;

namespace Impulse.Dashboard.Debug.DemoScreens.TaylorDemo;

public class DateWeight
{
    public DateWeight(DateTime date, float weight)
    {
        Date = date;
        Weight = weight;
    }

    public DateTime Date { get; set; }

    public float Weight { get; set; }

    public string WeightFormatted => Weight.ToString("#.00");

    public string DateFormatted => Date.ToShortDateString();

    public override string ToString()
    {
        return $"{DateFormatted} => {WeightFormatted}";
    }
}
