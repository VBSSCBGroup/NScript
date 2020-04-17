Imports System.IO
Imports System.Text
Imports NScript.Helper

Public Class Form1
    Private RichHighlighter0 As New RichHighlighter()
    Private GoodList As New List(Of String)
    Private Sub RichTextBox1_TextChanged(sender As Object, e As EventArgs) Handles RichTextBox1.TextChanged
        Dim n1 As Int32 = RichTextBox1.SelectionStart
        RichTextBox1.Enabled = False
        Dim str As String = RichTextBox1.Text
        Dim eee As String() = RichTextBox1.Text.Split(" "c, vbLf)
        For Each ee In eee
            If ee Is Nothing Then
            Else
                For Each gd In GoodList
                    If gd.ToLower() = ee.ToLower() Then
                        str = str.Replace(ee, gd)
                        Exit For
                    End If
                Next
            End If
        Next
        RichTextBox1.Text = str
        RichTextBox1.SuspendLayout()
        RichHighlighter0.RichHighlight(0, RichTextBox1)
        RichTextBox1.Select(n1, 1)
        RichTextBox1.SelectionFont = New Font("宋体", 9, (FontStyle.Regular))
        RichTextBox1.SelectionColor = Color.Black
        RichTextBox1.Select(n1, 0)
        RichTextBox1.ResumeLayout()
        RichTextBox1.Enabled = True
        RichTextBox1.Focus()
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        GoodList.Add("Set")
        GoodList.Add("CreateObject")
        GoodList.Add("NScript")
        GoodList.Add("WSH")
        GoodList.Add("Wshshell")
        GoodList.Add("Wscript")
        GoodList.Add("If")
        GoodList.AddRange({"ElseIf",
                          "End",
                          "For",
                          "Do",
                          "Private",
                          "Sub",
                          "Function",
                          "Class", "With", "Enum",
                          "As",
                          "Object",
                          "While",
                          "Until",
                          "True",
                          "False",
                          "Then",
                          "Loop",
                          "Next",
                          "On Error",
                          "Goto"})
        For Each wawa In GoodList
            RichHighlighter0.KeywordsAdd(wawa)
        Next
    End Sub
    Private Function Gen1(str As String) As String
        Dim strbulid As New StringBuilder()
        strbulid.Append(
            "On Error Resume Next
Set NScript = CreateObject(""NScript.NetScript"",""NScript_"") '生成NScript对象
If err.count() <> 0 Then
     msgbox(""你的电脑不支持NScript!"")
End If
On Error Goto -1
")
        strbulid.Append(str)
        Return strbulid.ToString()
    End Function
    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click
        SaveFileDialog1.ShowDialog()
        Dim tw As New StreamWriter(SaveFileDialog1.FileName)
        tw.Write(Gen1(RichTextBox1.Text))
        tw.Close()
        MsgBox("生成成功")
    End Sub
End Class
