﻿namespace Xamarin.Forms.Controls.GalleryPages.CollectionViewGalleries.ItemSizeGalleries
{
	internal class DynamicItemSizeGallery : ContentPage
	{
		public DynamicItemSizeGallery(IItemsLayout itemsLayout)
		{
			var layout = new Grid
			{
				RowDefinitions = new RowDefinitionCollection
				{
					new RowDefinition { Height = GridLength.Auto },
					new RowDefinition { Height = GridLength.Auto },
					new RowDefinition { Height = GridLength.Star }
				}
			};

			var instructions = new Label
			{
				Text = "Tapping the items below should update their text, and the items should expand to accomodate the larger text."
			};

			var itemTemplate = ExampleTemplates.ExpandingTextTemplate();

			var collectionView = new CollectionView
			{
				ItemsLayout = itemsLayout,
				ItemTemplate = itemTemplate,
				AutomationId = "collectionview"
			};

			var generator = new ItemsSourceGenerator(collectionView, initialItems: 20);

			layout.Children.Add(generator);
			layout.Children.Add(instructions);
			layout.Children.Add(collectionView);

			Grid.SetRow(instructions, 1);
			Grid.SetRow(collectionView, 2);

			Content = layout;

			generator.GenerateItems();
		}
	}
}