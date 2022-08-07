using PlanManager.controller;
using PlanManager.controller.model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PlanManager.views
{
    /// <summary>
    /// 過去の予定一覧のレイアウト
    /// </summary>
    internal class OldPlanListStackLayout : PlanListStackLayout
    {
        internal OldPlanListStackLayout(MainPage mainPage) : base(mainPage)
        {
        }

        /// <summary>
        /// 予定一覧の初期化処理
        /// </summary>
        internal override async Task InitializePlanList(PlanController.OrderType orderType)
        {
            // 内部要素のクリア
            this.Clear();

            // 日付による並び替えかの判定
            bool orderTypeIsDate = orderType == PlanController.OrderType.DATE;

            // 日付表示ではない場合は、非表示
            this.IsVisible = orderTypeIsDate;

            // 日付表示になっている場合は要素の追加
            if (orderTypeIsDate)
            {
                // 注釈ラベルの追加
                this.AddOldLabel();

                // 予定一覧を画面上に追加
                foreach (Plan plan in this.MyParentPage.ThisPlanController.PlanList)
                {
                    // 今日より古いデータだけを追加
                    if (plan.Date < DateTime.Today)
                    {
                        this.AddPlan(plan);

                        // 数秒かけて増えるよう様に演出
                        await Task.Delay(10);
                    }
                }
            }
        }


        /// <summary>
        /// "Old"の注釈用ラベル生成
        /// </summary>
        private void AddOldLabel()
        {
            Label lbl = new Label();
            lbl.FontSize = 20;
            lbl.TextColor = Color.White;
            lbl.BackgroundColor = Color.DimGray;
            lbl.Margin = 5;
            lbl.Text = "Old";
            this.Children.Add(lbl);
        }
    }
}
