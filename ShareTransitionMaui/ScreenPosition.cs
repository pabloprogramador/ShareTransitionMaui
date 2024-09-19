using System;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using Microsoft.Maui.Controls;

#if IOS
using CoreGraphics;
using UIKit;
#endif

#if ANDROID
using Android.Graphics;
using Android.Views;
using Microsoft.Maui.Platform;
#endif

namespace ShareTransitionMaui
{
    public static class ViewExtensions
    {
        // Método de extensão para calcular a posição absoluta
        public static Microsoft.Maui.Graphics.Point GetAbsolutePosition(this IView view)
        {
            // Chama a função específica para a plataforma
#if IOS
        return GetAbsolutePositioniOS(view);
#elif ANDROID
            return GetAbsolutePositionAndroid(view);
#else
        return new Point(0, 0); // Outras plataformas podem ser tratadas aqui
#endif
        }

#if IOS
        // Implementação para iOS
        static Microsoft.Maui.Graphics.Point GetAbsolutePositioniOS(IView view)
        {
            // Obter a UIView associada à View MAUI
            var handler = view?.Handler;
            var nativeView = handler?.PlatformView as UIView;
            if (nativeView != null)
            {
                CGRect absoluteFrame = nativeView.ConvertRectToView(nativeView.Bounds, null);
                return new Point(absoluteFrame.X, absoluteFrame.Y);
            }
            return new Point(0, 0);
        }

#elif ANDROID        
        // Implementação para Android
        static Microsoft.Maui.Graphics.Point GetAbsolutePositionAndroid(IView view)
        {

            var handler = view?.Handler;
            var nativeView = handler?.PlatformView as Android.Views.View;
            if (nativeView != null)
            {

                int[] location = new int[2];
                nativeView.GetLocationOnScreen(location);

                int fix = 0;
                if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.R)
                {
                    var insets = nativeView.RootWindowInsets;

                    // Obter as áreas do sistema (barra de status, barra de navegação, etc.)
                    var statusBarInsets = insets.GetInsets(WindowInsets.Type.StatusBars());
                    fix = statusBarInsets.Top/2;
                }
                else
                {
                    var window = Platform.CurrentActivity.Window;
                    var flags = window.DecorView.SystemUiVisibility;

                    // Verifica se o aplicativo está em tela cheia (fullscreen)
                    bool isFullscreen = (flags & (StatusBarVisibility)SystemUiFlags.Fullscreen) == (StatusBarVisibility)SystemUiFlags.Fullscreen;


                    fix = isFullscreen ? 0 : 25;
                }


                return new Microsoft.Maui.Graphics.Point(location[0] / 2, location[1] / 2 - fix);
            }
            return new Microsoft.Maui.Graphics.Point(0, 0);
        }


#endif

    }
}