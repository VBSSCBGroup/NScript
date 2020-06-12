Namespace NSTFormXml
    '''<remarks/>
    <System.SerializableAttribute(),
 System.ComponentModel.DesignerCategoryAttribute("code"),
 System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True)>
    Partial Public Class FormDataControl

        Private IdField As String

        Private XField As Int32

        Private YField As Int32

        Private PxField As Int32

        Private PyField As Int32

        Private TextField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttribute("id")>
        Public Property Id() As String
            Get
                Return Me.IdField
            End Get
            Set
                Me.IdField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttribute("x")>
        Public Property X() As Int32
            Get
                Return Me.XField
            End Get
            Set
                Me.XField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttribute("y")>
        Public Property Y() As Int32
            Get
                Return Me.YField
            End Get
            Set
                Me.YField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttribute("sizex")>
        Public Property SizeX() As Int32
            Get
                Return Me.PxField
            End Get
            Set
                Me.PxField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttribute("sizey")>
        Public Property SizeY() As Int32
            Get
                Return Me.PyField
            End Get
            Set
                Me.PyField = Value
            End Set
        End Property
        '''<remarks/>
        <System.Xml.Serialization.XmlAttribute("text")>
        Public Property Text() As String
            Get
                Return Me.TextField
            End Get
            Set
                Me.TextField = Value
            End Set
        End Property
    End Class

End Namespace
