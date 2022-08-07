using PlanManager.common;
using PlanManager.controller;
using PlanManager.controller.model;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlanManager.views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlanElementLayout : StackLayout
    {
        /// <summary>
        /// このレイアウト上に表示される予定
        /// </summary>
        internal Plan ThisPlan { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="plan">予定情報</param>
        /// <param name="jp">日本語表示判定</param>
        internal PlanElementLayout(TapGestureRecognizer tapGesture, Plan plan, MainPage mainPage)
        {
            InitializeComponent();
            this.ThisPlan = plan;

            // メモの設定
            this.LblMemo.Text = plan.GetDisplayMemoInPlanList();

            // このレイアウトタップ時に、予定編集画面へ遷移するイベントを設定
            this.GestureRecognizers.Add(tapGesture);

            // 画面表示が"日付順"になっているかの設定
            bool displayTypeIsDate = mainPage.ThisSettingController.GetOrderType() == PlanController.OrderType.DATE;

            // 日付表示ではない場合は、以下項目を非表示
            this.LblDate.IsVisible = displayTypeIsDate;
            this.LblPriority.IsVisible = displayTypeIsDate;

            // 各ラベルの設定
            if (displayTypeIsDate)
            {
                // 日付の設定
                this.SetDate(plan.Date);

                // 優先度ラベルの設定
                this.LblPriority.Text = mainPage.TextData.PRIORITY + "：" + plan.Priority.ToString();
            }
        }

        /// <summary>
        /// 日付設定
        /// </summary>
        private void SetDate(DateTime dateTime)
        {
            if (dateTime == Const.DATETIME_DEFAULT)
                this.LblDate.IsVisible = false;
            else
                this.LblDate.Text = dateTime.Day.ToString();
        }
    }
}