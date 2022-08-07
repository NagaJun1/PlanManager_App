using PlanManager.common;
using PlanManager.controller;
using PlanManager.controller.model;
using PlanManager.pages;
using PlanManager.views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PlanManager
{
    /// <summary>
    /// 初期ページ
    /// </summary>
    public partial class MainPage : ContentPage
    {
        /// <summary>
        /// アプリの設定値を保持
        /// </summary>
        internal AppSettingController ThisSettingController { get; private set; } = new AppSettingController();

        /// <summary>
        /// 予定一覧コントローラー
        /// </summary>
        internal PlanController ThisPlanController { get; private set; } = new PlanController();

        /// <summary>
        /// 予定一覧のレイアウト
        /// </summary>
        private FuturePlanListStackLayout FuturePlanListLayout { get; set; }

        /// <summary>
        /// 処理当日以前の予定一覧
        /// </summary>
        private OldPlanListStackLayout OldPlanListLayout { get; set; }

        /// <summary>
        /// テキストデータ（JP or EN）
        /// </summary>
        internal ITextCommon TextData { get; private set; } = new TextJp();

        /// <summary>
        /// 予定一覧が読み込み中の判定
        /// </summary>
        private bool LoadingPlanList { get; set; } = false;

        /// <summary>
        /// 画面遷移中の判定
        /// </summary>
        private bool DuringScreenTransition { get; set; } = false;

        public MainPage()
        {
            InitializeComponent();

            // 予定一覧レイアウトの生成
            this.FuturePlanListLayout = new FuturePlanListStackLayout(this);
            this.FutureAndOldPlanList.Children.Add(this.FuturePlanListLayout);

            // 過去分の予定一覧レイアウトの生成
            this.OldPlanListLayout = new OldPlanListStackLayout(this);
            this.FutureAndOldPlanList.Children.Add(this.OldPlanListLayout);

            // 予定の新規追加ボタン押下で、編集ページへ遷移
            this.BtnAddPlan.Pressed += Btn_Pressed;

            // 画面表示タイミングで予定一覧を初期化
            this.Appearing += MainPage_Appearing;

            // 予定一覧並び替えボタンの初期化
            this.SetSortPlanListButton();

            // 設定ボタン押下で、設定画面に遷移
            this.BtnSetting.Pressed += BtnSetting_Pressed;
        }

        /// <summary>
        /// 画面表示のタイミングで、予定一覧の初期化
        /// </summary>
        private void MainPage_Appearing(object sender, EventArgs e)
        {
            // 処理最中は多重軌道を許容しない
            if (!this.LoadingPlanList)
            {
                // 読み込み中の判定に変更
                this.LoadingPlanList = true;

                // ページ内の言語を設定
                this.SettingLanguageInPage();

                // 予定一覧を初期化（並び替え）
                this.InitializePlanList(this.ThisSettingController.GetOrderType());
            }
        }

        /// <summary>
        /// ページ上の言語を設定値に対応するものに変更
        /// </summary>
        private void SettingLanguageInPage()
        {
            // 設定値に伴って
            if (Const.Language.JP.Equals(
                AppSettingController.LANGUAGE[this.ThisSettingController.GetLanguageIndex()]))
                this.TextData = new TextJp();
            else if (Const.Language.EN.Equals(
                AppSettingController.LANGUAGE[this.ThisSettingController.GetLanguageIndex()]))
                this.TextData = new TextEn();

            // 各ボタンのテキストを修正
            this.BtnAddPlan.Text = this.TextData.ADD_PLAN;
            this.BtnSortDate.Text = this.TextData.DATE;
            this.BtnSortPriority.Text=this.TextData.PRIORITY;
        }

        /// <summary>
        /// 予定一覧並び替えボタンの設定
        /// </summary>
        private void SetSortPlanListButton()
        {
            // 日付順で並び替え
            BtnSortDate.Text = this.TextData.DATE;
            BtnSortDate.Pressed += EventInitializePlanList(PlanController.OrderType.DATE);

            // 優先度順で並び替え
            BtnSortPriority.Text = this.TextData.PRIORITY;
            BtnSortPriority.Pressed += EventInitializePlanList(PlanController.OrderType.PRIORITY);
        }

        /// <summary>
        /// 予定一覧を初期化するイベントハンドラ
        /// </summary>
        private EventHandler EventInitializePlanList(PlanController.OrderType orderType)
        {
            return delegate
            {
                // 処理最中は多重軌道を許容しない
                if (!this.LoadingPlanList)
                {
                    // 読み込み中の判定に変更
                    this.LoadingPlanList = true;

                    // 現在の表示状態を記録
                    this.ThisSettingController.SetOrderType(orderType);

                    // 予定一覧を初期化（並び替え）
                    this.InitializePlanList(orderType);
                }
            };
        }

        /// <summary>
        /// ボタン押下で予定編集ページへ遷移
        /// </summary>
        private async void Btn_Pressed(object sender, EventArgs e)
        {
            await this.NavigationPageAsync(new PlanEditPage(this.ThisPlanController, this.TextData));
        }

        /// <summary>
        /// 予定一覧の初期化
        /// </summary>
        private async void InitializePlanList(PlanController.OrderType orderType)
        {
            // 予定一覧を並び替え
            this.ThisPlanController.SortPlanList(orderType);

            // 過去の予定一覧は、一度非表示にする
            this.OldPlanListLayout.IsVisible = false;

            // 予定一覧の初期化（予定の並びが日付なら今日からの予定のみを表示）
            await this.FuturePlanListLayout.InitializePlanList(orderType);

            // 今日以前の予定一覧を初期化（予定の並びが優先度なら使用しない）
            await this.OldPlanListLayout.InitializePlanList(orderType);

            // 予定一覧ロード中の判定を解除
            this.LoadingPlanList = false;
        }

        /// <summary>
        /// 画面遷移処理
        /// </summary>
        /// <param name="page">遷移先のページ</param>
        public async Task NavigationPageAsync(Page page)
        {
            // 画面遷移の多重起動防止
            if (!DuringScreenTransition)
            {
                // 画面遷移中の判定に変更
                this.DuringScreenTransition = true;

                // 画面遷移実行
                await this.Navigation.PushAsync(page);

                // 画面遷移終了判定に変更
                this.DuringScreenTransition = false;
            }
        }

        /// <summary>
        /// 設定ボタン押下で設定画面へ遷移
        /// </summary>
        private async void BtnSetting_Pressed(object sender, EventArgs e)
        {
            // 設定画面へ遷移
            await this.NavigationPageAsync(new SettingPage(this.ThisSettingController));
        }
    }
}