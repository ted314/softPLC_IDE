﻿#pragma checksum "..\..\ChooseBoard.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "364D7B9DEA9A8E3F13B9DF76C05CB4F32F5F189E"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using PLC;
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


namespace PLC {
    
    
    /// <summary>
    /// ChooseBoard
    /// </summary>
    public partial class ChooseBoard : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 16 "..\..\ChooseBoard.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button MSP430;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\ChooseBoard.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button LPC2119;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\ChooseBoard.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button STM32;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\ChooseBoard.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Arduino;
        
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
            System.Uri resourceLocater = new System.Uri("/PLC;component/chooseboard.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\ChooseBoard.xaml"
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
            this.MSP430 = ((System.Windows.Controls.Button)(target));
            
            #line 16 "..\..\ChooseBoard.xaml"
            this.MSP430.Click += new System.Windows.RoutedEventHandler(this.Board_Clicked);
            
            #line default
            #line hidden
            return;
            case 2:
            this.LPC2119 = ((System.Windows.Controls.Button)(target));
            
            #line 19 "..\..\ChooseBoard.xaml"
            this.LPC2119.Click += new System.Windows.RoutedEventHandler(this.Board_Clicked);
            
            #line default
            #line hidden
            return;
            case 3:
            this.STM32 = ((System.Windows.Controls.Button)(target));
            
            #line 22 "..\..\ChooseBoard.xaml"
            this.STM32.Click += new System.Windows.RoutedEventHandler(this.Board_Clicked);
            
            #line default
            #line hidden
            return;
            case 4:
            this.Arduino = ((System.Windows.Controls.Button)(target));
            
            #line 25 "..\..\ChooseBoard.xaml"
            this.Arduino.Click += new System.Windows.RoutedEventHandler(this.Board_Clicked);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

