// <copyright file="CompositionTargetEx.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Windows.Media;

namespace Impulse.SharedFramework.Viewer
{
    public static class CompositionTargetEx
    {
        private static TimeSpan last = TimeSpan.Zero;

        public static event EventHandler<RenderingEventArgs> Rendering
        {
            add
            {
                if (FrameUpdating == null)
                {
                    CompositionTarget.Rendering += CompositionTarget_Rendering;
                }

                FrameUpdating += value;
            }

            remove
            {
                FrameUpdating -= value;
                if (FrameUpdating == null)
                {
                    CompositionTarget.Rendering -= CompositionTarget_Rendering;
                }
            }
        }

        private static event EventHandler<RenderingEventArgs> FrameUpdating;

        private static void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            RenderingEventArgs args = (RenderingEventArgs)e;
            if (args.RenderingTime == last)
            {
                return;
            }

            last = args.RenderingTime;

            FrameUpdating(sender, args);
        }
    }
}
