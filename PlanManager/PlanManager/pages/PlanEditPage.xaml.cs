using PlanManager.common;
using PlanManager.controller;
using PlanManager.controller.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static PlanManager.common.Const;

namespace PlanManager.pages
{
    /// <summary>
    /// タスクの編集ページ
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlanEditPage : ContentPage
    {
        /// <summary>
        /// 編集対象のプランのID
        /// </summary>
        private Plan EditPlan { get; set; } = null;

        /// <summary>
        /// ローカルファイルの予定一覧をロード
        /// </summary>
        private PlanController ThisPlanController { get; set; }

        /// <summary>
        /// 予定削除判定フラグ
        /// </summary>
        private bool DeletePlan { get; set; } = false;

        /// <summary>
        /// 使用する言語の設定
        /// </summary>
        private ITextCommon TextLanguage { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="planId">処理対象の予定ID</param>
        internal PlanEditPage(PlanController planController, ITextCommon text, Plan plan = null)
        {
            InitializeComponent();

            // 予定一覧の取得
            this.ThisPlanController = planController;

            // アプリ内でページから移動したためにページが消えたときに発生
            this.Disappearing += PlanEditPage_Disappearing;

            // 日付変更処理時のイベント
            this.ChkEnableDate.CheckedChanged += ChkEnableDate_CheckedChanged;

            // 選択された言語に対応してラベルテキストを修正
            this.SetLanguage(text);

            // 編集対象の予定が引数に存在している場合
            if (plan != null)
                this.SettingTargetPlan(plan);

            // 編集対象のボタンの有無に応じて"削除ボタンを非表示にする"
            this.BtnDeletePlan.IsVisible = plan != null;

            // 優先度の選択項目の設定
            this.SetPriorityPicker();
        }

        /// <summary>
        /// 設定された言語に合わせてラベルを修正
        /// </summary>
        /// <param name="textCommon"></param>
        private void SetLanguage(ITextCommon textCommon)
        {
            this.TextLanguage = textCommon;
            this.BtnDeletePlan.Text = this.TextLanguage.DELETE;
            this.LblPriority.Text = this.TextLanguage.PRIORITY;
            this.LblDate.Text = this.TextLanguage.DATE;
        }

        /// <summary>
        /// 編集対象の予定が存在する場合に編集対象を記録
        /// </summary>
        private void SettingTargetPlan(Plan plan)
        {
            this.EditPlan = plan;

            // エディタにメモを保存
            this.PlanEditor.Text = plan.Memo;

            // 日付の設定、設定する日付が存在する場合はチェックボックスを有効化
            this.ChkEnableDate.IsChecked = plan.Date != Const.DATETIME_DEFAULT;
            if (this.ChkEnableDate.IsChecked)
                this.DatePickerPlan.Date = plan.Date;

            // 既存の予定がある場合に予定の削除ボタンを生成
            this.BtnDeletePlan.Pressed += BtnDeletePlan_Pressed;
        }

        /// <summary>
        /// ボタン押下で予定削除
        /// </summary>
        private async void BtnDeletePlan_Pressed(object sender, EventArgs e)
        {
            // 予定削除の確認アラート表示 => "OK"なら削除
            if (await this.DisplayAlert(
                this.TextLanguage.DELETE_TITLE, this.TextLanguage.DELETE_MESSAGE, "OK", "CANCEL"))
            {
                // 編集中の予定を削除
                this.ThisPlanController.DeletPlanById(this.EditPlan.Id);

                // 予定削除の判定に変更
                this.DeletePlan = true;

                // 編集画面を閉じる
                await this.Navigation.PopAsync(true);
            }
        }

        /// <summary>
        /// チェックボックスのチェックに応じて"DatePickerPlan"の表示を変更
        /// </summary>
        private void ChkEnableDate_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            this.DatePickerPlan.IsVisible = this.ChkEnableDate.IsChecked;
        }

        /// <summary>
        /// 優先度の選択項目の設定
        /// </summary>
        private void SetPriorityPicker()
        {
            // 優先度の選択項目を"0～5"で設定
            for (int i = 0; i <= 5; i++)
                this.PickerPriority.Items.Add(i.ToString());

            // 編集対象の予定から優先度を取得
            int priority = 0;
            if (this.EditPlan != null)
                priority = this.EditPlan.Priority;

            // 初期値を設定
            this.PickerPriority.SelectedIndex = priority;
        }

        /// <summary>
        /// ページが消える時にメモを保存
        /// </summary>
        private void PlanEditPage_Disappearing(object sender, EventArgs e)
        {
            // 予定削除時は処理しない
            if (this.DeletePlan)
                return;

            // エディタが空なら保存しない
            if (string.IsNullOrWhiteSpace(this.PlanEditor.Text))
                return;

            // 編集対象の予定が存在しない場合は、新規に作成
            if (this.EditPlan == null)
                this.EditPlan = this.ThisPlanController.NewPlan();

            // エディタの内容をメモに保存
            this.EditPlan.Memo = this.PlanEditor.Text;

            // 優先度の保存
            this.EditPlan.Priority = this.PickerPriority.SelectedIndex;

            // 日付の保存（日付変更操作が行われた場合のみ）
            if (this.ChkEnableDate.IsChecked)
                this.EditPlan.Date = this.DatePickerPlan.Date;
            else
                this.EditPlan.Date = Const.DATETIME_DEFAULT;

            // 作成した予定を登録
            this.ThisPlanController.AddPlan(this.EditPlan);
        }
    }
}