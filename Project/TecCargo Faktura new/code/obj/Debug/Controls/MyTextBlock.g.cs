﻿#pragma checksum "..\..\..\Controls\MyTextBlock.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "43CA7402EB9883BA7E0324C5A4A0F452"
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
using TecCargo_Faktura.Controls;
using TecCargo_Faktura.Models;


namespace TecCargo_Faktura.Controls {
    
    
    /// <summary>
    /// MyTextBlock
    /// </summary>
    public partial class MyTextBlock : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 8 "..\..\..\Controls\MyTextBlock.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal TecCargo_Faktura.Controls.MyTextBlock _Textblock;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\..\Controls\MyTextBlock.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label _hovertext;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\..\Controls\MyTextBlock.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock placeholder;
        
        #line default
        #line hidden
        
        
        #line 46 "..\..\..\Controls\MyTextBlock.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ScrollViewer textboxScroll;
        
        #line default
        #line hidden
        
        
        #line 53 "..\..\..\Controls\MyTextBlock.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Input;
        
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
            System.Uri resourceLocater = new System.Uri("/TecCargo Faktura System;component/controls/mytextblock.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Controls\MyTextBlock.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
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
            this._Textblock = ((TecCargo_Faktura.Controls.MyTextBlock)(target));
            
            #line 9 "..\..\..\Controls\MyTextBlock.xaml"
            this._Textblock.Loaded += new System.Windows.RoutedEventHandler(this._Textblock_Loaded);
            
            #line default
            #line hidden
            
            #line 10 "..\..\..\Controls\MyTextBlock.xaml"
            this._Textblock.MouseEnter += new System.Windows.Input.MouseEventHandler(this._Textblock_MouseEnter);
            
            #line default
            #line hidden
            
            #line 11 "..\..\..\Controls\MyTextBlock.xaml"
            this._Textblock.MouseLeave += new System.Windows.Input.MouseEventHandler(this._Textblock_MouseEnter);
            
            #line default
            #line hidden
            return;
            case 2:
            
            #line 19 "..\..\..\Controls\MyTextBlock.xaml"
            ((System.Windows.Controls.Grid)(target)).IsEnabledChanged += new System.Windows.DependencyPropertyChangedEventHandler(this._Grid_IsEnabledChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this._hovertext = ((System.Windows.Controls.Label)(target));
            
            #line 32 "..\..\..\Controls\MyTextBlock.xaml"
            this._hovertext.MouseEnter += new System.Windows.Input.MouseEventHandler(this._Textblock_MouseEnter);
            
            #line default
            #line hidden
            return;
            case 4:
            this.placeholder = ((System.Windows.Controls.TextBlock)(target));
            
            #line 44 "..\..\..\Controls\MyTextBlock.xaml"
            this.placeholder.IsVisibleChanged += new System.Windows.DependencyPropertyChangedEventHandler(this.placeholder_IsVisibleChanged);
            
            #line default
            #line hidden
            return;
            case 5:
            this.textboxScroll = ((System.Windows.Controls.ScrollViewer)(target));
            return;
            case 6:
            this.Input = ((System.Windows.Controls.TextBox)(target));
            
            #line 59 "..\..\..\Controls\MyTextBlock.xaml"
            this.Input.SizeChanged += new System.Windows.SizeChangedEventHandler(this.Input_SizeChanged);
            
            #line default
            #line hidden
            
            #line 60 "..\..\..\Controls\MyTextBlock.xaml"
            this.Input.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.Input_TextChanged);
            
            #line default
            #line hidden
            
            #line 61 "..\..\..\Controls\MyTextBlock.xaml"
            this.Input.LostFocus += new System.Windows.RoutedEventHandler(this.Input_LostFocus);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

