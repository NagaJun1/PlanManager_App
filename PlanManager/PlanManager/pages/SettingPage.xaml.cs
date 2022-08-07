using PlanManager.common;
using PlanManager.controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlanManager.pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingPage : ContentPage
    {
        /// <summary>
        /// 既存の設定値の取得
        /// </summary>
        private AppSettingController ThisAppSetting { get; set; }

        internal SettingPage(AppSettingController appSetting)
        {
            InitializeComponent();
            this.ThisAppSetting = appSetting;

            // ページ消去時に設定を保存
            this.Disappearing += SettingPage_Disappearing;

            // 言語の選択肢を登録
            this.SettingPickerLanguage();
        }

        /// <summary>
        /// "PickerLanguage"を設定
        /// </summary>
        private void SettingPickerLanguage()
        {
            // "Picker"に選択肢となる言語を登録
            for (int index = 0; index < AppSettingController.LANGUAGE.Length; index++)
                this.PickerLanguage.Items.Add(AppSettingController.LANGUAGE[index]);

            // "Picker"のインデックスを記録されている内容で登録
            this.PickerLanguage.SelectedIndex = this.ThisAppSetting.GetLanguageIndex();
        }

        /// <summary>
        /// ページ消去時に設定を保存
        /// </summary>
        private void SettingPage_Disappearing(object sender, EventArgs e)
        {
            // 選択されている言語のインデックスを保存
            this.ThisAppSetting.SaveLanguage(this.PickerLanguage.SelectedIndex);
        }
    }
}