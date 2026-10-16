# BookNameEditTool プロジェクトメモ

## プロジェクト概要

- Windows Forms を使った Windows デスクトップアプリケーション。
- ソリューション: `BookNameEditTool.sln`
- プロジェクト: `BookNameEditTool/BookNameEditTool.csproj`
- ターゲット: `.NET 8` (`net8.0-windows`)
- 出力形式: Windows GUI アプリケーション (`WinExe`)

## ソース構成

- `BookNameEditTool/Program.cs`
  - アプリケーションのエントリーポイント。
  - Windows Forms を初期化し、`Form1` を起動する。
- `BookNameEditTool/Form1.cs`
  - フォームの処理を記述する。
  - 起動時に `LoadWorkPath()` を呼び出し、設定された作業フォルダパスを `comboBox1` に表示する。
  - `button1`（取得）で作業フォルダ直下のフォルダ名を取得し、`comboBox2` に表示する。先頭の項目を初期選択する。
  - `button2`（検索）で対象フォルダ名を正規化し、SQLite のタイトルを部分一致検索して候補を `comboBox3` に表示する。
  - `button3`（フォルダ名作成）で作者・日本語タイトル・2桁ゼロ埋め巻数から名前を生成し、`textBox7` に表示する。
  - `button4`（リネーム）で現在の対象フォルダ名を `textBox7` の名前に変更する。Windowsで使えない文字などを除去する。
  - `button5`（DB登録）で選択候補の行を `rowid` で特定し、画面の入力値で更新する。巻数を整数として検証し、更新失敗時は修正箇所を通知する。
  - `button6`（完了）で入力欄と更新候補をクリアし、対象フォルダ一覧から選択項目を外して次項目を選択する。実フォルダは削除しない。
  - `button7`（OK）で選択中の更新候補の書誌情報を入力欄に表示する。
- `BookNameEditTool/Form1.Designer.cs`
  - フォームの UI 定義。
  - 「対象フォルダパス」(`comboBox1`)、 「対象フォルダ」(`comboBox2`)、 「更新候補」(`comboBox3`)、書誌情報欄とフォルダ名欄を配置している。
  - ボタン表示: `button1`「取得」、`button2`「検索」、`button3`「フォルダ名作成」、`button4`「リネーム」、`button5`「DB登録」、`button6`「完了」、`button7`「OK」。
- `BookNameEditTool/Form1.resx`
  - Windows Forms のリソースファイル。
- `BookNameEditTool/json/paths.json`
  - `comp.path`、`work.path`、`DB.path` を設定する JSON ファイル。
- `BookNameEditTool/db/`
  - `booklist.db` を配置。SQLite の `booklist` テーブルに `Title_JP`, `Title_EN`, `author`, `Vol`, `status`, `updatetime`, `memo` がある。

## 現在の処理

1. アプリの出力先にある `json/paths.json` を開く。
2. JSON の `work.path` を取得する。
3. `comboBox1` にパスを表示する。
4. 「取得」を押すと、パス直下のフォルダ名を大文字・小文字を区別せずに並べ替え、`comboBox2` に表示する。項目がある場合は先頭を選択する。
5. 「検索」を押すと、対象フォルダ名の最初の `_v` 以降を除去し、残った `_` を半角スペースに置き換えた文字列で `booklist.Title_JP` または `booklist.Title_EN` を部分一致検索する。結果を「更新候補」(`comboBox3`) に表示し、先頭候補を初期選択する。「OK」を押すと、選択候補の書誌情報を入力欄に表示する。見つからない場合は候補と入力欄をクリアして通知する。
6. 「フォルダ名作成」を押すと、`[作者] タイトル(日本語) 第{巻数}巻` を生成して `textBox7` に表示する。巻数は2桁になるようゼロ埋めする。
7. 「リネーム」を押すと、現在選択中の対象フォルダを `textBox7` の名前に変更する。ファイル名に使えない文字と末尾の空白・ピリオドを除去し、予約デバイス名には先頭に `_` を付ける。変更先が存在するなどの失敗はメッセージで通知する。
8. 「DB登録」を押すと、検索候補の `rowid` に対応するレコードを表示中の書誌情報で更新する。巻数は空欄または0以上の整数として扱い、DBエラー時は原因と確認箇所をダイアログ表示する。
9. 「完了」を押すと、`textBox1`〜`textBox7` と更新候補をクリアし、現在選択中の対象フォルダを一覧から外して次の項目を選択する。フォルダパスと実フォルダは維持する。

プロジェクトファイルでは `json/**/*.json` を出力先へコピーする設定になっている。SQLite 接続には `Microsoft.Data.Sqlite` を使用し、DB ファイルの場所は `paths.json` の `DB.path` から取得する。相対パスはアプリのベースフォルダを基準に解決する。

## 今回の作業・進捗

- `paths.json` の `work.path` 配下にあるフォルダ一覧をフォーム起動時に表示する処理を追加。
- 起動時に `work.path` を表示し、「取得」押下でその配下のフォルダ一覧を読み込む処理を追加。
- 「検索」ボタンで `Title_JP` / `Title_EN` 部分一致検索し、候補一覧に結果を表示する処理を追加。
- 「DB登録」で選択候補の `rowid` のレコードを更新し、巻数や DB エラーを検証・通知する処理を追加。
- 「完了」で入力欄・更新候補のクリアと、対象フォルダ一覧からの選択項目除去を行う処理を追加。
- JSON をビルド出力先へコピーし、DB は `paths.json` の `DB.path` から直接参照する設定に変更。
- 実 DB に対する検索 SQL の実行を確認済み。エディター診断ではエラーなし。
- 現在の `DB.path` は `D:\VM\share\BookNameEditTool\BookNameEditTool\db\booklist.db` を指す。
- 現在の C#・Designer・プロジェクトファイルのエディター診断はエラーなし。SQLite の検索 SQL と UPDATE 文は実 DB に対して確認したが、UPDATE の検証はロールバック済み。
- `dotnet build` は実行環境に .NET SDK が見つからず未実施。
- Git リポジトリではないため、Git の変更状態は確認できていない。
- 実際の Windows Forms 起動・ボタン操作による動作確認は未実施。

## 次回の確認候補

- .NET 8 SDK のある Windows 環境で NuGet 復元、ビルド、起動を行い、取得・検索の一連の動作を確認する。
- `paths.json` の絶対パスが利用環境ごとに異なる場合は、設定方法やエラー表示を検討する。
- .NET 8 SDK のある環境でビルド・起動し、検索候補の表示、DB更新、フォルダ名作成・リネーム・完了の一連の画面操作を確認する。
- `button5` でのDB更新が実データに対して意図した1件のみを更新することを確認する。