﻿//      *********    DO NOT MODIFY THIS FILE     *********
//      This file is regenerated by a design tool. Making
//      changes to this file can cause errors.
namespace Expression.Blend.SampleData.KeyList
{
	using System; 

// To significantly reduce the sample data footprint in your production application, you can set
// the DISABLE_SAMPLE_DATA conditional compilation constant and disable sample data at runtime.
#if DISABLE_SAMPLE_DATA
	internal class Keys { }
#else

	public class Keys : System.ComponentModel.INotifyPropertyChanged
	{
		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}

		public Keys()
		{
			try
			{
				System.Uri resourceUri = new System.Uri("/DIS;component/SampleData/KeyList/KeyList.xaml", System.UriKind.Relative);
				if (System.Windows.Application.GetResourceStream(resourceUri) != null)
				{
					System.Windows.Application.LoadComponent(this, resourceUri);
				}
			}
			catch (System.Exception)
			{
			}
		}

		private KeyCollection _KeyCollection = new KeyCollection();

		public KeyCollection KeyCollection
		{
			get
			{
				return this._KeyCollection;
			}
		}
	}

	public class Key : System.ComponentModel.INotifyPropertyChanged
	{
		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}

		private double _productkeyid = 0;

		public double productkeyid
		{
			get
			{
				return this._productkeyid;
			}

			set
			{
				if (this._productkeyid != value)
				{
					this._productkeyid = value;
					this.OnPropertyChanged("productkeyid");
				}
			}
		}

		private string _productkey = string.Empty;

		public string productkey
		{
			get
			{
				return this._productkey;
			}

			set
			{
				if (this._productkey != value)
				{
					this._productkey = value;
					this.OnPropertyChanged("productkey");
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

		private string _status = string.Empty;

		public string status
		{
			get
			{
				return this._status;
			}

			set
			{
				if (this._status != value)
				{
					this._status = value;
					this.OnPropertyChanged("status");
				}
			}
		}

		private string _hardwareid = string.Empty;

		public string hardwareid
		{
			get
			{
				return this._hardwareid;
			}

			set
			{
				if (this._hardwareid != value)
				{
					this._hardwareid = value;
					this.OnPropertyChanged("hardwareid");
				}
			}
		}

		private string _modifieddate = string.Empty;

		public string modifieddate
		{
			get
			{
				return this._modifieddate;
			}

			set
			{
				if (this._modifieddate != value)
				{
					this._modifieddate = value;
					this.OnPropertyChanged("modifieddate");
				}
			}
		}
	}

	public class KeyCollection : System.Collections.ObjectModel.ObservableCollection<Key>
	{ 
	}
#endif
}
