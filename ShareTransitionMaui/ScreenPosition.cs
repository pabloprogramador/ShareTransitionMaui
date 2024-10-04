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
        private static DisplayInfo mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
        
        public static Microsoft.Maui.Graphics.Point GetAbsolutePosition(this IView view)
        {

#if IOS
            var fix = GetAbsolutePositioniOS(Shell.Current.CurrentPage);
            return GetAbsolutePositioniOS(view, fix.Y);
#elif ANDROID
            var fix = GetAbsolutePositionAndroid(Shell.Current.CurrentPage);
            return GetAbsolutePositionAndroid(view, fix.Y);
#else
        return new Point(0, 0);
#endif
        }

#if IOS
        static Microsoft.Maui.Graphics.Point GetAbsolutePositioniOS(IView view, double fixHeight = 0)
        {
            var handler = view?.Handler;
            var nativeView = handler?.PlatformView as UIView;
            if (nativeView != null)
            {
                CGRect absoluteFrame = nativeView.ConvertRectToView(nativeView.Bounds, null);
                return new Point(absoluteFrame.X, absoluteFrame.Y - fixHeight);
            }
            return new Point(0, -fixHeight);
        }

#elif ANDROID        
        static Microsoft.Maui.Graphics.Point GetAbsolutePositionAndroid(IView view, double fixHeight = 0)
        {
            var handler = view?.Handler;
            var nativeView = handler?.PlatformView as Android.Views.View;
            if (nativeView != null)
            {

                int[] location = new int[2];
                nativeView.GetLocationOnScreen(location);

                return new Microsoft.Maui.Graphics.Point(location[0] / mainDisplayInfo.Density, (location[1] / mainDisplayInfo.Density) - fixHeight);
            }
            return new Microsoft.Maui.Graphics.Point(0, -fixHeight);
        }
#endif

    }
}