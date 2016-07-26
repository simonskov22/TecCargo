using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace TecCargo_Faktura.Models
{
    public static class CustomCommands
    {
        public static readonly RoutedUICommand Exit = new RoutedUICommand
                           (
                                   "Exit",
                                   "Exit",
                                   typeof(CustomCommands),
                                   new InputGestureCollection()
                                   {
                                        new KeyGesture(Key.F4, ModifierKeys.Alt)
                                   }
                           );
    }
}
