﻿#pragma checksum "..\..\..\windows\ExcSelectUI.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "67232DF0B374F8FE2B7D8CC06CFDB27524DF2881"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using Infragistics;
using Infragistics.Collections;
using Infragistics.Controls;
using Infragistics.Controls.Charts;
using Infragistics.Controls.Editors;
using Infragistics.Controls.Interactions;
using Infragistics.Controls.Interactions.Primitives;
using Infragistics.Controls.Maps;
using Infragistics.Controls.Menus;
using Infragistics.DragDrop;
using Infragistics.Themes;
using Shris.NewEnergy.iPlant.Device;
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


namespace Shris.NewEnergy.iPlant.Device {
    
    
    /// <summary>
    /// ExcSelectUI
    /// </summary>
    public partial class ExcSelectUI : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 83 "..\..\..\windows\ExcSelectUI.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock TB_Title;
        
        #line default
        #line hidden
        
        
        #line 85 "..\..\..\windows\ExcSelectUI.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border Border_AllSelect;
        
        #line default
        #line hidden
        
        
        #line 97 "..\..\..\windows\ExcSelectUI.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock TB_Search;
        
        #line default
        #line hidden
        
        
        #line 98 "..\..\..\windows\ExcSelectUI.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox TE_CallContent;
        
        #line default
        #line hidden
        
        
        #line 103 "..\..\..\windows\ExcSelectUI.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Shris.NewEnergy.iPlant.Device.SelectItemUC SIU_Items;
        
        #line default
        #line hidden
        
        
        #line 105 "..\..\..\windows\ExcSelectUI.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid Grid_SearchTip;
        
        #line default
        #line hidden
        
        
        #line 112 "..\..\..\windows\ExcSelectUI.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border BT_Save;
        
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
            System.Uri resourceLocater = new System.Uri("/iPlantMLM;component/windows/excselectui.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\windows\ExcSelectUI.xaml"
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
            
            #line 74 "..\..\..\windows\ExcSelectUI.xaml"
            ((System.Windows.Controls.Border)(target)).PreviewMouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.TextBlock_PreviewMouseLeftButtonUp);
            
            #line default
            #line hidden
            return;
            case 2:
            this.TB_Title = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 3:
            this.Border_AllSelect = ((System.Windows.Controls.Border)(target));
            
            #line 85 "..\..\..\windows\ExcSelectUI.xaml"
            this.Border_AllSelect.PreviewMouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.Border_PreviewMouseLeftButtonUp_1);
            
            #line default
            #line hidden
            return;
            case 4:
            this.TB_Search = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 5:
            this.TE_CallContent = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.SIU_Items = ((Shris.NewEnergy.iPlant.Device.SelectItemUC)(target));
            return;
            case 7:
            this.Grid_SearchTip = ((System.Windows.Controls.Grid)(target));
            return;
            case 8:
            this.BT_Save = ((System.Windows.Controls.Border)(target));
            
            #line 112 "..\..\..\windows\ExcSelectUI.xaml"
            this.BT_Save.MouseUp += new System.Windows.Input.MouseButtonEventHandler(this.BT_Save_MouseUp);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

