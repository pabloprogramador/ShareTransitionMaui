using System;
using Microsoft.Maui.Controls.Shapes;

namespace ShareTransitionMaui
{
    public static class AnimationExtensions
    {
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


        //public static void WidthTo(this View view,
        //                       double value,
        //                       double lenght,
        //                       Easing easing,
        //                       Action<bool> finished = null,
        //                       Action<double> updated = null)
        //{
        //    var animation = new Animation((value) => {
        //        Console.WriteLine($"::::{value}");
        //        view.WidthRequest = value;
        //        updated?.Invoke(value);
        //    },
        //                                  view.Width,
        //                                  value,
        //                                  easing);

        //    animation.Commit(view,
        //                     "WidthToAnimation",
        //                     16,
        //                     (uint)lenght,
        //                     finished: (value, cancelled) => { finished?.Invoke(cancelled); });
        //}



        //public static void HeightTo(this View view,
        //                    double targetHeight,
        //                    double length,
        //                    Easing easing,
        //                    Action<bool> finished = null,
        //                    Action<double> updated = null)
        //{
        //    // Verifica o valor de largura atual a ser usado como ponto de partida
        //    double initialHeight = view.HeightRequest > 0 ? view.HeightRequest : view.Height;

        //    // Garante que a animação não comece do zero
        //    var animation = new Animation(value =>
        //    {
        //        view.HeightRequest = value;
        //        updated?.Invoke(value); // Chama o callback de atualização, se fornecido
        //    },
        //    initialHeight, // Inicia a animação a partir da largura atual
        //    targetHeight,  // Anima até o valor de largura desejado
        //    easing);

        //    animation.Commit(view,
        //                     "HeightToAnimation",
        //                     16, // Frame rate
        //                     (uint)length, // Duração da animação
        //                     finished: (value, cancelled) => finished?.Invoke(cancelled)); // Callback quando a animação termina
        //}


    }

}

