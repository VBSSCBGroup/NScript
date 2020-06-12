Imports System.Drawing
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Windows.Forms
Imports NScript.Drawing
Namespace NScript
    <ComClass()> <ComVisible(True)>
    Public Class NstForm
        ' 可创建的 COM 类必须具有一个不带参数的 Public Sub New() 
        ' 否则， 将不会在 
        ' COM 注册表中注册此类，且无法通过
        ' CreateObject 创建此类。
        Public Sub New()
            MyBase.New()
            AddHandler NForm.Paint, Sub()
                                        RaiseEvent FormPaint()
                                    End Sub
            AH(NForm)
        End Sub
        Private ReadOnly NForm As New Form()
        Public ReadOnly Property NaiveForm As Form
            Get
                Return NForm
            End Get
        End Property
        Public Event Click(Sender As Object, e As ClickEventArgs)
        Public Event MouseEvent(Sender As Object, e As MouseEventArgs)
        Public Event KeybroadEvent(Sender As Object, e As KeyboradEventArgs)
        Public Sub Bulid(code As String)
            If code Is Nothing Then
                Return
            End If

            Dim wa As New StringReader(code.Replace("#", Chr(34)))
            Dim xmlreader As Xml.XmlReader = Xml.XmlReader.Create(wa)
            Dim xmlserv As New Xml.Serialization.XmlSerializer(GetType(NSTFormXml.FormData))
            Dim formdata As NSTFormXml.FormData = xmlserv.Deserialize(xmlreader)
            NForm.SuspendLayout()
            With NForm
                .Name = formdata.Id
                .Text = formdata.Text
                .Location = New Point(formdata.X, formdata.Y)
            End With
            If formdata.SizeX = 0 And formdata.SizeY = 0 Then
            Else
                NForm.Size = New Size(formdata.SizeX， formdata.SizeY)
            End If
            If formdata.Label IsNot Nothing Then
                For Each lbl In formdata.Label
                    Dim ctl As New Label() With {.Name = lbl.Id, .Text = lbl.Text, .Location = New Point(lbl.X, lbl.Y)}
                    If lbl.SizeX = 0 And lbl.SizeY = 0 Then
                        ctl.AutoSize = True
                    Else
                        ctl.Size = New Size(lbl.SizeX, lbl.SizeY)
                    End If
                    NForm.Controls.Add(ctl)
                    AH(ctl)
                Next
            End If
            If formdata.TextBox IsNot Nothing Then
                For Each lbl In formdata.TextBox
                    Dim ctl As New TextBox() With {.Name = lbl.Id, .Text = lbl.Text, .Location = New Point(lbl.X, lbl.Y)}
                    If lbl.SizeX = 0 And lbl.SizeY = 0 Then
                    Else
                        ctl.Size = New Size(lbl.SizeX, lbl.SizeY)
                    End If
                    NForm.Controls.Add(ctl)
                    AH(ctl)
                Next
            End If
            If formdata.Button IsNot Nothing Then
                For Each lbl In formdata.Button
                    Dim ctl As New Button() With {.Name = lbl.Id, .Text = lbl.Text, .Location = New Point(lbl.X, lbl.Y)}
                    If lbl.SizeX = 0 And lbl.SizeY = 0 Then
                    Else
                        ctl.Size = New Size(lbl.SizeX, lbl.SizeY)
                    End If
                    NForm.Controls.Add(ctl)
                    AH(ctl)
                Next
            End If
            NForm.ResumeLayout()
            wa.Dispose()
        End Sub
        Public ReadOnly Property Controls As IEnumerable
            Get
                Return CType(NForm.Controls, IEnumerable)
            End Get
        End Property
        Public Sub AddControl(obj0 As Control)
            NForm.Controls.Add(obj0)
        End Sub
        Private InnNGDI As NGDI
        Public Function CreateGraphics() As NGDI
            If InnNGDI Is Nothing Then
                InnNGDI = New NGDI()
            End If
            Return InnNGDI
        End Function
        Public Event FormPaint()
        Public ReadOnly Property Control(name As String) As Control
            Get
                For Each wa As Control In NForm.Controls
                    If wa.Name.ToLower(Globalization.CultureInfo.CurrentCulture) = name.ToLower(Globalization.CultureInfo.CurrentCulture) Then
                        Return wa
                    End If
                Next
                Return Nothing
            End Get
        End Property
        Friend Sub AH(ctl As Control)
            AddHandler ctl.Click, Sub(sender As Object, e As EventArgs)
                                      RaiseEvent Click(sender, New ClickEventArgs(sender))
                                  End Sub
            AddHandler ctl.MouseClick, Sub(sender As Object, e As EventArgs)
                                           RaiseEvent MouseEvent(sender, New MouseEventArgs(sender, MouseEventType.MousePress))
                                       End Sub
            AddHandler ctl.MouseUp, Sub(sender As Object, e As EventArgs)
                                        RaiseEvent MouseEvent(sender, New MouseEventArgs(sender, MouseEventType.MouseUp))
                                    End Sub
            AddHandler ctl.MouseDown, Sub(sender As Object, e As EventArgs)
                                          RaiseEvent MouseEvent(sender, New MouseEventArgs(sender, MouseEventType.MouseDown))
                                      End Sub
            AddHandler ctl.KeyDown, Sub(sender As Object, e As KeyEventArgs)
                                        RaiseEvent KeybroadEvent(sender, New KeyboradEventArgs(sender, e))
                                    End Sub
            AddHandler ctl.KeyUp, Sub(sender As Object, e As KeyEventArgs)
                                      RaiseEvent KeybroadEvent(sender, New KeyboradEventArgs(sender, e))
                                  End Sub
        End Sub
#Disable Warning
        Public Sub Show()
            NForm.Show()
        End Sub
        Public Function ShowDialog() As Int32
            Return CInt(NForm.ShowDialog())
        End Function
        Public Sub Hide()
            NForm.Hide()
        End Sub
        Public Sub Close()
            NForm.Close()
        End Sub
        Public Sub Dispose()
            NForm.Dispose()
        End Sub
#Enable Warning
    End Class
End Namespace
