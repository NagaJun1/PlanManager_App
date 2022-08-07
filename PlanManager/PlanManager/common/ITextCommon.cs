using System;
using System.Collections.Generic;
using System.Text;

namespace PlanManager.common
{
    internal interface ITextCommon
    {
        /// <summary>
        /// 日付
        /// </summary>
        string DATE { get; }

        /// <summary>
        /// 優先度
        /// </summary>
        string PRIORITY { get; }

        /// <summary>
        /// 追加
        /// </summary>
        string ADD_PLAN { get; }

        /// <summary>
        /// 削除
        /// </summary>
        string DELETE { get; }

        /// <summary>
        /// 予定削除時のアラートタイトル
        /// </summary>
        string DELETE_TITLE { get; }

        /// <summary>
        /// 予定削除時のアラートメッセージ
        /// </summary>
        string DELETE_MESSAGE { get; }
    }
}
