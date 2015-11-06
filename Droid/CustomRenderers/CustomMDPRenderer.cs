using System;
using mdp_right;
using mdp_right.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Support.V4.View;
using Android.Views;

[assembly: ExportRenderer(typeof(CustomMDP), typeof(CustomMDPRenderer))]
namespace mdp_right.Droid
{
    public class CustomMDPRenderer : PageRenderer
    {
        private GestureDetectorCompat mDetector;

        public CustomMDPRenderer()
            : base()
        {
            mDetector = new GestureDetectorCompat(Context, new MyGestureListener(this));
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            mDetector.OnTouchEvent(e);

            //var action = MotionEventCompat.GetActionMasked(e);
            var percent = 1 - ((Width - e.GetX()) / Width);
            switch (e.Action)
            {
                case MotionEventActions.Down:
                    ((CustomMDP)Element).DoDown(percent);
                    break;
                case MotionEventActions.Up:
                    ((CustomMDP)Element).DoUp(percent);
                    break;
                case MotionEventActions.Move:
                    ((CustomMDP)Element).DoMove(percent);
                    break;
                default:
                    break;
            }

            return base.OnTouchEvent(e);
        }

        public override bool DispatchTouchEvent(MotionEvent e)
        {
            OnTouchEvent(e);
            return base.DispatchTouchEvent(e);
        }

        public override bool OnInterceptTouchEvent(MotionEvent ev)
        {
            OnTouchEvent(ev);
            return base.OnInterceptTouchEvent(ev);
        }

        private class MyGestureListener : GestureDetector.SimpleOnGestureListener
        {
            private CustomMDPRenderer renderer;

            public MyGestureListener(CustomMDPRenderer renderer)
            {
                this.renderer = renderer;
            }

            public override bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
            {
                var percent1 = 1 - ((renderer.Width - e1.GetX()) / renderer.Width);
                var percent2 = 1 - ((renderer.Width - e2.GetX()) / renderer.Width);
                ((CustomMDP)renderer.Element).DoFling(percent1, percent2);
                return true;
            }
        }
    }
}

