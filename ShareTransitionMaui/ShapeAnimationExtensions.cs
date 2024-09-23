using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;


namespace ShareTransitionMaui
{
    public static class ShapeAnimationExtensions
    {
        public static async Task AnimateShapeAsync(Shape source, Shape current, Shape target, uint duration, Easing easing, Action onCompleted = null)
        {

            var currentPoint = current.GetAbsolutePosition();
            var nextPoint = target.GetAbsolutePosition();


            AnimateSizePosition(source,
                currentPoint.X, currentPoint.Y,
                nextPoint.X, nextPoint.Y,
                current.Width, target.Width,
                current.Height, target.Height, duration, easing, null);


            //tempGrid.TranslationX = currentPoint.X;
            //tempGrid.TranslationY = currentPoint.Y;
            //source.WidthRequest = current.Width;
            //Console.WriteLine($"=========={current.Width}");
            //Console.WriteLine($"=========={target.Width}");

            //await Task.Delay(100);
            //tempGrid.WidthRequest = target.Width;
            //tempGrid.HeightRequest = target.Height;
            //source.TranslationX = nextPoint.X;
            //source.TranslationY = nextPoint.Y;
            //source.SizeTo(current.Width, target.Width, current.Height, target.Height, duration, easing);
            //source.WidthTo(target.Width, 700, easing);
            //source.HeightTo(target.Height, 100, easing);
            //source.TranslateTo(nextPoint.X, nextPoint.Y, 200, easing);

            //await Task.Delay((int)duration + 100);
            //onCompleted?.Invoke();
            //return;



            // 1. Animação de Background (ou Gradiente)
            if (source.Fill is SolidColorBrush sourceBackground && target.Fill is SolidColorBrush targetBackground)
            {
                ColorAnimation(source, sourceBackground.Color, targetBackground.Color, duration, easing);
            }
            else if (source.Fill is LinearGradientBrush sourceGradient && target.Fill is LinearGradientBrush targetGradient)
            {
                GradientAnimation(source, sourceGradient, targetGradient, duration, easing);
            }
            else if (target.Fill == null || (target.Fill is SolidColorBrush solid && solid.Color == Colors.Transparent))
            {
                // Mantém a cor inicial se o destino for transparente
            }

            // 2. Animação de Shadow
            //if (source.Shadow != null && target.Shadow != null)
            //{
            //    ShadowAnimation(source, source.Shadow, target.Shadow, duration, easing);
            //}

            // 6. Animação de StrokeThickness
            source.DoubleTo(source.StrokeThickness, target.StrokeThickness, t => source.StrokeThickness = t, duration, easing);

            // 7. Animação de Stroke
            if (source.Stroke is SolidColorBrush sourceStroke && target.Stroke is SolidColorBrush targetStroke)
            {
                ColorAnimation(source, sourceStroke.Color, targetStroke.Color, duration, easing, color => source.Stroke = new SolidColorBrush(color));
            }

            // 8. Animação de StrokeShape
            if (source is RoundRectangle sourceShape && target is RoundRectangle targetShape)
            {
                AnimationExtensions.RadiusTo(sourceShape, sourceShape.CornerRadius, targetShape.CornerRadius, duration);
                
            }

            // Ao final da animação, chamar o callback se houver
            await Task.Delay((int)duration+100);
            onCompleted?.Invoke();
        }

        private static void AnimateSizePosition(
        VisualElement element,
        double startX, double startY,
        double endX, double endY,
        double startWidth, double endWidth,
        double startHeight, double endHeight,
        uint duration,
        Easing easingType,
        Action onCompleted)
        {
            
            element.Opacity = 1;
            element.TranslationX = startX;
            element.TranslationY = startY;
            element.WidthRequest = startWidth;
            element.HeightRequest = startHeight;

            element.Animate("CustomAnimationSizePosition", new Animation(v =>
            {
                element.TranslationX = startX + (endX - startX) * v;
                element.TranslationY = startY + (endY - startY) * v;

                element.WidthRequest = startWidth + (endWidth - startWidth) * v;
                element.HeightRequest = startHeight + (endHeight - startHeight) * v;

#if IOS
                // No iOS, obtenha a UIView nativa do controle (por exemplo, um Button)
                var nativeView = element.Handler.PlatformView as UIKit.UIView;

                // Força o redesenho da View
                nativeView?.SetNeedsDisplay();
#endif

            }), 16, length: duration, easing: easingType, (d, v) =>
            {
                onCompleted?.Invoke();
            });
        }

       

        private static async Task ColorAnimation(Shape shape, Color startColor, Color endColor, uint duration, Easing easing, Action<Color> callback = null)
        {
            await shape.ColorTo(startColor, endColor, c =>
            {
                if (callback != null)
                {
                    // Se o callback for fornecido, invocamos
                    callback(c);
                }
                else
                {
                    // Se o callback for nulo, alteramos o Background do shape
                    shape.Fill = new SolidColorBrush(c);
                }
            }, duration, easing);
        }


        private static async Task GradientAnimation(Shape shape, LinearGradientBrush startGradient, LinearGradientBrush endGradient, uint duration, Easing easing)
        {
            var startStops = startGradient.GradientStops.ToList();
            var endStops = endGradient.GradientStops.ToList();

            for (int i = 0; i < startStops.Count; i++)
            {
                var startColor = startStops[i].Color;
                var endColor = i < endStops.Count ? endStops[i].Color : startColor;

                await shape.ColorTo(startColor, endColor, color =>
                {
                    startStops[i].Color = color;
                    shape.Fill = startGradient;
                }, duration, easing);
            }
        }

        private static async Task ShadowAnimation(Shape shape, Shadow startShadow, Shadow endShadow, uint duration, Easing easing)
        {
            await Task.WhenAll(
                shape.DoubleTo(startShadow.Offset.X, endShadow.Offset.X, x => shape.Shadow.Offset = new Point(x, shape.Shadow.Offset.Y), duration, easing),
                shape.DoubleTo(startShadow.Offset.Y, endShadow.Offset.Y, y => shape.Shadow.Offset = new Point(shape.Shadow.Offset.X, y), duration, easing),
                shape.DoubleTo(startShadow.Radius, endShadow.Radius, r => shape.Shadow.Radius = (float)r, duration, easing),
                shape.DoubleTo(startShadow.Opacity, endShadow.Opacity, o => shape.Shadow.Opacity = (float)o, duration, easing)
            );
        }

        private static async Task ThicknessAnimation(Shape shape, Thickness startThickness, Thickness endThickness, Action<Thickness> callback, uint duration, Easing easing)
        {
            var animationTasks = new List<Task>
        {
            shape.DoubleTo(startThickness.Left, endThickness.Left, v => startThickness.Left = v, duration, easing),
            shape.DoubleTo(startThickness.Top, endThickness.Top, v => startThickness.Top = v, duration, easing),
            shape.DoubleTo(startThickness.Right, endThickness.Right, v => startThickness.Right = v, duration, easing),
            shape.DoubleTo(startThickness.Bottom, endThickness.Bottom, v => startThickness.Bottom = v, duration, easing)
        };

            await Task.WhenAll(animationTasks);
            callback?.Invoke(startThickness);
        }
    }

}