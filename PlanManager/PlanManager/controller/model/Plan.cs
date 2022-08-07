using PlanManager.common;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlanManager.controller.model
{
    /// <summary>
    /// JSON に格納する予定の情報
    /// </summary>
    public class Plan
    {
        public Plan()
        {
        }

        public Plan(string id)
        {
            this.Id = id;
        }

        /// <summary>
        /// 予定に紐づくID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 日付
        /// </summary>
        public DateTime Date { get; set; } = Const.DATETIME_DEFAULT;

        /// <summary>
        /// 優先度（デフォルトは"0"）
        /// </summary>
        public int Priority { get; set; } = 0;

        /// <summary>
        /// メモの内容
        /// </summary>
        public string Memo { get; set; } = string.Empty;

        /// <summary>
        /// 予定一覧に表示する短縮後のメモ
        /// </summary>
        public string GetDisplayMemoInPlanList()
        {
            // 改行前のみを取得
            string displayMemo = this.Memo.TrimStart().Split('\n')[0];

            // 見切れない様に、15文字で切る
            if (15 < displayMemo.Length)
                return displayMemo.Substring(0, 15);
            else
                return displayMemo;
        }
    }
}