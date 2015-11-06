using System;
using mdp_right.iOS;
using mdp_right;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using Foundation;

[assembly: ExportRenderer(typeof(CustomMDP), typeof(CustomMDPRenderer))]
namespace mdp_right.iOS
{
    public class CustomMDPRenderer : PageRenderer
    {
        private CustomMDP last;

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);
            var touch = touches.AnyObject as UITouch;

            if (touch != null)
            {
                var target = GetTouchTarget(touch);
                if (target != null)
                    target.RaiseEntered();
                last = target;
            }
        }

        public override void TouchesMoved(NSSet touches, UIEvent evt)
        {
            base.TouchesMoved(touches, evt);
            var touch = touches.AnyObject as UITouch;

            if (touch != null)
            {
                var target = GetTouchTarget(touch);

                if (last != target)
                {
                    if (target != null)
                        target.RaiseEntered();
                    if (last != null)
                        last.RaiseExited();
                    last = target;
                }
            }
        }

        public override void TouchesCancelled(NSSet touches, UIEvent evt)
        {
            base.TouchesCancelled(touches, evt);
            var touch = touches.AnyObject as UITouch;

            if (touch != null)
            {
                var target = GetTouchTarget(touch);
                if (target != null)
                    target.RaiseExited();
                last = null;
            }
        }

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            base.TouchesEnded(touches, evt);
            var touch = touches.AnyObject as UITouch;

            if (touch != null)
            {
                var target = GetTouchTarget(touch);
                if (target != null)
                    target.RaiseExited();
                last = null;
            }
        }

        private CustomMDP GetTouchTarget(UITouch touch)
        {
            CustomMDP target = null;
            var grid = Element.FindByName<ContentPage>("Detail");

            var point = touch.LocationInView(this.View);
            var x = point.X - grid.X;
            var y = point.Y - grid.Y;

            var content = grid.Content as StackLayout;
            foreach (var child in content.Children)
            {
                if (child.Bounds.Contains(x, y))
                {
                    break;
                }
            }

            return target;
        }
    }
}
