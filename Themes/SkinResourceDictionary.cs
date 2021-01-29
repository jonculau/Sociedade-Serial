using System;
using System.Collections.Generic;
using System.Text;

namespace Sociedade_Serial.Themes
{
    class SkinResourceDictionary: System.Windows.ResourceDictionary
    {
        private Uri _LightSource;
        private Uri _DarkSource;

        public Uri LightSource
        {
            get { return _LightSource; }
            set
            {
                _LightSource = value;
                UpdateSource();
            }
        }
        public Uri DarkSource
        {
            get { return _DarkSource; }
            set
            {
                _DarkSource = value;
                UpdateSource();
            }
        }

        public void UpdateSource()
        {    
            base.Source = App.Skin == Skin.Dark ? DarkSource : LightSource;
        }
    }
}
