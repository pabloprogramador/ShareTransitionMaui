using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;


namespace ShareTransitionMaui
{
    public static class ShapeAnimation
    {
        public static async Task AnimateShapeAsync(Shape source, Shape current, Shape target, uint duration, Easing easing, Action onCompleted = null)
        {

            var currentPoint = current.GetAbsolutePosition();
            var nextPoint = target.GetAbsolutePosition();


            source.AnimateSizePosition(
                currentPoint.X, currentPoint.Y,
                nextPoint.X, nextPoint.Y,
                current.Width, target.Width,
                current.Height, target.Height, duration, easing, null);

            // Animação de Background (ou Gradiente)
            if (source.Fill is SolidColorBrush sourceBackground && target.Fill is SolidColorBrush targetBackground)
            {
               source.ColorShapeTo(sourceBackground.Color, targetBackground.Color, duration, easing);
            }
            else if (source.Fill is LinearGradientBrush sourceGradient && target.Fill is LinearGradientBrush targetGradient)
            {
                source.GradientShapeTo(sourceGradient, targetGradient, duration, easing);
            }
            else if (target.Fill == null || (target.Fill is SolidColorBrush solid && solid.Color == Colors.Transparent))
            {
                // Mantém a cor inicial se o destino for transparente
            }

            // Animação de Shadow
            if (current.Shadow != null && target.Shadow != null)
            {
                source.Shadow = new Shadow();
                //ShadowAnimation(source, source.Shadow, target.Shadow, duration, easing);
                AnimationExtensions.ShadowTo(source,
                    current.Shadow.Radius, target.Shadow.Radius,
                    current.Shadow.Opacity, target.Shadow.Opacity,
                    current .Shadow.Offset, target.Shadow.Offset,
                    duration
                    );
            }

            // Animação de StrokeThickness
            source.DoubleTo(source.StrokeThickness, target.StrokeThickness, t => source.StrokeThickness = t, duration, easing);

            // 7. Animação de Stroke
            if (source.Stroke is SolidColorBrush sourceStroke && target.Stroke is SolidColorBrush targetStroke)
            {
                source.ColorShapeTo(sourceStroke.Color, targetStroke.Color, duration, easing, color => source.Stroke = new SolidColorBrush(color));
            }

            // Animação de StrokeShape
            if (source is RoundRectangle sourceShape && target is RoundRectangle targetShape)
            {
                AnimationExtensions.RadiusTo(sourceShape, sourceShape.CornerRadius, targetShape.CornerRadius, duration);

            }

            // Ao final da animação, chamar o callback se houver
            await Task.Delay((int)duration + 100);
            onCompleted?.Invoke();
        }


    }

}