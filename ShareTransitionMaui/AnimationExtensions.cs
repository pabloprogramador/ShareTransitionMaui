using System;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace ShareTransitionMaui
{
    public static class AnimationExtensions
    {
        public static async Task ColorShapeTo(this Shape shape, Color startColor, Color endColor, uint duration, Easing easing, Action<Color> callback = null)
        {
            await shape.ColorTo(startColor, endColor, c =>
            {
                if (callback != null)
                {
                    callback(c);
                }
                else
                {
                    shape.Fill = new SolidColorBrush(c);
                }
            }, duration, easing);
        }


        public static async Task GradientShapeTo(this Shape shape, LinearGradientBrush startGradient, LinearGradientBrush endGradient, uint duration, Easing easing)
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

        public static void AnimateSizePosition(
        this VisualElement element,
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

            }), 16, length: duration, easing: easingType, (d, v) =>
            {
                onCompleted?.Invoke();
            });
        }

        // Animação para uma propriedade do tipo Color
        public static Task ColorTo(this VisualElement element, Color startColor, Color endColor, Action<Color> callback, uint length = 250, Easing easing = null)
        {
            easing ??= Easing.Linear;
            var taskCompletionSource = new TaskCompletionSource<bool>();

            var animation = new Animation(v =>
            {
                var newColor = Color.FromRgba(
                    startColor.Red + v * (endColor.Red - startColor.Red),
                    startColor.Green + v * (endColor.Green - startColor.Green),
                    startColor.Blue + v * (endColor.Blue - startColor.Blue),
                    startColor.Alpha + v * (endColor.Alpha - startColor.Alpha)
                );
                callback(newColor);
            });

            animation.Commit(element, "ColorTo", 16, length, easing, (v, c) => taskCompletionSource.SetResult(c));
            return taskCompletionSource.Task;
        }

        // Animação para uma propriedade do tipo double
        public static Task DoubleTo(this VisualElement element, double start, double end, Action<double> callback, uint length = 250, Easing easing = null)
        {
            easing ??= Easing.Linear;
            var taskCompletionSource = new TaskCompletionSource<bool>();

            var animation = new Animation(v =>
            {
                var newValue = start + v * (end - start);
                callback(newValue);
            });

            animation.Commit(element, "DoubleTo", 16, length, easing, (v, c) => taskCompletionSource.SetResult(c));
            return taskCompletionSource.Task;
        }

        public static async Task RadiusTo(VisualElement element, CornerRadius startRadius, CornerRadius endRadius, uint duration)
        {
            // Create an animation for each corner
            var animation = new Animation(v =>
            {
                // Update the CornerRadius property of the RoundRectangle
                if (element is RoundRectangle shape)
                {
                    shape.CornerRadius = new CornerRadius(
                        startRadius.TopLeft + (endRadius.TopLeft - startRadius.TopLeft) * v,
                        startRadius.TopRight + (endRadius.TopRight - startRadius.TopRight) * v,
                        startRadius.BottomRight + (endRadius.BottomRight - startRadius.BottomRight) * v,
                        startRadius.BottomLeft + (endRadius.BottomLeft - startRadius.BottomLeft) * v
                    );
                }
            }, 0, 1);

            // Set the duration of the animation
            animation.Commit(element, "AnimateCornerRadius", 16, duration);
        }

        public static async Task Labelto(this Label label, double startFontSize, double endFontSize, Color startFontColor, Color endFontColor, Color startBackgroundColor, Color endBackgroundColor, uint duration)
        {
            // Create an animation for FontSize and FontColor
            var animation = new Animation(v =>
            {
                // Animate FontSize
                label.FontSize = startFontSize + (endFontSize - startFontSize) * v;

                // Animate FontColor
                label.TextColor = Color.FromRgba(
                    startFontColor.Red + (endFontColor.Red - startFontColor.Red) * v,
                    startFontColor.Green + (endFontColor.Green - startFontColor.Green) * v,
                    startFontColor.Blue + (endFontColor.Blue - startFontColor.Blue) * v,
                    startFontColor.Alpha + (endFontColor.Alpha - startFontColor.Alpha) * v
                );

                if (startBackgroundColor != null)
                {
                    // Animate BackgroundColor
                    label.BackgroundColor = Color.FromRgba(
                        startBackgroundColor.Red + (endBackgroundColor.Red - startBackgroundColor.Red) * v,
                        startBackgroundColor.Green + (endBackgroundColor.Green - startBackgroundColor.Green) * v,
                        startBackgroundColor.Blue + (endBackgroundColor.Blue - startBackgroundColor.Blue) * v,
                        startBackgroundColor.Alpha + (endBackgroundColor.Alpha - startBackgroundColor.Alpha) * v
                    );
                }
                
            }, 0, 1);

            // Commit the animation
            animation.Commit(label, "AnimateLabel", 16, duration);

            await Task.CompletedTask; // Ensures method can be awaited
        }

        public static async Task ShadowTo(this View view, double startRadius, double endRadius, double startOpacity, double endOpacity, Point startOffset, Point endOffset, uint duration)
        {
            
            // Create an animation to modify the shadow properties
            var animation = new Animation(v =>
            {
                    // Update the shadow properties during the animation
                    view.Shadow = new Shadow
                    {
                        Radius = (float)(startRadius + (endRadius - startRadius) * v),
                        Opacity = (float)(startOpacity + (endOpacity - startOpacity) * v),
                        Offset = new Point(
                            startOffset.X + (endOffset.X - startOffset.X) * v,
                            startOffset.Y + (endOffset.Y - startOffset.Y) * v
                        ),
                        Brush = view.Shadow?.Brush ?? new SolidColorBrush(Colors.Black)
                    };
                //Console.WriteLine($"opacity>>{view.Shadow.Opacity}");
                //Console.WriteLine($"radius>>{view.Shadow.Radius}");
                //Console.WriteLine($"offset>>{view.Shadow.Offset}");

            }, 0, 1);

            // Commit the animation
            animation.Commit(view, "AnimateShadow", 16, duration);

            await Task.CompletedTask; // Ensures method can be awaited
        }

    }

}

