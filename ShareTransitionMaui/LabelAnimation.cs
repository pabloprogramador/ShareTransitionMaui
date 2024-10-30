using System;
using Microsoft.Maui.Controls.Shapes;

namespace ShareTransitionMaui
{
	public static class LabelAnimation
	{
        public static async Task AnimateLabelAsync(Label source, Label current, Label target, uint duration, Easing easing, Action onCompleted = null)
        {

            var currentPoint = current.GetAbsolutePosition();
            var nextPoint = target.GetAbsolutePosition();

            source.AnimateSizePosition(
                currentPoint.X, currentPoint.Y,
                nextPoint.X, nextPoint.Y,
                current.Width, target.Width,
                current.Height, target.Height, duration, easing, null);
            
            // Animação de Shadow
            if (current.Shadow != null && target.Shadow != null)
            {
                source.Shadow = new Shadow();
                //ShadowAnimation(source, source.Shadow, target.Shadow, duration, easing);
                AnimationExtensions.ShadowTo(source,
                    current.Shadow.Radius, target.Shadow.Radius,
                    current.Shadow.Opacity, target.Shadow.Opacity,
                    current.Shadow.Offset, target.Shadow.Offset,
                    duration
                    );
            }

            source.Labelto(
                current.FontSize, target.FontSize,
                current.TextColor, target.TextColor,
                current.BackgroundColor, target.BackgroundColor,
                duration);

           
            // Ao final da animação, chamar o callback se houver
            await Task.Delay((int)duration + 100);
            onCompleted?.Invoke();
        }
    }
}

