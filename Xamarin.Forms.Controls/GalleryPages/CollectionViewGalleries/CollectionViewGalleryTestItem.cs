using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms.Internals;

namespace Xamarin.Forms.Controls.GalleryPages.CollectionViewGalleries
{
	[Preserve(AllMembers = true)]
	public class CollectionViewGalleryTestItem : INotifyPropertyChanged
	{
		private string _caption;

		public DateTime Date { get; set; }

		public string Caption
		{
			get => _caption;
			set
			{
				_caption = value;
				OnPropertyChanged();
			}
		}

		public string Image { get; set; }

		public int Index { get; set; }
		public ICommand Command { get; set; }

		public CollectionViewGalleryTestItem(DateTime date, string caption, string image, int index)
		{
			Date = date;
			Caption = caption;
			Image = image;
			Index = index;

			Command = new Command(() => Caption += " Lorem ipsum dolor sit amet, qui eleifend adversarium ei, pro tamquam pertinax inimicus ut. Quis assentior ius no, ne vel modo tantas omnium, sint labitur id nec. Mel ad cetero repudiare definiebas, eos sint placerat cu.");
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public override string ToString()
		{
			return $"Item: {Index}";
		}

		protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}