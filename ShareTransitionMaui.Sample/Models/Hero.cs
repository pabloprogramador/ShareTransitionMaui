using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ShareTransitionMaui.Sample.Models
{
	public class Hero : INotifyPropertyChanged
    {
		public string Name { get; set; }
		public string Price { get; set; }
		public string Color1 { get; set; }
		public string Color2 { get; set; }
        public string Image { get; set; }

        private string _classIdTitle;
        public string ClassIdTitle
        {
            get { return _classIdTitle; }
            set
            {
                if (_classIdTitle != value)
                {
                    _classIdTitle = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _classIdPrice;
        public string ClassIdPrice
        {
            get { return _classIdPrice; }
            set
            {
                if (_classIdPrice != value)
                {
                    _classIdPrice = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _classIdImage;
        public string ClassIdImage
        {
            get { return _classIdImage; }
            set
            {
                if (_classIdImage != value)
                {
                    _classIdImage = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _classIdBorder;
        public string ClassIdBorder
        {
            get { return _classIdBorder; }
            set
            {
                if (_classIdBorder != value)
                {
                    _classIdBorder = value;
                    OnPropertyChanged();
                }
            }
        }

        private double _position;
        public double Position
        {
            get { return _position; }
            set
            {
                if (_position != value)
                {
                    _position = value;
                    OnPropertyChanged();
                }
            }
        }

        private double _size;
        public double Size
        {
            get { return _size; }
            set
            {
                if (_size != value)
                {
                    _size = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}