Imports System.Runtime.InteropServices
Imports System.Windows.Forms
Imports NScript.Drawing

Namespace NScript
    <ComClass()> <ComVisible(True)> Public Class ClickEventArgs
        Private ReadOnly Sender As Control
        Public Sub New(sender As Control)
            Me.Sender = sender
        End Sub

        Public ReadOnly Property SenderId As String
            Get
                Return Sender.Name
            End Get
        End Property
        Public ReadOnly Property MousePoint As CPoint
            Get
                Return Control.MousePosition
            End Get
        End Property
        Public ReadOnly Property MouseButton As MouseButtons
            Get
                Return Control.MouseButtons
            End Get
        End Property
    End Class
    <ComVisible(True)> Public Enum MouseEventType
        MouseUp
        MousePress
        MouseDown
    End Enum
    <ComVisible(True)> Public Enum KeyboradEventType
        KeyUp
        KeyPress
        KeyDown
    End Enum
    ''' <summary>
    ''' 键盘事件修饰符
    ''' </summary>
    <ComVisible(True)> <Flags> Public Enum KeyboradEventMods
        ''' <summary>
        ''' 没有按下任何键
        ''' </summary>
        None = 0
        ''' <summary>
        ''' 按下了Shift键
        ''' </summary>
        Shift = 1
        ''' <summary>
        ''' 按下了Alt键
        ''' </summary>
        Alt = 2
        ''' <summary>
        ''' 按下了Ctrl键
        ''' </summary>
        Ctrl = 4
        ''' <summary>
        ''' 保留1
        ''' </summary>
        NoMeaing1 = 8
        ''' <summary>
        ''' 保留2
        ''' </summary>
        NoMeaing2 = 16
    End Enum
    <ComClass()> <ComVisible(True)> Public Class KeyboradEventArgs
        Private ReadOnly Sender As Control
        Private ReadOnly keyevent As KeyEventArgs
        Public Sub New(sender As Control, keyevent As KeyEventArgs)
            Me.keyevent = keyevent
            Me.Sender = sender
        End Sub
        Public ReadOnly Property Keys As Keys
            Get
                Return keyevent.KeyCode
            End Get
        End Property
        Public ReadOnly Property Modifiers As KeyboradEventMods
            Get
                Dim Value As KeyboradEventMods = KeyboradEventMods.None
                If keyevent.Shift Then Value = Value Or KeyboradEventMods.Shift
                If keyevent.Alt Then Value = Value Or KeyboradEventMods.Alt
                If keyevent.Control Then Value = Value Or KeyboradEventMods.Ctrl
                Return Value
            End Get
        End Property
        Public ReadOnly Property KeyChar As String
            Get
                Dim res As String
                res = CStr(keyevent.KeyCode)
                Return If(res.Length > 1, "?", res)
            End Get
        End Property
        Public ReadOnly Property SenderId As String
            Get
                Return Sender.Name
            End Get
        End Property

    End Class
    <ComClass()> <ComVisible(True)> Public Class MouseEventArgs
        Private ReadOnly Sender As Control
        Private ReadOnly _Type As MouseEventType
        Public ReadOnly Property EventType() As MouseEventType
            Get
                Return _Type
            End Get
        End Property
        Public Sub New(sender As Control, type As MouseEventType)
            Me.Sender = sender
            _Type = type
        End Sub

        Public ReadOnly Property SenderId As String
            Get
                Return Sender.Name
            End Get
        End Property
        Public ReadOnly Property MousePoint As CPoint
            Get
                Return New CPoint(Control.MousePosition)
            End Get
        End Property
        Public ReadOnly Property MouseButton As MouseButtons
            Get
                Return Control.MouseButtons
            End Get
        End Property
    End Class
End Namespace