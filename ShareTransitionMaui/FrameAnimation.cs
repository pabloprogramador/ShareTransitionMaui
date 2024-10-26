using System;
using Microsoft.Maui.Controls.Shapes;

namespace ShareTransitionMaui
{
	public static class FrameAnimation
	{
        public static async Task AnimateFrameAsync(Frame source, Frame current, Frame target, uint duration, Easing easing, Action onCompleted = null)
        {

            var currentPoint = current.GetAbsolutePosition();
            var nextPoint = target.GetAbsolutePosition();


            source.AnimateSizePosition(
                currentPoint.X, currentPoint.Y,
                nextPoint.X, nextPoint.Y,
                current.Width, target.Width,
                current.Height, target.Height, duration, easing, null);

            source.ColorTo(current.BorderColor, target.BorderColor,
                c => source.BorderColor = c) ;

            source.ColorTo(current.BackgroundColor, target.BackgroundColor,
                c => source.BackgroundColor = c);

            AnimationExtensions.DoubleTo(source, current.CornerRadius, target.CornerRadius,
                c => source.CornerRadius = (float)c, duration) ;

            await Task.Delay((int)duration + 100);
            onCompleted?.Invoke();
        }
    }
}

