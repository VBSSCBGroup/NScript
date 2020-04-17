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
        DataGridView1.Rows.Add("My App", "FormAutoGen" & (Rnd() * 10000), 0, 0, 400, 400, "Form")
    End Sub
    Private Function GenCode()
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
        strbulid.AppendFormat("<FormData text=#{0}# x=#{1}# y=#{2}# id=#{3}# px=#{4}# py=#{5}#>", rows("_Text").Value, rows("X").Value, rows("Y").Value, rows("Id").Value, rows("SizeX").Value, rows("SizeY").Value)
        For Each wawa As DataGridViewRow In DataGridView1.Rows
            rows = wawa.Cells
            If rows("_Type").Value = "Form" Or wawa.IsNewRow Then
            Else
                strbulid.AppendFormat("<{6} text=#{0}# x=#{1}# y=#{2}# id=#{3}# sizex=#{4}# sizey=#{5}#/>", rows("_Text").Value, rows("X").Value, rows("Y").Value, rows("Id").Value, rows("SizeX").Value, rows("SizeY").Value, rows("_Type").Value)
            End If
        Next
        strbulid.Append("</FormData>")
        Return strbulid.ToString()
    End Function
    Public nform As New NSTForm
    Private Sub UpdataForm()
        nform.NaiveForm.Controls.Clear()
        nform.Bulid(GenCode())
        For Each wawa As Control In nform.NaiveForm.Controls
            AddHandler wawa.MouseDown, Sub(sender1 As Object, e1 As EventArgs)
                                           movecontrol = sender1
                                           oldpoint = Control.MousePosition - movecontrol.Location
                                           movecontrol.Cursor = Cursors.NoMove2D
                                       End Sub
            AddHandler wawa.MouseUp, Sub(sender1 As Object, e1 As EventArgs)
                                         movecontrol.Cursor = Cursors.Default
                                         movecontrol = Nothing
                                         oldpoint = New Point()
                                     End Sub
        Next
        nform.NaiveForm.MdiParent = Me
        nform.NaiveForm.StartPosition = FormStartPosition.Manual
        nform.NaiveForm.Location = Panel1.Location
        nform.NaiveForm.TopMost = True
        nform.NaiveForm.Show()
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        UpdataForm()
    End Sub
    Private movecontrol As Control, oldpoint As Point
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        TextBox1.Text = GenCode()
    End Sub
    Private n As Int32
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
End Class
