using System;
using System.Collections.Generic;
using System.Text;

namespace PlanManager.common
{
    internal class TextJp : ITextCommon
    {
        public string DATE { get; } = "日付";

        public string PRIORITY { get; } = "優先度";

        public string ADD_PLAN { get; } = "追加";

        public string DELETE_TITLE { get; } = "予定削除";

        public string DELETE_MESSAGE { get; } = "編集中の予定を削除します。";

        public string DELETE { get; } = "削除";
    }
}
