using Sociedade_Serial.Themes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Sociedade_Serial
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public enum Skin {Dark, Default}
    public partial class App : Application
    {
        private static Skin _skin = Skin.Default;
        public static Skin Skin { get => _skin; set {

                _skin = value;
                foreach (ResourceDictionary skinDic in App.Current.Resources.MergedDictionaries)
                    if (skinDic is SkinResourceDictionary)
                        ((SkinResourceDictionary)skinDic).UpdateSource();
            }
        }

        public void teste()
        {
            
        }



    }
}
