﻿#pragma checksum "..\..\..\View\OpenList.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "E7E8EA79D2A2A46BAE2C107592814C55"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
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


namespace TecCargo_Læreplads_DB.View {
    
    
    /// <summary>
    /// OpenList
    /// </summary>
    public partial class OpenList : System.Windows.Window, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 29 "..\..\..\View\OpenList.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox TextBox_Search;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\..\View\OpenList.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView ListViewOpenFileNames;
        
        #line default
        #line hidden
        
        /// <summary>
        /// TextBoxFileName_name Name Field
        /// </summary>
        
        #line 48 "..\..\..\View\OpenList.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        public System.Windows.Controls.Label TextBoxFileName_name;
        
        #line default
        #line hidden
        
        
        #line 53 "..\..\..\View\OpenList.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ButtonFileName_save;
        
        #line default
        #line hidden
        
        
        #line 54 "..\..\..\View\OpenList.xaml"
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
            System.Uri resourceLocater = new System.Uri("/TecCargo Læreplads DB;component/view/openlist.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\View\OpenList.xaml"
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
            this.TextBox_Search = ((System.Windows.Controls.TextBox)(target));
            
            #line 29 "..\..\..\View\OpenList.xaml"
            this.TextBox_Search.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.TextBox_TextChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            this.ListViewOpenFileNames = ((System.Windows.Controls.ListView)(target));
            
            #line 32 "..\..\..\View\OpenList.xaml"
            this.ListViewOpenFileNames.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.ListViewOpenFileNames_SelectionChanged);
            
            #line default
            #line hidden
            
            #line 32 "..\..\..\View\OpenList.xaml"
            this.ListViewOpenFileNames.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.ListViewOpenFileNames_MouseDoubleClick);
            
            #line default
            #line hidden
            return;
            case 4:
            this.TextBoxFileName_name = ((System.Windows.Controls.Label)(target));
            return;
            case 5:
            this.ButtonFileName_save = ((System.Windows.Controls.Button)(target));
            
            #line 53 "..\..\..\View\OpenList.xaml"
            this.ButtonFileName_save.Click += new System.Windows.RoutedEventHandler(this.ButtonFileName_save_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.ButtonFileName_canel = ((System.Windows.Controls.Button)(target));
            
            #line 54 "..\..\..\View\OpenList.xaml"
            this.ButtonFileName_canel.Click += new System.Windows.RoutedEventHandler(this.ButtonFileName_cancel_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 3:
            
            #line 39 "..\..\..\View\OpenList.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ListViewButton_Delete_Click);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}

