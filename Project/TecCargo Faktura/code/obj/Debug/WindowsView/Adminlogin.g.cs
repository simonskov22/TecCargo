﻿#pragma checksum "..\..\..\WindowsView\Adminlogin.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "4E55A1B72E07692035BC30FDD544BDDA"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace TecCargo_Faktura.WindowsView {
    
    
    /// <summary>
    /// Adminlogin
    /// </summary>
    public partial class Adminlogin : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        /// <summary>
        /// TextboxAdmin_Username Name Field
        /// </summary>
        
        #line 8 "..\..\..\WindowsView\Adminlogin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        public System.Windows.Controls.TextBox TextboxAdmin_Username;
        
        #line default
        #line hidden
        
        /// <summary>
        /// TextboxAdmin_Password Name Field
        /// </summary>
        
        #line 9 "..\..\..\WindowsView\Adminlogin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        public System.Windows.Controls.PasswordBox TextboxAdmin_Password;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/TecCargo Faktura System;component/windowsview/adminlogin.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\WindowsView\Adminlogin.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.TextboxAdmin_Username = ((System.Windows.Controls.TextBox)(target));
            return;
            case 2:
            this.TextboxAdmin_Password = ((System.Windows.Controls.PasswordBox)(target));
            return;
            case 3:
            
            #line 10 "..\..\..\WindowsView\Adminlogin.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click_Cancel);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 11 "..\..\..\WindowsView\Adminlogin.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click_Login);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 12 "..\..\..\WindowsView\Adminlogin.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

