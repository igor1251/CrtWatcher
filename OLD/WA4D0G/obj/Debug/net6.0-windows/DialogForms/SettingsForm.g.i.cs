﻿#pragma checksum "..\..\..\..\DialogForms\SettingsForm.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "77B56C794F0C4BF7540243E504BB38DA27F10476"
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
using System.Windows.Controls.Ribbon;
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
using WA4D0G.DialogForms;


namespace WA4D0G.DialogForms {
    
    
    /// <summary>
    /// SettingsForm
    /// </summary>
    public partial class SettingsForm : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 23 "..\..\..\..\DialogForms\SettingsForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox daysCountTextBox;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\..\..\DialogForms\SettingsForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton personalKeyStoreRadioButton;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\..\..\DialogForms\SettingsForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton localDbRadioButton;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\..\..\DialogForms\SettingsForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button acceptButton;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\..\..\DialogForms\SettingsForm.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button cancelButton;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "6.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/WA4D0G;component/dialogforms/settingsform.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\DialogForms\SettingsForm.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "6.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.daysCountTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 2:
            this.personalKeyStoreRadioButton = ((System.Windows.Controls.RadioButton)(target));
            
            #line 25 "..\..\..\..\DialogForms\SettingsForm.xaml"
            this.personalKeyStoreRadioButton.Checked += new System.Windows.RoutedEventHandler(this.personalKeyStoreRadioButton_Checked);
            
            #line default
            #line hidden
            return;
            case 3:
            this.localDbRadioButton = ((System.Windows.Controls.RadioButton)(target));
            
            #line 26 "..\..\..\..\DialogForms\SettingsForm.xaml"
            this.localDbRadioButton.Checked += new System.Windows.RoutedEventHandler(this.localDbRadioButton_Checked);
            
            #line default
            #line hidden
            return;
            case 4:
            this.acceptButton = ((System.Windows.Controls.Button)(target));
            
            #line 29 "..\..\..\..\DialogForms\SettingsForm.xaml"
            this.acceptButton.Click += new System.Windows.RoutedEventHandler(this.acceptButton_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.cancelButton = ((System.Windows.Controls.Button)(target));
            
            #line 30 "..\..\..\..\DialogForms\SettingsForm.xaml"
            this.cancelButton.Click += new System.Windows.RoutedEventHandler(this.cancelButton_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

