using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace Xamarin.Forms.Platform.iOS
{
	public abstract class TemplatedCell : ItemsViewCell
	{
		protected nfloat ConstrainedDimension;

		[Export("initWithFrame:")]
		protected TemplatedCell(CGRect frame) : base(frame)
		{
		}

		public IVisualElementRenderer VisualElementRenderer { get; private set; }

		public override void ConstrainTo(nfloat constant)
		{
			ConstrainedDimension = constant;
		}

		public override UICollectionViewLayoutAttributes PreferredLayoutAttributesFittingAttributes(
			UICollectionViewLayoutAttributes layoutAttributes)
		{
			var nativeView = VisualElementRenderer.NativeView;

			var size = Measure();

			nativeView.Frame = new CGRect(CGPoint.Empty, size);
			VisualElementRenderer.Element.Layout(nativeView.Frame.ToRectangle());

			layoutAttributes.Frame = nativeView.Frame;

			return layoutAttributes;
		}

		public override void PrepareForReuse()
		{
			base.PrepareForReuse();
			ClearSubviews();
		}

		public void SetRenderer(IVisualElementRenderer renderer)
		{
			VisualElementRenderer = renderer;
			var nativeView = VisualElementRenderer.NativeView;

			InitializeContentConstraints(nativeView);
		}

		protected void Layout(CGSize constraints)
		{
			var nativeView = VisualElementRenderer.NativeView;

			var width = constraints.Width;
			var height = constraints.Height;

			VisualElementRenderer.Element.Measure(width, height, MeasureFlags.IncludeMargins);

			nativeView.Frame = new CGRect(0, 0, width, height);

			VisualElementRenderer.Element.Layout(nativeView.Frame.ToRectangle());
		}

		void ClearSubviews()
		{
			for (int n = ContentView.Subviews.Length - 1; n >= 0; n--)
			{
				ContentView.Subviews[n].RemoveFromSuperview();
			}
		}

		public override bool Selected
		{
			get => base.Selected;
			set
			{
				base.Selected = value;

				var element = VisualElementRenderer?.Element;

				if (element != null)
				{
					VisualStateManager.GoToState(element, value 
						? VisualStateManager.CommonStates.Selected 
						: VisualStateManager.CommonStates.Normal);
				}
			}
		}
	}
}