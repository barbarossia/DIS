﻿//      *********    DO NOT MODIFY THIS FILE     *********
//      This file is regenerated by a design tool. Making
//      changes to this file can cause errors.
namespace Expression.Blend.SampleData.SearchPartList
{
	using System; 

// To significantly reduce the sample data footprint in your production application, you can set
// the DISABLE_SAMPLE_DATA conditional compilation constant and disable sample data at runtime.
#if DISABLE_SAMPLE_DATA
	internal class Parts { }
#else

	public class Parts : System.ComponentModel.INotifyPropertyChanged
	{
		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}

		public Parts()
		{
			try
			{
				System.Uri resourceUri = new System.Uri("/DIS;component/SampleData/SearchPartList/SearchPartList.xaml", System.UriKind.Relative);
				if (System.Windows.Application.GetResourceStream(resourceUri) != null)
				{
					System.Windows.Application.LoadComponent(this, resourceUri);
				}
			}
			catch (System.Exception)
			{
			}
		}

		private PartCollection _PartCollection = new PartCollection();

		public PartCollection PartCollection
		{
			get
			{
				return this._PartCollection;
			}
		}
	}

	public class Part : System.ComponentModel.INotifyPropertyChanged
	{
		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}

		private string _mspartname = string.Empty;

		public string mspartname
		{
			get
			{
				return this._mspartname;
			}

			set
			{
				if (this._mspartname != value)
				{
					this._mspartname = value;
					this.OnPropertyChanged("mspartname");
				}
			}
		}

		private string _mspartnumber = string.Empty;

		public string mspartnumber
		{
			get
			{
				return this._mspartnumber;
			}

			set
			{
				if (this._mspartnumber != value)
				{
					this._mspartnumber = value;
					this.OnPropertyChanged("mspartnumber");
				}
			}
		}

		private string _oempartname = string.Empty;

		public string oempartname
		{
			get
			{
				return this._oempartname;
			}

			set
			{
				if (this._oempartname != value)
				{
					this._oempartname = value;
					this.OnPropertyChanged("oempartname");
				}
			}
		}

		private string _oempartnumber = string.Empty;

		public string oempartnumber
		{
			get
			{
				return this._oempartnumber;
			}

			set
			{
				if (this._oempartnumber != value)
				{
					this._oempartnumber = value;
					this.OnPropertyChanged("oempartnumber");
				}
			}
		}
	}

	public class PartCollection : System.Collections.ObjectModel.ObservableCollection<Part>
	{ 
	}
#endif
}