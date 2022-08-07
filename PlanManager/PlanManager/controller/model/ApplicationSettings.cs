using PlanManager.common;
using static PlanManager.controller.PlanController;

namespace PlanManager.controller.model
{
    public class ApplicationSettings
    {
        /// <summary>
        /// 予定一覧の表示状態が"日付順"になっているかの判定
        /// </summary>
        public bool DisplayTypeIsDate { get; set; } = true;

        /// <summary>
        /// 使用している言語
        /// </summary>
        public int Language { get; set; } = 0;
    }
}
