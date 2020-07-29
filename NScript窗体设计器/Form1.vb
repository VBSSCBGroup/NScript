Imports System.Text
Imports NScript

Public Class Form1
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Select Case ComboBox1.Text.ToLower()
            Case "label"
                DataGridView1.Rows.Add("Label", "Label" & n, 0, 0, 0, 0, "Label")
            Case "textbox"
                DataGridView1.Rows.Add("TextBox", "TextBox" & n, 0, 0, 0, 0, "TextBox")
            Case "button"
                DataGridView1.Rows.Add("Button", "Button" & n, 0, 0, 0, 0, "Button")
        End Select
        n += 1
        UpdataForm()
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        DataGridView1.Rows.Add("My App", "FormAutoGen" & Int(Rnd() * 1000000), 0, 0, 400, 400, "Form")
        UpdataForm()
    End Sub
    Private Function GenXmlCode()
        Dim strbulid As New StringBuilder
        Dim frm As DataGridViewRow
        For Each wawa As DataGridViewRow In DataGridView1.Rows
            If wawa.Cells("_Type").Value = "Form" Then
                frm = wawa
            End If
        Next
        If frm Is Nothing Then
            Return ""
        End If
        Dim rows = frm.Cells
        Escape(rows)
        strbulid.AppendFormat("<FormData text=#{0}# x=#{1}# y=#{2}# id=#{3}# px=#{4}# py=#{5}#>", rows("_Text").Value, rows("X").Value, rows("Y").Value, rows("Id").Value, rows("SizeX").Value, rows("SizeY").Value)
        For Each wawa As DataGridViewRow In DataGridView1.Rows
            Escape(rows)
            rows = wawa.Cells
            If rows("_Type").Value = "Form" Or wawa.IsNewRow Then
            Else
                strbulid.AppendFormat("<{6} text=#{0}# x=#{1}# y=#{2}# id=#{3}# sizex=#{4}# sizey=#{5}#/>", rows("_Text").Value, rows("X").Value, rows("Y").Value, rows("Id").Value, rows("SizeX").Value, rows("SizeY").Value, rows("_Type").Value)
            End If
        Next
        strbulid.Append("</FormData>")

        Return strbulid.ToString()
    End Function
    Sub Escape(rows As DataGridViewCellCollection)
        For Each rw As DataGridViewCell In rows
            rw.Value = rw.Value.ToString().Replace("#", "/#")
        Next
    End Sub
    Private Function GenVbsCode()
        Dim strbulid As New StringBuilder
        Dim frm As DataGridViewRow
        For Each wawa As DataGridViewRow In DataGridView1.Rows
            If wawa.Cells("_Type").Value = "Form" Then
                frm = wawa
            End If
        Next
        If frm Is Nothing Then
            Return ""
        End If
        Dim rows = frm.Cells
        Dim tsr As New StringBuilder()
        Dim clkhnd As New Dictionary(Of String, String) 'Click
        Dim msehnd As New Dictionary(Of String, String) 'MouseEvent
        Dim keyhnd As New Dictionary(Of String, String) 'KeyboardEvent
        For Each e In EventHandlers
            Select Case e.Key.Item2.ToLower()
                Case "click"
                    clkhnd.Add(e.Key.Item1, e.Value)
                Case "mouseevent"
                    msehnd.Add(e.Key.Item1, e.Value)
                Case "keyboardevent"
                    keyhnd.Add(e.Key.Item1, e.Value)
                Case Else

            End Select
        Next
        If clkhnd.Count <> 0 Then
            tsr.AppendFormat("Sub {0}_Click(sender,e)
Select Case e.SenderId
", rows("Id").Value)
            For Each clkh In clkhnd
                tsr.AppendLine("Case """ & clkh.Key & """" & vbCrLf &
                               clkh.Value)
            Next
            tsr.AppendLine("End Select
End Sub")
        End If
        If msehnd.Count <> 0 Then
            tsr.AppendFormat("Sub {0}_MouseEvent(sender,e)
Select Case e.SenderId
", rows("Id").Value)
            For Each clkh In msehnd
                tsr.AppendLine("Case """ & clkh.Key & """" & vbCrLf &
                               clkh.Value)
            Next
            tsr.AppendLine("End Select
End Sub")
        End If
        If keyhnd.Count <> 0 Then
            tsr.AppendFormat("Sub {0}_KeybroadEvent(sender,e)
Select Case e.SenderId
", rows("Id").Value)
            For Each clkh In keyhnd
                tsr.AppendLine("Case """ & clkh.Key & """" & vbCrLf &
                               clkh.Value)
            Next
            tsr.AppendLine("End Select
End Sub")
        End If
        strbulid.AppendFormat("Set {0} = Wscript.CreateObject(""NScript.NSTForm"",""{0}_"")
{1}
{0}.bulid """, rows("Id").Value, tsr.ToString())
        Dim str222 As String = rows("Id").Value
        strbulid.AppendFormat("<FormData text=#{0}# x=#{1}# y=#{2}# id=#{3}# px=#{4}# py=#{5}#>", rows("_Text").Value, rows("X").Value, rows("Y").Value, rows("Id").Value, rows("SizeX").Value, rows("SizeY").Value)
        For Each wawa As DataGridViewRow In DataGridView1.Rows
            rows = wawa.Cells
            If rows("_Type").Value = "Form" Or wawa.IsNewRow Then
            Else
                strbulid.AppendFormat("<{6} text=#{0}# x=#{1}# y=#{2}# id=#{3}# sizex=#{4}# sizey=#{5}#/>", rows("_Text").Value, rows("X").Value, rows("Y").Value, rows("Id").Value, rows("SizeX").Value, rows("SizeY").Value, rows("_Type").Value)
            End If
        Next
        strbulid.Append("</FormData>""")
        strbulid.AppendFormat("
{0}.ShowDialog()", str222)
        Return strbulid.ToString()
    End Function
    Public nform As New NSTForm
    Private Sub UpdataForm()
        nform.NaiveForm.Controls.Clear()
        nform.Bulid(GenXmlCode())
        For Each wawa As Control In nform.NaiveForm.Controls
            AddHandler wawa.MouseDown, Sub(sender1 As Object, e1 As EventArgs)
                                           movecontrol = sender1
                                           oldpoint = Control.MousePosition - movecontrol.Location
                                           movecontrol.Cursor = Cursors.NoMove2D
                                       End Sub
            AddHandler wawa.MouseUp, Sub(sender1 As Object, e1 As EventArgs)
                                         If movecontrol IsNot Nothing Then
                                             movecontrol.Cursor = Cursors.Default
                                             movecontrol = Nothing
                                             oldpoint = New Point()
                                         End If
                                     End Sub
            AddHandler wawa.Click, Sub(sender2 As Object, e2 As System.Windows.Forms.MouseEventArgs)
                                       If TypeOf sender2 Is Button Then
                                           Dim sjbd As New EventHandlerEdit(Me, CType(sender2, Control).Name)
                                           If movecontrol IsNot Nothing Then
                                               movecontrol.Cursor = Cursors.Default
                                               movecontrol = Nothing
                                               oldpoint = New Point()
                                           End If
                                           sjbd.Show()
                                       End If
                                   End Sub
            AddHandler wawa.MouseDoubleClick, Sub(sender2 As Object, e2 As System.Windows.Forms.MouseEventArgs)
                                                  Dim sjbd As New EventHandlerEdit(Me, CType(sender2, Control).Name)
                                                  If movecontrol IsNot Nothing Then
                                                      movecontrol.Cursor = Cursors.Default
                                                      movecontrol = Nothing
                                                      oldpoint = New Point()
                                                  End If
                                                  sjbd.Show()
                                              End Sub
        Next
        nform.NaiveForm.Location = Me.Location
        nform.NaiveForm.StartPosition = FormStartPosition.Manual
        nform.NaiveForm.Text = "窗体预览"
        nform.NaiveForm.TopMost = True
        nform.NaiveForm.Show()
    End Sub
    Friend EventHandlers As New Dictionary(Of Tuple(Of String, String), String)
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        UpdataForm()
    End Sub
    Private movecontrol As Control, oldpoint As Point
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        TextBox1.Text = GenVbsCode()
    End Sub
    Private n As Int32

    Public Sub New()

        ' 此调用是设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。

    End Sub

    Private Sub DataGridView1_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellEndEdit
        UpdataForm()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If movecontrol Is Nothing Then
        Else
            For Each wawa As DataGridViewRow In DataGridView1.Rows
                Dim rows = wawa.Cells
                If rows("Id").Value = movecontrol.Name Then
                    rows("X").Value = (Control.MousePosition - oldpoint).X
                    rows("Y").Value = (Control.MousePosition - oldpoint).Y
                End If
            Next
            movecontrol.Location = Control.MousePosition - oldpoint
        End If
    End Sub

    Private Sub Form1_Click(sender As Object, e As EventArgs) Handles TextBox1.DoubleClick
        Id.HeaderText = "(你的)名字"
    End Sub

    Private Sub Form1_Move(sender As Object, e As EventArgs) Handles Me.Move
        nform.NaiveForm.Location = Me.Location
    End Sub
End Class
