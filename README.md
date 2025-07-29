# ModelComparer
🚀 Revitモデル比較アドイン  
- 差分検出（追加・削除・変更）  
- ダクト要素に特化  
- Revit上で新旧モデルを比較・可視化


---

🔧 機能概要

| 内容 | 詳細 |
|------|------|
| 対象カテゴリ | `DuctCurves`（ダクト）※今後拡張予定 |
| 識別キー | `Mark` パラメータ |
| 比較項目 | `Width`, `Height`, `Length`（※変更可能） |
| 出力方法 | `TaskDialog` にて比較結果を表示 |
| モデル読み込み | 外部ファイル（旧モデル）を直接開いて比較 |

---

> ※以下のような差分を表示します（例）

・追加: Mark=D1001
・削除: Mark=D1050
・変更: Mark=D1020 [Width] 500 → 600


---

📁 インストール方法

1. このリポジトリをクローンまたはダウンロード  
2. `ModelComparer.dll` を Revitの Addins フォルダに配置  
3. 以下のような `.addin` ファイルを作成して読み込む：

---

<?xml version="1.0" encoding="utf-8"?>
<RevitAddIns>
  <AddIn Type="Command">
    <Name>ModelComparer</Name>
    <Assembly>C:\test\ModelComparer\ModelComparer\bin\Debug\ModelComparer.dll</Assembly>
    <AddInId>15D9BE97-2568-4392-9462-0C84F7A4D4A7</AddInId>
    <FullClassName>ModelComparer.Command</FullClassName>
    <VendorId>KengoTanaka</VendorId>
    <VendorDescription>KengoTanaka</VendorDescription>
  </AddIn>
</RevitAddIns>

---

🔮 将来の構想（TODO）
 ・ファイル選択ダイアログ対応（旧モデル選択をGUI化
 ・CSV出力に対応
 ・比較対象をダクト以外にも拡張（配管・器具等）
 ・差分を色分けでモデル上に表示（UI強化）

👤 作者
Kengo Tanaka
元BIMオペ → DX志望。Revitアドイン開発を独学で継続中
→ 現在、Revit API × C# による建設SaaS転職を目指してポートフォリオ公開中

💬 ライセンス & お問い合わせ
ライセンス：MIT（※自由に使ってOK）

質問・コラボ・採用相談は Issues または GitHub Profile からどうぞ



