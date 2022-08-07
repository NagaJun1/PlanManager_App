using PlanManager.pages;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlanManager
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // NavigationPage()を設定しないと、Navigationでエラーになる
            this.MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
        }

        /// <summary>
        /// 画面非表示時の処理
        /// </summary>
        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
