# BookNameEditTool — ソリューション概要 (AI 用)

このファイルは、AI エージェントやコントリビュータがこのソリューションで作業する際にすぐ参照できるように作成した簡潔な概要です。

## 概要
- ソリューション名: BookNameEditTool
- ルートパス: F:/resource/BookNameEditTool/
- 主用途: フォルダ名（書籍名）をデータベースと照合して編集・リネームする Windows Forms (.NET) アプリケーション

## プロジェクト
- BookNameEditTool (Single project WinForms アプリ)
  - ターゲット: net8.0-windows
  - UseWindowsForms: true
  - Nullable: enable
  - 主要ファイル
	- Program.cs — アプリケーションのエントリポイント
	- Form1.cs — メインフォーム（ロジック含む）
	- Form1.Designer.cs — フォームのデザイナーコード
	- BookNameEditTool.csproj — プロジェクト定義
  - 依存パッケージ
	- Microsoft.Data.Sqlite (SQLite を使用)
  - 配布/アセット
	- json/**/*.json はビルド出力にコピー (paths.json を参照)
	- db/booklist.db は出力にコピーされる (データベース)

## 重要な設定 / ファイル
- json/paths.json — ワークパス（work.path）や DB.path を指定する設定ファイル。Form1.LoadWorkPath() / LoadDatabasePath() が参照します。
- db/booklist.db — SQLite データベース（booklist テーブルを想定）

## 動作概要（Form1 の主要動作）
1. 起動時に paths.json から work.path を読み、 comboBox1 にセット（ただしデザイン時は読み込みを避ける保護あり）。
2. 「取得」ボタンで work.path のサブフォルダ一覧を comboBox2 に表示。
3. comboBox2 で選択したフォルダ名を基に検索ワードを作成し、SQLite DB を検索して候補を comboBox3 に表示。
4. 候補選択→編集→リネームや DB 更新を実行する機能。

## 既知の問題・注意点
- Windows Forms デザイナーで起動時にエラーが出る問題が報告されていました。原因の多くは以下:
  - Visual Studio に必要なワークロード/デザイナコンポーネントが未インストール
  - デザイナー実行時に実行されるランタイムコード（ファイル I/O、DB 接続、静的初期化）による例外
- 対策として Form1 のコンストラクタにデザイン時判定（LicenseManager.UsageMode とプロセス名チェック）を入れ、デザイン時は早期リターンするガードを追加済み。
- また、comboBox2.Items.AddRange の nullable 警告を object[] に変換して解消済み。

## 開発時のチェックリスト
- ビルド: Visual Studio 2022/2026 などで「Restore NuGet Packages」→ Build
- 必要ワークロード: Visual Studio の「.NET デスクトップ開発」「Windows Forms .NET Designer」等を有効化
- SDK: dotnet SDK 10.x (環境に応じてインストール済みであること)
- キャッシュ: デザイナー問題発生時は .vs/, bin/, obj/ を削除して再ビルド

## デバッグのためのヒント
- デザイナーエラー発生時は Visual Studio の出力ウィンドウを開き、"Show output from" を "Windows Forms Designer" に切り替えてログを確認する。
- デザイナー側のエラーはスタックトレースだけでなく、最初の例外メッセージ（InnerException）を参照すること。
- コード側での I/O / DB 接続はデザイン時に実行されないようにする（IsDesignMode 判定や静的初期化の回避）。

## 変更履歴（簡易）
- デザイン時の初期化を回避するガードを Form1 コンストラクタに追加
- comboBox2 の AddRange での nullable 警告を修正
- .gitignore を更新して /.vs/ /bin/ /obj/ を無視するように設定

## 作業依頼テンプレート（AI が実行する時の手順）
1. ソリューションをクリーン → ビルド確認（dotnet build または Visual Studio で Rebuild）
2. デザイナー問題の場合は出力ログ（Windows Forms Designer）を収集して分析
3. コード修正は小さく、デザイン時ガードや静的初期化の遅延化を優先
4. 変更をコミットし、再度ビルドとデザイナー表示を確認

---
このファイルが AI エージェントによる作業の参照用ドキュメントです。追記や修正が必要であれば項目を指示してください。
