﻿#pragma checksum "..\..\..\..\GUI\MainWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "D6609904C31E024353E5083ACD879CA1022299CAC205FD7F145FDB6C94C6C7E9"
//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.42000
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------

using FelicaLib;
using OxyPlot.Wpf;
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


namespace FericaReader {
    
    
    /// <summary>
    /// MainWindow
    /// </summary>
    public partial class MainWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 19 "..\..\..\..\GUI\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Button1;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\..\..\GUI\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label LabelResult;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\..\..\GUI\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid DataGridCardResult;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\..\..\GUI\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox TextBoxCalcValue;
        
        #line default
        #line hidden
        
        
        #line 33 "..\..\..\..\GUI\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid DataGridDBResult;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\..\..\GUI\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ButtonWeekly;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\..\..\GUI\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox ComboProcess;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\..\..\GUI\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ButtonViewDB;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\..\..\GUI\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ButtonMakeChart;
        
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
            System.Uri resourceLocater = new System.Uri("/FericaReader;component/gui/mainwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\GUI\MainWindow.xaml"
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
            this.Button1 = ((System.Windows.Controls.Button)(target));
            
            #line 19 "..\..\..\..\GUI\MainWindow.xaml"
            this.Button1.Click += new System.Windows.RoutedEventHandler(this.Button1_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this.LabelResult = ((System.Windows.Controls.Label)(target));
            return;
            case 3:
            this.DataGridCardResult = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 4:
            this.TextBoxCalcValue = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.DataGridDBResult = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 6:
            this.ButtonWeekly = ((System.Windows.Controls.Button)(target));
            
            #line 35 "..\..\..\..\GUI\MainWindow.xaml"
            this.ButtonWeekly.Click += new System.Windows.RoutedEventHandler(this.ButtonWeekly_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.ComboProcess = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 8:
            this.ButtonViewDB = ((System.Windows.Controls.Button)(target));
            
            #line 40 "..\..\..\..\GUI\MainWindow.xaml"
            this.ButtonViewDB.Click += new System.Windows.RoutedEventHandler(this.ButtonViewDB_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.ButtonMakeChart = ((System.Windows.Controls.Button)(target));
            
            #line 43 "..\..\..\..\GUI\MainWindow.xaml"
            this.ButtonMakeChart.Click += new System.Windows.RoutedEventHandler(this.ButtonMakeChart_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

