Imports System.Runtime.InteropServices
Namespace NScript
    <ComClass()> <ComVisible(True)> Public Class Timer
        Implements IDisposable
        Public Event Tick()
        Private Tim As System.Windows.Forms.Timer
        Public Sub New()
            Tim = New System.Windows.Forms.Timer With {
                .Interval = 100
            }
            AddHandler Tim.Tick, AddressOf Ticke
        End Sub
        Friend Sub Ticke(sender As Object, e As EventArgs)
            RaiseEvent Tick()
        End Sub
        Public Sub Start(Optional Interval As Int32 = 100)
            Tim.Interval = Interval
            Tim.Enabled = True
        End Sub
        Public Property Enabled As Boolean
            Get
                Return Tim.Enabled
            End Get
            Set(value As Boolean)
                Tim.Enabled = value
            End Set
        End Property
        Public Property Interval As Int32
            Get
                Return Tim.Interval
            End Get
            Set(value As Int32)
                Tim.Interval = value
            End Set
        End Property

#Region "IDisposable Support"
        Private disposedValue As Boolean ' 要检测冗余调用

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not disposedValue Then
                If disposing Then
                    Tim.Dispose()
                    ' TODO: 释放托管状态(托管对象)。
                End If
                Tim = Nothing
                ' TODO: 释放未托管资源(未托管对象)并在以下内容中替代 Finalize()。
                ' TODO: 将大型字段设置为 null。
            End If
            disposedValue = True
        End Sub

        ' TODO: 仅当以上 Dispose(disposing As Boolean)拥有用于释放未托管资源的代码时才替代 Finalize()。
        'Protected Overrides Sub Finalize()
        '    ' 请勿更改此代码。将清理代码放入以上 Dispose(disposing As Boolean)中。
        '    Dispose(False)
        '    MyBase.Finalize()
        'End Sub

        ' Visual Basic 添加此代码以正确实现可释放模式。
        Public Sub Dispose() Implements IDisposable.Dispose
            ' 请勿更改此代码。将清理代码放入以上 Dispose(disposing As Boolean)中。
            Dispose(True)
            ' TODO: 如果在以上内容中替代了 Finalize()，则取消注释以下行。
            GC.SuppressFinalize(Me)
        End Sub
#End Region
    End Class
End Namespace
