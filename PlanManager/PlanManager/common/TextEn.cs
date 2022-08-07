using System;
using System.Collections.Generic;
using System.Text;

namespace PlanManager.common
{
    internal class TextEn : ITextCommon
    {
        public string DATE { get; } = "date";

        public string PRIORITY { get; } = "priority";

        public string ADD_PLAN { get; } = "Add Plan";

        public string DELETE_TITLE { get; } = "delete plan";

        public string DELETE_MESSAGE { get; } = "Delete the plan being edited.";

        public string DELETE { get; } = "delete";
    }
}
