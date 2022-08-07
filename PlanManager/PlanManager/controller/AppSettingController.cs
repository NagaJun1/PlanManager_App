using Newtonsoft.Json;
using PlanManager.common;
using PlanManager.controller.model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static PlanManager.controller.PlanController;

namespace PlanManager.controller
{
    /// <summary>
    /// アプリの設定値の保持ファイル
    /// </summary>
    internal class AppSettingController
    {
        /// <summary>
        /// 言語
        /// </summary>
        internal static string[] LANGUAGE = { Const.Language.EN, Const.Language.JP };

        /// <summary>
        /// JSONファイルの中身の保存先
        /// </summary>
        private ApplicationSettings ThisAppSetting { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        internal AppSettingController()
        {
            if (File.Exists(this.GetFilePath()))
            {
                // ファイルが作成済みの場合は、ファイルの内容をロード
                using (StreamReader sr = new StreamReader(this.GetFilePath()))
                {
                    string json = sr.ReadToEnd();
                    this.ThisAppSetting = JsonConvert.DeserializeObject<ApplicationSettings>(json);
                }
            }
            else
                this.ThisAppSetting = new ApplicationSettings();
        }

        /// <summary>
        /// 予定一覧の表示状態を更新
        /// </summary>
        public void SetOrderType(OrderType orderType)
        {
            this.ThisAppSetting.DisplayTypeIsDate = orderType == OrderType.DATE;

            // 更新した内容をファイルに記録
            this.SaveFile();
        }

        /// <summary>
        /// 設定値を基に"OrderType"を取得
        /// </summary>
        public OrderType GetOrderType()
        {
            if (this.ThisAppSetting.DisplayTypeIsDate)
                return OrderType.DATE;
            else
                return OrderType.PRIORITY;
        }

        /// <summary>
        /// 設定ファイルのパスを取得
        /// </summary>
        private string GetFilePath()
        {
            string localAppFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            return Path.Combine(localAppFolder, "application_setting.json");
        }

        /// <summary>
        /// ローカルファイルに現在の設定値を保存
        /// </summary>
        private void SaveFile()
        {
            using (StreamWriter sw = new StreamWriter(this.GetFilePath(), false))
            {
                // 設定値をローカルファイルに保存
                string json = JsonConvert.SerializeObject(this.ThisAppSetting);
                sw.Write(json);
            }
        }

        /// <summary>
        /// 使用言語の設定を保存
        /// </summary>
        /// <param name="language">言語配列のインデックス（"LANGUAGE"で設定）</param>
        internal void SaveLanguage(int language)
        {
            // 言語を上書き
            this.ThisAppSetting.Language = language;

            // ローカルファイルに保存
            this.SaveFile();
        }

        /// <summary>
        /// 設定値に保存されている言語のインデックスを取得
        /// </summary>
        public int GetLanguageIndex()
        {
            return this.ThisAppSetting.Language;
        }
    }
}
