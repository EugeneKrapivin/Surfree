using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terminal.Gui;

namespace Surfree.Host;

internal class MainWindow : Window
{
    public MainWindow()
    {
        Width = Dim.Fill(0);
        Height = Dim.Fill(0);
        X = 0;
        Y = 0;
        Visible = true;
        Modal = false;
        Data = "window";
        Title = "Surfree";
        CanFocus = false;
    }
}
