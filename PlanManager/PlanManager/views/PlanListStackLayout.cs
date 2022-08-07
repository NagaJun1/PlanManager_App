using PlanManager.common;
using PlanManager.controller;
using PlanManager.controller.model;
using PlanManager.pages;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PlanManager.views
{
    /// <summary>
    /// 予定一覧のレイアウト
    /// </summary>
    internal abstract class PlanListStackLayout : StackLayout
    {
        /// <summary>
        /// 予定押下時イベント
        /// </summary>
        private TapGestureRecognizer PlanTapGesture { get; set; } = new TapGestureRecognizer();

        /// <summary>
        /// 予定一覧の表示ページ
        /// </summary>
        internal MainPage MyParentPage { get; private set; }

        /// <summary>
        /// 直近に生成した補足ラベルのテキスト
        /// </summary>
        internal string MostRecentSupplementaryLabel { get; set; } = string.Empty;

        internal PlanListStackLayout(MainPage mainPage)
        {
            this.MyParentPage = mainPage;

            // 予定押下で、編集ページへ遷移
            this.PlanTapGesture.Tapped += PlanTapGesture_Tapped;
        }

        /// <summary>
        /// 予定押下イベント
        /// </summary>
        /// <param name="sender">PlanElementLayout</param>
        private async void PlanTapGesture_Tapped(object sender, EventArgs e)
        {
            // 押下対象が"PlanElementLayout"であることを確認
            if (sender is PlanElementLayout plan)
            {
                // 押下されたことを明確にするために色を変更
                plan.BackgroundColor = Color.Gray;

                // 予定編集ページへ遷移
                PlanEditPage planEditPage
                    = new PlanEditPage(this.MyParentPage.ThisPlanController, this.MyParentPage.TextData, plan.ThisPlan);
                await this.MyParentPage.NavigationPageAsync(planEditPage);
            }
        }

        /// <summary>
        /// 予定編集用ボタンの生成
        /// </summary>
        public void AddPlan(Plan plan)
        {
            // 補足ラベル追加
            this.AddSupportLabel(plan);

            // 予定一覧に表示する項目の生成
            PlanElementLayout planElement = new PlanElementLayout(this.PlanTapGesture, plan, this.MyParentPage);
            planElement.Opacity = 0;
            this.Children.Add(planElement);

            // アニメーションの設定
            planElement.FadeTo(1, 300);
        }

        /// <summary>
        /// "年/月"、もしくは"優先度：～"の補足ラベルの追加
        /// </summary>
        /// <param name="beforeLabelText">直近で生成した補足ラベルのテキスト</param>
        /// <param name="plan">処理対象の予定</param>
        private void AddSupportLabel(Plan plan)
        {
            // この処理内で生成したラベルテキスト
            string newLblText;

            // 日付表示になっている場合
            if (this.MyParentPage.ThisSettingController.GetOrderType() == PlanController.OrderType.DATE)
                // 日付がデフォルトなら生成しない
                if (plan.Date == Const.DATETIME_DEFAULT)
                    newLblText = "-";
                else
                    newLblText = plan.Date.Year + "/" + plan.Date.Month.ToString();
            else
                // 表示が優先度順になっている場合
                newLblText = this.MyParentPage.TextData.PRIORITY + ":" + plan.Priority.ToString();

            // 新規に年月、優先度が追加された場合は、ラベルを追加
            if (!this.MostRecentSupplementaryLabel.Equals(newLblText))
                this.AddSubLabel(newLblText);

            // 今回生成したものを保持
            this.MostRecentSupplementaryLabel = newLblText;
        }

        /// <summary>
        /// 年・月、優先度のラベルを追加
        /// </summary>
        private void AddSubLabel(string text)
        {
            Label lbl = new Label();
            lbl.Text = text;
            lbl.FontSize = 20;
            lbl.TextColor = Color.Black;
            lbl.BackgroundColor = Color.PowderBlue;
            lbl.Padding = 3;
            lbl.Margin = 3;
            lbl.Opacity = 0;
            this.Children.Add(lbl);

            // アニメーションの設定
            lbl.FadeTo(1, 300);
        }

        /// <summary>
        /// 各要素の初期化
        /// </summary>
        internal void Clear()
        {
            // 子要素の除去
            this.Children.Clear();

            // 直近で生成したラベルを空に設定
            this.MostRecentSupplementaryLabel = string.Empty;
        }


        /// <summary>
        /// 予定一覧の初期化処理
        /// </summary>
        internal abstract Task InitializePlanList(PlanController.OrderType orderType);
    }
}