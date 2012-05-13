Const ADS_PROPERTY_CLEAR = 1
' Declarations are commented for use with VBScript
Dim oRootDSE 'As IADs
Dim oConn 'As ADODB.Connection
Dim oRecordset 'As ADODB.Recordset
Dim strAttribute 'As String
Dim strADsPath 'As String
Dim oAttribute 'As IADs
Dim bReplicate 'As Boolean

strAttribute = "Picture"  	'Replace with the name of the attribute to change replication  
				'"thumbnailPhoto" is the name of the attribute, but the common name (cn) is "Picture"


bReplicate = True            		'Replicate to GC True/False

Set oConn = CreateObject("ADODB.Connection")
Set oRootDSE = GetObject("LDAP://RootDSE")

oConn.Provider = "ADsDSOObject"
oConn.Open "ADs Provider"

strQuery = "<LDAP://" & oRootDSE.Get("schemaNamingContext") _
	& ">;(&(objectClass=attributeSchema)(cn=" & strAttribute & "));cn,adspath;subtree"
Set oRecordset = oConn.Execute(strQuery)


oRecordset.MoveFirst
strADsPath = oRecordset.Fields("ADsPath")  'store the path of the object in the schema

Set oAttribute = GetObject(strADsPath) 'Get the object in the schema
If bReplicate Then
  oAttribute.Put "isMemberOfPartialAttributeSet", True    'Set the property to true
Else
  oAttribute.PutEx ADS_PROPERTY_CLEAR, "isMemberOfPartialAttributeSet", 0   'Clear the property
End If

'Write to schema
oAttribute.SetInfo

'Clean Up
Set oAttribute = Nothing
Set oRootDSE = Nothing
oRecordset.Close
oConn.Close
Set oConn = Nothing
Set oRecordset = Nothing		