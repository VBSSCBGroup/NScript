Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Text
Imports IWshRuntimeLibrary
Namespace NScript
    <ComVisible(True)> <ComClass()> Public Class NSConsole
        Implements TextStream
#Region "颜色部分"
        Public Property ForeColor As Int32
            Get
                Return Console.ForegroundColor
            End Get
            Set(value As Int32)
                Console.ForegroundColor = value
            End Set
        End Property
        Public Property BackColor As Int32
            Get
                Return Console.BackgroundColor
            End Get
            Set(value As Int32)
                Console.BackgroundColor = value
            End Set
        End Property
#End Region
#Region "New"
        Public Sub New()

        End Sub
#End Region
#Region "扩展方法"
        Public Sub Clear()
            Console.Clear()
        End Sub
        Public Sub InitConsole()
            Dim HasConsole As Boolean = False
            Try
                Console.CursorLeft = 12
                HasConsole = True
            Catch ex As IOException
                HasConsole = False
            End Try
            If HasConsole Then
            Else
                Dim str As String = Command()
                Shell("cmd /c cscript.exe //nologo " & Chr(34) & str & Chr(34), AppWinStyle.NormalFocus)
                Environment.Exit(10)
            End If
        End Sub
        Public Sub SetColor(Fore As Int32, Back As Int32)
            Console.ForegroundColor = Fore
            Console.BackgroundColor = Back
        End Sub
        Public Sub Echo(Str As String)
            Me.Write(Str)
        End Sub
        Public Function Question(StrQ As String) As String
            Console.Write(StrQ)
            Return Me.ReadLine()
        End Function
        Public Function Ask(StrQ As String) As String
            Return Me.Question(StrQ)
        End Function
#End Region
#Region "流"
        Private _In As TextStream
        Public Property StdIn As TextStream
            Get
                If _In Is Nothing Then
                    _In = New ConsoleReader()
                End If
                Return _In
            End Get
            Set(value As TextStream)
                _In = value
            End Set
        End Property
        Private _Out As TextStream
        Public Property StdOut As TextStream
            Get
                If _Out Is Nothing Then
                    _Out = New ConsoleWriter()
                End If
                Return _Out
            End Get
            Set(value As TextStream)
                _Out = value
            End Set
        End Property
        Private _Err As TextStream
        Public Property StdErr As TextStream
            Get
                If _Err Is Nothing Then
                    _Err = New ConsoleError()
                End If
                Return _Err
            End Get
            Set(value As TextStream)
                _Err = value
            End Set
        End Property

        Friend Class ConsoleReader
            Inherits ConsoleStreamBase

            Public Overrides Sub Write(Text As String)
                Throw New NotImplementedException()
            End Sub

            Public Overrides Sub WriteLine(Optional Text As String = "")
                Throw New NotImplementedException()
            End Sub

            Public Overrides Function Read(Characters As Integer) As String
                Dim str As New StringBuilder()
                For i = 0 To Characters
                    str.Append(Console.ReadKey().KeyChar)
                Next
                Return str.ToString()
            End Function

            Public Overrides Function ReadLine() As String
                Return Console.ReadLine()
            End Function

            Public Overrides Function ReadAll() As String
                Throw New NotImplementedException()
            End Function
        End Class
        Friend Class ConsoleWriter
            Inherits ConsoleStreamBase

            Public Overrides Sub Write(Text As String)
                Console.Write(Text)
            End Sub

            Public Overrides Sub WriteLine(Optional Text As String = "")
                Console.WriteLine(Text)
            End Sub

            Public Overrides Function Read(Characters As Integer) As String
                Throw New NotImplementedException()
            End Function

            Public Overrides Function ReadLine() As String
                Throw New NotImplementedException()
            End Function

            Public Overrides Function ReadAll() As String
                Throw New NotImplementedException()
            End Function
        End Class
        Friend Class ConsoleError
            Inherits ConsoleStreamBase

            Public Overrides Sub Write(Text As String)
                Console.Error.Write(Text)
            End Sub

            Public Overrides Sub WriteLine(Optional Text As String = "")
                Console.Error.WriteLine(Text)
            End Sub

            Public Overrides Function Read(Characters As Integer) As String
                Throw New NotImplementedException()
            End Function

            Public Overrides Function ReadLine() As String
                Throw New NotImplementedException()
            End Function

            Public Overrides Function ReadAll() As String
                Throw New NotImplementedException()
            End Function
        End Class
        Friend MustInherit Class ConsoleStreamBase
            Implements TextStream
            Public MustOverride Function Read(Characters As Integer) As String Implements ITextStream.Read

            Public MustOverride Function ReadLine() As String Implements ITextStream.ReadLine

            Public MustOverride Function ReadAll() As String Implements ITextStream.ReadAll

            Public MustOverride Sub Write(Text As String) Implements ITextStream.Write

            Public MustOverride Sub WriteLine(Optional Text As String = "") Implements ITextStream.WriteLine

            Public Sub WriteBlankLines(Lines As Integer) Implements ITextStream.WriteBlankLines
                For i = 0 To Lines
                    Me.ReadLine()
                Next
            End Sub

            Public Sub Skip(Characters As Integer) Implements ITextStream.Skip
                Me.Read(Characters)
            End Sub

            Public Sub SkipLine() Implements ITextStream.SkipLine
                Me.ReadLine()
            End Sub

            Public Sub Close() Implements ITextStream.Close
                Throw New NotSupportedException()
            End Sub

            Public ReadOnly Property Line As Integer Implements ITextStream.Line
                Get
                    Throw New NotSupportedException()
                End Get
            End Property

            Public ReadOnly Property Column As Integer Implements ITextStream.Column
                Get
                    Throw New NotSupportedException()
                End Get
            End Property

            Public ReadOnly Property AtEndOfStream As Boolean Implements ITextStream.AtEndOfStream
                Get
                    Throw New NotSupportedException()
                End Get
            End Property

            Public ReadOnly Property AtEndOfLine As Boolean Implements ITextStream.AtEndOfLine
                Get
                    Throw New NotSupportedException()
                End Get
            End Property
        End Class
#End Region
#Region "TextStream实现"


        Public Function Read(Characters As Integer) As String Implements ITextStream.Read
            Return StdIn.Read(Characters)
        End Function

        Public Function ReadLine() As String Implements ITextStream.ReadLine
            Return StdIn.ReadLine()
        End Function

        Public Function ReadAll() As String Implements ITextStream.ReadAll
            Return StdIn.ReadAll()
        End Function

        Public Sub Write(Text As String) Implements ITextStream.Write
            StdOut.Write(Text)
        End Sub

        Public Sub WriteLine(Optional Text As String = "") Implements ITextStream.WriteLine
            StdOut.WriteLine(Text)
        End Sub

        Public Sub WriteBlankLines(Lines As Integer) Implements ITextStream.WriteBlankLines
            StdOut.WriteBlankLines(Lines)
        End Sub

        Public Sub Skip(Characters As Integer) Implements ITextStream.Skip
            StdIn.Skip(Characters)
        End Sub

        Public Sub SkipLine() Implements ITextStream.SkipLine
            StdIn.SkipLine()
        End Sub

        Public Sub Close() Implements ITextStream.Close
            If _In IsNot Nothing Then
                If TypeOf StdIn IsNot ConsoleError Then
                    StdIn.Close()
                End If
            End If
            If _Out IsNot Nothing Then
                If TypeOf _Out IsNot ConsoleError Then
                    _Out.Close()
                End If
            End If
            If _Err IsNot Nothing Then
                If TypeOf StdErr IsNot ConsoleError Then
                    StdErr.Close()
                End If
            End If
            Me.Finalize()
        End Sub

        Public ReadOnly Property Line As Integer Implements ITextStream.Line
            Get
                Throw New NotSupportedException()
            End Get
        End Property

        Public ReadOnly Property Column As Integer Implements ITextStream.Column
            Get
                Throw New NotSupportedException()
            End Get
        End Property

        Public ReadOnly Property AtEndOfStream As Boolean Implements ITextStream.AtEndOfStream
            Get
                Throw New NotSupportedException()
            End Get
        End Property

        Public ReadOnly Property AtEndOfLine As Boolean Implements ITextStream.AtEndOfLine
            Get
                Throw New NotSupportedException()
            End Get
        End Property
#End Region
    End Class
End Namespace
