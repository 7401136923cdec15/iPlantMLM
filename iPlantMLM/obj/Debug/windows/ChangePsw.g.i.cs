﻿#pragma checksum "..\..\..\windows\ChangePsw.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "76040A82B9A56A5DC66A8A64A8786EF5BB44F700"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
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


namespace iPlantMLM {
    
    
    /// <summary>
    /// ChangePsw
    /// </summary>
    public partial class ChangePsw : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 19 "..\..\..\windows\ChangePsw.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox Tbx_OldPsw;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\..\windows\ChangePsw.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox Tbx_NewPsw;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\..\windows\ChangePsw.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox Tbx_ConfirmPsw;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\..\windows\ChangePsw.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Btn_Confirm;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\..\windows\ChangePsw.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Btn_Cancel;
        
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
            System.Uri resourceLocater = new System.Uri("/iPlantMLM;component/windows/changepsw.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\windows\ChangePsw.xaml"
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
            this.Tbx_OldPsw = ((System.Windows.Controls.PasswordBox)(target));
            return;
            case 2:
            this.Tbx_NewPsw = ((System.Windows.Controls.PasswordBox)(target));
            return;
            case 3:
            this.Tbx_ConfirmPsw = ((System.Windows.Controls.PasswordBox)(target));
            return;
            case 4:
            this.Btn_Confirm = ((System.Windows.Controls.Button)(target));
            
            #line 28 "..\..\..\windows\ChangePsw.xaml"
            this.Btn_Confirm.Click += new System.Windows.RoutedEventHandler(this.Btn_Confirm_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.Btn_Cancel = ((System.Windows.Controls.Button)(target));
            
            #line 35 "..\..\..\windows\ChangePsw.xaml"
            this.Btn_Cancel.Click += new System.Windows.RoutedEventHandler(this.Btn_Cancel_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
