using System;

namespace PlanManager.common
{
    internal class Const
    {
        /// <summary>
        /// 予定一覧を保管するJSON
        /// </summary>
        public const string PLAN_LIST_JSON = "planlist.json";

        /// <summary>
        /// 日付のデフォルト値
        /// </summary>
        public static DateTime DATETIME_DEFAULT = DateTime.MaxValue;

        /// <summary>
        /// 設定画像ファイルパス
        /// </summary>
        public const string IMG_SETTING = "setting.png";

        /// <summary>
        /// 言語
        /// </summary>
        internal static class Language
        {
            public const string EN = "English";
            public const string JP = "日本語";
        }
    }
}
