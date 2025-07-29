using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ModelComparer
{
    [Transaction(TransactionMode.Manual)]
    public class Command : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document newDoc = uidoc.Document;

            // 🔧 旧モデルのパス（必要に応じてファイル選択ダイアログに置き換え可）
            string oldModelPath = @"C:\test\旧モデル格納先(ModelComparer)\TestA.rvt";

            Autodesk.Revit.ApplicationServices.Application app = commandData.Application.Application;
            Document oldDoc = app.OpenDocumentFile(oldModelPath);

            try
            {
                using (Transaction tx = new Transaction(newDoc, "比較結果を出力"))
                {
                    tx.Start();

                    var result = CompareElements(newDoc, oldDoc);
                    TaskDialog.Show("モデル比較", result);

                    tx.Commit();
                }

                oldDoc.Close(false);
                return Result.Succeeded;

            }
            catch (Exception ex)
            {
                message = ex.Message;
                if (oldDoc != null)
                {
                    oldDoc.Close(false);
                }
                return Result.Failed;

            }
        }
        // 新旧モデルの要素を比較して差分を文字列で返す
        private string CompareElements(Document newDoc, Document oldDoc)
        {
            BuiltInCategory targetCategory = BuiltInCategory.OST_DuctCurves;
            string keyParam = "Mark";
            string[] compareParams = { "Width", "Height", "Length" };// ここは環境に応じて調整

            var newElems = GetElementDict(newDoc, targetCategory, keyParam);
            var oldElems = GetElementDict(oldDoc, targetCategory, keyParam);

            List<string> log = new List<string>();

            // 追加
            foreach (var key in newElems.Keys.Except(oldElems.Keys))
            {
                log.Add($"追加: Mark={key}");
            }

            // 削除
            foreach (var key in oldElems.Keys.Except(newElems.Keys))
            {
                log.Add($"削除: Mark={key}");
            }

            // 変更
            foreach (var key in newElems.Keys.Intersect(oldElems.Keys))
            {
                Element newElem = newElems[key];
                Element oldElem = oldElems[key];

                foreach (string pname in compareParams)
                {
                    string newVal = newElem.LookupParameter(pname)?.AsValueString() ?? "";
                    string oldVal = oldElem.LookupParameter(pname)?.AsValueString() ?? "";

                    if (newVal != oldVal)
                    {
                        log.Add($"変更: Mark={key} [{pname}] {oldVal} → {newVal}");
                    }
                }
            }

            return log.Count > 0 ? string.Join(Environment.NewLine, log) : "差分なし";
        }
        // Markをキーとして要素を辞書化（空はGUIDで代替）
        private Dictionary<string, Element> GetElementDict(Document doc, BuiltInCategory category, string keyParamName)
        {
            return new FilteredElementCollector(doc)
                .OfCategory(category)
                .WhereElementIsNotElementType()
                .ToDictionary(
                    e => e.LookupParameter(keyParamName)?.AsString() ?? Guid.NewGuid().ToString(),
                    e => e
                );
        }
    }
}