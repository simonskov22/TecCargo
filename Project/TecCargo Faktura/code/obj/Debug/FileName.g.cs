﻿#pragma checksum "..\..\FileName.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "8C791E91C7212E4E2C5C8F50AA3F1634"
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


namespace TecCargo_Faktura {
    
    
    /// <summary>
    /// FileName
    /// </summary>
    public partial class FileName : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 13 "..\..\FileName.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label windowTitle;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\FileName.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView ListViewOpenFileNames;
        
        #line default
        #line hidden
        
        /// <summary>
        /// TextBoxFileName_name Name Field
        /// </summary>
        
        #line 43 "..\..\FileName.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        public System.Windows.Controls.Label TextBoxFileName_name;
        
        #line default
        #line hidden
        
        
        #line 46 "..\..\FileName.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ButtonFileName_save;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\FileName.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ButtonFileName_canel;
        
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
            System.Uri resourceLocater = new System.Uri("/TecCargo Faktura System;component/filename.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\FileName.xaml"
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
            
            #line 4 "..\..\FileName.xaml"
            ((TecCargo_Faktura.FileName)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.windowTitle = ((System.Windows.Controls.Label)(target));
            return;
            case 3:
            
            #line 27 "..\..\FileName.xaml"
            ((System.Windows.Controls.TextBox)(target)).TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.TextBox_TextChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.ListViewOpenFileNames = ((System.Windows.Controls.ListView)(target));
            
            #line 30 "..\..\FileName.xaml"
            this.ListViewOpenFileNames.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.ListViewOpenFileNames_SelectionChanged);
            
            #line default
            #line hidden
            
            #line 30 "..\..\FileName.xaml"
            this.ListViewOpenFileNames.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.ListViewOpenFileNames_MouseDoubleClick);
            
            #line default
            #line hidden
            return;
            case 5:
            this.TextBoxFileName_name = ((System.Windows.Controls.Label)(target));
            return;
            case 6:
            this.ButtonFileName_save = ((System.Windows.Controls.Button)(target));
            
            #line 46 "..\..\FileName.xaml"
            this.ButtonFileName_save.Click += new System.Windows.RoutedEventHandler(this.ButtonFileName_save_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.ButtonFileName_canel = ((System.Windows.Controls.Button)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

