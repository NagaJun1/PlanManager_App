using PlanManager.controller;
using PlanManager.controller.model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PlanManager.views
{
    /// <summary>
    /// 今日を含めて未来の予定一覧のレイアウト
    /// </summary>
    internal class FuturePlanListStackLayout : PlanListStackLayout
    {
        internal FuturePlanListStackLayout(MainPage mainPage) : base(mainPage)
        {
        }

        internal override async Task InitializePlanList(PlanController.OrderType orderType)
        {
            // 子要素の初期化
            this.Clear();

            // 表示タイプが日付になっているかの判定
            bool orderTypeIsDate = orderType == PlanController.OrderType.DATE;

            // 予定一覧を画面上に追加
            foreach (Plan plan in this.MyParentPage.ThisPlanController.PlanList)
            {
                // 予定の並びが"日付順"になっている場合は、過去のデータを追加しない
                if (plan.Date < DateTime.Today && orderTypeIsDate)
                    continue;

                // プラン編集用ボタンの生成 => ボタン押下で画面表示
                this.AddPlan(plan);

                // 数秒かけて増えるよう様に演出
                await Task.Delay(10);
            }
        }
    }
}
