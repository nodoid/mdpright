using Xamarin.Forms;
using System.Threading.Tasks;
using System;

namespace mdp_right
{
    public class CustomMDP : MultiPage<Page>
    {
        protected override Page CreateDefault(object item)
        { 
            return new ContentPage();
        }

        public Page Master { set; get; }

        public Page Detail { set; get; }

        public enum OrientationType
        {
            Left,
            Right,
            Undefined
        }

        public event EventHandler Entered;
        public event EventHandler Exited;

        public void RaiseEntered()
        {
            if (Entered != null)
                Entered(this, EventArgs.Empty);
        }

        public void RaiseExited()
        {
            if (Exited != null)
                Exited(this, EventArgs.Empty);
        }

        public OrientationType Orientation { get; set; }

        public float MasterPercent { get; set; }

        bool masterMoving, masterExtended, flingOpen, flingClosed;

        float masterOffSet;

        public CustomMDP()
        {
            masterMoving = masterExtended = flingOpen = flingClosed = false;
            masterOffSet = 0;
            SizeChanged += (s, e) =>
            {
                DoAppear();
            };
        }

        public async void DoAppear()
        {
            while (Width == -1)
                await Task.Delay(1); 

            if (ParentView != null)
            {
                if (ParentView.GetType() == typeof(TabbedPage) && ((TabbedPage)ParentView).Children.IndexOf(this) > 0)
                {
                    Children.Add(Detail);
                    Children.Add(Master);
                }
                else
                {
                    Children.Add(Master);
                    Children.Add(Detail);
                }
            }
            else
            {
                Children.Add(Master);
                Children.Add(Detail);
            }
            Detail.Layout(new Rectangle(0, 0, Width, Height));
            Master.Layout(GetMasterClosedRectangle());       
        }

        private bool Left()
        {
            return Orientation == OrientationType.Left;
        }

        private bool Right()
        {
            return Orientation == OrientationType.Right;
        }

        private Rectangle GetMasterClosedRectangle()
        {
            if (Right())
                return new Rectangle(Width, 0, Width * MasterPercent, Height);
            else if (Left())
                return new Rectangle(0 - Width, 0, Width * MasterPercent, Height);
            return new Rectangle(0, 0, 0, 0);
        }

        private Rectangle GetMasterOpenedRectangle()
        {
            if (Right())
                return new Rectangle(Width * (1 - MasterPercent), 0, Width * MasterPercent, Height);
            else if (Left())
                return new Rectangle(0, 0, Width * MasterPercent, Height);
            return new Rectangle(0, 0, 0, 0);
        }

        public void DoMove(float percent)
        {
            if (masterMoving && !OutsideMasterWidth(percent + masterOffSet))
                SetMasterPosition(percent + masterOffSet);
        }

        private async void SetMasterPosition(float percent)
        {
            if (Right())
                await Master.LayoutTo(new Rectangle(Width * percent, 0, Width * MasterPercent, Height), 1);
            else if (Left())
                await Master.LayoutTo(new Rectangle((Width * percent) - (Width * MasterPercent), 0, Width * MasterPercent, Height), 1);
        }

        public void DoUp(float percent)
        {
            if (masterMoving)
            {
                if (flingOpen)
                {
                    DoSmoothOpen();
                    flingOpen = false;
                }
                else if (flingClosed)
                {
                    DoSmoothClose();
                    flingClosed = false;
                }
                else if (BeyondMasterHalfWay(percent + masterOffSet))
                    DoSmoothOpen();
                else if (!BeyondMasterHalfWay(percent + masterOffSet))
                    DoSmoothClose();
                masterMoving = false;
            }
        }

        private bool BeyondMasterHalfWay(float percent)
        {
            if (Right())
                return percent <= 1 - (MasterPercent / 2);
            else if (Left())
                return percent > MasterPercent / 2;
            return false;
        }

        public void DoDown(float percent)
        {
            if (!masterExtended)
            {
                if (WithinScreenEdge(percent))
                {
                    masterMoving = true;
                    Detail.IsEnabled = false;
                }
            }
            else
            {
                if (OutsideMasterWidth(percent))
                {
                    DoSmoothClose();
                }
                else
                {
                    SetOffSet(percent);
                    masterMoving = true;
                }
            }
        }

        private void SetOffSet(float percent)
        {
            if (Right())
                masterOffSet = 0 - (percent - (1 - MasterPercent));
            else if (Left())
                masterOffSet = MasterPercent - percent;
        }

        private bool WithinScreenEdge(float percent)
        {
            if (Right())
                return percent >= 0.98;
            else if (Left())
                return percent <= 0.02;
            return false;
        }

        private bool OutsideMasterWidth(float percent)
        {
            if (Right())
                return percent < (1 - MasterPercent);
            else if (Left())
                return percent > MasterPercent;
            return false;
        }

        public void DoFling(float percent1, float percent2)
        {
            if (!masterExtended)
            {
                if (WithinScreenEdge(percent1) && IsOpenFling(percent1, percent2))
                {
                    flingOpen = true;
                    flingClosed = false;
                }
            }
            else
            {
                if (!IsOpenFling(percent1, percent2))
                {
                    flingClosed = true;
                    flingOpen = false;
                }
            }
        }

        private bool IsOpenFling(float percent1, float percent2)
        {
            if (Right())
                return percent1 > percent2;
            else if (Left())
                return percent1 < percent2;
            return false;
        }

        private async void DoSmoothOpen()
        {
            FadeOutDetail();
            await Master.LayoutTo(GetMasterOpenedRectangle(), 250, Easing.SinOut);
            masterExtended = true;
            masterOffSet = 0;
        }

        private async void FadeOutDetail()
        {
            await Detail.FadeTo(0.4);
        }

        private async void DoSmoothClose()
        {
            FadeInDetail();
            await Master.LayoutTo(GetMasterClosedRectangle(), 250, Easing.SinOut);
            masterExtended = false;
            Detail.IsEnabled = true;
            masterOffSet = 0;
        }

        private async void FadeInDetail()
        {
            await Detail.FadeTo(1);
        }
    }
}

