using Newtonsoft.Json;
using PlanManager.common;
using PlanManager.controller.model;
using System;
using System.Collections.Generic;
using System.IO;

namespace PlanManager.controller
{
    /// <summary>
    /// JSON ファイルで予定一覧を管理
    /// </summary>
    public class PlanController
    {
        public List<Plan> PlanList { get; private set; }

        /// <summary>
        /// 予定一覧、並び替えタイプ
        /// </summary>
        public enum OrderType
        {
            PRIORITY = 1,
            DATE = 2,
        }

        /// <summary>
        /// "PlanList"を初期化
        /// </summary>
        public PlanController()
        {
            // "PlanList"を初期化 
            this.ReadPlanListInFile();
        }

        /// <summary>
        /// ローカルファイルに保存された"PlanList"を読み取る
        /// </summary>
        private void ReadPlanListInFile()
        {
            if (File.Exists(this.GetJsonFilePath()))
            {
                using (StreamReader stream = new StreamReader(this.GetJsonFilePath()))
                {
                    this.PlanList = JsonConvert.DeserializeObject<List<Plan>>(stream.ReadToEnd());
                }
            }

            // 初期化できていない場合は、初期化する
            if (this.PlanList == null)
                this.PlanList = new List<Plan>();
        }

        /// <summary>
        /// "PlanList"をJSONファイルに保存する
        /// </summary>
        private void SaveJsonFile()
        {
            using (StreamWriter stream = new StreamWriter(this.GetJsonFilePath(), false))
            {
                // JSON にシリアライズして、ファイルに書き込み
                stream.Write(JsonConvert.SerializeObject(this.PlanList));
            }
        }

        /// <summary>
        /// JSONファイルのパスを取得
        /// </summary>
        private string GetJsonFilePath()
        {
            string localAppFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            return Path.Combine(localAppFolder, Const.PLAN_LIST_JSON);
        }

        /// <summary>
        /// "PlanList"に"plan"を追加。重複するIDが存在する場合は上書き
        /// </summary>
        public void AddPlan(Plan newPlan)
        {
            // "ID"が一致するインデックスを取得
            int index = this.GetIdIndexInPlanList(newPlan.Id);

            // 重複する"ID"があるなら上書き、無いなら追加
            if (0 <= index)
                this.PlanList[index] = newPlan;
            else
                this.PlanList.Add(newPlan);

            // .jsonファイルの保存
            this.SaveJsonFile();
        }

        /// <summary>
        /// "ID"が重複しない"Plan"を生成
        /// </summary>
        public Plan NewPlan()
        {
            while (true)
            {
                string guid = Guid.NewGuid().ToString();

                // "ID"が一致するインデックスを取得
                int index = this.GetIdIndexInPlanList(guid);

                // 重複しない IDが生成されたら戻り値に設定
                if (index < 0)
                    return new Plan(guid);
            }
        }

        /// <summary>
        /// 引数のIDに紐づく予定の削除
        /// </summary>
        /// <param name="id"></param>
        public void DeletPlanById(string id)
        {
            // 処理対象の予定のインデックスを取得
            int index = this.GetIdIndexInPlanList(id);
            if (!(index < 0))
            {
                this.PlanList.RemoveAt(index);

                // .jsonファイルの保存
                this.SaveJsonFile();
            }
        }

        /// <summary>
        /// "PlanList"の中で"ID"が一致するインデックスを取得
        /// </summary>
        /// <returns>一致する値が無い場合は"-1"</returns>
        private int GetIdIndexInPlanList(string id)
        {
            for (int index = 0; index < this.PlanList.Count; index++)
            {
                if (id.Equals(this.PlanList[index].Id))
                    return index;
            }
            return -1;
        }

        /// <summary>
        /// IDに紐づくプランを取得
        /// </summary>
        /// <param name="id">取得対象のID</param>
        public Plan GetPlanById(string id)
        {
            int index = this.GetIdIndexInPlanList(id);
            if (index < 0)
                return null;
            else
                return this.PlanList[index];
        }

        /// <summary>
        /// "PlanList"の並び替え
        /// </summary>
        public void SortPlanList(OrderType orderType)
        {
            if (orderType == OrderType.DATE)
                this.OrderByDate();
            else if (orderType == OrderType.PRIORITY)
                this.OrderByPriority();
        }

        /// <summary>
        /// 日付で"PlanList"を並び替え
        /// </summary>
        private void OrderByDate()
        {
            for (int index1 = 0; index1 < this.PlanList.Count; index1++)
            {
                for (int index2 = 0; index2 < this.PlanList.Count; index2++)
                {
                    if (this.PlanList[index1].Date < this.PlanList[index2].Date)
                    {
                        // 日付が小さい場合は前に持ってくる
                        Plan plan = this.PlanList[index2];
                        this.PlanList[index2] = this.PlanList[index1];
                        this.PlanList[index1] = plan;
                    }
                }
            }
        }

        /// <summary>
        /// 優先度で"PlanList"を並び替え
        /// </summary>
        private void OrderByPriority()
        {
            for (int index1 = 0; index1 < this.PlanList.Count; index1++)
            {
                for (int index2 = 0; index2 < this.PlanList.Count; index2++)
                {
                    if (this.PlanList[index1].Priority > this.PlanList[index2].Priority)
                    {
                        // 優先度が大きい場合は、前に持ってくる
                        var plan = this.PlanList[index2];
                        this.PlanList[index2] = this.PlanList[index1];
                        this.PlanList[index1] = plan;
                    }
                }
            }
        }
    }
}
