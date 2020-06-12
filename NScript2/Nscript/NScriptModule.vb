Namespace NScript
    Friend Module NScriptModule
        'Develop Note:记得版本格式：（一个Double）|(版本名)
        Public Ver As String = GetLocalString("Ver")
        Public Error1 As String = GetLocalString("Error0x1")

        Private WithEvents Appd As AppDomain = AppDomain.CurrentDomain

        Private Sub Appd_UnhandledException(sender As Object, e As UnhandledExceptionEventArgs) Handles Appd.UnhandledException

        End Sub

        Private Sub Appd_ProcessExit(sender As Object, e As EventArgs) Handles Appd.ProcessExit

        End Sub

        Public Function GetLocalString(LocalKey As String) As String
            Dim resset As Resources.ResourceSet = My.Resources.ResourceManager.GetResourceSet(Globalization.CultureInfo.CurrentCulture, True, True)
            For Each res As DictionaryEntry In resset
                If CType(res.Key, String).ToLower() = LocalKey.ToLower() Then
                    If TypeOf res.Value Is String Then
                        Return res.Value
                    End If
                End If
            Next
            resset.Dispose()
            Return String.Format(Nothing, "Not Found LocalString""{0}"".", LocalKey)
        End Function
    End Module
End Namespace
