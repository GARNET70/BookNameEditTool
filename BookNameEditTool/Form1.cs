using System.Text.Json;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.Data.Sqlite;

namespace BookNameEditTool
{
    /// <summary>
    /// メインのフォーム。書籍フォルダ名の検索、編集、リネーム、およびデータベース連携の UI ロジックを含みます。
    /// </summary>
    public partial class Form1 : Form
    {
        private string? _folderVolume;

        #region Designer generated code
        public Form1()
        {
            // デザイナー上での例外を防ぐため、デザイン時は InitializeComponent の例外を吸収して終了する
            if (IsDesignMode())
            {
                try
                {
                    InitializeComponent();
                }
                catch
                {
                    // デザイナー表示中の例外は無視してデザイナーホストに制御を戻す
                }

                // デザイン時はこれ以上の初期化を行わない
                return;
            }

            InitializeComponent();
            comboBox2.SelectedIndexChanged += comboBox2_SelectedIndexChanged;

            // 通常実行時のみワークパスを読み込む
            LoadWorkPath();
        }

        private static bool IsDesignMode()
        {
            // デザイナー上では LicenseManager.UsageMode が Designtime になる
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                return true;
            }

            // Visual Studio のプロセス名でも判定（保険）
            try
            {
                using var proc = Process.GetCurrentProcess();
                return string.Equals(proc.ProcessName, "devenv", StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region button1_Click: ワークパスのフォルダ一覧を取得して comboBox2 に追加
        private void button1_Click(object? sender, EventArgs e)
        {
            comboBox2.Items.Clear();

            var workPath = comboBox1.Text;
            if (!Directory.Exists(workPath))
            {
                return;
            }

            var dirs = Directory.GetDirectories(workPath)
                .Select(Path.GetFileName)
                .Where(name => name is not null)
                .OrderBy(name => name, StringComparer.OrdinalIgnoreCase)
                .Select(name => (object)name!)
                .ToArray();

            comboBox2.Items.AddRange(dirs);

            if (comboBox2.Items.Count > 0)
            {
                comboBox2.SelectedIndex = 0;
            }
        }
        #endregion

        #region button2_Click: データベース検索と候補リストの更新
        private void button2_Click(object? sender, EventArgs e)
        {
            var title = textBox8.Text.Trim();
            var versionMarkerIndex = title.IndexOf("_v", StringComparison.Ordinal);
            if (versionMarkerIndex >= 0)
            {
                var folderVolume = title[(versionMarkerIndex + 2)..].Trim();
                _folderVolume = string.IsNullOrWhiteSpace(folderVolume) ? null : folderVolume;
                title = title[..versionMarkerIndex];
                title = title.Replace('_', ' ').Trim();
            }
            else
            {
                //後ろから最初の半角スペースを探して、そこから後ろの文字列を巻数として扱う
                var lastSpaceIndex = title.LastIndexOf(' ');
                if (lastSpaceIndex >= 0)
                {
                    var folderVolume = title[(lastSpaceIndex + 1)..].Trim();
                    _folderVolume = string.IsNullOrWhiteSpace(folderVolume) ? null : folderVolume;
                    title = title[..lastSpaceIndex];
                }
                else
                {
                    _folderVolume = null;
                }
            }
      
            if (string.IsNullOrWhiteSpace(title))
            {
                MessageBox.Show("検索する対象フォルダを選択してください。", "検索", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            comboBox3.Items.Clear();
            ClearBookFields();
            label11.Text = string.Empty;
            if (_folderVolume is not null)
            {
                textBox4.Text = _folderVolume;
            }

            try
            {
                var databasePath = LoadDatabasePath();
                if (string.IsNullOrWhiteSpace(databasePath))
                {
                    MessageBox.Show("paths.json に DB.path が設定されていません。", "検索", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!File.Exists(databasePath))
                {
                    MessageBox.Show($"DB.path に指定されたデータベースが見つかりません。{Environment.NewLine}{databasePath}", "検索", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var connectionString = new SqliteConnectionStringBuilder
                {
                    DataSource = databasePath,
                    Mode = SqliteOpenMode.ReadOnly
                }.ToString();

                using var connection = new SqliteConnection(connectionString);
                connection.Open();

                using var command = connection.CreateCommand();
                command.CommandText = "SELECT rowid, Title_JP, Title_EN, author, Vol, status, memo FROM booklist WHERE Title_JP LIKE $title ESCAPE '\\' OR Title_EN LIKE $title ESCAPE '\\' ORDER BY Title_JP COLLATE NOCASE, Title_EN COLLATE NOCASE";
                var searchPattern = title.Replace("\\", "\\\\").Replace("%", "\\%").Replace("_", "\\_");
                command.Parameters.AddWithValue("$title", $"%{searchPattern}%");

                using var reader = command.ExecuteReader();
                var candidates = new List<BookCandidate>();
                while (reader.Read())
                {
                    candidates.Add(new BookCandidate(
                        reader.GetInt64(reader.GetOrdinal("rowid")),
                        reader["Title_JP"].ToString() ?? string.Empty,
                        reader["Title_EN"].ToString() ?? string.Empty,
                        reader["author"].ToString() ?? string.Empty,
                        reader["Vol"].ToString() ?? string.Empty,
                        reader["status"].ToString() ?? string.Empty,
                        reader["memo"].ToString() ?? string.Empty));
                }

                if (candidates.Count == 0)
                {
                    MessageBox.Show("一致するレコードが見つかりませんでした。", "検索", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                comboBox3.Items.AddRange(candidates.Cast<object>().ToArray());
                comboBox3.SelectedIndex = 0;
                label13.Text = $"検索結果: {candidates.Count} 件";
                label11.Text = candidates[0].Volume;
                if (_folderVolume is null)
                {
                    textBox4.Text = candidates[0].Volume;
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show($"データベース検索に失敗しました。{Environment.NewLine}{exception.Message}", "検索", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region button3_Click: フォルダ名の生成
        private void button3_Click(object? sender, EventArgs e)
        {
            var volume = textBox4.Text.Trim().PadLeft(2, '0');
            textBox7.Text = $"[{textBox3.Text.Trim()}] {textBox1.Text.Trim()} 第{volume}巻";
        }
        #endregion

        #region button4_Click: フォルダ名のリネーム
        private void button4_Click(object? sender, EventArgs e)
        {
            var parentPath = comboBox1.Text;
            var sourceName = comboBox2.Text;
            var destinationName = SanitizeFolderName(textBox7.Text);

            if (string.IsNullOrWhiteSpace(parentPath) || string.IsNullOrWhiteSpace(sourceName))
            {
                MessageBox.Show("対象フォルダを選択してください。", "リネーム", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (string.IsNullOrWhiteSpace(destinationName))
            {
                MessageBox.Show("有効なフォルダ名がありません。", "リネーム", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var sourcePath = Path.Combine(parentPath, sourceName);
            var destinationPath = Path.Combine(parentPath, destinationName);
            if (!Directory.Exists(sourcePath))
            {
                MessageBox.Show($"対象フォルダが見つかりません。{Environment.NewLine}{sourcePath}", "リネーム", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.Equals(sourceName, destinationName, StringComparison.Ordinal))
            {
                MessageBox.Show("変更後のフォルダ名が現在の名前と同じです。", "リネーム", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (Directory.Exists(destinationPath) || File.Exists(destinationPath))
            {
                MessageBox.Show($"変更先は既に存在します。{Environment.NewLine}{destinationPath}", "リネーム", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Directory.Move(sourcePath, destinationPath);
                textBox7.Text = destinationName;

                var selectedIndex = comboBox2.SelectedIndex;
                if (selectedIndex >= 0)
                {
                    comboBox2.Items[selectedIndex] = destinationName;
                    comboBox2.SelectedIndex = selectedIndex;
                }
                textBox8.Text = destinationName;
            }
            catch (Exception exception)
            {
                MessageBox.Show($"フォルダのリネームに失敗しました。{Environment.NewLine}{exception.Message}", "リネーム", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region button5_Click: データベースの更新
        private void button5_Click(object? sender, EventArgs e)
        {
            if (comboBox3.SelectedItem is not BookCandidate candidate)
            {
                MessageBox.Show("更新するレコードを「更新候補」から選択してください。", "DB登録", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            object volumeValue = DBNull.Value;
            var volumeText = textBox4.Text.Trim();
            if (!string.IsNullOrEmpty(volumeText))
            {
                if (!int.TryParse(volumeText, out var volume) || volume < 0)
                {
                    MessageBox.Show("巻数には0以上の整数を入力してください。修正箇所: 巻数", "DB登録", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBox4.Focus();
                    return;
                }

                volumeValue = volume;
            }

            try
            {
                var databasePath = LoadDatabasePath();
                if (string.IsNullOrWhiteSpace(databasePath))
                {
                    MessageBox.Show("paths.json の DB.path が設定されていません。修正箇所: DB.path", "DB登録", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!File.Exists(databasePath))
                {
                    MessageBox.Show($"データベースが見つかりません。修正箇所: paths.json の DB.path{Environment.NewLine}{databasePath}", "DB登録", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var connectionString = new SqliteConnectionStringBuilder
                {
                    DataSource = databasePath,
                    Mode = SqliteOpenMode.ReadWrite
                }.ToString();

                using var connection = new SqliteConnection(connectionString);
                connection.Open();

                using var command = connection.CreateCommand();
                command.CommandText = "UPDATE booklist SET Title_JP = $titleJP, Title_EN = $titleEN, author = $author, Vol = $volume, status = $status, updatetime = datetime('now', 'localtime'), memo = $memo WHERE rowid = $rowId";
                command.Parameters.AddWithValue("$titleJP", textBox1.Text);
                command.Parameters.AddWithValue("$titleEN", textBox2.Text);
                command.Parameters.AddWithValue("$author", textBox3.Text);
                command.Parameters.AddWithValue("$volume", volumeValue);
                command.Parameters.AddWithValue("$status", textBox5.Text);
                command.Parameters.AddWithValue("$memo", textBox6.Text);
                command.Parameters.AddWithValue("$rowId", candidate.RowId);

                if (command.ExecuteNonQuery() == 0)
                {
                    MessageBox.Show("更新対象のレコードが見つかりません。再検索してから登録してください。", "DB登録", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var updatedCandidate = new BookCandidate(
                    candidate.RowId,
                    textBox1.Text,
                    textBox2.Text,
                    textBox3.Text,
                    volumeText,
                    textBox5.Text,
                    textBox6.Text);
                var selectedIndex = comboBox3.SelectedIndex;
                comboBox3.Items[selectedIndex] = updatedCandidate;
                comboBox3.SelectedIndex = selectedIndex;

                //MessageBox.Show("表示内容でデータベースを更新しました。", "DB登録", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (SqliteException exception)
            {
                var guidance = exception.SqliteErrorCode switch
                {
                    8 => "データベースが読み取り専用です。DBファイルと保存先フォルダの書き込み権限を確認してください。",
                    5 or 6 => "データベースがロックされています。他のアプリでの使用を終了してから再試行してください。",
                    19 => "データベースの制約に違反しました。タイトル、作者、巻数、状態、memo の入力内容を確認してください。",
                    _ => "DB.path、booklist テーブルの列名、データベースのアクセス権を確認してください。"
                };
                MessageBox.Show($"データベースを更新できませんでした。{Environment.NewLine}{guidance}{Environment.NewLine}{exception.Message}", "DB登録", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (UnauthorizedAccessException exception)
            {
                MessageBox.Show($"データベースへの書き込み権限がありません。DBファイルと保存先フォルダのアクセス権を確認してください。{Environment.NewLine}{exception.Message}", "DB登録", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception exception)
            {
                MessageBox.Show($"データベースを更新できませんでした。DB.path、DBファイルの状態、入力内容を確認してください。{Environment.NewLine}{exception.Message}", "DB登録", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region button6_Click: 完了ボタンの処理
        private void button6_Click(object? sender, EventArgs e)
        {
            var selectedIndex = comboBox2.SelectedIndex;
            if (selectedIndex < 0)
            {
                MessageBox.Show("完了にする対象フォルダを選択してください。", "完了", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var parentPath = comboBox1.Text;
            var sourceName = comboBox2.Text;

            if (string.IsNullOrWhiteSpace(parentPath) || string.IsNullOrWhiteSpace(sourceName))
            {
                MessageBox.Show("対象フォルダが不明です。フォルダを選択してください。", "完了", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; 
            }

            var sourcePath = Path.Combine(parentPath, sourceName);
            if (!Directory.Exists(sourcePath))
            {
                MessageBox.Show($"対象フォルダが見つかりません: {sourcePath}", "完了", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var compPath = LoadCompPath();
            if (string.IsNullOrWhiteSpace(compPath))
            {
                MessageBox.Show("paths.json に comp.path が設定されていません。", "完了", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                Directory.CreateDirectory(compPath);

                var sanitized = SanitizeFolderName(sourceName);
                var destinationPath = Path.Combine(compPath, sanitized);
                var attempt = 1;
                while (Directory.Exists(destinationPath) || File.Exists(destinationPath))
                {
                    destinationPath = Path.Combine(compPath, sanitized + "_" + attempt.ToString());
                    attempt++;
                }

                Directory.Move(sourcePath, destinationPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"フォルダの移動に失敗しました: {ex.Message}", "完了", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // UI 更新
            ClearBookFields();
            textBox7.Clear();
            comboBox3.Items.Clear();
            comboBox3.Text = string.Empty;
            comboBox2.Items.RemoveAt(selectedIndex);
            _folderVolume = null;
            label11.Text = string.Empty;
            label13.Text = string.Empty;

            if (selectedIndex < comboBox2.Items.Count)
            {
                comboBox2.SelectedIndex = selectedIndex;
                button2_Click(sender, e);
            }
            else
            {
                comboBox2.SelectedIndex = -1;
                comboBox2.Text = string.Empty;
            }

        }
        #endregion

        #region button7_Click: 更新候補の表示
        private void button7_Click(object? sender, EventArgs e)
        {
            if (comboBox3.SelectedItem is not BookCandidate candidate)
            {
                MessageBox.Show("表示する更新候補を選択してください。", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            textBox1.Text = candidate.TitleJP;
            textBox2.Text = candidate.TitleEN;
            textBox3.Text = candidate.Author;
            textBox4.Text = _folderVolume ?? candidate.Volume;
            label11.Text = candidate.Volume;
            textBox5.Text = candidate.Status;
            textBox6.Text = candidate.Memo;
        }
        #endregion

        #region クラス

        private string? LoadDatabasePath()
        {
            var pathsFile = Path.Combine(AppContext.BaseDirectory, "json", "paths.json");
            if (!File.Exists(pathsFile))
            {
                return null;
            }

            using var document = JsonDocument.Parse(File.ReadAllText(pathsFile));
            if (!document.RootElement.TryGetProperty("DB", out var database) ||
                !database.TryGetProperty("path", out var pathElement))
            {
                return null;
            }

            var databasePath = pathElement.GetString();
            return string.IsNullOrWhiteSpace(databasePath)
                ? null
                : Path.GetFullPath(databasePath, AppContext.BaseDirectory);
        }

        private string? LoadCompPath()
        {
            var pathsFile = Path.Combine(AppContext.BaseDirectory, "json", "paths.json");
            if (!File.Exists(pathsFile))
            {
                return null;
            }

            using var document = JsonDocument.Parse(File.ReadAllText(pathsFile));
            if (!document.RootElement.TryGetProperty("comp", out var comp) ||
                !comp.TryGetProperty("path", out var pathElement))
            {
                return null;
            }

            var compPath = pathElement.GetString();
            return string.IsNullOrWhiteSpace(compPath)
                ? null
                : Path.GetFullPath(compPath, AppContext.BaseDirectory);
        }

        private void LoadWorkPath()
        {
            var pathsFile = Path.Combine(AppContext.BaseDirectory, "json", "paths.json");
            if (!File.Exists(pathsFile))
            {
                return;
            }

            using var document = JsonDocument.Parse(File.ReadAllText(pathsFile));
            if (!document.RootElement.TryGetProperty("work", out var work) ||
                !work.TryGetProperty("path", out var pathElement))
            {
                return;
            }

            var workPath = pathElement.GetString();
            if (string.IsNullOrWhiteSpace(workPath))
            {
                return;
            }

            comboBox1.Items.Add(workPath);
            comboBox1.SelectedIndex = 0;
        }

        private void comboBox2_SelectedIndexChanged(object? sender, EventArgs e)
        {
            textBox8.Text = comboBox2.SelectedIndex >= 0 ? comboBox2.Text : string.Empty;
        }

        private static string SanitizeFolderName(string folderName)
        {
            var invalidCharacters = Path.GetInvalidFileNameChars();
            var sanitizedName = new string(folderName
                .Where(character => !invalidCharacters.Contains(character))
                .ToArray())
                .Trim()
                .TrimEnd(' ', '.');

            var baseName = sanitizedName.Split('.', 2)[0].TrimEnd(' ', '.');
            if (ReservedWindowsFolderNames.Contains(baseName))
            {
                sanitizedName = $"_{sanitizedName}";
            }

            return sanitizedName;
        }

        private static readonly HashSet<string> ReservedWindowsFolderNames = new(StringComparer.OrdinalIgnoreCase)
        {
            "CON", "PRN", "AUX", "NUL",
            "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9",
            "LPT1", "LPT2", "LPT3", "LPT4", "LPT5", "LPT6", "LPT7", "LPT8", "LPT9"
        };

        private void ClearBookFields()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
        }

        /// <summary>
        /// データベース検索からの候補を表すレコード型です。
        /// </summary>
        private sealed record BookCandidate(
            long RowId,
            string TitleJP,
            string TitleEN,
            string Author,
            string Volume,
            string Status,
            string Memo)
        {
            public override string ToString() =>
                string.IsNullOrWhiteSpace(TitleJP) ? TitleEN : TitleJP;
        }
        #endregion
    }
}