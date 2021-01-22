using System.IO.Ports;
using System.Windows.Controls;
using System.Collections.Generic;
using System;
namespace Sociedade_Serial
{
    /// <summary>
    /// Interação lógica para Test.xam
    /// </summary>
    public partial class Test : UserControl
    {

        private SerialPort port;
        private List<command> commands;

        public Test()
        {
            InitializeComponent();
        }
    }
}
